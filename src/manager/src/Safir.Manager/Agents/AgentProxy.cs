using System.Collections;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Options;
using Safir.Agent.Protos;
using Safir.Manager.Configuration;
using Safir.Protos;
using Host = Safir.Protos.Host;

namespace Safir.Manager.Agents;

public class AgentProxy : IAgents, IAgent
{
    public AgentProxy(IOptions<ManagerOptions> options)
    {
        var requestProxy = new JsonFileRequestProxy(options.Value.ProxyDataDirectory);
        FileSystem = new FileSystemProxy(requestProxy);
        Host = new HostProxy(requestProxy);
    }

    public FileSystem.FileSystemClient FileSystem { get; }

    public Host.HostClient Host { get; }

    public string Name => "Proxy";

    public IEnumerator<IAgent> GetEnumerator()
    {
        yield return this;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IAgent this[string name] => this;

    private class FileSystemProxy : FileSystem.FileSystemClient
    {
        private readonly JsonFileRequestProxy _requestProxy;

        public FileSystemProxy(JsonFileRequestProxy requestProxy)
        {
            _requestProxy = requestProxy ?? throw new ArgumentNullException(nameof(requestProxy));
        }

        public override AsyncServerStreamingCall<FileSystemEntry> ListFiles(
            Empty request,
            Metadata headers = null!, // Base implementation
            DateTime? deadline = null,
            CancellationToken cancellationToken = default)
        {
            return _requestProxy.RequestAsyncEnumerable<FileSystemEntry>(
                    "FileSystem/ListFiles",
                    new(),
                    cancellationToken)
                .AsAsyncServerStreamingCall();
        }

        public override AsyncServerStreamingCall<FileSystemEntry> ListFiles(Empty request, CallOptions options)
        {
            return _requestProxy.RequestAsyncEnumerable<FileSystemEntry>(
                    "FileSystem/ListFiles",
                    new(),
                    options.CancellationToken)
                .AsAsyncServerStreamingCall();
        }

        protected override FileSystem.FileSystemClient NewInstance(ClientBaseConfiguration configuration)
        {
            return new FileSystemProxy(_requestProxy);
        }
    }

    private class HostProxy : Host.HostClient
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

        public override HostInfo GetInfo(
            Empty request,
            Metadata headers = null!, // Base implementation
            DateTime? deadline = null,
            CancellationToken cancellationToken = default)
        {
            return GetInfoProxy(cancellationToken).GetAwaiter().GetResult();
        }

        public override HostInfo GetInfo(Empty request, CallOptions options)
        {
            return GetInfoProxy(options.CancellationToken).GetAwaiter().GetResult();
        }

        public override AsyncUnaryCall<HostInfo> GetInfoAsync(
            Empty request,
            Metadata headers = null!,
            DateTime? deadline = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return new(
                GetInfoProxy(cancellationToken),
                Task.FromResult(Metadata.Empty),
                () => Status.DefaultSuccess,
                () => Metadata.Empty,
                () => { });
        }

        public override AsyncUnaryCall<HostInfo> GetInfoAsync(Empty request, CallOptions options)
        {
            return new(
                GetInfoProxy(options.CancellationToken),
                Task.FromResult(Metadata.Empty),
                () => Status.DefaultSuccess,
                () => Metadata.Empty,
                () => { });
        }

        protected override Host.HostClient NewInstance(ClientBaseConfiguration configuration)
        {
            return new HostProxy(_requestProxy);
        }

        private async Task<HostInfo> GetInfoProxy(CancellationToken cancellationToken)
        {
            var result = await _requestProxy.RequestAsync<HostInfo>(
                "Host/GetInfo",
                null,
                cancellationToken);

            return result ?? new HostInfo();
        }
    }
}
