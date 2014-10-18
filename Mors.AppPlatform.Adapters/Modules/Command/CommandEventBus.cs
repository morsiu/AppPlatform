using Mors.AppPlatform.Support.Events;
using Mors.AppPlatform.Adapters.Modules.Domain;
using Mors.AppPlatform.Common.Transactions;

namespace Mors.AppPlatform.Adapters.Modules.Command
{
    public sealed class CommandEventBus : IEventBus
    {
        private readonly Mors.AppPlatform.Support.Events.IEventBus _eventBus;

        public CommandEventBus(Mors.AppPlatform.Support.Events.IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public Journeys.Domain.Infrastructure.IEventBus ForDomain()
        {
            return new DomainEventBusAdapter(_eventBus);
        }

        public ITransactional<IEventBus> Lift()
        {
            return new CommandTransactedEventBus(_eventBus);
        }
    }
}
