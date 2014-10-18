using System;
using Mors.AppPlatform.Support.Dispatching;
using Mors.AppPlatform.Adapters.Dispatching;

namespace Mors.AppPlatform.Adapters.Modules.WpfClient
{
    public sealed class WpfClientCommandHandlerRegistry : Application.Client.Wpf.ICommandHandlerRegistry
    {
        private readonly HandlerRegistry _handlerRegistry;

        public WpfClientCommandHandlerRegistry(HandlerRegistry handlerRegistry)
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
