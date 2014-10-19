﻿using System;
using Mors.AppPlatform.Common.Transactions;
using Mors.AppPlatform.Support.Events;

namespace Mors.AppPlatform.Adapters.Services
{
    public sealed class EventSourcingEventBus : Support.EventSourcing.IEventBus
    {
        private readonly IEventBus _eventBus;

        public EventSourcingEventBus(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public void RegisterListener<TEvent>(Action<TEvent> handler)
        {
            _eventBus.RegisterListener(new EventListener<TEvent>(handler));
        }

        ITransactional<Support.EventSourcing.IEventBus> IProvideTransactional<Support.EventSourcing.IEventBus>.Lift()
        {
            return new EventSourcingTransactedEventBus(_eventBus);
        }
    }
}