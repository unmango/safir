using System.Collections.Generic;
using System.Linq;

namespace System.CommandLine
{
    /// <summary>
    /// The context used while building the application.
    /// </summary>
    public class ApplicationBuilderContext
    {
        /// <summary>
        /// Initializes a new instance of an <see cref="ApplicationBuilderContext"/>
        /// with the specified <paramref name="properties"/>.
        /// </summary>
        /// <param name="properties"></param>
        public ApplicationBuilderContext(IEnumerable<KeyValuePair<object, object>> properties)
        {
            Properties = properties?.ToDictionary(x => x.Key, x => x.Value)
                ?? throw new ArgumentNullException(nameof(properties));
        }

        /// <summary>
        /// Gets the properties of the context.
        /// </summary>
        public IDictionary<object, object> Properties { get; }
    }
}
