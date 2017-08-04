// <copyright file="CorePackage.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Safir.Core
{
    using Settings;
    using SimpleInjector;

    public static class CorePackage
    {
        public static void RegisterServices(Container container)
        {
            container.RegisterSingleton<ISettingStore, XmlSettingStore>();
        }
    }
}
