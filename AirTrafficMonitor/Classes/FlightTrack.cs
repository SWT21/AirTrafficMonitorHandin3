using System;
using System.Collections.Generic;
using AirTrafficMonitor.Interfaces;

namespace AirTrafficMonitor.Classes
{
    public class FlightTrack : ITrack
    {
        private string _tag;
        private int _coordinateX;
        private int _coordinateY;
        private int _altitude;
        private double _course;
        private double _velocity;

        public FlightTrack()
        {
            SeparationTrackList = new List<ITrack>();
            IsSeparationTrackListChanged = false;
        }

        public string Tag
        {
            get => _tag;
            set => _tag = value.Length <= 6 
                ? value 
                : throw new Exception("Tag value must be at least 6 characters");
        }

        public int CoordinateX
        {
            get => _coordinateX;
            set => _coordinateX = value >= 0
                ? value
                : throw new Exception("X coordiante must be 0 or greater");
        }

        public int CoordinateY
        {
            get => _coordinateY;
            set => _coordinateY = value >= 0
                ? value
                : throw new Exception("Y coordiante must be 0 or greater");
        }

        public int Altitude
        {
            get => _altitude;
            set => _altitude = value >= 0
                ? value
                : throw new Exception("Altitude must be 0 or greater");
        }

        public double Velocity
        {
            get => _velocity;
            set => _velocity = value >= 0
                ? value
                : throw new Exception("Velocity must be 0 or greater");
        }

        public double Course
        {
            get => _course;
            set => _course = value <= 360 && 0 <= value
                ? value
                : throw new Exception("The course must be between 0 and 360 degrees");
        }

        public DateTime UpdateTimestamp { get; set; }
        public DateTime SeparationTimestamp { get; set; }
        public List<ITrack> SeparationTrackList { get; set; }
        public bool IsSeparationTrackListChanged { get; set; }
    }
}
