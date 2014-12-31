using System;
using System.Threading.Tasks;
using Mors.AppPlatform.Support.Dispatching.Exceptions;

namespace Mors.AppPlatform.Support.Dispatching
{
    public sealed class AsyncHandlerScheduler
    {
        private readonly IHandlerSink _sink;
        private readonly IHandlerRegistry _registry;

        public AsyncHandlerScheduler(IHandlerRegistry registry, IHandlerSink sink)
        {
            _registry = registry;
            _sink = sink;
        }

        public Task<object> Schedule(object key, object parameter)
        {
            Func<object, object> handler;
            if (_registry.Retrieve(key, out handler))
            {
                var resultSource = new TaskCompletionSource<object>();
                _sink.Enqueue(
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
