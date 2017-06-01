using System.Configuration;

namespace Safir.Core
{
    using ConfigFile;
    using System;

    public class DefaultValue
    {
        static DefaultValueSection section = 
            ConfigurationManager.GetSection("defaultValues") as DefaultValueSection;

        [Obsolete]
        public static string ConnectionString => section.Values["ConnectionString"];

        public static string Get(string value) {
            return section.Values[value];
        }
    }
}
