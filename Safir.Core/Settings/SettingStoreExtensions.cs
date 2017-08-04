// <copyright file="SettingStoreExtensions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Safir.Core.Settings
{
    using System;

    public static class SettingStoreExtensions
    {
        public static T Get<T>(this ISettingStore settings, string key)
            where T : IConvertible {
            var value = settings[key];
            var type = typeof(T);
            if (type.IsEnum) {
                return (T)Enum.Parse(type, value);
            } else {
                return (T)Convert.ChangeType(value, type);
            }
        }

        public static void Set<T>(this ISettingStore settings, string key, T value) {
            settings.Set(key, value.ToString());
        }
    }
}
