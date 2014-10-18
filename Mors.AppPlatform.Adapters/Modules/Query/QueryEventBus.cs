using System;

namespace Mors.AppPlatform.Adapters.Modules.Query
{
    public sealed class QueryEventBus : Application.Query.IEventBus
    {
        private readonly Mors.AppPlatform.Support.Events.IEventBus _eventBus;

        public QueryEventBus(Mors.AppPlatform.Support.Events.IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public void RegisterListener<TEvent>(Action<TEvent> handler)
        {
            _eventBus.RegisterListener(new Mors.AppPlatform.Support.Events.EventListener<TEvent>(handler));
        }
    }
}
