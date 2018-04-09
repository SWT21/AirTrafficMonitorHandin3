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
        string Tag { get; }
        int CoordinateX { get; }
        int CoordinateY { get; }
        int Altitude { get; }
        double Velocity { get; }
        DateTime Timestamp { get; }
        double Course { get; }
    }
}
