using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirTrafficMonitor.Interfaces;

namespace AirTrafficMonitor.Classes
{
    public class LogfileOutput : IOutput
    {
        public void OutputLine(string str)
        {
            throw new NotImplementedException();
        }

        public void OutputTracks(List<ITrack> tracks)
        {
            throw new NotImplementedException();
        }

        public void CleanUp()
        {
            throw new NotImplementedException();
        }
    }
}
