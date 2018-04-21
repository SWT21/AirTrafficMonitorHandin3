using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirTrafficMonitor.Interfaces;

namespace AirTrafficMonitor.Classes
{
    public class TrackCalculator : ITrackCalculator
    {
        public double CalculateHorizontalDistance(ITrack track1, ITrack track2)
        {
            int coordinateXDelta = track1.CoordinateX - track2.CoordinateX;
            int coordinateYDelta = track1.CoordinateY - track2.CoordinateY;

            var x = Math.Abs(coordinateXDelta);
            var y = Math.Abs(coordinateYDelta);

            return Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
        }

        public int CalculateVerticalDistance(ITrack track1, ITrack track2)
        {
            return Math.Abs(track1.Altitude - track2.Altitude);
        }

        public double CalucalateCourse(ITrack trackNew, ITrack trackExisting)
        {
            int coordinateXDelta = trackNew.CoordinateX - trackExisting.CoordinateX;
            int coordinateYDelta = trackNew.CoordinateY - trackExisting.CoordinateY;

            // X = 0 , Y = positive
            if (coordinateXDelta == 0 && coordinateYDelta > 0) return 0; // Straight North

            // X = positive , Y = 0
            if (coordinateXDelta > 0 && coordinateYDelta == 0) return 90; // Straight East

            // X = 0 , Y = negative
            if (coordinateXDelta == 0 && coordinateYDelta < 0) return 180; // Straight South

            // X = negative , Y = 0
            if (coordinateXDelta < 0 && coordinateYDelta == 0) return 270; // Straight West

            // X = positive, Y = positive 
            if (coordinateXDelta > 0 && coordinateYDelta > 0)
                return CourseFormula(coordinateXDelta, coordinateYDelta, 0);

            // X = positive, Y = negative 
            if (coordinateXDelta > 0 && coordinateYDelta < 0)
                return CourseFormula(coordinateXDelta, coordinateYDelta, 90);

            // X = negative , Y = negative
            if (coordinateXDelta < 0 && coordinateYDelta < 0)
                return CourseFormula(coordinateXDelta, coordinateYDelta, 180);

            // X = negative , Y = positive
            if (coordinateXDelta < 0 && coordinateYDelta > 0)
                return CourseFormula(coordinateXDelta, coordinateYDelta, 270);

            return -1;
        }

        private static double CourseFormula(int coordinateXDelta, int coordinateYDelta, int quadrantDegrees)
        {
            var x = Math.Abs(coordinateXDelta);
            var y = Math.Abs(coordinateYDelta);
            var dist = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));

            return Math.Acos((1.0 / 2.0) * (Math.Pow(y, 2) - Math.Pow(x, 2) + Math.Pow(dist, 2)) / (y * dist)) * (180 / Math.PI) + quadrantDegrees;
        }
    }
}
