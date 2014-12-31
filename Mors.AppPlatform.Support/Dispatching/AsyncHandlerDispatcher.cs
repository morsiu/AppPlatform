using System;
using System.Threading.Tasks;
using Mors.AppPlatform.Support.Dispatching.Exceptions;

namespace Mors.AppPlatform.Support.Dispatching
{
    public sealed class AsyncHandlerDispatcher
    {
        private readonly IHandlerQueue _queue;
        private readonly IHandlerRegistry _registry;

        public AsyncHandlerDispatcher(IHandlerRegistry registry, IHandlerQueue queue)
        {
            _registry = registry;
            _queue = queue;
        }

        public Task<object> Dispatch(object key, object parameter)
        {
            Func<object, object> handler;
            if (_registry.Retrieve(key, out handler))
            {
                var resultSource = new TaskCompletionSource<object>();
                _queue.Enqueue(
                    () =>
                    {
                        try
                        {
                            resultSource.SetResult(handler(parameter));
                        }
                        catch (Exception e)
                        {
                            resultSource.SetException(e);
                        }
                    });
                return resultSource.Task;
            }
            else
            {
                throw new HandlerNotFoundException();
            }
        }
    }
}
