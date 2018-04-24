using System;
using AirTrafficMonitor.Interfaces;
using TransponderReceiver;

namespace AirTrafficMonitor.Classes
{
    public class TransponderObjectification : ITransponderObjectification
    {
        private readonly IAirspaceMonitor _airspaceMonitor;
        public ITrack Track { get; private set; }
        public ITransponderReceiver TransponderReceiver { get; }
        public IOutput ConsoleOutput { get; set; }
        public IOutput LogfileOutput { get; set; }

        public TransponderObjectification(ITransponderReceiver transponderReceiver, IAirspaceMonitor airspaceMonitor)
        {
            TransponderReceiver = transponderReceiver;
            TransponderReceiver.TransponderDataReady += ReceiverOnTransponderDataReady;

            _airspaceMonitor = airspaceMonitor;
            
            ConsoleOutput = new ConsoleOutput();
            LogfileOutput = new LogfileOutput();
        }

        public void ReceiverOnTransponderDataReady(object sender, RawTransponderDataEventArgs e)
        {
            foreach (var data in e.TransponderData)
            {
                Track = new FlightTrack();
                ObjectifyTransponderData(data, Track);

                // Block event until DetectSeparation is done
                while (!_airspaceMonitor.IsDoneDetectSpearation) { }

                _airspaceMonitor.AddTrack(Track);
            }

            ConsoleOutput.OutputDictionary(_airspaceMonitor.TrackDict);
            LogfileOutput.OutputDictionary(_airspaceMonitor.TrackDict);
            LogfileOutput.OutputSeparationEvents(_airspaceMonitor.TrackDict);
        }

        public void ObjectifyTransponderData(string transponderData, ITrack track)
        {
            var split = transponderData.Split(';');

            track.Tag = split[0];
            track.CoordinateX = int.Parse(split[1]);
            track.CoordinateY = int.Parse(split[2]);
            track.Altitude = int.Parse(split[3]);
            track.UpdateTimestamp = DateTime.ParseExact(split[4], "yyyyMMddHHmmssfff", null);
            track.Velocity = 0;
            track.Course = 0;
        }
    }
}