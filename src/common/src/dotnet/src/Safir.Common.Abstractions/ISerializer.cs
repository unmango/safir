using System;
using System.Buffers;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Safir.Common
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public interface ISerializer
    {
        T Deserialize<T>(ReadOnlyMemory<byte> value);

        ValueTask<T> DeserializeAsync<T>(ReadOnlyMemory<byte> value, CancellationToken cancellationToken = default);

        ValueTask<object> DeserializeAsync(Type type, ReadOnlyMemory<byte> value, CancellationToken cancellationToken = default);
        
        void Serialize<T>(IBufferWriter<byte> writer, T value);

        void Serialize<T>(Stream stream, T value);

        ValueTask SerializeAsync<T>(IBufferWriter<byte> writer, T value, CancellationToken cancellationToken = default);
    }
}
