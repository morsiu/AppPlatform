using Mors.AppPlatform.Support.Events;
using Mors.AppPlatform.Support.Transactions;
using Mors.AppPlatform.Adapters.Modules.Domain;

namespace Mors.AppPlatform.Adapters.Modules.Command
{
    internal sealed class CommandTransactedEventBus : IEventBus, ITransactional<IEventBus>
    {
        private readonly ITransactional<IEventBus> _eventBus;

        public CommandTransactedEventBus(Mors.AppPlatform.Support.Events.IEventBus eventBus)
        {
            _eventBus = eventBus.Lift();
        }

        public Journeys.Domain.Infrastructure.IEventBus ForDomain()
        {
            return new DomainEventBusAdapter(_eventBus.Object);
        }

        public ITransactional<IEventBus> Lift()
        {
            return this;
        }

        public IEventBus Object
        {
            get { return this; }
        }

        public void Abort()
        {
            _eventBus.Abort();
        }

        public void Commit()
        {
            _eventBus.Commit();
        }
    }
}
