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

        private readonly int _minHorizontalDistance, _minVerticalDistance;
        private readonly ITrackCalculator _trackCalculator;

        public SeparationMonitor(ITrackCalculator trackCalculator)
        {
            _minHorizontalDistance = 5000;
            _minVerticalDistance = 300;

            _trackCalculator = trackCalculator;
        }

        public void DetectSpearation(Dictionary<string, ITrack> trackDict)
        {
            if (trackDict.Count < 2) return;

            foreach (var track1 in trackDict)
            {
                foreach (var track2 in trackDict)
                {
                    if (track1.Key == track2.Key) continue;

                    VerticalDistance = _trackCalculator.CalculateVerticalDistance(track1.Value, track2.Value);
                    HorizontalDistance = _trackCalculator.CalculateHorizontalDistance(track1.Value, track2.Value);

                    if (VerticalDistance <= _minVerticalDistance && HorizontalDistance <= _minHorizontalDistance)
                        SeparationEvent?.Invoke(this, new SeparationEventArgs(track1.Value, track2.Value));
                    else
                        SeparationDoneEvent?.Invoke(this, new SeparationEventArgs(track1.Value, track2.Value));
                }
            }
        }

        public int VerticalDistance { get; private set; }
        public double HorizontalDistance { get; private set; }
    }
}
