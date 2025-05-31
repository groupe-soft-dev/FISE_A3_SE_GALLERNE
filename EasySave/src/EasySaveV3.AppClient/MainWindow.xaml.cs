using EasySaveV3.AppClient.Notifiers;
using EasySaveV3.AppClient.Interfaces;
using EasySaveV3.AppClient.Services;
using EasySaveV3.AppClient.ViewModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace EasySaveV3.AppClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            IMessageBoxNotifier notifier = new MessageBoxNotifier();
            INavigationService navigation = new NavigationService();
            SocketClientService socket = new SocketClientService();
            MainViewModel SettingsVM = new MainViewModel(navigation, notifier, socket);
            DataContext = SettingsVM;
        }
    }
}