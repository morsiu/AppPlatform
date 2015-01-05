using System;
using Mors.AppPlatform.Support.Transactions;

namespace Mors.AppPlatform.Support.EventSourcing.Dependencies
{
    public interface IEventBus
    {
        void RegisterListener<TEvent>(Action<TEvent> handler);
    }
}
