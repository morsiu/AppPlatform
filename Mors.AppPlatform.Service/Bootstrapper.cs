using Mors.AppPlatform.Support.Dispatching;
using Mors.AppPlatform.Support.Repositories;
using Mors.AppPlatform.Adapters.Dispatching;
using Mors.AppPlatform.Adapters.Services;
using Repositories = Mors.AppPlatform.Adapters.Services.Repositories;
using Mors.AppPlatform.Support.Transactions;

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
            var transaction = new Transaction();

            var eventSourcingModule = new Support.EventSourcing.Module(
                new EventSourcingEventBus(eventBus),
                idFactory.IdImplementationType,
                eventFileName);

            var journeysBootstrapper = new Journeys.Application.Bootstrapper();
            journeysBootstrapper.BootstrapEventSourcing(
                new EventSourcing(eventSourcingModule),
                new Repositories(repositories),
                new EventBus(eventBus));
            journeysBootstrapper.BootstrapQueries(
                new QueryHandlerRegistry(handlerRegistry),
                new EventBus(transaction.Register(eventBus)),
                new QueryDispatcher(handlerDispatcher));
            journeysBootstrapper.BootstrapCommands(
                new CommandHandlerRegistry(handlerRegistry),
                new Repositories(transaction.Register(repositories)),
                new EventBus(transaction.Register(eventBus)),
                new IdFactory(idFactory),
                new QueryDispatcher(handlerDispatcher));

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
