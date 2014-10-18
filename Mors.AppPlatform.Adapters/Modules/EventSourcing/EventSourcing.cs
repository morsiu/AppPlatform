using System;

namespace Mors.AppPlatform.Adapters.Modules.EventSourcing
{
    public sealed class EventSourcing : IEventSourcing
    {
        private readonly Mors.AppPlatform.Support.EventSourcing.Module _eventSourcingModule;

        public EventSourcing(Mors.AppPlatform.Support.EventSourcing.Module eventSourcingModule)
        {
            _eventSourcingModule = eventSourcingModule;
        }

        public void RegisterEventReplayer<TEvent>(Action<TEvent> eventReplayer)
        {
            _eventSourcingModule.Register(eventReplayer);
        }
    }
}
