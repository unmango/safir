using System;
using System.Collections.Generic;
using System.IO;

namespace Safir.Core.Settings
{
    using Application;
    using Helpers;

    public class XmlSettingsStore : BaseSettingsStore
    {
        private string _settingsFilePath;

        public XmlSettingsStore(IAppMeta appMeta)
        {
            _settingsFilePath = Path.Combine(
                AppFolderHelper.GetAppFolderPath(appMeta.AppName),
                appMeta.AppName + ".config");
        }

        public override void Load()
        {
            if (String.IsNullOrEmpty(_settingsFilePath))
                return;
            LoadSettings(XmlHelper.LoadXml<Dictionary<string, string>>(_settingsFilePath));
        }

        public override void Save()
        {
            if (String.IsNullOrEmpty(_settingsFilePath))
                return;
            XmlHelper.SaveXml(GetSettingSnapshot(), _settingsFilePath);
        }
    }
}
