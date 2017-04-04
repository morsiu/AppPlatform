using System.Windows;

namespace Mors.AppPlatform.Client
{
    public interface IApplication
    {
        string DesribeSelfForTitleBarOfMainWindow();

        UIElement CreateUiForInteractionWithSelf();
    }
}
