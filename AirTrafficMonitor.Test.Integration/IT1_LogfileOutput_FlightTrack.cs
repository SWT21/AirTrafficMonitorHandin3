using System;
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
    class IT1_LogfileOutput_FlightTrack
    {
        private IOutput _logfileOutput;
        private ITrack _flightTrack1;
        private ITrack _flightTrack2;
        private IAirspaceMonitor _airspaceMonitor;

        [SetUp]
        public void Setup()
        {
            _airspaceMonitor = new AirspaceMonitor(10000, 10000, 90000, 90000, 500, 20000, new TrackCalculator());
            _logfileOutput = new LogfileOutput();
            _flightTrack1 = new FlightTrack() { Tag = "1", Altitude = 500, CoordinateX = 10000, CoordinateY = 10000 };
            _flightTrack2 = new FlightTrack() { Tag = "2", Altitude = 700, CoordinateX = 12000, CoordinateY = 12000 };
        }

        [Test]
        public void OutputSeparationEvents_IsSeparationTrackListChanged_SetToFalse()
        {
            _airspaceMonitor.TrackDict.Add(_flightTrack1.Tag, _flightTrack1);
            _airspaceMonitor.TrackDict.Add(_flightTrack2.Tag, _flightTrack2);

            _flightTrack1.SeparationTrackList.Add(_flightTrack2);
            _flightTrack2.SeparationTrackList.Add(_flightTrack1);
            
            _flightTrack1.IsSeparationTrackListChanged = true;
            _flightTrack2.IsSeparationTrackListChanged = true;
            
            _logfileOutput.OutputSeparationEvents(_airspaceMonitor.TrackDict);

            Assert.That(_flightTrack1.IsSeparationTrackListChanged, Is.EqualTo(false));
        }
    }
}
