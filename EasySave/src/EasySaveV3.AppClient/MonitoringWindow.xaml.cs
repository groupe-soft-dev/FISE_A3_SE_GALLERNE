using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using EasySaveV3.AppClient.Notifiers;
using EasySaveV3.AppClient.ViewModel;
using System.Windows;

namespace EasySaveV3.AppClient
{
    /// <summary>
    /// Interaction logic for MonitoringWindow.xaml
    /// </summary>
    public partial class MonitoringWindow : Window
    {
        public MonitoringWindow(MonitoringViewModel vm)
        {
            InitializeComponent();
            this.DataContext = vm;
        }
    }
}