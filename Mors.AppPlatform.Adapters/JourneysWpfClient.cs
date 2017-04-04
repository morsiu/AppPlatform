using Mors.AppPlatform.Adapters.Journeys;
using System.Windows;

namespace Mors.AppPlatform.Adapters
{
    public static class JourneysWpfClient
    {
        public static UIElement Bootstrap(
            Service.Client.RequestFactory requestFactory,
            Support.Events.IEventBus eventBus,
            Support.Dispatching.HandlerDispatcher handlerDispatcher,
            Support.Dispatching.IHandlerRegistry handlerRegistry,
            Support.Repositories.GuidIdFactory idFactory)
        {
            var bootstrapper = new Mors.Journeys.Application.Client.Wpf.Bootstrapper(
                new ClientWpfEventBus(eventBus),
                new ClientWpfCommandDispatcher(requestFactory, handlerDispatcher),
                new ClientWpfCommandHandlerRegistry(handlerRegistry),
                new ClientWpfQueryDispatcher(requestFactory, handlerDispatcher),
                new ClientWpfQueryHandlerRegistry(handlerRegistry),
                new ClientWpfIdFactory(idFactory));
            return bootstrapper.Bootstrap();
        }
    }
}
