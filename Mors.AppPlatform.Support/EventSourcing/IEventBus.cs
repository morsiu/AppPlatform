using System;
using Mors.AppPlatform.Support.Transactions;

namespace Mors.AppPlatform.Support.EventSourcing
{
    public interface IEventBus
    {
        void RegisterListener<TEvent>(Action<TEvent> handler);
    }
}
