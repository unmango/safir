using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Safir.Manager.Agents
{
    internal class JsonFileRequestProxy
    {
        private static readonly ConcurrentDictionary<object, object?> _requestMap = new();
        private static readonly JsonSerializerOptions _serializerOptions = new();
        private readonly string _root;
        private readonly string _fileName;

        public JsonFileRequestProxy(string root, string? fileName = null)
        {
            _root = root.Trim();
            _fileName = fileName ?? "response.json";
        }

        public ValueTask<T?> RequestAsync<T>(string path, object? args = null, CancellationToken cancellationToken = default)
        {
            return _requestMap.TryGetValue(Key(path, args), out var result)
                ? new ValueTask<T?>((T?)result)
                : RequestAsyncCore<T>(_root, path, _fileName, args, cancellationToken);
        }

        public async IAsyncEnumerable<T> RequestAsyncEnumerable<T>(
            string path,
            object? args = null,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var result = await RequestAsync<List<T>>(path, args, cancellationToken);

            if (result == null) yield break;

            foreach (var fileSystemEntry in result)
                yield return fileSystemEntry;
        }

        private static async ValueTask<T?> RequestAsyncCore<T>(
            string root,
            string path,
            string fileName,
            object? args,
            CancellationToken cancellationToken = default)
        {
            var file = ResponseFile(root, path, fileName);

            if (file == null) return default;

            try
            {
                await using var stream = File.OpenRead(file);
                var result = await JsonSerializer.DeserializeAsync<T>(stream, _serializerOptions, cancellationToken);
                _requestMap.TryAdd(Key(path, args), result);
                return result;
            }
            catch (Exception)
            {
                return default;
            }
        }

        private static string Key(string path, object? args) => $"{path}({args})";

        private static string? ResponseFile(string root, string path, string fileName)
        {
            if (!Path.IsPathRooted(root))
            {
                var assembly = Assembly.GetExecutingAssembly().Location;
                root = Path.Join(Path.GetDirectoryName(assembly), root);
            }

            if (!Directory.Exists(root)) return null;

            var context = Path.Combine(root, path);

            if (File.Exists(context)) return context;

            var file = Path.Combine(context, fileName);

            return File.Exists(file) ? file : null;
        }
    }
}
