using System.Collections.Generic;
using System.ComponentModel;

namespace Mors.AppPlatform.Client
{
    internal sealed class WindowViewModel : INotifyPropertyChanged
    {
        private IAvailableApplication _selectedApplication;
        private IActiveApplication _activeApplication;

        public WindowViewModel(IEnumerable<IAvailableApplication> applications)
        {
            Applications = applications;
            _activeApplication = new AppPlatformPlaceholder();
        }

        public IEnumerable<IAvailableApplication> Applications { get; }

        public IAvailableApplication SelectedApplication
        {
            get { return _selectedApplication; }
            set
            {
                _selectedApplication = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedApplication)));
                ActiveApplication = _selectedApplication?.ActiveApplication() ?? new AppPlatformPlaceholder();
            }
        }

        public IActiveApplication ActiveApplication
        {
            get { return _activeApplication; }
            set
            {
                _activeApplication = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ActiveApplication)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
