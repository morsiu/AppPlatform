using System;
using Mors.AppPlatform.Support.Dispatching;
using Mors.AppPlatform.Support.Dispatching.Exceptions;
using Mors.AppPlatform.Adapters.Messages;
using System.Threading.Tasks;

namespace Mors.AppPlatform.Adapters.Dispatching
{
    public sealed class Command
    {
        private readonly object _commandSpecification;

        public Command(object commandSpecification)
        {
            _commandSpecification = commandSpecification;
        }

        public void Dispatch(HandlerDispatcher dispatcher)
        {
            var commandKey = CommandKey.From(_commandSpecification);
            try
            {
                dispatcher.Dispatch(commandKey, _commandSpecification);
            }
            catch (HandlerNotFoundException)
            {
                throw new InvalidOperationException(string.Format(FailureMessages.NoHandlerRegisteredForCommandOfType, commandKey));
            }
        }

        public Task Schedule(AsyncHandlerScheduler scheduler)
        {
            var commandKey = CommandKey.From(_commandSpecification);
            try
            {
                return scheduler.Schedule(commandKey, _commandSpecification);
            }
            catch (HandlerNotFoundException)
            {
                throw new InvalidOperationException(string.Format(FailureMessages.NoHandlerRegisteredForCommandOfType, commandKey));
            }
        }
    }
}
