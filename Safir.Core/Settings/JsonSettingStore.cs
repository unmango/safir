// <copyright file="JsonSettingStore.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Safir.Core.Settings
{
    using System.Collections.Generic;
    using System.IO;
    using Application;
    using Helpers;

    public class JsonSettingStore : BaseSettingsStore
    {
        private string _settingsFilePath;

        public JsonSettingStore(IAppMeta appMeta) {
            _settingsFilePath = Path.Combine(
                AppFolderHelper.GetAppFolderPath(appMeta.AppName),
                appMeta.AppName + ".config");
        }

        public override void Load() {
            if (string.IsNullOrEmpty(_settingsFilePath))
                return;
            LoadSettings(JsonHelper.LoadJson<Dictionary<string, string>>(_settingsFilePath));
        }

        public override void Save() {
            if (string.IsNullOrEmpty(_settingsFilePath))
                return;
            JsonHelper.SaveXml(GetSettingSnapshot(), _settingsFilePath);
        }
    }
}
