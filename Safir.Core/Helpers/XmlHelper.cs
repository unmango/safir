// <copyright file="XmlHelper.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Safir.Core.Helpers
{
    using System;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Text;
    using System.Xml;
    using log4net;

    public static class XmlHelper
    {
        private static readonly ILog _log = LogHelper.GetLogger();

        public static void SaveXml<T>(
            T obj,
            string path,
            Encoding encoding = null)
            where T : class {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException(nameof(path));

            var serializer = new DataContractSerializer(typeof(T));
            var settings = new XmlWriterSettings {
                Indent = true,
                CloseOutput = true,
                Encoding = encoding ?? Encoding.UTF8
            };
            var writer = XmlWriter.Create(
                new FileStream(path, FileMode.Create),
                settings);

            serializer.WriteObject(writer, obj);
            writer.Close();
        }

        public static T LoadXml<T>(
            string path,
            Func<T> defaultValueFunc = null)
            where T : class {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException(nameof(path));
            if (!File.Exists(path))
                return defaultValueFunc?.Invoke();

            var serializer = new DataContractSerializer(typeof(T));
            var settings = new XmlReaderSettings { CloseInput = true };
            var reader = XmlReader.Create(
                new FileStream(path, FileMode.Open),
                settings);

            try {
                return (T)serializer.ReadObject(reader);
            } catch (SerializationException e) {
                _log.Error(e);
                return default(T);
            } finally {
                reader.Close();
            }
        }
    }
}
