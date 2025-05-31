using EasySaveV3.AppClient.Interfaces;
using EasySaveV3.AppClient.Services;
using EasySaveV3.AppClient.Notifiers;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace EasySaveV3.AppClient.ViewModel
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        //protected readonly ILocalizer _localizer;
        protected readonly SocketClientService _socketClient;

        public ViewModelBase(SocketClientService socketClient)
        {
            _socketClient = socketClient ?? throw new ArgumentNullException(nameof(socketClient));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private Dictionary<string, string> _translations = new();

        public async Task LoadTranslationsAsync()
        {
            try
            {
                string json = await _socketClient.SendCommandAsync("GET_TRANSLATIONS", null);
                _translations = JsonSerializer.Deserialize<Dictionary<string, string>>(json) ?? new Dictionary<string, string>();
                OnPropertyChanged("Item[]");
            }
            catch (Exception ex)
            {
                // Log ou notifier l'erreur
                _translations = new Dictionary<string, string>();
                OnPropertyChanged("Item[]");
            }
        }

        public string this[string key] => _translations.ContainsKey(key) ? _translations[key] : $"[{key}]";


        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
        {
            if (Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
