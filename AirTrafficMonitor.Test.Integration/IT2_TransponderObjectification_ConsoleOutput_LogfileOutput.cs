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

namespace AirTrafficMonitor.Test.Integration
{
    [TestFixture]
    class IT2_TransponderObjectification_ConsoleOutput_LogfileOutput
    {
        private IAirspaceMonitor _airspaceMonitor;
        private ITransponderReceiver _transponderReceiver;
        private ITransponderObjectification _transponderObjectification;
        private RawTransponderDataEventArgs _fakeTransponderData;

        [SetUp]
        public void Setup()
        {
            _airspaceMonitor = Substitute.For<IAirspaceMonitor>();
            _transponderReceiver = Substitute.For<ITransponderReceiver>();
            _transponderObjectification = new TransponderObjectification(_transponderReceiver, _airspaceMonitor);
            _fakeTransponderData = new RawTransponderDataEventArgs(new List<string>() { "Tag;0;0;0;00010101010101001" });
            _transponderObjectification.ConsoleOutput = Substitute.For<IOutput>();
            _transponderObjectification.LogfileOutput = Substitute.For<IOutput>();
        }

        public void RaiseEvent_TransponderDataReady()
        {
            _transponderReceiver.TransponderDataReady += Raise.EventWith(_fakeTransponderData);
        }

        [Test]
        public void ReceiverOnTransponderDataReady_ConsoleOutput_RecievedOutputDictionary()
        {
            _airspaceMonitor.IsDoneDetectSpearation.Returns(true);
            RaiseEvent_TransponderDataReady();

            _transponderObjectification.ConsoleOutput.Received().OutputDictionary(_airspaceMonitor.TrackDict);
        }

        [Test]
        public void ReceiverOnTransponderDataReady_LogfileOutput_RecievedOutputDictionary()
        {
            _airspaceMonitor.IsDoneDetectSpearation.Returns(true);
            RaiseEvent_TransponderDataReady();

            _transponderObjectification.LogfileOutput.Received().OutputDictionary(_airspaceMonitor.TrackDict);
        }

        [Test]
        public void ReceiverOnTransponderDataReady_LogfileOutput_RecievedOutputSeparationEvents()
        {
            _airspaceMonitor.IsDoneDetectSpearation.Returns(true);
            RaiseEvent_TransponderDataReady();

            _transponderObjectification.LogfileOutput.Received().OutputSeparationEvents(_airspaceMonitor.TrackDict);
        }
    }
}
