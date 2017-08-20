namespace Safir.ViewModels.Events
{
    using Manager.Audio;

    public class PlaySongEvent
    {
        public IPlayable Song { get; set; }
    }
}
