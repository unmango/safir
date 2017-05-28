using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Safir.Core.Helpers
{
    public static class XmlHelper
    {
        public static void SaveXml<T>(
            T obj, string path,
            Encoding encoding = null) where T : class
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            if (String.IsNullOrEmpty(path)) throw new ArgumentNullException(nameof(path));

            var s = new XmlSerializer(typeof(T));
            var w = new StreamWriter(
                new FileStream(path, FileMode.Create),
                encoding ?? Encoding.UTF8);

            s.Serialize(w, obj);
        }

        public static T LoadXml<T>(
            string path,
            Func<T> defaultValueFunc = null,
            Encoding encoding = null) where T : class
        {
            if (String.IsNullOrEmpty(path)) throw new ArgumentNullException(nameof(path));
            if (!File.Exists(path)) return defaultValueFunc?.Invoke();

            var s = new XmlSerializer(typeof(T));
            var w = new StreamReader(
                new FileStream(path, FileMode.Open),
                encoding ?? Encoding.UTF8);

            return (T)s.Deserialize(w);
        }
    }
}
