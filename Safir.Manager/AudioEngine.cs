using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Safir.Manager
{
    public class AudioEngine
    {
        private float _volume;
        private long _position;

        public AudioEngine() {

        }

        #region Public Properties

        public float Volume {
            get { return _volume; }
            set {
                _volume = value;
            }
        }

        public long Position {
            get { return _position; }
            set {
                _position = value;
            }
        }

        #endregion
    }
}
