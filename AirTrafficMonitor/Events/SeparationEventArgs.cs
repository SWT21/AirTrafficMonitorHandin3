using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirTrafficMonitor.Interfaces;

namespace AirTrafficMonitor.Events
{
    public class SeparationEventArgs : EventArgs
    {
        public ITrack Track1 { get; set; }
        public ITrack Track2 { get; set; }

        public SeparationEventArgs(ITrack track1, ITrack track2)
        {
            Track1 = track1;
            Track2 = track2;
        }
    }
}
