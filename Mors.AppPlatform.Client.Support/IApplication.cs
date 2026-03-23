using System.Windows;

namespace Mors.AppPlatform.Client.Support;

public interface IApplication
{
    string DescribeSelfForTitleBarOfMainWindow();

    string DescribeSelfForSelectionUi();

    UIElement CreateUiForInteractionWithSelf();
}