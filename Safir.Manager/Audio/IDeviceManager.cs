// <copyright file="IDeviceManager.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Safir.Manager.Audio
{
    using CSCore.CoreAudioAPI;

    public interface IDeviceManager
    {
        IMMDeviceCollection ActiveOutputDevices { get; }

        IMMDevice GetDefaultDevice(DataFlow dataFlow = DataFlow.Render, Role role = Role.Multimedia);
    }
}