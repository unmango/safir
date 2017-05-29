using System.Configuration;

namespace Safir.Core.ConfigFile.DefaultValues
{
    public class DefaultValueCollection : ConfigurationElementCollection
    {
        public new string this[string i]
        {
            get
            {
                return (BaseGet(i) as DefaultValueElement).Value;
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new DefaultValueElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return (element as DefaultValueElement).Key;
        }
    }
}
