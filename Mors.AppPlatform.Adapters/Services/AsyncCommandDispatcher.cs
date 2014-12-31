using System.Threading.Tasks;
using Mors.AppPlatform.Support.Dispatching;

namespace Mors.AppPlatform.Adapters.Services
{
    public sealed class AsyncCommandDispatcher
    {
        private readonly AsyncHandlerScheduler _handlerScheduler;

        public AsyncCommandDispatcher(AsyncHandlerScheduler handlerScheduler)
        {
            _handlerScheduler = handlerScheduler;
        }

        public Task Dispatch(object commandSpecification)
        {
            var command = new Dispatching.Command(commandSpecification);
            return command.Schedule(_handlerScheduler);
        }
    }
}
