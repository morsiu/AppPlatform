using System.Threading.Tasks;
using Mors.AppPlatform.Support.Dispatching;
using Mors.AppPlatform.Adapters.Dispatching;

namespace Mors.AppPlatform.Adapters.Modules.Service
{
    public sealed class AsyncQueryDispatcher
    {
        private readonly AsyncHandlerDispatcher _handlerDispatcher;

        public AsyncQueryDispatcher(AsyncHandlerDispatcher handlerDispatcher)
        {
            _handlerDispatcher = handlerDispatcher;
        }

        public Task<object> Dispatch(object querySpecification)
        {
            var key = new QueryKey(querySpecification.GetType());
            return _handlerDispatcher.Dispatch(key, querySpecification);
        }
    }
}
