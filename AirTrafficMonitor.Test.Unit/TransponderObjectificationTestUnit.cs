using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirTrafficMonitor.Classes;
using AirTrafficMonitor.Interfaces;
using NSubstitute;
using NSubstitute.Core.Arguments;
using NUnit.Framework;
using NUnit.Framework.Internal;
using TransponderReceiver;

namespace AirTrafficMonitor.Test.Unit
{
    [TestFixture]
    class TransponderObjectificationTestUnit
    {
        private ITransponderObjectification _uut;
        private ITrack _flightTrack;
        private IAirspaceMonitor _airspaceMonitor;
        private ITransponderReceiver _transponderReceiver;
        private RawTransponderDataEventArgs _fakeTransponderData;

        [SetUp]
        public void Setup()
        {
            _flightTrack = new FlightTrack();
            _airspaceMonitor = Substitute.For<IAirspaceMonitor>();
            _transponderReceiver = Substitute.For<ITransponderReceiver>();
            _uut = new TransponderObjectification(_transponderReceiver, _airspaceMonitor);
            _fakeTransponderData = new RawTransponderDataEventArgs(new List<string>(){"Tag;0;0;0;00010101010101001"});
            _uut.ConsoleOutput = Substitute.For<IOutput>();
            _uut.LogfileOutput = Substitute.For<IOutput>();
        }

        public void RaiseEvent_TransponderDataReady()
        {
            _transponderReceiver.TransponderDataReady += Raise.EventWith(_fakeTransponderData);
        }

        [TestCase("Tag;0;0;0;00010101010101001")]
        [TestCase("Tag;1;1;1;99991230235959999")]
        public void ObjectifyTransponderData_DataIsObjectified(string dataString)
        {
            _uut.ObjectifyTransponderData(dataString, _flightTrack);

            Assert.That(_flightTrack.Tag, Is.EqualTo(dataString.Split(';')[0]));
            Assert.That(_flightTrack.Altitude.ToString(), Is.EqualTo(dataString.Split(';')[1]));
            Assert.That(_flightTrack.CoordinateX.ToString(), Is.EqualTo(dataString.Split(';')[2]));
            Assert.That(_flightTrack.CoordinateY.ToString(), Is.EqualTo(dataString.Split(';')[3]));
            Assert.That(_flightTrack.UpdateTimestamp.ToString("yyyyMMddHHmmssfff"), Is.EqualTo(dataString.Split(';')[4]));
            Assert.That(_flightTrack.Velocity, Is.EqualTo(0));
            Assert.That(_flightTrack.Course, Is.EqualTo(0));
        }

        [Test]
        public void ReceiverOnTransponderDataReady_AirspaceMonitorRecieved_Addtrack()
        {
            _airspaceMonitor.IsDoneDetectSpearation.Returns(true);
            RaiseEvent_TransponderDataReady();

           _airspaceMonitor.Received().AddTrack(Arg.Any<ITrack>());
        }

        [Test]
        public void ReceiverOnTransponderDataReady_ConsoleOutputRecieved_OutputDictionary()
        {
            _airspaceMonitor.IsDoneDetectSpearation.Returns(true);
            RaiseEvent_TransponderDataReady();

            _uut.ConsoleOutput.Received().OutputDictionary(_airspaceMonitor.TrackDict);
        }

        [Test]
        public void ReceiverOnTransponderDataReady_LogfileOutputRecieved_OutputDictionary()
        {
            _airspaceMonitor.IsDoneDetectSpearation.Returns(true);
            RaiseEvent_TransponderDataReady();

            _uut.LogfileOutput.Received().OutputDictionary(_airspaceMonitor.TrackDict);
        }

        [Test]
        public void ReceiverOnTransponderDataReady_LogfileOutputRecieved_OutputSeparationEvents()
        {
            _airspaceMonitor.IsDoneDetectSpearation.Returns(true);
            RaiseEvent_TransponderDataReady();

            _uut.LogfileOutput.Received().OutputSeparationEvents(_airspaceMonitor.TrackDict);
        }
    }
}
