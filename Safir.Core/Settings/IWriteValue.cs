// <copyright file="IWriteValue.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Safir.Core.Settings
{
    public interface IWriteValue<in T>
    {
        void Set(string key, T value);
    }
}
