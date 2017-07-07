// <copyright file="ISoundOutManager.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Safir.Manager.Audio
{
    using CSCore.CoreAudioAPI;
    using CSCore.SoundOut;

    public interface ISoundOutManager
    {
        ISoundOut OpenSong(IPlayable file);
    }
}