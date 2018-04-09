using System;
using AirTrafficMonitor.Interfaces;

namespace AirTrafficMonitor.Classes
{
    public class FlightTrack : ITrack
    {
        private string _tag;
        private int _coordinateXOld;
        private int _coordinateYOld;

        public FlightTrack(string transpoderData)
        {
            ExtractTranponderData(transpoderData);
        }

        public void ExtractTranponderData(string transponderData)
        {
            var split = transponderData.Split(';');

            Tag = split[0];
            CoordinateX = Int32.Parse(split[1]);
            CoordinateY = Int32.Parse(split[2]);
            Altitude = Int32.Parse(split[3]);
            Timestamp = DateTime.ParseExact(split[4], "yyyyMMddHHmmssfff", null);
            Velocity = 0;
            Course = 0;

        }

        public string Tag
        {
            get => _tag;
            private set
            {
                if (value.Length <= 6)
                {
                    _tag = value;
                }
                else
                {
                    throw new Exception("Tag value must be at least 6 characters");
                }
                
            }
        }

        public int CoordinateX { get; private set; }
        public int CoordinateY { get; private set; }
        public int Altitude { get; private set; }
        public double Velocity { get; private set; }
        public DateTime Timestamp { get; private set; }
        public double Course { get; private set; }

        public void RefreshTrack(ITrack track)
        {
            if (track.Tag != _tag)
            {
                throw new Exception("The track tag doenst match the tag of this object.");
            }

            _coordinateXOld = CoordinateX;
            _coordinateYOld = CoordinateY;

            CoordinateX = track.CoordinateX;
            CoordinateY = track.CoordinateY;
            
            double timeDiffSec = track.Timestamp.Subtract(Timestamp).TotalSeconds;
            double distanceTraveledMeters = Math.Sqrt(Math.Pow((_coordinateXOld - CoordinateX), 2) + Math.Pow((_coordinateYOld - CoordinateY), 2));

            Velocity = distanceTraveledMeters / timeDiffSec;
            Course = CalucalateCourse(CoordinateX, CoordinateY);

            Altitude = track.Altitude;

        }
        private double CalucalateCourse(int coordinateX, int coordinateY)
        {
            int coordinateXDiff = coordinateX - _coordinateXOld;
            int coordinateYDiff = coordinateX - _coordinateXOld;

            // X = neg , Y = pos
            if (coordinateXDiff < 0 && coordinateYDiff > 0) 
            {
                var a = Math.Abs(coordinateYDiff);
                var b = Math.Abs(coordinateXDiff);
                var c = Math.Sqrt(Math.Pow(a, 2) + Math.Pow(b, 2));

                return Math.Acos((1.0 / 2.0) * (Math.Pow(a, 2) - Math.Pow(b, 2) + Math.Pow(c, 2)) / (a * c)) * (180 / Math.PI) + 270;
            }

            // Y = neg , X = pos
            if (coordinateYDiff < 0 && coordinateXDiff > 0) 
            {
                var a = Math.Abs(coordinateYDiff);
                var b = Math.Abs(coordinateXDiff);
                var c = Math.Sqrt(Math.Pow(a, 2) + Math.Pow(b, 2));
                
                return Math.Acos((1.0 / 2.0) * (Math.Pow(a, 2) - Math.Pow(b, 2) + Math.Pow(c, 2)) / (a * c)) * (180 / Math.PI) + 90;
            }

            // X = neg , Y = neg
            if (coordinateXDiff < 0 && coordinateYDiff < 0) 
            {
                var a = Math.Abs(coordinateYDiff);
                var b = Math.Abs(coordinateXDiff);
                var c = Math.Sqrt(Math.Pow(a, 2) + Math.Pow(b, 2));

                return Math.Acos((1.0 / 2.0) * (Math.Pow(a, 2) - Math.Pow(b, 2) + Math.Pow(c, 2)) / (a * c)) * (180 / Math.PI) + 180;
            }

            // X = pos , Y = 0
            if (coordinateXDiff > 0 && coordinateYDiff == 0)
            {
                return 90; // Straight East
            }

            // X = neg , Y = 0
            if (coordinateXDiff < 0 && coordinateYDiff == 0)
            {
                return 270; // Straight West
            }

            // X = 0 , Y = pos
            if (coordinateXDiff == 0 && coordinateYDiff > 0)
            {
                return 0; // Straight North
            }

            // X = 0 , Y = neg
            if (coordinateXDiff == 0 && coordinateYDiff < 0)
            {
                return 180; // Straight South
            }

            // X = pos , Y = pos
            {
                var a = Math.Abs(coordinateYDiff);
                var b = Math.Abs(coordinateXDiff);
                var c = Math.Sqrt(Math.Pow(a, 2) + Math.Pow(b, 2));

                return Math.Acos((1.0 / 2.0) * (Math.Pow(a, 2) - Math.Pow(b, 2) + Math.Pow(c, 2)) / (a * c)) * (180 / Math.PI);
            }


        } 
    }
}
