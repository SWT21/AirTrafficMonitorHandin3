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
    class IT3_SeparationMonitor_TrackCalculator
    {
        private ISeparationMonitor _separationMonitor;
        private ITrackCalculator _trackCalculator;
        private ITrack _flightTrack1;
        private ITrack _flightTrack2;
        private Dictionary<string, ITrack> _fakeTrackDict;

        [SetUp]
        public void Setup()
        {
            _trackCalculator = new TrackCalculator();
            _separationMonitor = new SeparationMonitor(_trackCalculator);
            _flightTrack1 = new FlightTrack { Tag = "1", Altitude = 500, CoordinateX = 0, CoordinateY = 0};
            _flightTrack2 = new FlightTrack { Tag = "2", Altitude = 700, CoordinateX = 1000, CoordinateY = 0};
            _fakeTrackDict = new Dictionary<string, ITrack>();
            _fakeTrackDict.Add(_flightTrack1.Tag, _flightTrack1);
            _fakeTrackDict.Add(_flightTrack2.Tag, _flightTrack2);

        }

        [Test]
        public void DetectSpearation_Calls_CalculateVerticalDistance_ReturnsCorrectValue()
        {
            _separationMonitor.DetectSpearation(_fakeTrackDict);

            Assert.That(_separationMonitor.VerticalDistance, Is.EqualTo(200));
        }

        [Test]
        public void DetectSpearation_Calls_CalculateHorizontalDistance_ReturnsCorrectValue()
        {
            _separationMonitor.DetectSpearation(_fakeTrackDict);

            Assert.That(_separationMonitor.HorizontalDistance, Is.EqualTo(1000));
        }
    }
}
