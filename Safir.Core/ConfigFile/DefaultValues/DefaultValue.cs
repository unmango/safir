using System.Configuration;

namespace Safir.Core
{
    using ConfigFile;

    public class DefaultValue
    {
        static DefaultValueSection section = 
            ConfigurationManager.GetSection("defaultValues") as DefaultValueSection;

        public static string ConnectionString => section.Values["ConnectionString"];
    }
}
