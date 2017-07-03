using CSCore.CoreAudioAPI;

namespace Safir.Manager
{
    public class Devices
    {
        private const int SUCCESS_CODE = 1;
        
        private IMMDeviceCollection _devices;
        private IMMDeviceEnumerator _enumerator;

        public Devices() {
            _enumerator = (IMMDeviceEnumerator)new MMDeviceEnumerator();
        }

        public Devices(IMMDeviceEnumerator enumerator) {
            _enumerator = enumerator;
        }

        public IMMDeviceCollection ActiveDevices {
            get {
                if (_devices == null) {
                    _enumerator.EnumAudioEndpoints(DataFlow.Render, DeviceState.Active, out _devices);
                }
                return _devices;
            }
        }

        public IMMDevice GetDefaultDevice(
            DataFlow flow = DataFlow.Render,
            Role role = Role.Multimedia) {
            var result = _enumerator.GetDefaultAudioEndpoint(flow, role, out IMMDevice device);
            return result == SUCCESS_CODE ? device : null;
        }
        

    }
}
