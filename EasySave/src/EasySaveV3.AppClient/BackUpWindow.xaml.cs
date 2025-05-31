using System.Text;
using System.Windows;
using Core.Model.Services;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using EasySaveV3.AppClient.Services;
using EasySaveV3.AppClient.Interfaces;
using EasySaveV3.AppClient.ViewModel;

namespace EasySaveV3.AppClient
{
    /// <summary>
    /// Interaction logic for BackUpWindow.xaml
    /// </summary>
    public partial class BackUpWindow : Window
    {
        public BackUpWindow(BackUpViewModel vm)
        {
            InitializeComponent();
            IFileDialogService dialog = new FileDialogService();
            vm.InitializeDialogService(dialog);

            DataContext = vm;
        }

        private readonly IFileDialogService _fileDialogService = new FileDialogService();

        private void BrowseSource_Click(object sender, RoutedEventArgs e)
        {
            var folderPath = _fileDialogService.SelectFolder();
            if (!string.IsNullOrEmpty(folderPath))
            {
                if (DataContext is BackUpViewModel vm)
                {
                    vm.dirSource = folderPath;
                }
            }
        }

        private void BrowseTarget_Click(object sender, RoutedEventArgs e)
        {
            var folderPath = _fileDialogService.SelectFolder();
            if (!string.IsNullOrEmpty(folderPath))
            {
                if (DataContext is BackUpViewModel vm)
                {
                    vm.dirTarget = folderPath;
                }
            }
        }

    }
}