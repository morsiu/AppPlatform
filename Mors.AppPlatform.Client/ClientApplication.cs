using System.Windows;

namespace Mors.AppPlatform.Client
{
    internal sealed class ClientApplication : IAvailableApplication, IActiveApplication
    {
        private readonly IApplication _application;

        public ClientApplication(IApplication application)
        {
            _application = application;
        }

        public string Description => _application.DescribeSelfForSelectionUi();

        public UIElement Content => _application.CreateUiForInteractionWithSelf();

        public string Title => _application.DescribeSelfForTitleBarOfMainWindow();

        public IActiveApplication ActiveApplication() => this;
    }
}
