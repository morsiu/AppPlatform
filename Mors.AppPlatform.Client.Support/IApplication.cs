using System.Windows;

namespace Mors.AppPlatform.Client
{
    public interface IApplication
    {
        string DescribeSelfForTitleBarOfMainWindow();

        string DescribeSelfForSelectionUi();

        UIElement CreateUiForInteractionWithSelf();
    }
}
