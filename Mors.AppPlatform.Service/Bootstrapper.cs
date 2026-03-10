using Mors.AppPlatform.Adapters.Services;
using Mors.AppPlatform.Service.Adapters;
using Mors.AppPlatform.Service.Infrastructure;
using Mors.AppPlatform.Support.Dispatching;
using Mors.AppPlatform.Support.EventSourcing;
using Mors.AppPlatform.Support.EventSourcing.Storage;
using Mors.AppPlatform.Support.Repositories;
using Mors.AppPlatform.Support.Serialization;
using Mors.AppPlatform.Support.Transactions;
using System.Threading;

namespace Mors.AppPlatform.Service
{
    internal sealed class Bootstrapper
    {
        private AsyncHandlerDispatcher _handlerDispatcher;
        private AspNetCoreHost _host;

        public void Bootstrap(Settings configuration)
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
            _handlerDispatcher = new AsyncHandlerDispatcher(
                new PrioritizedHandlerSource(
                    new[]
                    {
                        new DependentHandlerSource(
                            commandHandlerSource,
                            new[]
                            {
                                queryHandlerSource.NoRunningHandlersEvent,
                                commandHandlerSource.NoRunningHandlersEvent
                            }),
                        new DependentHandlerSource(
                            queryHandlerSource,
                            new[] { commandHandlerSource.NoRunningHandlersEvent })
                    }));

            var contentTypeAwareSerializer = new ContentTypeAwareSerializer(knownTypesSet.GetKnownTypes());
            _host =
                new AspNetCoreHost(
                    queryDispatcher,
                    commandDispatcher,
                    contentTypeAwareSerializer,
                    configuration.SitesPath,
                    configuration.HostUri);
        }

        public void RunService()
        {
            new Thread(_handlerDispatcher.Run).Start();
            _host.Run();
        }
    }
}
