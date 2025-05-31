﻿using Newtonsoft.Json;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace EasySaveV3.AppServer.Model.Managers
{
    public interface IAppConfigManager
    {
        string GetAppConfigParameter(string p);
        void ChangeAppConfigParameter(string p, string v);


    }
}

