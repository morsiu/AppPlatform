using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Mors.AppPlatform.Client
{
    internal sealed class WindowViewModel : INotifyPropertyChanged
    {
        private IApplicationSelectionViewModel _selectedApplication;
        private IApplicationPresentationViewModel _presentedApplication;

        public WindowViewModel(IEnumerable<IApplicationSelectionViewModel> applications)
        {
            Applications = applications;
            _presentedApplication = new AppPlatformPlaceholder();
        }

        public IEnumerable<IApplicationSelectionViewModel> Applications { get; }

        public IApplicationSelectionViewModel SelectedApplication
        {
            get { return _selectedApplication; }
            set
            {
                _selectedApplication = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedApplication)));
                PresentedApplication = _selectedApplication?.PresentedApplication() ?? new AppPlatformPlaceholder();
            }
        }

        public IApplicationPresentationViewModel PresentedApplication
        {
            get { return _presentedApplication; }
            set
            {
                _presentedApplication = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PresentedApplication)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
