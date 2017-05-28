using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Safir.Core.Settings
{
    public abstract class BaseSettingsStore : ISettingStore
    {
        protected BaseSettingsStore()
        {
            this.CurrentSettings = new ConcurrentDictionary<string, string>();
        }

        protected ConcurrentDictionary<string, string> CurrentSettings { get; set; }

        protected Dictionary<string, string> GetSettingSnapshot()
        {
            return new Dictionary<string, string>(this.CurrentSettings);
        }

        protected void LoadSettings(IEnumerable<KeyValuePair<string, string>> settings = null)
        {
            if (settings != null) this.CurrentSettings = new ConcurrentDictionary<string, string>(settings);
            else this.CurrentSettings.Clear();
        }

        public string this[string key]
        {
            get
            {
                if (key == null) throw new ArgumentNullException(nameof(key));

                string value;
                return this.CurrentSettings.TryGetValue(key, out value) ? value : null;
            }
            set
            {
                if (key == null) throw new ArgumentNullException(nameof(key));

                this.CurrentSettings.AddOrUpdate(key, value, (k, v) => value);
            }
        }

        public string Get(string key)
        {
            return this[key];
        }

        public void Set(string key, string value)
        {
            this[key] = value;
        }

        public abstract void Load();

        public abstract void Save();
    }
}
