using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirTrafficMonitor.Interfaces
{
    public interface ITrack
    {
        string Tag { get; set; }
        int CoordinateX { get; set; }
        int CoordinateY { get; set; }
        int Altitude { get; set; }
        double Velocity { get; set; }
        double Course { get; set; }
        DateTime UpdateTimestamp { get; set; }
        DateTime SeparationTimestamp { get; set; }
        List<ITrack> SeparationTrackList { get; set; }
        bool IsSeparationTrackListChanged { get; set; }
    }
}
