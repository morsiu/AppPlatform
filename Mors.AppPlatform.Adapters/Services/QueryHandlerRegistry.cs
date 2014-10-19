using System;
using Mors.AppPlatform.Adapters.Dispatching;
using Mors.AppPlatform.Common;
using Mors.AppPlatform.Support.Dispatching;

namespace Mors.AppPlatform.Adapters.Services
{
    public sealed class QueryHandlerRegistry : Common.Services.IQueryHandlerRegistry
    {
        private readonly HandlerRegistry _handlerRegistry;

        public QueryHandlerRegistry(HandlerRegistry handlerRegistry)
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
