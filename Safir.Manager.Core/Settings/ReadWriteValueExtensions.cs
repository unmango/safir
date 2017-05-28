using Common.Extensions;
using System;

namespace Safir.Core.Settings
{
    public static class ReadWriteValueExtensions
    {
        public static void Set(
            this IWriteValue<string> writeValue,
            string key, object value)
        {
            if (writeValue == null) throw new ArgumentNullException(nameof(writeValue));
            if (key == null) throw new ArgumentNullException(nameof(key));

            writeValue.Set(key, value.ToType<string>());
        }

        public static T Get<T>(
            this IReadValue<string> readValue,
            string key,
            Func<T> getDefaultValue)
        {
            if (readValue == null) throw new ArgumentNullException(nameof(readValue));
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (getDefaultValue == null) throw new ArgumentNullException(nameof(getDefaultValue));

            string value = readValue.Get(key);
            return value.IsDefault() ? getDefaultValue() : value.ToType<T>();
        }

        public static T Get<T>(
            this IReadValue<string> readValue,
            string key,
            T defaultValue = default(T))
        {
            if (readValue == null) throw new ArgumentNullException(nameof(readValue));
            if (key == null) throw new ArgumentNullException(nameof(key));

            string value = readValue.Get(key);
            return value.IsDefault() ? defaultValue : value.ToType<T>();
        }
    }
}
