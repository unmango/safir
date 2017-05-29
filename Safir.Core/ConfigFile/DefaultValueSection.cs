using System.Configuration;

namespace Safir.Core.ConfigFile
{
    using DefaultValues;

    public class DefaultValueSection : ConfigurationSection
    {
        [ConfigurationProperty("values", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(DefaultValueCollection))]
        public DefaultValueCollection Values
        {
            get
            {
               return base["values"] as DefaultValueCollection;
            }
        }
    }
}
