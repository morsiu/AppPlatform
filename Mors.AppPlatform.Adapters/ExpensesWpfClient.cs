using System.Windows;
using Mors.AppPlatform.Client;
using Mors.Expenses.Client.Wpf;

namespace Mors.AppPlatform.Adapters
{
    public sealed class ExpensesWpfClient : IApplication
    {
        public UIElement CreateUiForInteractionWithSelf()
        {
            return new Bootstrapper().Bootstrap();
        }

        public string DescribeSelfForSelectionUi()
        {
            return "Expenses";
        }

        public string DescribeSelfForTitleBarOfMainWindow()
        {
            return "Expenses";
        }
    }
}
