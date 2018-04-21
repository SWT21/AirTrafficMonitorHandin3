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
    class FlightTrackTestUnit
    {
        private ITrack _uut;

        [SetUp]
        public void Setup()
        {
            _uut = new FlightTrack();
        }

        [Test]
        public void Ctor_IsSeparationTrackListChanged_False()
        {
            Assert.That(_uut.IsSeparationTrackListChanged, Is.EqualTo(false));
        }

        [Test]
        public void Set_TagGreaterThanSix_ExceptionExpected()
        {
            Assert.Throws<System.Exception>(() => _uut.Tag = "1234567");
        }

        [Test]
        public void Set_CoordinateXLessThanZero_ExceptionExpected()
        {
            Assert.Throws<System.Exception>(() => _uut.CoordinateX = -1);
        }

        [Test]
        public void Set_CoordinateYLessThanZero_ExceptionExpected()
        {
            Assert.Throws<System.Exception>(() => _uut.CoordinateY = -1);
        }

        [Test]
        public void Set_AltitudeLessThanZero_ExceptionExpected()
        {
            Assert.Throws<System.Exception>(() => _uut.Altitude = -1);
        }

        [Test]
        public void Set_VelocityLessThanZero_ExceptionExpected()
        {
            Assert.Throws<System.Exception>(() => _uut.Velocity = -1);
        }

        [Test]
        public void Set_CourseLessThanZero_ExceptionExpected()
        {
            Assert.Throws<System.Exception>(() => _uut.Course = -1);
        }

        [Test]
        public void Set_CourseGreaterThan360_ExceptionExpected()
        {
            Assert.Throws<System.Exception>(() => _uut.Course = 361);
        }
    }
}
