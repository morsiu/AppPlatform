using System;
using Mors.AppPlatform.Adapters.Services;
using Mors.AppPlatform.Support.Dispatching;
using Mors.AppPlatform.Support.Repositories;

namespace Mors.AppPlatform.Client
{
    internal sealed class Bootstrapper
    {
        public void Bootstrap()
        {
            var eventBus = new Support.Events.EventBus();
            var idFactory = new GuidIdFactory();
            var handlerRegistry = new HandlerRegistry();
            var handlerDispatcher = new HandlerDispatcher(handlerRegistry);

            var wpfClientBootstrapper = new Journeys.Application.Client.Wpf.Bootstrapper(
                new EventBus(eventBus),
                new NetworkCommandDispatcher(new Uri("http://localhost:65363/api/command"), handlerDispatcher),
                new CommandHandlerRegistry(handlerRegistry),
                new NetworkQueryDispatcher(new Uri("http://localhost:65363/api/query"), handlerDispatcher),
                new QueryHandlerRegistry(handlerRegistry),
                new IdFactory(idFactory));
            wpfClientBootstrapper.Bootstrap();
            wpfClientBootstrapper.Run();
        }
    }
}
