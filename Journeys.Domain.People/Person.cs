﻿using Journeys.Common;
using Journeys.Domain.Infrastructure;
using Journeys.Domain.Infrastructure.Exceptions;
using Journeys.Domain.Infrastructure.Markers;
using Journeys.Events;

namespace Journeys.Domain.People
{
    [Aggregate]
    public sealed class Person : IHasId
    {
        private readonly IId _id;

        public Person(IId id, string name, IEventBus eventBus)
        {
            if (string.IsNullOrEmpty(name))
                throw new InvariantViolationException(Messages.PersonMustHaveAName);
            _id = id;
            eventBus.Publish(new PersonCreatedEvent(id, name));
        }

        IId IHasId.Id
        {
            get { return _id; }
        }
    }
}
