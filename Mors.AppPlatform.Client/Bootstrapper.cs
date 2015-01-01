using Mors.AppPlatform.Adapters;
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

            JourneysWpfClient.BootstrapAndRun(
                eventBus,
                handlerDispatcher,
                handlerRegistry,
                idFactory);
        }
    }
}
