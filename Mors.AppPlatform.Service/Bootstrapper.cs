using System;
using Mors.AppPlatform.Adapters;
using Mors.AppPlatform.Adapters.Services;
using Mors.AppPlatform.Service.Host;
using Mors.AppPlatform.Service.Infrastructure;
using Mors.AppPlatform.Service.Properties;
using Mors.AppPlatform.Support.Dispatching;
using Mors.AppPlatform.Support.EventSourcing;
using Mors.AppPlatform.Support.EventSourcing.Storage;
using Mors.AppPlatform.Support.Repositories;
using Mors.AppPlatform.Support.Transactions;
using Nancy.Hosting.Self;

namespace Mors.AppPlatform.Service
{
    internal sealed class Bootstrapper
    {
        private AsyncHandlerDispatcher _handlerDispatcher;
        private NancyHost _host;

        public void Bootstrap(Settings configuration)
        {
            var eventBus = new Support.Events.EventBus();
            var idFactory = new GuidIdFactory();
            var handlerRegistry = new HandlerRegistry();
            var handlerDispatcher = new HandlerDispatcher(handlerRegistry);
            var repositories = new Repositories();

            var eventSourcingModule = new EventSourcingModule(
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
                new Transaction());

            WordsApplication.Bootstrap(
                handlerRegistry,
                eventBus,
                new Transaction(),
                eventSourcingModule);

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

            var hostBoostrapper = new HostBootstrapper(queryDispatcher, commandDispatcher, configuration.SitesPath);
            _host = new NancyHost(
                hostBoostrapper,
                new HostConfiguration
                {
                    UrlReservations =
                        new UrlReservations
                        {
                            CreateAutomatically = configuration.CreateUrlReservation,
                            User = configuration.UrlReservationUser 
                        }
                },
                new Uri(configuration.HostUri));
        }

        public void RunService()
        {
            _host.Start();
            _handlerDispatcher.Run();
        }
    }
}
