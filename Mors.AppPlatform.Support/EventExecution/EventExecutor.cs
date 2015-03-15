using System;
using System.Collections.Generic;
using Event = System.Object;
using LogEntry = System.Object;
using EventHandler = System.Action<System.Object>;

namespace Mors.AppPlatform.Support.EventExecution
{
    using EventHandlerList = IReadOnlyCollection<EventHandler>;

    public sealed class EventExecutor
    {
        private readonly Logger _logger;
        private readonly Func<Event, EventHandlerList> _eventHandlersRetriever;

        public EventExecutor(
            Action<LogEntry> logger,
            Func<Event, EventHandlerList> eventHandlersRetriever)
        {
            _logger = new Logger(logger);
            _eventHandlersRetriever = eventHandlersRetriever;
        }

        public bool Execute(Event @event)
        {
            EventHandlerList eventHandlers;
            if (!RetrieveEventHandlers(@event, out eventHandlers))
            {
                return false;
            }

            if (!ExecuteEventHandlers(@event, eventHandlers))
            {
                return false;
            }

            return true;
        }

        private bool ExecuteEventHandlers(Event @event, EventHandlerList eventHandlers)
        {
            var anyEventHandlerFailed = false;
            foreach (var eventHandler in eventHandlers)
            {
                try
                {
                    eventHandler(@event);
                }
                catch (Exception exception)
                {
                    _logger.LogExceptionFromEventHandler(@event, eventHandler, exception);
                    anyEventHandlerFailed = true;
                }
            }
            return !anyEventHandlerFailed;
        }

        private bool RetrieveEventHandlers(LogEntry @event, out EventHandlerList eventHandlers)
        {
            try
            {
                eventHandlers = _eventHandlersRetriever(@event);
                return true;
            }
            catch (Exception exception)
            {
                _logger.LogExceptionFromEventHandlersRetriever(@event, exception);
                eventHandlers = null;
                return false;
            }
        }

        private class Logger 
        {
            private readonly Action<LogEntry> _logger;

            public Logger(EventHandler logger)
            {
                _logger = logger;
            }

            public void LogExceptionFromEventHandlersRetriever(Event @event, Exception exception)
            {
                _logger(new { Origin = "EventExecutor", Event = "Exception thrown from event handlers retriever", HandledEvent = @event, Exception = exception });
            }

            public void LogExceptionFromEventHandler(Event @event, EventHandler eventHandler, Exception exception)
            {
                _logger(new { Origin = "EventExecutor", Event = "Exception thrown from event handler", HandledEvent = @event, EventHandler = eventHandler, Exception = exception });
            }
        }
    }
}
