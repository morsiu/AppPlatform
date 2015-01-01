using Mors.AppPlatform.Support.Dispatching;
using Mors.AppPlatform.Support.Repositories;
using Mors.AppPlatform.Adapters.Services;
using Mors.AppPlatform.Support.Transactions;
using Mors.AppPlatform.Adapters;

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
            var repositories = new Repositories();
            var transaction = new Transaction();

            var eventSourcingModule = new Support.EventSourcing.Module(
                new EventSourcingEventBus(eventBus),
                idFactory.IdImplementationType,
                eventFileName);

            JourneysApplication.Bootstrap(
                handlerRegistry,
                handlerDispatcher,
                eventBus,
                repositories,
                eventSourcingModule,
                idFactory,
                transaction);

            eventSourcingModule.ReplayEvents();
            eventSourcingModule.StoreNewEvents();

            var commandHandlerQueue = new HandlerQueue();
            var queryHandlerQueue = new HandlerQueue();
            QueryDispatcher = new AsyncQueryDispatcher(new AsyncHandlerScheduler(handlerRegistry, queryHandlerQueue));
            CommandDispatcher = new AsyncCommandDispatcher(new AsyncHandlerScheduler(handlerRegistry, commandHandlerQueue));

            var commandHandlerSource = new TrackingHandlerSource(new TransactedHandlerSource(commandHandlerQueue, transaction));
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
