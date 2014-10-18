using System;
using Mors.AppPlatform.Support.Dispatching;
using Mors.AppPlatform.Service.Client;

namespace Mors.AppPlatform.Adapters.Modules.WpfClient
{
    public sealed class NetworkCommandDispatcher : Common.Services.ICommandDispatcher
    {
        private readonly HandlerDispatcher _handlerDispatcher;
        private readonly Uri _commandRequestUri;

        public NetworkCommandDispatcher(Uri commandRequestUri, HandlerDispatcher handlerDispatcher)
        {
            _handlerDispatcher = handlerDispatcher;
            _commandRequestUri = commandRequestUri;
        }

        public void Dispatch<TCommand>(TCommand command)
        {
            if (IsInternal(typeof(TCommand)))
            {
                DispatchInternal(command);
            }
            else
            {
                DispatchExternal(command);
            }
        }

        private static bool IsInternal(Type commandType)
        {
            return !commandType.IsPublic;
        }

        private void DispatchExternal(object command)
        {
            var request = new CommandRequest(_commandRequestUri, command);
            request.Run();
        }

        private void DispatchInternal(object command)
        {
            var commandAdapter = new Dispatching.Command(command);
            commandAdapter.Execute(_handlerDispatcher);
        }
    }
}
