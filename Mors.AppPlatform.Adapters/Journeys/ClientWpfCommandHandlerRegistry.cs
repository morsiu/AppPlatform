using System;
using Mors.AppPlatform.Adapters.Dispatching;
using Mors.AppPlatform.Support.Dispatching;

namespace Mors.AppPlatform.Adapters.Journeys
{
    internal sealed class ClientWpfCommandHandlerRegistry : Mors.Journeys.Application.Client.Wpf.ICommandHandlerRegistry
    {
        private readonly IHandlerRegistry _handlerRegistry;

        public ClientWpfCommandHandlerRegistry(IHandlerRegistry handlerRegistry)
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
