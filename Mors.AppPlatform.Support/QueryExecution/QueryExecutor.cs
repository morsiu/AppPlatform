using System;
using Query = System.Object;
using QueryResult = System.Object;
using LogEntry = System.Object;

namespace Mors.AppPlatform.Support.QueryExecution
{
    using QueryHandler = Func<Query, QueryResult>;

    public sealed class QueryExecutor
    {
        private readonly Logger _logger;
        private readonly Func<LogEntry, QueryHandler> _queryHandlerRetriever;

        public QueryExecutor(
            Action<LogEntry> logger,
            Func<Query, QueryHandler> queryHandlerRetriever)
        {
            _logger = new Logger(logger);
            _queryHandlerRetriever = queryHandlerRetriever;
        }

        public QueryExecutionReport Execute(Query query)
        {
            QueryHandler queryHandler;
            if (!RetrieveQueryHandler(query, out queryHandler))
            {
                return QueryExecutionReport.CreateFailureReport();
            }

            QueryResult queryResult;
            if (!ExecuteQueryHandler(queryHandler, query, out queryResult))
            {
                return QueryExecutionReport.CreateFailureReport();
            }

            return QueryExecutionReport.CreateCompletionReport(queryResult);
        }

        private bool ExecuteQueryHandler(QueryHandler queryHandler, LogEntry query, out LogEntry queryResult)
        {
            try
            {
                queryResult = queryHandler(query);
                return true;
            }
            catch (Exception exception)
            {
                _logger.LogExceptionFromQueryHandler(query, exception);
                queryResult = null;
                return false;
            }
        }

        private bool RetrieveQueryHandler(LogEntry query, out QueryHandler queryHandler)
        {
            try
            {
                queryHandler = _queryHandlerRetriever(query);
            }
            catch (Exception exception)
            {
                _logger.LogExceptionFromQueryHandlerRetriever(query, exception);
                queryHandler = null;
                return false;
            }
            if (queryHandler == null)
            {
                _logger.LogMissingQueryHandler(query);
                return false;
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

            internal void LogExceptionFromQueryHandlerRetriever(Query query, Exception exception)
            {
                _logger(new { Origin = "QueryExecutor", Event = "Exception thrown from query handler retriever", Query = query, Exception = exception });
            }

            internal void LogMissingQueryHandler(Query query)
            {
                _logger(new { Origin = "QueryExecutor", Event = "Missing handler for query", Query = query });
            }

            internal void LogExceptionFromQueryHandler(LogEntry query, Exception exception)
            {
                _logger(new { Origin = "QueryExecutor", Event = "Exception thrown from query handler", Query = query, Exception = exception });
            }
        }
    }
}
