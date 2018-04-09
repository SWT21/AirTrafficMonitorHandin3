using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirTrafficMonitor.Interfaces;

namespace AirTrafficMonitor.Classes
{
    public class AirspaceMonitor : IMonitor
    {
        private int _north, _east, _south, _west;
        private int _minAltitude, _maxAltitude;
        public List<ITrack> TrackList { get; private set; } = new List<ITrack>();

        public AirspaceMonitor(int south, int west, int north, int east, int minAltitude, int maxAltitude)
        {
            if (north <= south || east <= west)
                throw new Exception("Wrong coordinates for airspace. Rules: north <= south || east <= west");
            if (south < 10000 || west < 10000 || north > 90000 || east > 90000)
                throw new Exception("Wrong coordinates for airspace. Rules: south < 10000 || west < 10000 || north > 90000 || east > 90000");
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
        }

        public string MonitorName { get; set; }
        public string MonitorType { get; set; }

        public void AddTrack(string transponderData)
        {
            var newTrack = new FlightTrack(transponderData);

            if (IsWithinAirspace(newTrack))
            {
                TrackList.Add(newTrack);
            }
        }

        public bool IsWithinAirspace(FlightTrack track)
        {
            if (track.CoordinateX > _west && track.CoordinateX < _east )
            {
                if (track.CoordinateY > _south && track.CoordinateY < _north)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            } 
        }

    }
}
