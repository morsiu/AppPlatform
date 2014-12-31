using Mors.AppPlatform.Adapters.Services;
using Mors.AppPlatform.Support.Dispatching;
using Mors.AppPlatform.Support.Repositories;
using Repositories = Mors.AppPlatform.Adapters.Services.Repositories;

namespace Mors.AppPlatform.Service
{
    internal sealed class Bootstrapper
    {
        private AsyncHandlerDispatcher _handlerDispatcher;

        public AsyncQueryDispatcher QueryDispatcher { get; private set; }

        public AsyncCommandDispatcher CommandDispatcher { get; private set; }

        public void Bootstrap(string eventFileName)
        {
            var eventBus = new Support.Events.EventBus();
            var idFactory = new GuidIdFactory();
            var handlerRegistry = new HandlerRegistry();
            var handlerDispatcher = new HandlerDispatcher(handlerRegistry);
            var repositories = new Support.Repositories.Repositories();

            var eventSourcingModule = new Support.EventSourcing.Module(
                new EventSourcingEventBus(eventBus),
                idFactory.IdImplementationType,
                eventFileName);

            var bootstrapper = new Journeys.Application.Bootstrapper(
                new EventBus(eventBus),
                new Repositories(repositories),
                new IdFactory(idFactory),
                new CommandHandlerRegistry(handlerRegistry),
                new QueryDispatcher(handlerDispatcher),
                new QueryHandlerRegistry(handlerRegistry),
                new EventSourcing(eventSourcingModule));
            bootstrapper.Bootstrap();

            eventSourcingModule.ReplayEvents();
            eventSourcingModule.StoreNewEvents();

            var commandHandlerQueue = new HandlerQueue();
            var queryHandlerQueue = new HandlerQueue();
            QueryDispatcher = new AsyncQueryDispatcher(new AsyncHandlerScheduler(handlerRegistry, queryHandlerQueue));
            CommandDispatcher = new AsyncCommandDispatcher(new AsyncHandlerScheduler(handlerRegistry, commandHandlerQueue));

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
        }

        public void RunScheduledHandlers()
        {
            _handlerDispatcher.Run();
        }
    }
}
