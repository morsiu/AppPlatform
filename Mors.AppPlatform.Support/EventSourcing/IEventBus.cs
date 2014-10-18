using System;
using Mors.AppPlatform.Support.Transactions;

namespace Mors.AppPlatform.Support.EventSourcing
{
    public interface IEventBus : IProvideTransactional<IEventBus>
    {
        void RegisterListener<TEvent>(Action<TEvent> handler);
    }
}
