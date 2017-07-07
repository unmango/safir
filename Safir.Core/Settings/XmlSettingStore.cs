// <copyright file="XmlSettingStore.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Safir.Core.Settings
{
    using System.Collections.Generic;
    using System.IO;
    using Application;
    using Helpers;

    public class XmlSettingStore : BaseSettingsStore
    {
        private string _settingsFilePath;

        public XmlSettingStore(IAppMeta appMeta) {
            _settingsFilePath = Path.Combine(
                AppFolderHelper.GetAppFolderPath(appMeta.AppName),
                appMeta.AppName + ".config");
        }

        public override void Load() {
            if (string.IsNullOrEmpty(_settingsFilePath))
                return;
            LoadSettings(XmlHelper.LoadXml<Dictionary<string, string>>(_settingsFilePath));
        }

        public override void Save() {
            if (string.IsNullOrEmpty(_settingsFilePath))
                return;
            XmlHelper.SaveXml(GetSettingSnapshot(), _settingsFilePath);
        }
    }
}
