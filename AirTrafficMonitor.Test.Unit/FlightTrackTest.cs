using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirTrafficMonitor.Classes;
using NUnit.Framework;

namespace AirTrafficMonitor.Test.Unit
{
    [TestFixture]
    class FlightTrackTest
    {
        [TestCase("Tag;0;0;0;00010101010101001")]
        [TestCase("Tag;1;1;1;99991230235959999")]
        public void Extract_CanExtract(string expected)
        {
            var uut = new FlightTrack(expected);

            Assert.That(uut.Tag, Is.EqualTo(expected.Split(';')[0]));
            Assert.That(uut.Altitude.ToString(), Is.EqualTo(expected.Split(';')[1]));
            Assert.That(uut.CoordinateX.ToString(), Is.EqualTo(expected.Split(';')[2]));
            Assert.That(uut.CoordinateY.ToString(), Is.EqualTo(expected.Split(';')[3]));
            Assert.That(uut.Timestamp.ToString("yyyyMMddHHmmssfff"), Is.EqualTo(expected.Split(';')[4]));

        }
    }
}
