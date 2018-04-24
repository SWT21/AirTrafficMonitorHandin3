using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirTrafficMonitor.Events;

namespace AirTrafficMonitor.Interfaces
{
    public interface ISeparationMonitor
    {
        event EventHandler<SeparationEventArgs> SeparationEvent;
        event EventHandler<SeparationEventArgs> SeparationDoneEvent;
        void DetectSpearation(Dictionary<string, ITrack> trackDict);
        int VerticalDistance { get; }
        double HorizontalDistance { get; }
    }
}
