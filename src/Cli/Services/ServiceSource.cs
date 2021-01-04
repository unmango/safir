namespace Cli.Services
{
    // ReSharper disable once ClassNeverInstantiated.Global
    internal record ServiceSource
    {
        public SourceType? Type { get; init; }
        
        public CommandType? Command { get; init; }
        
        public string? Name { get; init; }
        
        public int? Priority { get; init; }
        
        public string? CloneUrl { get; init; }
        
        public string? ToolName { get; init; }
        
        public string? ExtraArgs { get; init; }
        
        public string? SourceDirectory { get; init; }
        
        public string? ProjectFile { get; init; }
        
        public string? ImageName { get; init; }
        
        public string? Tag { get; init; }
        
        public string? BuildContext { get; init; }
    }
}
