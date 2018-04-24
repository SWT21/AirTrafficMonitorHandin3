using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirTrafficMonitor.Interfaces
{
    public interface IAirspaceMonitor
    {
        Dictionary<string, ITrack> TrackDict { get; }
        ISeparationMonitor SeparationMonitor { get; }
        void AddTrack(ITrack trackNew);
        void RefreshTrack(ITrack track);
        bool IsInAirspace(ITrack track);
        bool IsInTrackDict(ITrack track);
        bool IsDoneDetectSpearation { get; }
        double TrackCourse { get;}
    }
}
