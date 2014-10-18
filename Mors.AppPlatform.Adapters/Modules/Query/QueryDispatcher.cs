using System.Net.Mime.MediaTypeNames;
using Mors.AppPlatform.Support.Dispatching;
using Mors.AppPlatform.Adapters.Dispatching;
using Mors.AppPlatform.Common;

namespace Mors.AppPlatform.Adapters.Modules.Query
{
    public sealed class QueryDispatcher : Application.Query.IQueryDispatcher
    {
        private readonly HandlerDispatcher _handlerDispatcher;

        public QueryDispatcher(HandlerDispatcher handlerDispatcher)
        {
            _handlerDispatcher = handlerDispatcher;
        }

        public TResult Dispatch<TResult>(IQuery<TResult> querySpecification)
        {
            var query = new Query<TResult>(querySpecification);
            return query.Execute(_handlerDispatcher);
        }
    }
}
