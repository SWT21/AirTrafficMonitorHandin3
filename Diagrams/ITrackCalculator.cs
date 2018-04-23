using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirTrafficMonitor.Interfaces
{
    public interface ITrackCalculator
    {
        double CalculateHorizontalDistance(ITrack track1, ITrack track2);
        int CalculateVerticalDistance(ITrack track1, ITrack track2);
        double CalucalateCourse(ITrack trackExisting, ITrack trackNew);
    }
}
