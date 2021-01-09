using Microsoft.VisualStudio.TestTools.UnitTesting;
using MightyLittleGeodesy;

namespace MightyLittleGeodesyTests {
    
    [TestClass]
    public class CrsCoordinateTest {
        
        [TestMethod]
        public void CreateCoordinatePoint() {
            CrsCoordinate crsCoordinate = CrsCoordinate.CreateCoordinatePoint(3006, 20.0, 60.0);
            Assert.AreEqual(3006, crsCoordinate.epsgNumber);
            Assert.AreEqual(20.0, crsCoordinate.xLongitude);
            Assert.AreEqual(60.0, crsCoordinate.yLatitude);
        }

    }
}
