using System;
using Mors.AppPlatform.Adapters.Dispatching;
using Mors.AppPlatform.Support.Dispatching;

namespace Mors.AppPlatform.Adapters.Journeys
{
    internal sealed class ApplicationCommandHandlerRegistry : Mors.Journeys.Application.ICommandHandlerRegistry
    {
        private readonly IHandlerRegistry _handlerRegistry;

        public ApplicationCommandHandlerRegistry(IHandlerRegistry handlerRegistry)
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
