// <copyright file="ISettingStore.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Safir.Core.Settings
{
    public interface ISettingStore : IReadValue<string>, IWriteValue<string>, IIndexable<string>
    {
        void Load();

        void Save();
    }
}
