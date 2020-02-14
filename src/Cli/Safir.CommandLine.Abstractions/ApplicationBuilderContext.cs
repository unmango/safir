using System.Collections.Generic;
using System.Linq;

namespace System.CommandLine
{
    public class ApplicationBuilderContext
    {
        private readonly IDictionary<object, object> _properties;

        public ApplicationBuilderContext(IEnumerable<KeyValuePair<object, object>> properties)
        {
            _properties = properties?.ToDictionary(x => x.Key, x => x.Value)
                ?? throw new ArgumentNullException(nameof(properties));
        }
    }
}
