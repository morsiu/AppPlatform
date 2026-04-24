using Mors.AppPlatform.Service.Adapters;
using Mors.AppPlatform.Service.Infrastructure;
using Mors.AppPlatform.Support.Dispatching;
using Mors.AppPlatform.Support.EventSourcing;
using Mors.AppPlatform.Support.EventSourcing.Storage;
using Mors.AppPlatform.Support.Repositories;
using Mors.AppPlatform.Support.Serialization;
using Mors.AppPlatform.Support.Transactions;
using System.Threading;

namespace Mors.AppPlatform.Service;

internal sealed class Service
{
    private readonly AsyncHandlerDispatcher _handlerDispatcher;
    private readonly AspNetCoreHost _host;

    private Service(AsyncHandlerDispatcher handlerDispatcher, AspNetCoreHost host)
    {
        _handlerDispatcher = handlerDispatcher;
        _host = host;
    }

    public static Service Create(Settings configuration)
    {
        var eventBus = new Support.Events.EventBus();
        var idFactory = new GuidIdFactory();
        var handlerRegistry = new HandlerRegistry();
        var handlerDispatcher = new HandlerDispatcher(handlerRegistry);
        var repositories = new Repositories();
        var knownTypesSet = new KnownTypesSet();

        var eventSourcingModule =
            new EventSourcingModule(
                new EventSourcingEventBus(eventBus),
                supportedEventTypes =>
                    new XmlFileEventStore(
                        configuration.EventFilePath,
                        supportedEventTypes),
                idFactory.IdImplementationType);

        JourneysApplication.Bootstrap(
            handlerRegistry,
            handlerDispatcher,
            eventBus,
            repositories,
            eventSourcingModule,
            idFactory,
            knownTypesSet,
            new Transaction());

        WordsApplication.Bootstrap(
            handlerRegistry,
            eventBus,
            new Transaction(),
            eventSourcingModule,
            idFactory,
            knownTypesSet);

        eventSourcingModule.ReplayEvents();
        eventSourcingModule.StoreNewEvents();

        var commandHandlerQueue = new HandlerQueue();
        var queryHandlerQueue = new HandlerQueue();
        var queryDispatcher = new AsyncQueryDispatcher(new AsyncHandlerScheduler(handlerRegistry, queryHandlerQueue));
        var commandDispatcher = new AsyncCommandDispatcher(new AsyncHandlerScheduler(handlerRegistry, commandHandlerQueue));

        var commandHandlerSource = new TrackingHandlerSource(commandHandlerQueue);
        var queryHandlerSource = new TrackingHandlerSource(queryHandlerQueue);
        var asyncHandlerDispatcher = new AsyncHandlerDispatcher(
            new PrioritizedHandlerSource(
            [
                new DependentHandlerSource(
                        commandHandlerSource,
                        [
                            queryHandlerSource.NoRunningHandlersEvent,
                            commandHandlerSource.NoRunningHandlersEvent
                        ]),
                    new DependentHandlerSource(
                        queryHandlerSource,
                        [commandHandlerSource.NoRunningHandlersEvent])
            ]));

        var contentTypeAwareSerializer = new ContentTypeAwareSerializer(knownTypesSet.GetKnownTypes());
        var host =
            new AspNetCoreHost(
                queryDispatcher,
                commandDispatcher,
                contentTypeAwareSerializer,
                configuration.SitesPath,
                configuration.HostUri);
        return new Service(asyncHandlerDispatcher, host);
    }

    public void Run()
    {
        new Thread(_handlerDispatcher.Run).Start();
        _host.Run();
    }
}