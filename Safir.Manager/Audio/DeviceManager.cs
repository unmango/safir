// <copyright file="DeviceManager.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Safir.Manager.Audio
{
    using CSCore.CoreAudioAPI;

    public class DeviceManager : IDeviceManager
    {
        private const int SUCCESS_CODE = 1;
        
        private IMMDeviceCollection _devices;
        private IMMDeviceEnumerator _enumerator;

        public DeviceManager() {
            _enumerator = (IMMDeviceEnumerator)new MMDeviceEnumerator();
        }

        public DeviceManager(IMMDeviceEnumerator enumerator) {
            _enumerator = enumerator;
        }

        public IMMDeviceCollection ActiveOutputDevices {
            get {
                if (_devices == null) {
                    _enumerator.EnumAudioEndpoints(DataFlow.Render, DeviceState.Active, out _devices);
                }

                return _devices;
            }
        }

        public IMMDevice GetDefaultDevice(DataFlow flow = DataFlow.Render, Role role = Role.Multimedia) {
            var result = _enumerator.GetDefaultAudioEndpoint(flow, role, out IMMDevice device);
            return result == SUCCESS_CODE ? device : null;
        }
    }
}
