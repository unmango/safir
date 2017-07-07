﻿// <copyright file="IWaveSourceManager.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Safir.Manager.Audio
{
    using CSCore;

    public interface IWaveSourceManager
    {
        IWaveSource GetWaveSource(IPlayable file);
    }
}
