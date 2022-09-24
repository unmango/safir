using System.Net.NetworkInformation;
using JetBrains.Annotations;

namespace Safir.Common;

[PublicAPI]
public static class NetUtil
{
    public static bool IsFree(int port)
    {
        var properties = IPGlobalProperties.GetIPGlobalProperties();
        var listeners = properties.GetActiveTcpListeners();
        var openPorts = listeners.Select(item => item.Port).ToArray();
        return openPorts.All(openPort => openPort != port);
    }

    public static int NextFreePort(int port = 0)
    {
        port = port > 0 ? port : new Random().Next(1, 65535);

        while (!IsFree(port)) {
            port += 1;
        }

        return port;
    }
}
