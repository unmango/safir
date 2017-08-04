// <copyright file="IPopularimeter.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Safir.Core.Popularimeter
{
    internal interface IPopularimeter
    {
        string User { get; set; }

        int Rating { get; set; }

        int PlayCount { get; set; }
    }
}