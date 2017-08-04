// <copyright file="IIndexable.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Safir.Core.Settings
{
    public interface IIndexable<T>
    {
        T this[string index] { get; set; }
    }
}
