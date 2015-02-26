using Mors.AppPlatform.Adapters.Words;
using Mors.AppPlatform.Support.Dispatching;
using Mors.AppPlatform.Support.Events;
using Mors.AppPlatform.Support.EventSourcing;
using Mors.AppPlatform.Support.Transactions;

namespace Mors.AppPlatform.Adapters
{
    public static class WordsApplication
    {
        public static void Bootstrap(
            IHandlerRegistry handlerRegistry,
            IEventBus eventBus,
            Transaction transaction,
            EventSourcingModule eventSourcingModule)
        {
            var bootstrapper = new Mors.Words.Bootstrapper();
            bootstrapper.BootstrapCommands(
                new ApplicationCommandHandlerRegistry(handlerRegistry, transaction, new ApplicationEventBus(eventBus)).Register);
        }
    }
}
