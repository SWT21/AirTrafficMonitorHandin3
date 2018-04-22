using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirTrafficMonitor.Classes;
using AirTrafficMonitor.Interfaces;
using NUnit.Framework;

namespace AirTrafficMonitor.Test.Unit
{
    [TestFixture]
    class TrackCalculatorTestUnit
    {
        private ITrackCalculator _uut;
        private ITrack _track1;
        private ITrack _track2;

        [SetUp]
        public void Setup()
        {
            _uut = new TrackCalculator();
            _track1 = new FlightTrack();
            _track2 = new FlightTrack();
        }

        [TestCase(0,3000,0,4000,5000)]
        public void CalculateHorizontalDistance_Returns_CorrectValue(int x1, int x2, int y1, int y2, int result)
        {
            _track1 = new FlightTrack { CoordinateX = x1, CoordinateY = y1};
            _track2 = new FlightTrack { CoordinateX = x2, CoordinateY = y2};

            Assert.That(_uut.CalculateHorizontalDistance(_track1, _track2), Is.EqualTo(result));
        }

        [TestCase(0, 300, 300)]
        [TestCase(1500, 2500, 1000)]
        public void CalculateVerticalDistance_Returns_CorrectValue(int alt1, int alt2, int result)
        {
            _track1 = new FlightTrack { Altitude = alt1 };
            _track2 = new FlightTrack { Altitude = alt2 };

            Assert.That(_uut.CalculateVerticalDistance(_track1, _track2), Is.EqualTo(result));
        }

        [TestCase(0, 0, 100, 0, 0)]
        [TestCase(100, 0, 0, 0, 90)]
        [TestCase(0, 0, 0, 100, 180)]
        [TestCase(0, 100, 0, 0, 270)]
        [TestCase(100, 0, 100, 0, 45)]
        [TestCase(100, 0, 0, 100, 135)]
        [TestCase(0, 100, 0, 100, 225)]
        [TestCase(0, 100, 100, 0, 315)]
        public void CalucalateCourse_Returns_CorrectValue(int x1, int x2, int y1, int y2, double result)
        {
            _track1 = new FlightTrack { CoordinateX = x1, CoordinateY = y1 };
            _track2 = new FlightTrack { CoordinateX = x2, CoordinateY = y2 };

            Assert.That(_uut.CalucalateCourse(_track1, _track2), Is.EqualTo(result).Within(.001));
        }
    }
}
