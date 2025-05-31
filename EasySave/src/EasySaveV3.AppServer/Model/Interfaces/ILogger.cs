using EasySaveV3.AppServer.Model.Services;
using System.Text.Json;

namespace EasySaveV3.AppServer
{

    public interface ILogger
    {
        public string GetDailyLogDirectory();

        public string GetDailyLogPath();

        public string GetStatusLogPath();

        public void AddLogInfo(LogType logType, Dictionary<string, object> logEntry);
    }
}
