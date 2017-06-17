using Mors.AppPlatform.Adapters.Journeys;
using Mors.AppPlatform.Client;
using Mors.Journeys.Application.Client.Wpf;
using System.Windows;
using System;

namespace Mors.AppPlatform.Adapters
{
    public class JourneysWpfClient : IApplication
    {
        private static Bootstrapper _bootstrapper;

        public JourneysWpfClient(
            Service.Client.RequestFactory requestFactory,
            Support.Events.IEventBus eventBus,
            Support.Dispatching.HandlerDispatcher handlerDispatcher,
            Support.Dispatching.IHandlerRegistry handlerRegistry,
            Support.Repositories.GuidIdFactory idFactory)
        {
            _bootstrapper = new Bootstrapper(
                new ClientWpfEventBus(eventBus),
                new ClientWpfCommandDispatcher(requestFactory, handlerDispatcher),
                new ClientWpfCommandHandlerRegistry(handlerRegistry),
                new ClientWpfQueryDispatcher(requestFactory, handlerDispatcher),
                new ClientWpfQueryHandlerRegistry(handlerRegistry),
                new ClientWpfIdFactory(idFactory));
        }

        public UIElement CreateUiForInteractionWithSelf()
        {
            return _bootstrapper.Bootstrap();
        }

        public string DescribeSelfForSelectionUi()
        {
            return "Journeys";
        }

        public string DescribeSelfForTitleBarOfMainWindow()
        {
            return "Journeys";
        }
    }
}
