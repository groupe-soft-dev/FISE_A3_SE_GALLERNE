using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EasySaveV3.AppServer.Model.Managers
{
    public interface IResourceManager
    {
        public void LoadTranslations(string language);
        public string Translate(string key);
        public List<string> GetAvailableLanguages();
        public Dictionary<string, string> GetAllTranslations();
    }
}
