namespace Safir.Common;

public sealed class Disposable : IDisposable
{
    public static readonly IDisposable NoOp = new Disposable();

    private Disposable() { }

    public void Dispose() { }
}
