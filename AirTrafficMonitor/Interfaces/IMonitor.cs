using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirTrafficMonitor.Interfaces
{
    public interface IMonitor
    {
        string MonitorName { get; set; }
        string MonitorType { get; set; }
        List<ITrack> TrackList { get; }
        void AddTrack(string data);
    }
}
