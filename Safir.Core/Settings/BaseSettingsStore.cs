// <copyright file="BaseSettingsStore.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Safir.Core.Settings
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;

    public abstract class BaseSettingsStore : ISettingStore
    {
        protected BaseSettingsStore() {
            CurrentSettings = new ConcurrentDictionary<string, string>();
        }

        protected ConcurrentDictionary<string, string> CurrentSettings { get; set; }

        public string this[string key] {
            get {
                if (key == null) {
                    throw new ArgumentNullException(nameof(key));
                }

                return CurrentSettings.TryGetValue(key, out string value) ? value : null;
            }

            set {
                if (key == null) {
                    throw new ArgumentNullException(nameof(key));
                }

                CurrentSettings.AddOrUpdate(key, value, (k, v) => value);
            }
        }

        public string Get(string key) {
            return this[key];
        }

        public void Set(string key, string value) {
            this[key] = value;
        }

        public abstract void Load();

        public abstract void Save();

        protected Dictionary<string, string> GetSettingSnapshot() {
            return new Dictionary<string, string>(CurrentSettings);
        }

        protected void LoadSettings(IEnumerable<KeyValuePair<string, string>> settings = null) {
            if (settings != null) {
                CurrentSettings = new ConcurrentDictionary<string, string>(settings);
            } else {
                CurrentSettings.Clear();
            }
        }
    }
}
