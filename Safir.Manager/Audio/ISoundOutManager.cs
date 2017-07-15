// <copyright file="ISoundOutManager.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Safir.Manager.Audio
{
    using System;
    using CSCore.SoundOut;

    public interface ISoundOutManager : IDisposable
    {
        ISoundOut OpenSong(IPlayable file);
    }
}