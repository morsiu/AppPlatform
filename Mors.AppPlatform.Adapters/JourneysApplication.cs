using Mors.AppPlatform.Adapters.Journeys;
using Mors.AppPlatform.Support.Dispatching;
using Mors.AppPlatform.Support.Events;
using Mors.AppPlatform.Support.EventSourcing;
using Mors.AppPlatform.Support.Repositories;
using Mors.AppPlatform.Support.Transactions;

namespace Mors.AppPlatform.Adapters
{
    public static class JourneysApplication
    {
        public static void Bootstrap(
            IHandlerRegistry handlerRegistry,
            HandlerDispatcher handlerDispatcher,
            IEventBus eventBus,
            IRepositories repositories,
            EventSourcingModule eventSourcingModule,
            GuidIdFactory idFactory,
            Transaction transaction)
        {
            var bootstrapper = new Mors.Journeys.Application.Bootstrapper();
            bootstrapper.BootstrapEventSourcing(
                new ApplicationEventSourcing(eventSourcingModule),
                new ApplicationRepositories(repositories),
                new ApplicationEventBus(eventBus).Publish);
            bootstrapper.BootstrapQueries(
                new ApplicationQueryHandlerRegistry(handlerRegistry),
                new ApplicationEventBus(eventBus),
                new ApplicationQueryDispatcher(handlerDispatcher));
            bootstrapper.BootstrapCommands(
                new ApplicationCommandHandlerRegistry(handlerRegistry, transaction),
                new ApplicationRepositories(transaction.Register(repositories)),
                new ApplicationEventBus(transaction.Register(eventBus)).Publish,
                () => idFactory.Create(),
                new ApplicationQueryDispatcher(handlerDispatcher));
        }
    }
}
