namespace Mors.AppPlatform.Support.QueryExecution
{
    public sealed class QueryExecutionReport
    {
        private readonly bool _queryComplete;
        private readonly object _queryResult;

        public static QueryExecutionReport CreateCompletionReport(object queryResult)
        {
            return new QueryExecutionReport(true, queryResult);
        }

        public static QueryExecutionReport CreateFailureReport()
        {
            return new QueryExecutionReport(false, null);
        }

        private QueryExecutionReport(bool queryComplete, object queryResult)
        {
            _queryComplete = queryComplete;
            _queryResult = queryResult;
        }

        public bool QueryComplete { get { return _queryComplete; } }

        public object QueryResult { get { return _queryResult; } }
    }
}
