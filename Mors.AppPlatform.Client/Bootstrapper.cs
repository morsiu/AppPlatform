using Mors.AppPlatform.Adapters;
using Mors.AppPlatform.Service.Client;
using Mors.AppPlatform.Support.Dispatching;
using Mors.AppPlatform.Support.Repositories;
using System;

namespace Mors.AppPlatform.Client
{
    internal sealed class Bootstrapper
    {
        public void BootstrapAndRun()
        {
            var requestFactory = new RequestFactory(
                new Uri("http://localhost:65363/api/command"),
                new Uri("http://localhost:65363/api/query"));
            var eventBus = new Support.Events.EventBus();
            var idFactory = new GuidIdFactory();
            var handlerRegistry = new HandlerRegistry();
            var handlerDispatcher = new HandlerDispatcher(handlerRegistry);
            var application = new Application();
            var window = new MainWindow(
                JourneysWpfClient.Bootstrap(
                    requestFactory,
                    eventBus,
                    handlerDispatcher,
                    handlerRegistry,
                    idFactory));
            application.Run(window);
        }
    }
}
