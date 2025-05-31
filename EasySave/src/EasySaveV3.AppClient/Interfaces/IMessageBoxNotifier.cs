using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySaveV3.AppClient.Interfaces
{
    public interface IMessageBoxNotifier
    {
        void ShowError(string message);
        void ShowSuccess(string message);
        bool ShowWarning(string message);
    }
}
