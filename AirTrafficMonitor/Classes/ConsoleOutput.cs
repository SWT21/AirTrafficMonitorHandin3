using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirTrafficMonitor.Interfaces;

namespace AirTrafficMonitor.Classes
{
    public class ConsoleOutput : IOutput
    {
        public void OutputLine(string str)
        {
            Console.WriteLine(str);
        }

        public void OutputTracks(List<ITrack> tracks)
        {
            //Console.Clear();
            foreach (var track in tracks)
            {
                Console.WriteLine($"Tag:{track.Tag} | Altitude:{track.Altitude} | x:{track.CoordinateX}, y:{track.CoordinateY} | Timestamp:{track.Timestamp}.{track.Timestamp.Millisecond}");
            }
        }

        public void CleanUp()
        {
            Console.Clear();
        }
    }
}
