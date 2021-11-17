using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Cli.Services.Configuration;
using Cli.Services.Sources;
using Microsoft.Extensions.Options;

namespace Cli.Services
{
    internal class DefaultServiceRegistry : IServiceRegistry, IDisposable
    {
        private const int StaticallyConfiguredServices = 2;
        
        private readonly IOptionsMonitor<ServiceOptions> _optionsMonitor;
        private readonly IDisposable _changeToken;
        private readonly Dictionary<string, IService> _services = new(StaticallyConfiguredServices + 1);

        public DefaultServiceRegistry(IOptionsMonitor<ServiceOptions> optionsMonitor)
        {
            _optionsMonitor = optionsMonitor;
            PopulateServices(optionsMonitor.CurrentValue);
            _changeToken = optionsMonitor.OnChange(PopulateServices);
        }

        public IEnumerable<IService> Services => _services.Values;
        
        public void Dispose()
        {
            _changeToken.Dispose();
        }

        private void PopulateServices(ServiceOptions options)
        {
            foreach (var (key, value) in options)
            {
                var sources = value.Sources.Select(x => x.GetSource());
                var service = value.GetService(sources);
                _services.Add(key, service);
            }
        }

        #region Dictionary Support

        IEnumerator<KeyValuePair<string, IService>> IEnumerable<KeyValuePair<string, IService>>.GetEnumerator()
        {
            return _services.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _services.GetEnumerator();
        }

        int IReadOnlyCollection<KeyValuePair<string, IService>>.Count => _services.Count;

        IEnumerable<string> IReadOnlyDictionary<string, IService>.Keys => _services.Keys;

        IEnumerable<IService> IReadOnlyDictionary<string, IService>.Values => _services.Values;

        IService IReadOnlyDictionary<string, IService>.this[string key] => _services[key];

        bool IReadOnlyDictionary<string, IService>.ContainsKey(string key)
        {
            // Maybe faster? Also may be incorrect...
            return _optionsMonitor.CurrentValue.ContainsKey(key);
        }

        bool IReadOnlyDictionary<string, IService>.TryGetValue(
            string key,
            [MaybeNullWhen(false)] out IService service)
        {
            return _services.TryGetValue(key, out service);
        }

        #endregion
    }
}
