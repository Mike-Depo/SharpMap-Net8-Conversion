using NetTopologySuite.Geometries;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnitTests
{
    [TestFixture]
    public class GeoApiExTests
    {
        [Test]
        public void TestCloseRing()
        {
            var threeDeeCoord1 = new Coordinate(0, 0);
            threeDeeCoord1.Z = 5;
            var threeDeeCoord2 = new Coordinate(0, 100);
            threeDeeCoord2.Z = 10;
            var threeDeeCoord3 = new Coordinate(100, 100);
            threeDeeCoord3.Z = 20;
            var threeDeeCoord4 = new Coordinate(100, 0);
            threeDeeCoord4.Z = 30;

            var coords = new List<Coordinate>(new Coordinate[] {
                threeDeeCoord1,
                threeDeeCoord2,
                threeDeeCoord3,
                threeDeeCoord4
            });

            coords.EnsureValidRing();
            Assert.AreEqual(5,coords.Count);

            Assert.AreEqual(5, coords[4].Z);

        }

        [Test]
        public void TestCloseRing2()
        {
            var coords = new List<Coordinate>(new Coordinate[] {
                new Coordinate(0,0),
                new Coordinate(0,100),
                new Coordinate(100,100),
                new Coordinate(100,0)
            });

            coords.EnsureValidRing();
            Assert.AreEqual(5, coords.Count);
        }
    }
}
