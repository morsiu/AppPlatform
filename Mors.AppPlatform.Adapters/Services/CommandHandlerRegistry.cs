using System;
using Mors.AppPlatform.Adapters.Dispatching;
using Mors.AppPlatform.Common.Services;
using Mors.AppPlatform.Support.Dispatching;

namespace Mors.AppPlatform.Adapters.Services
{
    public sealed class CommandHandlerRegistry : ICommandHandlerRegistry
    {
        private readonly IHandlerRegistry _handlerRegistry;

        public CommandHandlerRegistry(IHandlerRegistry handlerRegistry)
        {
            _handlerRegistry = handlerRegistry;
        }

        public void SetHandler<TCommand>(Action<TCommand> handler)
        {
            var commandKey = CommandKey.From<TCommand>();
            _handlerRegistry.Set(commandKey, command => { handler((TCommand)command); return null; });
        }
    }
}
