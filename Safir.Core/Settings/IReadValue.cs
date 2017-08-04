// <copyright file="IReadValue.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Safir.Core.Settings
{
    public interface IReadValue<out T>
    {
        T Get(string key);
    }
}
