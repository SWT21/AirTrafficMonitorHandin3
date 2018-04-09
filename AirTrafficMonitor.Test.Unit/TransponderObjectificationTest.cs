using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirTrafficMonitor.Interfaces;
using NSubstitute;
using NUnit.Framework;
using NUnit.Framework.Internal;
using AirTrafficMonitor.Classes;
using TransponderReceiver;

namespace AirTrafficMonitor.Test.Unit
{
    [TestFixture]
    public class TransponderObjectificationTest
    {
        private TransponderObjectification _uut;
        private ITransponderReceiver _transponderReceiver;

        [SetUp]
        public void Setup()
        {
            _transponderReceiver = Substitute.For<ITransponderReceiver>();
            _uut = new TransponderObjectification(_transponderReceiver);
            _uut.Output = Substitute.For<IOutput>();
        }

        [Test]
        public void ReceiverOnTransponderDataReady_FireEvent_PrintObject()
        {
            var args = new TransponderDataEventArgs();
            _transponderReceiver.TransponderDataReady += Raise.EventWith(new object(), new RawTransponderDataEventArgs(new List<string>
            {
                "Tag;50000;50000;0;00010101010101001"
            }));

            var track = new FlightTrack("Tag;50000;50000;0;00010101010101001");
            
            Assert.That(_uut.AirspaceMonitor.TrackList[0].Tag, Is.EqualTo(track.Tag));
            Assert.That(_uut.AirspaceMonitor.TrackList[0].Altitude, Is.EqualTo(track.Altitude));
            Assert.That(_uut.AirspaceMonitor.TrackList[0].CoordinateX, Is.EqualTo(track.CoordinateX));
            Assert.That(_uut.AirspaceMonitor.TrackList[0].CoordinateY, Is.EqualTo(track.CoordinateY));
            Assert.That(_uut.AirspaceMonitor.TrackList[0].Timestamp, Is.EqualTo(track.Timestamp));
        }
    }
}
