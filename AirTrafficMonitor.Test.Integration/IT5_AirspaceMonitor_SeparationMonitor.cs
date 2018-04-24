using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirTrafficMonitor.Classes;
using AirTrafficMonitor.Events;
using AirTrafficMonitor.Interfaces;
using NSubstitute;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace AirTrafficMonitor.Test.Integration
{
    [TestFixture]
    class IT5_AirspaceMonitor_SeparationMonitor
    {
        private IAirspaceMonitor _airspaceMonitor;
        private ITrackCalculator _trackCalculator;
        private ITrack _flightTrack1;
        private ITrack _flightTrack2;

        [SetUp]
        public void Setup()
        {
            _trackCalculator = Substitute.For<ITrackCalculator>();
            _airspaceMonitor = new AirspaceMonitor(10000, 10000, 90000, 90000, 500, 20000, new TrackCalculator());
            _flightTrack1 = new FlightTrack() {Tag = "1", Altitude = 500, CoordinateX = 10000, CoordinateY = 10000};
            _flightTrack2 = new FlightTrack() {Tag = "2", Altitude = 700, CoordinateX = 12000, CoordinateY = 12000};
        }

        [Test]
        public void AddTrack_Calls_DetectSeparation_TrackCalculatorRecievedCalculateVerticalDistance()
        {
            _airspaceMonitor = new AirspaceMonitor(10000, 10000, 90000, 90000, 500, 20000, _trackCalculator);

            _airspaceMonitor.AddTrack(_flightTrack1);
            _airspaceMonitor.AddTrack(_flightTrack2);
            
            _trackCalculator.Received().CalculateVerticalDistance(_flightTrack1, _flightTrack2);
        }

        [Test]
        public void AddTrack_Calls_DetectSeparation_TrackCalculatorRecievedCalculateHorizontalDistance()
        {
            _airspaceMonitor = new AirspaceMonitor(10000, 10000, 90000, 90000, 500, 20000, _trackCalculator);

            _airspaceMonitor.AddTrack(_flightTrack1);
            _airspaceMonitor.AddTrack(_flightTrack2);

            _trackCalculator.Received().CalculateHorizontalDistance(_flightTrack1, _flightTrack2);
        }


        [Test]
        public void AddTrack_Calls_DetectSeparation_SeparationEventWasRaised()
        {
            bool eventRaised = false;

            _airspaceMonitor.SeparationMonitor.SeparationEvent += delegate(object sender, SeparationEventArgs e)
            {
                eventRaised = true;
            };

            _airspaceMonitor.AddTrack(_flightTrack1);
            _airspaceMonitor.AddTrack(_flightTrack2);

            Assert.That(eventRaised, Is.EqualTo(true));
        }

        [Test]
        public void AddTrack_Calls_DetectSeparation_SeparationDoneEventWasRaised()
        {
            bool eventRaised = false;

            _airspaceMonitor.SeparationMonitor.SeparationDoneEvent += delegate(object sender, SeparationEventArgs e)
            {
                eventRaised = true;
            };

            var newFlightTrack =
                new FlightTrack() {Tag = "2", Altitude = 2000, CoordinateX = 90000, CoordinateY = 90000};

            _airspaceMonitor.AddTrack(_flightTrack1);
            _airspaceMonitor.AddTrack(newFlightTrack);

            Assert.That(eventRaised, Is.EqualTo(true));
        }

        [Test]
        public void AddTrack_Calls_DetectSeparation_SeparationEventWasRaised_IsSeparationTrackListChangedTrue()
        {
            _airspaceMonitor.AddTrack(_flightTrack1);
            _airspaceMonitor.AddTrack(_flightTrack2);

            Assert.That(_flightTrack1.IsSeparationTrackListChanged, Is.EqualTo(true));
            Assert.That(_flightTrack2.IsSeparationTrackListChanged, Is.EqualTo(true));
        }

    }
}
