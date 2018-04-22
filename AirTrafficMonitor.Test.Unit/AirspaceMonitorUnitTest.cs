using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using AirTrafficMonitor.Classes;
using AirTrafficMonitor.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace AirTrafficMonitor.Test.Unit
{
    [TestFixture]
    class AirspaceMonitorUnitTest
    {
        private IAirspaceMonitor _uut;
        private ITrackCalculator _trackCalculatorSubstitute;
        private ITrack _flightTrack;

        [SetUp]
        public void Setup()
        {
            _trackCalculatorSubstitute = Substitute.For<ITrackCalculator>();
            _uut = new AirspaceMonitor(10000, 10000, 90000, 90000, 500, 20000, _trackCalculatorSubstitute);
            _flightTrack = new FlightTrack();
        }

        [TestCase(10000, 10000, 10000, 10000, 500, 20000)]
        [TestCase(10001, 10001, 10000, 10000, 500, 20000)]
        [TestCase(9999, 10000, 90000, 90000, 500, 20000)]
        [TestCase(10000, 9999, 90000, 90000, 500, 20000)]
        [TestCase(10000, 10000, 90001, 90000, 500, 20000)]
        [TestCase(10000, 10000, 90000, 90001, 500, 20000)]
        [TestCase(9999, 9999, 90001, 90001, 500, 20000)]
        public void Ctor_WrongCoordinatesForAirspace_ExceptionExpected(int south, int west, int north, int east, int minAltitude, int maxAltitude)
        {
            Assert.Throws<System.Exception>(() => _uut = new AirspaceMonitor(south,west,north,east,minAltitude,maxAltitude,_trackCalculatorSubstitute));
        }

        [TestCase(10000, 10000, 90000, 90000, 500, 500)]
        [TestCase(10000, 10000, 90000, 90000, 501, 500)]
        [TestCase(10000, 10000, 90000, 90000, 500, 20001)]
        [TestCase(10000, 10000, 90000, 90000, 499, 20000)]
        [TestCase(10000, 10000, 90000, 90000, 499, 20001)]
        public void Ctor_WrongAltitudeForAirspace_ExceptionExpected(int south, int west, int north, int east, int minAltitude, int maxAltitude)
        {
            Assert.Throws<System.Exception>(() => _uut = new AirspaceMonitor(south, west, north, east, minAltitude, maxAltitude, _trackCalculatorSubstitute));
        }

        [TestCase(10000, 10000, 500)]
        [TestCase(90000, 90000, 20000)]
        [TestCase(45000, 45000, 10000)]
        public void IsInAirspace_TrackWithinAirspace_ReturnsTrue(int x, int y, int a)
        {
            _flightTrack = new FlightTrack { CoordinateX = x, CoordinateY = y, Altitude = a };
            
            Assert.That(_uut.IsInAirspace(_flightTrack), Is.EqualTo(true));
        }

        [TestCase(9999, 9999, 499)]
        [TestCase(90001, 90001, 20001)]
        public void IsInAirspace_TrackNotWithinAirspace_ReturnsFalse(int x, int y, int a)
        {
            _flightTrack = new FlightTrack { CoordinateX = x, CoordinateY = y, Altitude = a };

            Assert.That(_uut.IsInAirspace(_flightTrack), Is.EqualTo(false));
        }

        [TestCase(10000, 10000, 500)]
        [TestCase(90000, 90000, 20000)]
        [TestCase(45000, 45000, 10000)]
        public void IsInTrackDict_TrackWithinAirspace_AlreadyInTrackDict_ReturnsTrue(int x, int y, int a)
        {
            _flightTrack = new FlightTrack { CoordinateX = x, CoordinateY = y, Altitude = a };
            _uut.AddTrack(_flightTrack);

            Assert.That(_uut.IsInTrackDict(_flightTrack), Is.EqualTo(true));
        }

        [TestCase(10000, 10000, 500)]
        [TestCase(90000, 90000, 20000)]
        [TestCase(45000, 45000, 10000)]
        public void IsInTrackDict_TrackWithinAirspace_NotAlreadyInTrackDict_ReturnsFalse(int x, int y, int a)
        {
            _flightTrack = new FlightTrack { CoordinateX = x, CoordinateY = y, Altitude = a };

            Assert.That(_uut.IsInTrackDict(_flightTrack), Is.EqualTo(false));
        }

        [TestCase(10000, 10000, 500)]
        [TestCase(90000, 90000, 20000)]
        [TestCase(45000, 45000, 10000)]
        public void AddTrack_TrackWithinAirspace_NotAlreadyInTrackDict_AddedToTrackDict(int x, int y, int a)
        {
            _flightTrack = new FlightTrack { CoordinateX = x, CoordinateY = y, Altitude = a };
            _uut.AddTrack(_flightTrack);

            Assert.That(_uut.TrackDict.Count, Is.EqualTo(1));
        }


        [TestCase(9999, 9999, 499)]
        [TestCase(90001, 90001, 20001)]
        public void AddTrack_TrackNotWithinAirspace_NotAlreadyInTrackDict_NotAddedToTrackDict(int x, int y, int a)
        {
            _flightTrack = new FlightTrack { CoordinateX = x, CoordinateY = y, Altitude = a };
            _uut.AddTrack(_flightTrack);

            Assert.That(_uut.TrackDict.Count, Is.EqualTo(0));
        }

        [TestCase(10000, 10000, 500)]
        public void AddTrack_TrackWithinAirspace_AlreadyInTrackDict_NotAddedToTrackDict(int x, int y, int a)
        {
            _flightTrack = new FlightTrack { Tag = "xxxxxx", CoordinateX = x, CoordinateY = y, Altitude = a };
            _uut.AddTrack(_flightTrack);
            _flightTrack = new FlightTrack { Tag = "xxxxxx", CoordinateX = x+1, CoordinateY = y+1, Altitude = a };
            _uut.AddTrack(_flightTrack);

            Assert.That(_uut.TrackDict.Count, Is.EqualTo(1));
        }

        [TestCase(10000, 10000, 500)]
        public void RefreshTrack_TrackWithinAirspace_AlreadyInTrackDict_CalucalateCourseCalled(int x, int y, int a)
        {
            _flightTrack = new FlightTrack() { Tag = "xxxxxx", CoordinateX = x, CoordinateY = y, Altitude = a };
            _uut.AddTrack(_flightTrack);

            var flightTrackNew = new FlightTrack() { Tag = "xxxxxx", CoordinateX = 20000, CoordinateY = 20000, Altitude = a };

            _uut.RefreshTrack(flightTrackNew);

            _trackCalculatorSubstitute.Received().CalucalateCourse(flightTrackNew, _flightTrack);
        }

        [TestCase(10000, 10000, 500)]
        public void RefreshTrack_TrackWithinAirspace_AlreadyInTrackDict_SetNewCoordinateXY(int x, int y, int a)
        {
            _flightTrack = new FlightTrack() { Tag = "xxxxxx", CoordinateX = x, CoordinateY = y, Altitude = a };
            _uut.AddTrack(_flightTrack);

            var flightTrackNew = new FlightTrack() { Tag = "xxxxxx", CoordinateX = 20000, CoordinateY = 20000, Altitude = a };

            _uut.RefreshTrack(flightTrackNew);

            Assert.That(_flightTrack.CoordinateX, Is.EqualTo(flightTrackNew.CoordinateX));
            Assert.That(_flightTrack.CoordinateY, Is.EqualTo(flightTrackNew.CoordinateY));
        }

        [TestCase(10000, 10000, 500)]
        public void RefreshTrack_TrackWithinAirspace_AlreadyInTrackDict_CorrectVelocity(int x, int y, int a)
        {
            _flightTrack = new FlightTrack() { Tag = "xxxxxx", CoordinateX = x, CoordinateY = y, Altitude = a };
            _flightTrack.UpdateTimestamp = new DateTime(1900, 1, 1, 0, 0, 0);
            _uut.AddTrack(_flightTrack);

            var flightTrackNew = new FlightTrack() { Tag = "xxxxxx", CoordinateX = 20000, CoordinateY = y, Altitude = a };
            flightTrackNew.UpdateTimestamp = new DateTime(1900, 1, 1, 0, 0, 50);
            _uut.RefreshTrack(flightTrackNew);

            Assert.That(_flightTrack.Velocity, Is.EqualTo(200));
        }

        [TestCase(10000, 10000, 500)]
        public void RefreshTrack_TrackWithinAirspace_AlreadyInTrackDict_SetCorrectUpdateTimestamp(int x, int y, int a)
        {
            _flightTrack = new FlightTrack() { Tag = "xxxxxx", CoordinateX = x, CoordinateY = y, Altitude = a };
            _flightTrack.UpdateTimestamp = new DateTime(1900, 1, 1, 0, 0, 0);
            _uut.AddTrack(_flightTrack);

            var flightTrackNew = new FlightTrack() { Tag = "xxxxxx", CoordinateX = 20000, CoordinateY = y, Altitude = a };
            flightTrackNew.UpdateTimestamp = new DateTime(1900, 1, 1, 0, 0, 50);
            _uut.RefreshTrack(flightTrackNew);

            Assert.That(_flightTrack.UpdateTimestamp, Is.EqualTo(flightTrackNew.UpdateTimestamp));
        }

        [TestCase(10000, 10000, 500)]
        public void RefreshTrack_TrackWithinAirspace_AlreadyInTrackDict_SetCorrectAltitude(int x, int y, int a)
        {
            _flightTrack = new FlightTrack() { Tag = "xxxxxx", CoordinateX = x, CoordinateY = y, Altitude = a };
            _uut.AddTrack(_flightTrack);

            var flightTrackNew = new FlightTrack() { Tag = "xxxxxx", CoordinateX = 20000, CoordinateY = y, Altitude = 1000 };
            _uut.RefreshTrack(flightTrackNew);

            Assert.That(_flightTrack.Altitude, Is.EqualTo(flightTrackNew.Altitude));
        }
    }
}
