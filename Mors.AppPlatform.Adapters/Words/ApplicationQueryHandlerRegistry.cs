using System;
using Mors.AppPlatform.Adapters.Dispatching;
using Mors.AppPlatform.Support.Dispatching;

namespace Mors.AppPlatform.Adapters.Words
{
    internal sealed class ApplicationQueryHandlerRegistry
    {
        private readonly IHandlerRegistry _handlerRegistry;

        public ApplicationQueryHandlerRegistry(IHandlerRegistry handlerRegistry)
        {
            _handlerRegistry = handlerRegistry;
        }

        public void Register(Type queryType, Func<object, object> handler)
        {
            var queryKey = new QueryKey(queryType);
            _handlerRegistry.Set(
                queryKey,
                query =>
                {
                    var result = handler(query);
                    return result;
                });
        }
    }
}
