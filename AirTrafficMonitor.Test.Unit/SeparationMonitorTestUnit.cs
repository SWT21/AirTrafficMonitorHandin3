using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirTrafficMonitor.Classes;
using AirTrafficMonitor.Interfaces;
using NSubstitute;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace AirTrafficMonitor.Test.Unit
{
    [TestFixture]
    class SeparationMonitorTestUnit
    {
        private ISeparationMonitor _uut;
        private ITrackCalculator _trackCalculatorSubstitute;
        private Dictionary<string, ITrack> _trackDict;

        [SetUp]
        public void Setup()
        {
            _trackCalculatorSubstitute = Substitute.For<ITrackCalculator>();
            _uut = new SeparationMonitor(_trackCalculatorSubstitute);
            _trackDict = new Dictionary<string, ITrack>();
        }

        [Test]
        public void DetectSpearation_trackDict_IsEmpty()
        {
            _uut.DetectSpearation(_trackDict);
            Assert.IsEmpty(_trackDict);
        }

        [Test]
        public void DetectSpearation_ValidParameter_CalculateVerticalDistance()
        {
            var track1 = new FlightTrack { Tag = "000000" };
            var track2 = new FlightTrack { Tag = "000001" };

            _trackDict.Add(track1.Tag, track1);
            _trackDict.Add(track2.Tag, track2);

            _uut.DetectSpearation(_trackDict);

            _trackCalculatorSubstitute.Received().CalculateVerticalDistance(track1, track2);
        }

        [Test]
        public void DetectSpearation_ValidParameter_CalculateHorizontalDistance()
        {
            var track1 = new FlightTrack { Tag = "000000" };
            var track2 = new FlightTrack { Tag = "000001" };

            _trackDict.Add(track1.Tag, track1);
            _trackDict.Add(track2.Tag, track2);

            _uut.DetectSpearation(_trackDict);

            _trackCalculatorSubstitute.Received().CalculateHorizontalDistance(track1, track2);
        }

        //CalculateVerticalDistance = 0, CalculateHorizontalDistance = 0
        [TestCase(1000, 1000, 1000, 1000, 1000, 1000)]
        //CalculateVerticalDistance = 5000, CalculateHorizontalDistance = 300
        [TestCase(0, 300, 3000, 0, 4000, 0)]
        public void DetectSpearation_SeparationEvent_IsRaised(int alt1, int alt2, int x1, int x2, int y1, int y2)
        {
            _uut = new SeparationMonitor(new TrackCalculator());

            bool isRaisedEvent = false;
            _uut.SeparationEvent += (o, e) => isRaisedEvent = true;

            var track1 = new FlightTrack { Tag = "000000", Altitude = alt1, CoordinateX = x1, CoordinateY = y1 };
            var track2 = new FlightTrack { Tag = "000001", Altitude = alt2, CoordinateX = x2, CoordinateY = y2 };

            _trackDict.Add(track1.Tag, track1);
            _trackDict.Add(track2.Tag, track2);

            _uut.DetectSpearation(_trackDict);

            Assert.IsTrue(isRaisedEvent);
        }

        //CalculateVerticalDistance ~ 5001.6, CalculateHorizontalDistance = 301
        [TestCase(0, 301, 3001, 0, 4000, 0)]
        public void DetectSpearation_SeparationDoneEvent_IsRaised(int alt1, int alt2, int x1, int x2, int y1, int y2)
        {
            _uut = new SeparationMonitor(new TrackCalculator());

            bool isRaisedEvent = false;
            _uut.SeparationDoneEvent += (o, e) => isRaisedEvent = true;

            var track1 = new FlightTrack { Tag = "000000", Altitude = alt1, CoordinateX = x1, CoordinateY = y1 };
            var track2 = new FlightTrack { Tag = "000001", Altitude = alt2, CoordinateX = x2, CoordinateY = y2 };

            _trackDict.Add(track1.Tag, track1);
            _trackDict.Add(track2.Tag, track2);

            _uut.DetectSpearation(_trackDict);

            Assert.IsTrue(isRaisedEvent);
        }
    }
}
