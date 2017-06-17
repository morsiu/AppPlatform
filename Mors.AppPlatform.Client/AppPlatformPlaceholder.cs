using System.Windows;

namespace Mors.AppPlatform.Client
{
    internal sealed class AppPlatformPlaceholder : IActiveApplication
    {
        public UIElement Content => null;

        public string Title => "AppPlatform";
    }
}
