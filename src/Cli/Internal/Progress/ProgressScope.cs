namespace Cli.Internal.Progress
{
    public record ProgressScope
    {
        private readonly string _name;

        public static readonly ProgressScope Empty = new(string.Empty);

        private ProgressScope(string name)
        {
            _name = name;
        }

        public static ProgressScope Named(string name) => new(name);
    }
}
