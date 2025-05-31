using EasySaveV3.AppClient.Interfaces;
using EasySaveV3.AppClient.Notifiers;
using EasySaveV3.AppClient.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EasySaveV3.AppClient.ViewModel
{
    public abstract class AppViewModel : INotifyPropertyChanged
    {
        //protected readonly ILocalizer _localizer;
        //protected static IUIErrorNotifier _notifier { get; private set; }

        public AppViewModel()
        {

        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
        {
            if (Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
