using System;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.IO;

namespace Safir.Data
{
    using Core;
    using Core.Settings;

    public class DatabaseManager
    {
        private readonly ISettingStore _settings;

        public DatabaseManager(ISettingStore settings) {
            _settings = settings;
        }

        public string ConnectionString {
            get {
                var conString = (string)_settings.Get("ConnectionString");
                if (String.IsNullOrEmpty(conString)) {
                    _settings.Set("ConnectionString", DefaultValue.ConnectionString);
                    conString = DefaultValue.ConnectionString;
                }
                if (!Valid(conString))
                    throw new FileNotFoundException("The connection string was invalid");
                return conString;
            }
            set {
                _settings.Set("ConnectionString", value);
            }
        }

        public void SaveChanges() {
            _settings.Save();
        }

        protected static bool Valid(string connectionString) {
            try {
                var builder = new SqlConnectionStringBuilder(connectionString);
                var source = builder.DataSource;
                var temp = new Uri(source).AbsolutePath;
                if (!File.Exists(source))
                    SQLiteConnection.CreateFile(source);
            } catch { return false; }
            return true;
        }
    }
}
