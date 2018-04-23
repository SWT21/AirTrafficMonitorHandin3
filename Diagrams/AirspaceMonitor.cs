using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirTrafficMonitor.Events;
using AirTrafficMonitor.Interfaces;

namespace AirTrafficMonitor.Classes
{
    public class AirspaceMonitor : IAirspaceMonitor
    {
        private readonly int _north, _east, _south, _west;
        private readonly int _minAltitude, _maxAltitude;
        private readonly ISeparationMonitor _separationMonitor;
        private readonly ITrackCalculator _trackCalculator;

        public Dictionary<string, ITrack> TrackDict { get; }

        public bool IsDoneDetectSpearation { get; private set; }

        public AirspaceMonitor(int south, int west, int north, int east, int minAltitude, int maxAltitude, ITrackCalculator trackCalculator)
        {
            if (north <= south || east <= west)
                throw new Exception("Wrong coordinates for airspace. Rule: north <= south || east <= west");
            if (south < 10000 || west < 10000 || north > 90000 || east > 90000)
                throw new Exception("Wrong coordinates for airspace. Rule: south < 10000 || west < 10000 || north > 90000 || east > 90000");
            if (maxAltitude <= minAltitude)
                throw new Exception("Wrong altitude for airspace. Rule: maxAltitude <= minAltitude");
            if (maxAltitude > 20000 || minAltitude < 500)
                throw new Exception("Wrong altitude for airspace. Rule: maxAltitude > 20000 || minAltitude < 500");

            _north = north;
            _east = east;
            _south = south;
            _west = west;
            _maxAltitude = maxAltitude;
            _minAltitude = minAltitude;

            TrackDict = new Dictionary<string, ITrack>();

            _trackCalculator = trackCalculator;

            _separationMonitor = new SeparationMonitor(_trackCalculator);
            _separationMonitor.SeparationEvent += SeparationMonitorOnSeparationEvent;
            _separationMonitor.SeparationDoneEvent += SeparationMonitorOnSeparationDoneEvent;
            
            IsDoneDetectSpearation = true;
        }

        private static void SeparationMonitorOnSeparationEvent(object sender, SeparationEventArgs e)
        {
            if (e.Track1.SeparationTrackList.Contains(e.Track2)) return;

            e.Track1.SeparationTimestamp = DateTime.Now;
            e.Track1.SeparationTrackList.Add(e.Track2);
            e.Track1.IsSeparationTrackListChanged = true;
        }

        private static void SeparationMonitorOnSeparationDoneEvent(object sender, SeparationEventArgs e)
        {
            if (!e.Track1.SeparationTrackList.Contains(e.Track2)) return;

            e.Track1.SeparationTrackList.Remove(e.Track2);
            e.Track1.IsSeparationTrackListChanged = true;

            if (e.Track1.SeparationTrackList.Count == 0)
                e.Track1.SeparationTimestamp = DateTime.MinValue;
        }

        public void AddTrack(ITrack trackNew)
        {
            IsDoneDetectSpearation = false;

            if (!IsInAirspace(trackNew))
            {
                TrackDict.Remove(trackNew.Tag);
                IsDoneDetectSpearation = true;
            }
            else
            {
                if (!IsInTrackDict(trackNew))
                    TrackDict.Add(trackNew.Tag, trackNew);
                else
                    RefreshTrack(trackNew);

                _separationMonitor.DetectSpearation(TrackDict);
                IsDoneDetectSpearation = true;
            }
        }

        public void RefreshTrack(ITrack track)
        {
            foreach (var trackExisting in TrackDict)
            {
                if (trackExisting.Key != track.Tag) continue;
                
                double timeDiffSec = track.UpdateTimestamp.Subtract(trackExisting.Value.UpdateTimestamp).TotalSeconds;
                double distanceTraveledMeters = Math.Sqrt(Math.Pow((trackExisting.Value.CoordinateX - track.CoordinateX), 2) + Math.Pow((trackExisting.Value.CoordinateY - track.CoordinateY), 2));

                trackExisting.Value.Course = _trackCalculator.CalucalateCourse(track, trackExisting.Value);

                trackExisting.Value.CoordinateX = track.CoordinateX;
                trackExisting.Value.CoordinateY = track.CoordinateY;

                trackExisting.Value.Velocity = distanceTraveledMeters / timeDiffSec;
                trackExisting.Value.UpdateTimestamp = track.UpdateTimestamp;
                trackExisting.Value.Altitude = track.Altitude;
            }
        }

        public bool IsInAirspace(ITrack track)
        {
            if (track.CoordinateX < _west || track.CoordinateX > _east) return false;
            if (track.CoordinateY < _south || track.CoordinateY > _north) return false;
            if (track.Altitude < _minAltitude || track.Altitude > _maxAltitude) return false;

            return true;
        }

        public bool IsInTrackDict(ITrack track)
        {
            foreach (var trackExisting in TrackDict)
                if (trackExisting.Value.Tag == track.Tag) return true;

            return false;
        }
    }
}
