// <copyright file="IDeviceManager.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Safir.Manager.Audio
{
    using System;
    using CSCore.CoreAudioAPI;

    public interface IDeviceManager : IDisposable
    {
        IMMDeviceCollection ActiveOutputDevices { get; }

        IMMDevice GetDefaultDevice(DataFlow dataFlow = DataFlow.Render, Role role = Role.Multimedia);
    }
}