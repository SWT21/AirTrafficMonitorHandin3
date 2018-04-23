using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AirTrafficMonitor.Interfaces
{
    public interface IOutput
    {
        void OutputString(string str);
        void OutputDictionary(Dictionary<string, ITrack> trackDict);
        void OutputSeparationEvents(Dictionary<string, ITrack> trackDict);
        void CleanUp();
    }
}
