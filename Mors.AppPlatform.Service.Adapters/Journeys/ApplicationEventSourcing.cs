using System;
using Mors.AppPlatform.Support.EventSourcing;

namespace Mors.AppPlatform.Service.Adapters.Journeys;

internal sealed class ApplicationEventSourcing : Mors.Journeys.Application.IEventSourcing
{
    private readonly EventSourcingModule _eventSourcingModule;

    public ApplicationEventSourcing(EventSourcingModule eventSourcingModule)
    {
        _eventSourcingModule = eventSourcingModule;
    }

    public void RegisterEventReplayer<TEvent>(Action<TEvent> eventReplayer)
    {
        _eventSourcingModule.Register(eventReplayer);
    }
}