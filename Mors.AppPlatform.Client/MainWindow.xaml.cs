using System.Windows;

namespace Mors.AppPlatform.Client
{
    internal partial class MainWindow
    {
        public MainWindow(UIElement content)
        {
            InitializeComponent();
            Content = content;
        }
    }
}
