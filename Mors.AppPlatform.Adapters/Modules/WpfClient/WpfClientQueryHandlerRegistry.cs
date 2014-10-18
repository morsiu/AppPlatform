using System;
using Mors.AppPlatform.Support.Dispatching;
using Mors.AppPlatform.Adapters.Dispatching;
using Mors.AppPlatform.Common;

namespace Mors.AppPlatform.Adapters.Modules.WpfClient
{
    public sealed class WpfClientQueryHandlerRegistry : IQueryHandlerRegistry
    {
        private readonly HandlerRegistry _handlerRegistry;

        public WpfClientQueryHandlerRegistry(HandlerRegistry handlerRegistry)
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
