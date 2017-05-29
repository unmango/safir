using Common.Extensions;
using System.Configuration;

namespace Safir.Core.ConfigFile.DefaultValues
{
    public class DefaultValueElement : ConfigurationElement
    {
        [ConfigurationProperty("key", IsRequired = true, IsKey = true)]
        public string Key
        {
            get { return this["key"] as string; }
            set { this["key"] = value; }
        }

        [ConfigurationProperty("value", IsRequired = true, IsKey = false)]
        public string Value
        {
            get { return (this["value"] as string).ReplaceEnvironmentVariables(); }
            set { this["value"] = value; }
        }
    }
}
