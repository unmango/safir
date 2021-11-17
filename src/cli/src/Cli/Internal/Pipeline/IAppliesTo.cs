namespace Cli.Internal.Pipeline
{
    internal delegate bool AppliesTo<in T>(T context);

    internal interface IAppliesTo<in T>
    {
        bool AppliesTo(T context);
    }
}
