namespace Safir.Core.Popularimeter
{
    internal interface IPopularimeter
    {
        string User { get; set; }
        int Rating { get; set; }
        int PlayCount { get; set; }
    }
}