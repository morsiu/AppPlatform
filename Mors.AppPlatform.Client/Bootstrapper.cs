using Mors.AppPlatform.Adapters;
using Mors.AppPlatform.Support.Dispatching;
using Mors.AppPlatform.Support.Repositories;

namespace Mors.AppPlatform.Client
{
    internal sealed class Bootstrapper
    {
        public void BootstrapAndRun()
        {
            var eventBus = new Support.Events.EventBus();
            var idFactory = new GuidIdFactory();
            var handlerRegistry = new HandlerRegistry();
            var handlerDispatcher = new HandlerDispatcher(handlerRegistry);
            var application = new Application();
            var window = new MainWindow(
                JourneysWpfClient.Bootstrap(
                    eventBus,
                    handlerDispatcher,
                    handlerRegistry,
                    idFactory));
            application.Run(window);
        }
    }
}
