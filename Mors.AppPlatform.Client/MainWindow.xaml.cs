using System.Collections.Generic;
using System.Linq;

namespace Mors.AppPlatform.Client
{
    internal partial class MainWindow
    {
        public MainWindow(IEnumerable<IApplication> applications)
        {
            DataContext = new WindowViewModel(applications.Select(x => new ClientApplication(x)));
            InitializeComponent();
        }
    }
}
