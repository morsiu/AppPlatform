using System.Threading.Tasks;
using Mors.AppPlatform.Adapters.Dispatching;
using Mors.AppPlatform.Support.Dispatching;

namespace Mors.AppPlatform.Adapters.Services
{
    public sealed class AsyncQueryDispatcher
    {
        private readonly AsyncHandlerScheduler _handlerScheduler;

        public AsyncQueryDispatcher(AsyncHandlerScheduler handlerScheduler)
        {
            _handlerScheduler = handlerScheduler;
        }

        public Task<object> Dispatch(object querySpecification)
        {
            var key = new QueryKey(querySpecification.GetType());
            return _handlerScheduler.Schedule(key, querySpecification);
        }
    }
}
