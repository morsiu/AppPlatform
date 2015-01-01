using System;
using Mors.AppPlatform.Adapters.Journeys;

namespace Mors.AppPlatform.Adapters
{
    public static class JourneysWpfClient
    {
        public static void BootstrapAndRun(
            Support.Events.IEventBus eventBus,
            Support.Dispatching.HandlerDispatcher handlerDispatcher,
            Support.Dispatching.IHandlerRegistry handlerRegistry,
            Support.Repositories.GuidIdFactory idFactory)
        {
            var bootstrapper = new Mors.Journeys.Application.Client.Wpf.Bootstrapper(
                new ClientWpfEventBus(eventBus),
                new ClientWpfCommandDispatcher(new Uri("http://localhost:65363/api/command"), handlerDispatcher),
                new ClientWpfCommandHandlerRegistry(handlerRegistry),
                new ClientWpfQueryDispatcher(new Uri("http://localhost:65363/api/query"), handlerDispatcher),
                new ClientWpfQueryHandlerRegistry(handlerRegistry),
                new ClientWpfIdFactory(idFactory));
            bootstrapper.Bootstrap();
            bootstrapper.Run();
        }
    }
}
