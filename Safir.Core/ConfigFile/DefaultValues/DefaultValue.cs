// <copyright file="DefaultValue.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Safir.Core
{
    using System;
    using System.Configuration;
    using ConfigFile;

    public class DefaultValue
    {
        private static DefaultValueSection _section =
            ConfigurationManager.GetSection("defaultValues") as DefaultValueSection;

        [Obsolete(message: "Kept for compatibility. Use Get(\"ConnectionString\")")]
        public static string ConnectionString => _section.Values["ConnectionString"];

        public static string Get(string value) {
            return _section.Values[value];
        }

        public static T Get<T>(string value)
            where T : IConvertible {
            string temp = Get(value);
            var type = typeof(T);
            if (type.IsEnum) {
                return (T)Enum.Parse(type, value);
            } else {
                return (T)Convert.ChangeType(value, type);
            }
        }
    }
}
