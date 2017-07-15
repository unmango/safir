// <copyright file="IWaveSourceManager.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Safir.Manager.Audio
{
    using System;
    using CSCore;

    public interface IWaveSourceManager : IDisposable
    {
        IWaveSource GetWaveSource(IPlayable file);
    }
}
