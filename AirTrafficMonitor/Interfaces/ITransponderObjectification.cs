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
    }
}
