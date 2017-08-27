// <copyright file="ConnectionStringHelper.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Safir.Data
{
    using System.Data.SqlClient;
    using Core;
    using Core.Settings;

    internal class ConnectionStringHelper
    {
        private const string CONNECTION_STRING = "ConnectionString";

        private static ISettingStore _settings;

        public ConnectionStringHelper(ISettingStore settings) {
            _settings = settings;
        }

        public static string Get() {
            // TODO: Update this functionality
            var connectionString = _settings?.Get(CONNECTION_STRING);

            if (string.IsNullOrEmpty(connectionString)) {
                connectionString = DefaultValue.Get(CONNECTION_STRING);
            }

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder {
                ConnectionString = connectionString
            };

            return builder.ToString();
        }
    }
}
