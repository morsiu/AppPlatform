using System;
using Mors.AppPlatform.Adapters.Dispatching;
using Mors.AppPlatform.Common;

namespace Mors.AppPlatform.Adapters.Modules.Query
{
    public sealed class QueryHandlerRegistry : IQueryHandlerRegistry
    {
        private readonly Mors.AppPlatform.Support.Dispatching.HandlerRegistry _handlerRegistry;

        public QueryHandlerRegistry(Mors.AppPlatform.Support.Dispatching.HandlerRegistry handlerRegistry)
        {
            _handlerRegistry = handlerRegistry;
        }

        public void SetHandler<TQuery, TResult>(QueryHandler<TQuery, TResult> handler)
            where TQuery : IQuery<TResult>
        {
            var queryKey = QueryKey.From<TQuery, TResult>();
            Func<object, object> adaptedHandler = query => handler((TQuery)query);
            _handlerRegistry.Set(queryKey, adaptedHandler);
        }
    }
}
