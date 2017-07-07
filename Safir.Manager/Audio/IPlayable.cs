// <copyright file="IPlayable.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Safir.Manager.Audio
{
    using System;

    public interface IPlayable
    {
        Uri File { get; set; }

        string FilePath { get; set; }
    }
}