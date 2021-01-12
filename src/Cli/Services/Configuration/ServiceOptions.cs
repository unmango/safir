using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Cli.Services.Configuration
{
    internal record ServiceOptions : IDictionary<string, ServiceEntry>
    {
        public const string DefaultInstallationDirectory = "services";
        private readonly Dictionary<string, ServiceEntry> _services = new();

        public string? CustomInstallationDirectory { get; init; }

        private static ServiceEntry WithName(ServiceEntry service, string name)
        {
            return service with { Name = service.Name ?? name };
        }

        #region Dictionary Support
        IEnumerator<KeyValuePair<string, ServiceEntry>> IEnumerable<KeyValuePair<string, ServiceEntry>>.GetEnumerator()
        {
            return _services.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_services).GetEnumerator();
        }

        void ICollection<KeyValuePair<string, ServiceEntry>>.Add(KeyValuePair<string, ServiceEntry> item)
        {
            var (key, value) = item;
            var updated = new KeyValuePair<string, ServiceEntry>(key, WithName(value, key));
            ((ICollection<KeyValuePair<string, ServiceEntry>>)_services).Add(updated);
        }

        void ICollection<KeyValuePair<string, ServiceEntry>>.Clear()
        {
            _services.Clear();
        }

        bool ICollection<KeyValuePair<string, ServiceEntry>>.Contains(KeyValuePair<string, ServiceEntry> item)
        {
            return ((ICollection<KeyValuePair<string, ServiceEntry>>)_services).Contains(item);
        }

        void ICollection<KeyValuePair<string, ServiceEntry>>.CopyTo(
            KeyValuePair<string, ServiceEntry>[] array,
            int arrayIndex)
        {
            ((ICollection<KeyValuePair<string, ServiceEntry>>)_services).CopyTo(array, arrayIndex);
        }

        bool ICollection<KeyValuePair<string, ServiceEntry>>.Remove(KeyValuePair<string, ServiceEntry> item)
        {
            return ((ICollection<KeyValuePair<string, ServiceEntry>>)_services).Remove(item);
        }

        int ICollection<KeyValuePair<string, ServiceEntry>>.Count => _services.Count;

        bool ICollection<KeyValuePair<string, ServiceEntry>>.IsReadOnly
            => ((ICollection<KeyValuePair<string, ServiceEntry>>)_services).IsReadOnly;

        void IDictionary<string, ServiceEntry>.Add(string key, ServiceEntry value)
        {
            _services.Add(key, WithName(value, key));
        }

        public bool ContainsKey(string key)
        {
            return _services.ContainsKey(key);
        }

        bool IDictionary<string, ServiceEntry>.Remove(string key) => _services.Remove(key);

        bool IDictionary<string, ServiceEntry>.TryGetValue(string key, [MaybeNullWhen(false)] out ServiceEntry value)
        {
            return _services.TryGetValue(key, out value);
        }

        public ServiceEntry this[string key]
        {
            get => _services[key];
            set => _services[key] = WithName(value, key);
        }

        public ICollection<string> Keys => ((IDictionary<string, ServiceEntry>)_services).Keys;

        public ICollection<ServiceEntry> Values => ((IDictionary<string, ServiceEntry>)_services).Values;
        #endregion
    }
}
