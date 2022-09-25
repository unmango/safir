namespace Safir.Grpc.Client;

public static class ClientName
{
    public static string FileSystem(string name) => $"{name}-filesystem";

    public static string Host(string name) => $"{name}-host";
}
