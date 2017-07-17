// <copyright file="DeviceManager.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Safir.Manager.Audio
{
    using System;
    using CSCore.CoreAudioAPI;

    public class DeviceManager : IDeviceManager
    {        
        private MMDeviceCollection _devices;
        private MMDeviceEnumerator _enumerator;

        public DeviceManager() {
            _enumerator = new MMDeviceEnumerator();
        }

        // public DeviceManager(IMMDeviceEnumerator enumerator) {
        //    _enumerator = enumerator;
        // }
        public MMDeviceCollection ActiveOutputDevices {
            get {
                if (_devices == null) {
                    _devices = _enumerator.EnumAudioEndpoints(DataFlow.Render, DeviceState.Active);
                }

                return _devices;
            }
        }

        public void Dispose() {
            throw new NotImplementedException();
        }

        public MMDevice GetDefaultDevice(DataFlow flow = DataFlow.Render, Role role = Role.Multimedia) {
            return _enumerator.GetDefaultAudioEndpoint(flow, role);
        }
    }
}
