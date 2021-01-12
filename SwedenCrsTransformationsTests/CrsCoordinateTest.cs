using NUnit.Framework;
using SwedenCrsTransformations;
using static SwedenCrsTransformationsTests.CrsProjectionFactoryTest; // to be able to use constants such as epsgNumberForSweref99tm

namespace SwedenCrsTransformationsTests {
    
    [TestFixture]
    public class CrsCoordinateTest {
        
        [Test]
        public void CreateCoordinatePointByEpsgNumber() {
            const double x = 20.0;
            const double y = 60.0;
            CrsCoordinate crsCoordinate = CrsCoordinate.CreateCoordinatePoint(epsgNumberForSweref99tm, x, y);
            Assert.AreEqual(epsgNumberForSweref99tm, crsCoordinate.CrsProjection.GetEpsgNumber());
            Assert.AreEqual(x, crsCoordinate.XLongitude);
            Assert.AreEqual(y, crsCoordinate.YLatitude);
        }

        [Test]
        public void CreateCoordinatePoint() {
            const double x = 22.5;
            const double y = 62.5;
            CrsCoordinate crsCoordinate = CrsCoordinate.CreateCoordinatePoint(CrsProjection.sweref_99_tm, x, y);
            Assert.AreEqual(epsgNumberForSweref99tm, crsCoordinate.CrsProjection.GetEpsgNumber());
            Assert.AreEqual(CrsProjection.sweref_99_tm, crsCoordinate.CrsProjection);
            Assert.AreEqual(x, crsCoordinate.XLongitude);
            Assert.AreEqual(y, crsCoordinate.YLatitude);
        }

    }
}
