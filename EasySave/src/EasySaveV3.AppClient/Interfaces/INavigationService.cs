using System.Windows.Input;
using EasySaveV3.AppClient.ViewModel;

namespace EasySaveV3.AppClient.Interfaces
{
    public interface INavigationService
    {
        void NavigateToMenu();
        void NavigateToSettings();
        void NavigateToBackUp(BackUpViewModel vm);
        void NavigateToMonitoring(MonitoringViewModel vm);
        void CloseMenu();
        void CloseSettings();
        void CloseBackUp();
        void CloseMonitoring();
        // etc.
    }

}