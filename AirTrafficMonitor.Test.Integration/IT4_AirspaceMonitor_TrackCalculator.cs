using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirTrafficMonitor.Classes;
using AirTrafficMonitor.Interfaces;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace AirTrafficMonitor.Test.Integration
{
    [TestFixture]
    class IT4_AirspaceMonitor_TrackCalculator
    {
        private IAirspaceMonitor _airspaceMonitor;
        private ISeparationMonitor _separationMonitor;
        private ITrackCalculator _trackCalculator;
        private ITrack _flightTrack;

        [SetUp]
        public void Setup()
        {
            _trackCalculator = new TrackCalculator();
            _airspaceMonitor = new AirspaceMonitor(10000, 10000, 90000, 90000, 500, 20000, _trackCalculator);
            _separationMonitor = new SeparationMonitor(_trackCalculator);
            _flightTrack = new FlightTrack { Tag = "1", CoordinateX = 0 };
            _airspaceMonitor.TrackDict.Add(_flightTrack.Tag, _flightTrack);
        }

        [Test]
        public void RefreshTrack_Calls_CalucalateCourse_ReturnsCorrectValue()
        {
            var newFlightTrack = new FlightTrack() {Tag = "1", CoordinateX = 1000};
            _airspaceMonitor.RefreshTrack(newFlightTrack);

            Assert.That(_airspaceMonitor.TrackCourse, Is.EqualTo(90));
        }
    }
}
