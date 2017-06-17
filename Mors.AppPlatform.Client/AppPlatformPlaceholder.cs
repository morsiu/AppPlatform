using System.Windows;

namespace Mors.AppPlatform.Client
{
    internal sealed class AppPlatformPlaceholder : IApplicationPresentationViewModel
    {
        public UIElement Content => null;

        public string Title => "AppPlatform";
    }
}
