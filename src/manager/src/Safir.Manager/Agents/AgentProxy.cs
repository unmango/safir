using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Options;
using Safir.Agent.Client;
using Safir.Agent.Protos;
using Safir.Manager.Configuration;
using Safir.Protos;

namespace Safir.Manager.Agents
{
    public class AgentProxy : IAgents, IAgent
    {
        public AgentProxy(IOptions<ManagerOptions> options)
        {
            var requestProxy = new JsonFileRequestProxy(options.Value.ProxyDataDirectory);
            FileSystem = new FileSystemProxy(requestProxy);
            Host = new HostProxy(requestProxy);
        }

        public IFileSystemClient FileSystem { get; }

        public IHostClient Host { get; }

        public string Name => "Proxy";

        public IEnumerator<IAgent> GetEnumerator()
        {
            yield return this;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IAgent this[string name] => this;

        private class FileSystemProxy : IFileSystemClient
        {
            private readonly JsonFileRequestProxy _requestProxy;

            public FileSystemProxy(JsonFileRequestProxy requestProxy)
            {
                _requestProxy = requestProxy ?? throw new ArgumentNullException(nameof(requestProxy));
            }

            public AsyncServerStreamingCall<FileSystemEntry> ListFiles(CancellationToken cancellationToken = default)
            {
                return ListFilesAsync(cancellationToken).AsAsyncServerStreamingCall();
            }

            public IAsyncEnumerable<FileSystemEntry> ListFilesAsync(CancellationToken cancellationToken = default)
            {
                return _requestProxy.RequestAsyncEnumerable<FileSystemEntry>(
                    "FileSystem/ListFiles",
                    new(),
                    cancellationToken);
            }
        }

        private class HostProxy : IHostClient
        {
            private readonly JsonFileRequestProxy _requestProxy;

            public HostProxy(JsonFileRequestProxy requestProxy)
            {
                _requestProxy = requestProxy ?? throw new ArgumentNullException(nameof(requestProxy));
            }

            public HostInfo GetInfo(CancellationToken cancellationToken = default)
            {
                return GetInfoAsync(cancellationToken).GetAwaiter().GetResult();
            }

            public AsyncUnaryCall<HostInfo> GetInfoAsync(CancellationToken cancellationToken = default)
            {
                return new(
                    Request(),
                    Task.FromResult(Metadata.Empty),
                    () => Status.DefaultSuccess,
                    () => Metadata.Empty,
                    () => { });

                async Task<HostInfo> Request()
                {
                    var result = await _requestProxy.RequestAsync<HostInfo>(
                        "Host/GetInfo",
                        null,
                        cancellationToken);

                    return result ?? new HostInfo();
                }
            }
        }
    }
}
