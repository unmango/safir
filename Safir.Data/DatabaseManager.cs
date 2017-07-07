namespace Safir.Data
{
    using System;
    using System.Data.SqlClient;
    using System.Data.SQLite;
    using System.IO;
    using Core;
    using Core.Settings;

    public class DatabaseManager
    {
        private const string CONNECTION_STRING_PROPERTY = "ConnectionString";

        private readonly ISettingStore _settings;

        public DatabaseManager(ISettingStore settings) {
            _settings = settings;
        }

        public string ConnectionString {
            get {
                var conString = _settings.Get(CONNECTION_STRING_PROPERTY);
                if (string.IsNullOrEmpty(conString)) {
                    conString = DefaultValue.Get(CONNECTION_STRING_PROPERTY);
                    _settings.Set(CONNECTION_STRING_PROPERTY, conString);
                }

                if (!Valid(conString))
                    throw new FileNotFoundException("The connection string was invalid");
                return conString;
            }

            set {
                _settings.Set(CONNECTION_STRING_PROPERTY, value);
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
            } catch {
                return false;
            }

            return true;
        }
    }
}
