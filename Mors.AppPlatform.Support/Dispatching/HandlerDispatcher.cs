using System;
using Mors.AppPlatform.Support.Dispatching.Exceptions;

namespace Mors.AppPlatform.Support.Dispatching
{
    public sealed class HandlerDispatcher
    {
        private readonly IHandlerRegistry _registry;

        public HandlerDispatcher(IHandlerRegistry registry)
        {
            _registry = registry;
        }

        public object Dispatch(object key, object parameter)
        {
            Func<object, object> handler;
            if (_registry.Retrieve(key, out handler))
            {
                return handler(parameter);
            }
            else
            {
                throw new HandlerNotFoundException();
            }
        }
    }
}
