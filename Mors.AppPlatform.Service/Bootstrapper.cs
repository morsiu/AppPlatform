using Mors.AppPlatform.Support.Dispatching;
using Mors.AppPlatform.Support.Repositories;
using Mors.AppPlatform.Adapters.Dispatching;
using Mors.AppPlatform.Adapters.Modules.Command;
using Mors.AppPlatform.Adapters.Modules.EventSourcing;
using Mors.AppPlatform.Adapters.Modules.Query;
using Mors.AppPlatform.Adapters.Modules.Service;

namespace Mors.AppPlatform.Service
{
    internal sealed class Bootstrapper
    {
        private HandlerScheduler _handlerScheduler;

        public ServiceQueryDispatcher QueryDispatcher { get; private set; }

        public ServiceCommandDispatcher CommandDispatcher { get; private set; }

        public void Bootstrap(string eventFileName)
        {
            var eventBus = new Mors.AppPlatform.Support.Events.EventBus();
            var idFactory = new GuidIdFactory();
            var handlerRegistry = new HandlerRegistry();
            var handlerDispatcher = new HandlerDispatcher(handlerRegistry);
            
            var queryBootstrapper = new Application.Query.Bootstrapper(
                new QueryEventBus(eventBus),
                new QueryDispatcher(handlerDispatcher),
                new QueryHandlerRegistry(handlerRegistry));
            queryBootstrapper.Bootstrap();

            var repositories = new Repositories();

            var eventSourcingModule = new Mors.AppPlatform.Support.EventSourcing.Module(
                new EventSourcingModuleEventBus(eventBus),
                idFactory.IdImplementationType,
                eventFileName);

            var commandBootstrapper = new Application.Command.Bootstrapper(
                new CommandEventBus(eventBus),
                new CommandRepositories(repositories),
                new CommandIdFactory(idFactory),
                new CommandHandlerRegistry(handlerRegistry),
                new CommandQueryDispatcher(handlerDispatcher));
            commandBootstrapper.Bootstrap();

            var eventSourcingBootstrapper = new Application.EventSourcing.Bootstrapper(
                new EventSourcingEventBus(eventBus),
                new EventSourcingRepositories(repositories),
                new EventSourcingIdFactory(idFactory),
                new EventSourcingQueryDispatcher(handlerDispatcher),
                new EventSourcing(eventSourcingModule));
            eventSourcingBootstrapper.Bootstrap();

            eventSourcingModule.ReplayEvents();
            eventSourcingModule.StoreNewEvents();

            var commandHandlerQueue = new HandlerQueue();
            var queryHandlerQueue = new HandlerQueue();
            QueryDispatcher = new ServiceQueryDispatcher(new AsyncHandlerDispatcher(handlerRegistry, queryHandlerQueue));
            CommandDispatcher = new ServiceCommandDispatcher(new AsyncHandlerDispatcher(handlerRegistry, commandHandlerQueue));
            _handlerScheduler = new HandlerScheduler(commandHandlerQueue, queryHandlerQueue);
        }

        public void RunScheduledHandlers()
        {
            _handlerScheduler.Run();
        }
    }
}
