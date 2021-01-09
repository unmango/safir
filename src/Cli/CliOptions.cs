// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Cli.Services;

namespace Cli
{
    internal record CliOptions
    {
        public ConfigOptions Config { get; init; } = new();

        public ServiceOptions Services { get; init; } = new();
    }

    internal record ConfigOptions
    {
        public string Directory { get; init; } = string.Empty;

        public bool Exists { get; init; }

        public string File { get; init; } = string.Empty;
    }

    internal record ServiceOptions : IReadOnlyDictionary<string, ServiceEntry>
    {
        public const string DefaultInstallationDirectory = "services";
        private ServiceEntry? _manager;

        public string? CustomInstallationDirectory { get; init; }

        public ServiceEntry? Manager
        {
            get => _manager;
            set {
                if (value == null) throw new ArgumentNullException(nameof(Manager));
                
                // Maybe won't always want to overwrite a custom name.
                _manager = value with { Name = nameof(Manager) };
            }
        }

        public IEnumerator<KeyValuePair<string, ServiceEntry>> GetEnumerator()
        {
            if (Manager != null) yield return new KeyValuePair<string, ServiceEntry>(nameof(Manager), Manager);
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public int Count => Keys.Count();

        public bool ContainsKey(string key) => Keys.Contains(key);

        public bool TryGetValue(string key, [MaybeNullWhen(false)] out ServiceEntry value)
        {
            value = null;

            using var enumerator = GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (enumerator.Current.Key != key) continue;
                
                value = enumerator.Current.Value;
                return true;
            }

            return false;
        }

        public ServiceEntry this[string key] => throw new System.NotImplementedException();

        public IEnumerable<string> Keys
        {
            get {
                using var enumerator = GetEnumerator();
                while (enumerator.MoveNext())
                    yield return enumerator.Current.Key;
            }
        }

        public IEnumerable<ServiceEntry> Values
        {
            get {
                using var enumerator = GetEnumerator();
                while (enumerator.MoveNext())
                    yield return enumerator.Current.Value;
            }
        }
    }
}
