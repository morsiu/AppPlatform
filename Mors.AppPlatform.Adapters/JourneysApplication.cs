using Mors.AppPlatform.Adapters.Journeys;

namespace Mors.AppPlatform.Adapters
{
    public static class JourneysApplication
    {
        public static void Bootstrap(
            Support.Dispatching.IHandlerRegistry handlerRegistry,
            Support.Dispatching.HandlerDispatcher handlerDispatcher,
            Support.Events.IEventBus eventBus,
            Support.Repositories.IRepositories repositories,
            Support.EventSourcing.Module eventSourcingModule,
            Support.Repositories.GuidIdFactory idFactory,
            Support.Transactions.Transaction transaction)
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
                new ApplicationCommandHandlerRegistry(handlerRegistry),
                new ApplicationRepositories(transaction.Register(repositories)),
                new ApplicationEventBus(transaction.Register(eventBus)).Publish,
                () => idFactory.Create(),
                new ApplicationQueryDispatcher(handlerDispatcher));
        }
    }
}
