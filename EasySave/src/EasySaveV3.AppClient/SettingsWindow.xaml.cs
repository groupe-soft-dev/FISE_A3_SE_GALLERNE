using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using EasySaveV3.AppClient.Interfaces;
using EasySaveV3.AppClient.Services;
using EasySaveV3.AppClient.ViewModel;

namespace EasySaveV3.AppClient
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
            INavigationService _navigation = new NavigationService();
           // SettingsViewModel SettingsVM = new SettingsViewModel(_navigation);
           // DataContext = SettingsVM;
        }
    }
}