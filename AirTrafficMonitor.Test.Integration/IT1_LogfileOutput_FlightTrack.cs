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
        private ITrack _flighTrack1;
        private ITrack _flighTrack2;
        private IAirspaceMonitor _airspaceMonitor;

        [SetUp]
        public void Setup()
        {
            _airspaceMonitor = new AirspaceMonitor(10000, 10000, 90000, 90000, 500, 20000, new TrackCalculator());
            _logfileOutput = new LogfileOutput();
            _flighTrack1 = new FlightTrack() { Tag = "1" };
            _flighTrack2 = new FlightTrack() { Tag = "2" };
        }

        [Test]
        public void OutputSeparationEvents_IsSeparationTrackListChanged_SetToFalse()
        {
            _airspaceMonitor.TrackDict.Add(_flighTrack1.Tag, _flighTrack1);
            _airspaceMonitor.TrackDict.Add(_flighTrack2.Tag, _flighTrack2);

            _flighTrack1.SeparationTrackList.Add(_flighTrack2);
            _flighTrack2.SeparationTrackList.Add(_flighTrack1);
            
            _flighTrack1.IsSeparationTrackListChanged = true;
            _flighTrack2.IsSeparationTrackListChanged = true;
            
            _logfileOutput.OutputSeparationEvents(_airspaceMonitor.TrackDict);

            Assert.That(_flighTrack1.IsSeparationTrackListChanged, Is.EqualTo(false));
        }
    }
}
