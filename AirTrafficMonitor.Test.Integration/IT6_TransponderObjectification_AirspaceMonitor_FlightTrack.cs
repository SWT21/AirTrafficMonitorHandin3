using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using AirTrafficMonitor.Classes;
using AirTrafficMonitor.Events;
using AirTrafficMonitor.Interfaces;
using NSubstitute;
using NUnit.Framework;
using TransponderReceiver;

namespace AirTrafficMonitor.Test.Integration
{
    [TestFixture]
    class IT6_TransponderObjectification_AirspaceMonitor_FlightTrack
    {
        private ITransponderObjectification _transponderObjectification;
        private IAirspaceMonitor _airspaceMonitor;
        private ITrack _flightTrack;
        private ITrackCalculator _trackCalculator;
        private ITransponderReceiver _transponderReceiver;
        private RawTransponderDataEventArgs _fakeTransponderData;


        [SetUp]
        public void Setup()
        {
            _flightTrack = new FlightTrack();
            _airspaceMonitor = new AirspaceMonitor(10000, 10000, 90000, 90000, 500, 20000, new TrackCalculator());
            _transponderReceiver = Substitute.For<ITransponderReceiver>();
            _transponderObjectification = new TransponderObjectification(_transponderReceiver, _airspaceMonitor);
            _fakeTransponderData = new RawTransponderDataEventArgs(new List<string>(){"Tag;0;0;0;00010101010101001"});
            _transponderObjectification.ConsoleOutput = Substitute.For<IOutput>();
            _transponderObjectification.LogfileOutput = Substitute.For<IOutput>();
        }

        public void RaiseEvent_TransponderDataReady()
        {
            _transponderReceiver.TransponderDataReady += Raise.EventWith(_fakeTransponderData);
        }

        [Test]
        public void AirspaceMonitor_IsDoneDetectSpearation_IsTrueByDefault()
        {
            Assert.That(_airspaceMonitor.IsDoneDetectSpearation, Is.EqualTo(true));
        }

        [Test]
        public void ReceiverOnTransponderDataReady_FlightTrackObjectIsCreated()
        {
            RaiseEvent_TransponderDataReady();
            Assert.That(_transponderObjectification.Track, Is.Not.EqualTo(null));
        }

        [Test]
        public void ReceiverOnTransponderDataReady_Calls_AddTrack_SeparationEventRaised()
        {
            bool eventRaised = false;

            _airspaceMonitor.SeparationMonitor.SeparationEvent += delegate (object sender, SeparationEventArgs e)
            {
                eventRaised = true;
            };

                _fakeTransponderData = new RawTransponderDataEventArgs(new List<string>()
                {
                    "Tag1;10000;10000;500;00010101010101001",
                    "Tag2;11000;11000;500;00010101010151001",

                });

                RaiseEvent_TransponderDataReady();

            Assert.That(eventRaised, Is.EqualTo(true));
        }

        [Test]
        public void ReceiverOnTransponderDataReady_Calls_AddTrack_SeparationDoneEventRaised()
        {
            bool eventRaised = false;

            _airspaceMonitor.SeparationMonitor.SeparationDoneEvent += delegate (object sender, SeparationEventArgs e)
            {
                eventRaised = true;
            };

            _fakeTransponderData = new RawTransponderDataEventArgs(new List<string>()
            {
                "Tag1;10000;10000;500;00010101010101001",
                "Tag2;11000;11000;2000;00010101010151001",
            });

            RaiseEvent_TransponderDataReady();

            Assert.That(eventRaised, Is.EqualTo(true));
        }
    }
}
