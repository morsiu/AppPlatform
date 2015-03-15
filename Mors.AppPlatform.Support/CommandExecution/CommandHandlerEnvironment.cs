using System;
using System.Collections.Generic;
using Event = System.Object;
using AggregateType = System.Type;
using Aggregate = System.Object;
using AggregateId = System.Object;
using Query = System.Object;
using QueryResult = System.Object;
using LogEntry = System.Object;

namespace Mors.AppPlatform.Support.CommandExecution
{
    using AggregateRetriever = Func<AggregateType, AggregateId, Aggregate>;
    using AggregateStorer = Action<AggregateType, AggregateId, Aggregate>;
    using QueryDispatcher = Func<Query, QueryResult>;
    using EventDispatcher = Action<Event>;
    using EventsStorer = Action<IReadOnlyList<Event>>;
    using AggregateKey = Tuple<AggregateType, AggregateId>;

    public interface ICommandHandlerEnvironment
    {
        void QueueEvent(Event @event);
        Aggregate RetrieveAggregate(AggregateType aggregateType, AggregateId aggregateId);
        void StoreAggregate(AggregateType aggregateType, AggregateId aggregateId, Aggregate aggregate);
        QueryResult DispatchQuery(Query query);
    }

    public sealed class CommandHandlerEnvironment : ICommandHandlerEnvironment
    {
        private readonly List<Event> _queuedEvents = new List<Event>();
        private readonly Dictionary<AggregateKey, Aggregate> _storedAggregates = new Dictionary<AggregateKey, Aggregate>();
        private readonly AggregateRetriever _aggregateRetriever;
        private readonly QueryDispatcher _queryDispatcher;
        private readonly Logger _logger;
        private readonly EventsStorer _eventsStorer;
        private readonly EventDispatcher _eventDispatcher;
        private readonly AggregateStorer _aggregateStorer;

        public CommandHandlerEnvironment(
            Action<LogEntry> logger,
            AggregateRetriever aggregateRetriever,
            QueryDispatcher queryDispatcher,
            AggregateStorer aggregateStorer,
            EventDispatcher eventDispatcher,
            EventsStorer eventsStorer)
        {
            _logger = new Logger(logger);
            _aggregateRetriever = aggregateRetriever;
            _queryDispatcher = queryDispatcher;
            _aggregateStorer = aggregateStorer;
            _eventDispatcher = eventDispatcher;
            _eventsStorer = eventsStorer;
        }

        public void QueueEvent(Event @event)
        {
            _queuedEvents.Add(@event);
        }

        public Aggregate RetrieveAggregate(AggregateType aggregateType, AggregateId aggregateId)
        {
            Aggregate aggregate;
            if (_storedAggregates.TryGetValue(new AggregateKey(aggregateType, aggregateId), out aggregate))
            {
                return aggregate;
            }
            return _aggregateRetriever(aggregateType, aggregateId);
        }

        public void StoreAggregate(AggregateType aggregateType, AggregateId aggregateId, Aggregate aggregate)
        {
            _storedAggregates[new AggregateKey(aggregateType, aggregateId)] = aggregate;
        }

        public QueryResult DispatchQuery(Query query)
        {
            return _queryDispatcher(query);
        }

        public CommandCommitResult Commit()
        {
            if (!StoreQueuedEvents())
            {
                return CommandCommitResult.EventsStoreFailed;
            }

            if (!StoreAggregates())
            {
                return CommandCommitResult.AggregatesStoreFailed;
            }

            if (!DispatchQueuedEvents())
            {
                return CommandCommitResult.CommitCompleteButSomeEventsPublishFailed;
            }

            return CommandCommitResult.CommitComplete;
        }

        public bool StoreQueuedEvents()
        {
            try
            {
                _eventsStorer(_queuedEvents);
                return true;
            }
            catch (Exception exception)
            {
                _logger.LogExceptionFromEventsStorer(exception);
                return false;
            }
        }

        private bool DispatchQueuedEvents()
        {
            var anyEventDispatchFailed = false;
            foreach (var queuedEvent in _queuedEvents)
            {
                try
                {
                    _eventDispatcher(queuedEvent);
                }
                catch (Exception exception)
                {
                    _logger.LogExceptionFromEventDispatcher(exception, queuedEvent);
                    anyEventDispatchFailed = true;
                }
            }
            return !anyEventDispatchFailed;
        }

        private bool StoreAggregates()
        {
            foreach (var aggregateEntry in _storedAggregates)
            {
                var aggregateType = aggregateEntry.Key.Item1;
                var aggregateId = aggregateEntry.Key.Item2;
                var aggregate = aggregateEntry.Value;
                try
                {
                    _aggregateStorer(aggregateType, aggregateId, aggregate);
                }
                catch (Exception exception)
                {
                    _logger.LogExceptionFromAggregateStorer(exception, aggregate, aggregateId, aggregateType);
                    return false;
                }
            }
            return true;
        }

        private sealed class Logger
        {
            private readonly Action<LogEntry> _logger;

            public Logger(Action<LogEntry> logger)
            {
                _logger = logger;
            }

            internal void LogExceptionFromEventsStorer(Exception exception)
            {
                _logger(new { Origin = "CommandHandlerEnvironment", Event = "Store of events generated by handler failed", Exception = exception });
            }

            internal void LogExceptionFromAggregateStorer(Exception exception, Aggregate aggregate, AggregateId aggregateId, AggregateType aggregateType)
            {
                _logger(new { Origin = "CommandHandlerEnvironment", Event = "Store of aggregate stored by command handler failed", Exception = exception, AggregateType = aggregateType, AggregateId = aggregateId });
            }

            internal void LogExceptionFromEventDispatcher(Exception exception, Event queuedEvent)
            {
                _logger(new { Origin = "CommandHandlerEnvironment", Event = "Dispatch of event queued by command handler failed", Exception = exception, QueuedEvent = queuedEvent });
            }
        }
    }
}
