using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirTrafficMonitor.Interfaces
{
    public interface IOutput
    {
        void OutputLine(string str);
        void OutputTracks(List<ITrack> tracks);
        void CleanUp();
    }
}
