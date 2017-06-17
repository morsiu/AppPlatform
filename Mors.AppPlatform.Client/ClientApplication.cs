using System.Windows;

namespace Mors.AppPlatform.Client
{
    internal sealed class ClientApplication : IApplicationSelectionViewModel, IApplicationPresentationViewModel
    {
        private IApplication _application;

        public ClientApplication(IApplication application)
        {
            _application = application;
        }

        public string Description => _application.DescribeSelfForSelectionUi();

        public UIElement Content => _application.CreateUiForInteractionWithSelf();

        public string Title => _application.DescribeSelfForTitleBarOfMainWindow();

        IApplicationPresentationViewModel IApplicationSelectionViewModel.PresentedApplication()
        {
            return this;
        }
    }
}
