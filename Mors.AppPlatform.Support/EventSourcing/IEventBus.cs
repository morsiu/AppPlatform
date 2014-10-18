using System;
using Mors.AppPlatform.Common.Transactions;

namespace Mors.AppPlatform.Support.EventSourcing
{
    public interface IEventBus : IProvideTransactional<IEventBus>
    {
        void RegisterListener<TEvent>(Action<TEvent> handler);
    }
}
