using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransponderReceiver;

namespace AirTrafficMonitor.Interfaces
{
    public interface ITransponderObjectification
    {
        ITransponderReceiver TransponderReceiver { get; }
        void ReceiverOnTransponderDataReady(object sender, RawTransponderDataEventArgs e);
        void ObjectifyTransponderData(string transponderData, ITrack track);
        ITrack Track { get; }
        IOutput ConsoleOutput { get; set; }
        IOutput LogfileOutput { get; set; }
    }
}
