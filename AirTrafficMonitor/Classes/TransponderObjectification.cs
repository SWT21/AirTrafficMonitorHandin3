using AirTrafficMonitor.Interfaces;
using TransponderReceiver;

namespace AirTrafficMonitor.Classes
{
    public class TransponderObjectification
    {
        public ITransponderReceiver Receiver { get; private set; }
        public IMonitor AirspaceMonitor { get; private set; } = new AirspaceMonitor(10000,10000,90000,90000,500,20000);
        public IOutput Output { get; set; } = new ConsoleOutput();

        public TransponderObjectification(ITransponderReceiver receiver)
        {
            Receiver = receiver;
            receiver.TransponderDataReady += ReceiverOnTransponderDataReady;
        }

        private void ReceiverOnTransponderDataReady(object sender, RawTransponderDataEventArgs rawTransponderDataEventArgs)
        {
            foreach (var data in rawTransponderDataEventArgs.TransponderData)
            {
                AirspaceMonitor.AddTrack(data);
            }

            Output.OutputTracks(AirspaceMonitor.TrackList);
        }
    
    }
}