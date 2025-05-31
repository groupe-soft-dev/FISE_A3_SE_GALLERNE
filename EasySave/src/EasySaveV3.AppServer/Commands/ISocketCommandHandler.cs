using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySaveV3.AppServer.Commands
{
    public interface ISocketCommandHandler
    {
        bool CanHandle(string command);
        string Handle(string command, string? payload);
    }

}
