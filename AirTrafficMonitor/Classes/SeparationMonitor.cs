using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirTrafficMonitor.Events;
using AirTrafficMonitor.Interfaces;

namespace AirTrafficMonitor.Classes
{
    public class SeparationMonitor : ISeparationMonitor
    {
        public event EventHandler<SeparationEventArgs> SeparationEvent;
        public event EventHandler<SeparationEventArgs> SeparationDoneEvent;

        private readonly int _maxHorizontalDistance, _maxVerticalDistance;

        public SeparationMonitor()
        {
            _maxHorizontalDistance = 5000;
            _maxVerticalDistance = 300;
        }

        public void DetectSpearation(Dictionary<string, ITrack> trackDict)
        {
            if (trackDict.Count < 2) return;

            foreach (var track1 in trackDict)
            {
                foreach (var track2 in trackDict)
                {
                    if (track1.Key == track2.Key) continue;

                    int verticalDistance = CalculateVerticalDistance(track1.Value, track2.Value);
                    double horizontalDistance = CalculateHorizontalDistance(track1.Value, track2.Value);

                    if (verticalDistance < _maxVerticalDistance && horizontalDistance < _maxHorizontalDistance)
                        SeparationEvent?.Invoke(this, new SeparationEventArgs(track1.Value, track2.Value));
                    else
                        SeparationDoneEvent?.Invoke(this, new SeparationEventArgs(track1.Value, track2.Value));
                }
            }
        }

        private static double CalculateHorizontalDistance(ITrack track1, ITrack track2)
        {
            int coordinateXDelta = track1.CoordinateX - track2.CoordinateX;
            int coordinateYDelta = track1.CoordinateY - track2.CoordinateY;

            var x = Math.Abs(coordinateXDelta);
            var y = Math.Abs(coordinateYDelta);

            return Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
        }

        private static int CalculateVerticalDistance(ITrack track1, ITrack track2)
        {
            return Math.Abs(track1.Altitude - track2.Altitude);
        }
    }
}
