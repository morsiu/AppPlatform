using System;
using System.Collections.Generic;
using Mors.AppPlatform.Support.CommandExecution;
using Event = System.Object;
using AggregateType = System.Type;
using Aggregate = System.Object;
using AggregateId = System.Object;
using Query = System.Object;
using QueryResult = System.Object;
using LogEntry = System.Object;


namespace Mors.AppPlatform.Support.Test.CommandExecution
{
    using AggregateRetriever = Func<AggregateType, AggregateId, Aggregate>;
    using AggregateStorer = Action<AggregateType, AggregateId, Aggregate>;
    using QueryDispatcher = Func<Query, QueryResult>;
    using EventDispatcher = Action<Event>;
    using EventsStorer = Action<IReadOnlyList<Event>>;
    using AggregateKey = Tuple<AggregateType, AggregateId>;

    public static class CommandHandlerEnvironmentFactory
    {
        public static CommandHandlerEnvironment Create(
            AggregateStorer aggregateStorer = null,
            EventDispatcher eventDispatcher = null)
        {
            return new CommandHandlerEnvironment(
                logEntry => { },
                (aggregateType, aggregateId) => { throw new NotImplementedException(); },
                (query) => { throw new NotImplementedException(); },
                aggregateStorer ?? ((aggregateType, aggregateId, aggregate) => { }),
                eventDispatcher ?? ((@event) => { }),
                (events) => { });
        }
    }
}
