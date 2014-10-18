using System.Threading.Tasks;
using Mors.AppPlatform.Support.Dispatching;

namespace Mors.AppPlatform.Adapters.Modules.Service
{
    public sealed class AsyncCommandDispatcher
    {
        private readonly AsyncHandlerDispatcher _handlerDispatcher;

        public AsyncCommandDispatcher(AsyncHandlerDispatcher handlerDispatcher)
        {
            _handlerDispatcher = handlerDispatcher;
        }

        public Task Dispatch<TCommand>(TCommand commandSpecification)
        {
            var command = new Dispatching.AsyncCommand(commandSpecification);
            return command.Execute(_handlerDispatcher);
        }
    }
}
