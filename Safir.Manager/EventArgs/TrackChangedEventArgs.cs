// <copyright file="TrackChangedEventArgs.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Safir.Manager.EventArgs
{
    using System;
    using Audio;

    public class TrackChangedEventArgs : EventArgs
    {
        public TrackChangedEventArgs(IPlayable newTrack) {
            NewTrack = newTrack;
        }

        public IPlayable NewTrack { get; protected set; }
    }
}
