using System;
using Mors.AppPlatform.Adapters.Dispatching;
using Mors.AppPlatform.Support.Dispatching;
using Mors.Journeys.Data;

namespace Mors.AppPlatform.Adapters.Journeys
{
    internal sealed class ClientWpfQueryHandlerRegistry : Mors.Journeys.Application.Client.Wpf.IQueryHandlerRegistry
    {
        private readonly IHandlerRegistry _handlerRegistry;

        public ClientWpfQueryHandlerRegistry(IHandlerRegistry handlerRegistry)
        {
            _handlerRegistry = handlerRegistry;
        }

        public void SetHandler<TQuery, TResult>(Func<TQuery, TResult> handler)
            where TQuery : IQuery<TResult>
        {
            var queryKey = QueryKey.From<TQuery, TResult>();
            Func<object, object> adaptedHandler = query => handler((TQuery)query);
            _handlerRegistry.Set(queryKey, adaptedHandler);
        }
    }
}
