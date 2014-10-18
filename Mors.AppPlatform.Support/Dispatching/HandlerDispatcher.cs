using System;
using Mors.AppPlatform.Support.Dispatching.Exceptions;

namespace Mors.AppPlatform.Support.Dispatching
{
    public sealed class HandlerDispatcher
    {
        private readonly HandlerRegistry _registry;

        public HandlerDispatcher(HandlerRegistry registry)
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
