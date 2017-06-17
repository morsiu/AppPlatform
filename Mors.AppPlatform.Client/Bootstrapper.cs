using Mors.AppPlatform.Adapters;
using Mors.AppPlatform.Client.Properties;
using Mors.AppPlatform.Service.Client;
using Mors.AppPlatform.Support.Dispatching;
using Mors.AppPlatform.Support.Repositories;

namespace Mors.AppPlatform.Client
{
    internal sealed class Bootstrapper
    {
        public void BootstrapAndRun()
        {
            var requestFactory =
                new RequestFactory(
                    Settings.Default.CommandRequestUri,
                    Settings.Default.QueryRequestUri);
            var eventBus = new Support.Events.EventBus();
            var idFactory = new GuidIdFactory();
            var handlerRegistry = new HandlerRegistry();
            var handlerDispatcher = new HandlerDispatcher(handlerRegistry);
            var application = new Application();
            var window =
                new MainWindow(
                    new[]
                    {
                        new JourneysWpfClient(
                            requestFactory,
                            eventBus,
                            handlerDispatcher,
                            handlerRegistry,
                            idFactory)
                    });
            application.Run(window);
        }
    }
}
