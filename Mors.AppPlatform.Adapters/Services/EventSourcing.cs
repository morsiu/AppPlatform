using System;

namespace Mors.AppPlatform.Adapters.Services
{
    public sealed class EventSourcing : Common.Services.IEventSourcing
    {
        private readonly Support.EventSourcing.Module _eventSourcingModule;

        public EventSourcing(Support.EventSourcing.Module eventSourcingModule)
        {
            _eventSourcingModule = eventSourcingModule;
        }

        public void RegisterEventReplayer<TEvent>(Action<TEvent> eventReplayer)
        {
            _eventSourcingModule.Register(eventReplayer);
        }
    }
}
