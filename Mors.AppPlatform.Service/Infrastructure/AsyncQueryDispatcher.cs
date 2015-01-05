using System.Threading.Tasks;
using Mors.AppPlatform.Adapters.Dispatching;
using Mors.AppPlatform.Support.Dispatching;

namespace Mors.AppPlatform.Service.Infrastructure
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
            var query = new Query(querySpecification);
            return query.Schedule(_handlerScheduler);
        }
    }
}
