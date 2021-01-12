using NUnit.Framework;
using SwedenCrsTransformations;

namespace SwedenCrsTransformationsTests
{
    [TestFixture]
    public class CrsProjectionFactoryTest {

        private const int epsgNumberForWgs84 = 4326;
        private const int epsgNumberForSweref99tm = 3006; // https://epsg.org/crs_3006/SWEREF99-TM.html
        private const int numberOfSweref99projections = 13; // with EPSG numbers 3006-3018
        private const int numberOfRTprojections = 6; // with EPSG numbers 3019-3024
        private const int numberOfWgs84Projectios = 1; // just to provide semantic instead of using a magic number 1 below
        private const int totalNumberOfProjections = numberOfSweref99projections + numberOfRTprojections + numberOfWgs84Projectios;


        [Test]
        public void GetCrsProjectionByEpsgNumber() {
            Assert.AreEqual(
                CrsProjection.sweref_99_tm,
                CrsProjectionFactory.GetCrsProjectionByEpsgNumber(epsgNumberForSweref99tm)
            );

            Assert.AreEqual(
                CrsProjection.sweref_99_23_15,
                CrsProjectionFactory.GetCrsProjectionByEpsgNumber(3018) // https://epsg.io/3018
            );

            Assert.AreEqual(
                CrsProjection.rt90_5_0_gon_o,
                CrsProjectionFactory.GetCrsProjectionByEpsgNumber(3024)  // https://epsg.io/3018
            );
        }

        [Test]
        public void GetAllCrsProjections() {
            var allCrsProjections = CrsProjectionFactory.GetAllCrsProjections();
            Assert.AreEqual(
                totalNumberOfProjections,
                allCrsProjections.Count
            );

            foreach(var crsProjection in allCrsProjections) {
                var crsProj = CrsProjectionFactory.GetCrsProjectionByEpsgNumber(crsProjection.GetEpsgNumber());
                Assert.AreEqual(crsProjection, crsProj);
            }
        }    


        // TODO move this method to CrsProjectionExtensionsTest
        [Test]
        public void GetEpsgNumber() {
            Assert.AreEqual(
                epsgNumberForSweref99tm,
                CrsProjection.sweref_99_tm.GetEpsgNumber()
            );

            Assert.AreEqual(
                epsgNumberForWgs84,
                CrsProjection.wgs84.GetEpsgNumber()
            );
        }

    }
}