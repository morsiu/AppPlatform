namespace Mors.AppPlatform.Client
{
    internal partial class MainWindow
    {
        public MainWindow(IApplication application)
        {
            InitializeComponent();
            Title = application.DesribeSelfForTitleBarOfMainWindow();
            Content = application.CreateUiForInteractionWithSelf();
        }
    }
}
