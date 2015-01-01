using System;

namespace Mors.AppPlatform.Adapters.Journeys
{
    internal sealed class ApplicationEventSourcing : Mors.Journeys.Application.IEventSourcing
    {
        private readonly Support.EventSourcing.Module _eventSourcingModule;

        public ApplicationEventSourcing(Support.EventSourcing.Module eventSourcingModule)
        {
            _eventSourcingModule = eventSourcingModule;
        }

        public void RegisterEventReplayer<TEvent>(Action<TEvent> eventReplayer)
        {
            _eventSourcingModule.Register(eventReplayer);
        }
    }
}
