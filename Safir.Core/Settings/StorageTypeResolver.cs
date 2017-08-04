// <copyright file="StorageTypeResolver.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Safir.Core.Settings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Application;

    internal class StorageTypeResolver
    {
        private const string PROPERTY_NAME = "SettingStorageType";
        private const string STORAGE_SUFFIX = "SettingStore";

        private static readonly Type _settingsBaseType = typeof(BaseSettingsStore);

        private readonly IAppMeta _appMeta;
        private readonly string _defaultStorageType;
        
        public StorageTypeResolver(IAppMeta appMeta) {
            _appMeta = appMeta;
            _defaultStorageType = DefaultValue.Get(PROPERTY_NAME);
        }

        internal Type Resolve() {
            Type settingsType = null;
            var implementations = GetImplementations();
            foreach (var type in implementations) {
                var settings = (ISettingStore)Activator.CreateInstance(type, _appMeta);
                try {
                    settings.Load();
                    settingsType = type;
                } catch (Exception) {
                    continue;
                }
            }

            return settingsType ?? DefaultStorageType(_appMeta);
        }

        private static IEnumerable<Type> GetImplementations() {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p =>
                    _settingsBaseType.IsAssignableFrom(p) &&
                    (!p.IsAbstract));
        }

        private Type DefaultStorageType(IAppMeta appmeta) {
            return Type.GetType(_defaultStorageType + STORAGE_SUFFIX);
        }

        private bool IsDefaultType(Type type) {
            return type.Name.IndexOf(_defaultStorageType, StringComparison.OrdinalIgnoreCase) >= 0;
        }
    }
}
