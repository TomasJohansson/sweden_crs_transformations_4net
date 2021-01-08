using Microsoft.VisualStudio.TestTools.UnitTesting;
using MightyLittleGeodesy;
using static MightyLittleGeodesy.Positions.RT90Position;
using static MightyLittleGeodesy.Positions.SWEREF99Position;

namespace MightyLittleGeodesyTests
{
    [TestClass]
    public class ProjectionFactoryTest {

        private const int epsgNumberForSweref99tm = 3006; // https://epsg.org/crs_3006/SWEREF99-TM.html
        private const int numberOfSweref99projections = 13; // with EPSG numbers 3006-3018
        private const int numberOfRTprojections = 6; // with EPSG numbers 3019-3024

        [TestMethod]
        public void GetSWEREFProjectionByEpsgNumber() {
            Assert.AreEqual(
                SWEREFProjection.sweref_99_tm,
                ProjectionFactory.GetSwerefProjectionByEpsgNumber(epsgNumberForSweref99tm)
            );

            Assert.AreEqual(
                SWEREFProjection.sweref_99_23_15,
                ProjectionFactory.GetSwerefProjectionByEpsgNumber(3018) // https://epsg.io/3018
            );
        }

        [TestMethod]
        public void GetRT90ProjectionProjectionByEpsgNumber() {
            Assert.AreEqual(
                RT90Projection.rt90_7_5_gon_v,
                ProjectionFactory.GetRT90ProjectionProjectionByEpsgNumber(3019) // https://epsg.io/3019
            );

            Assert.AreEqual(
                RT90Projection.rt90_5_0_gon_o,
                ProjectionFactory.GetRT90ProjectionProjectionByEpsgNumber(3024) // https://epsg.io/3024
            );
        }


        [TestMethod]
        public void GetAllSwerefProjections() {
            var allSwerefProjections = ProjectionFactory.GetAllSwerefProjections();
            Assert.AreEqual(
                numberOfSweref99projections,
                allSwerefProjections.Count
            );

            foreach(var swerefProjection in allSwerefProjections) {
                var swerefProj = ProjectionFactory.GetSwerefProjectionByEpsgNumber(swerefProjection.GetEpsgNumber());
                Assert.AreEqual(swerefProjection, swerefProj);
            }
        }    

        [TestMethod]
        public void GetAllRT90Projections() {
            var allRT90Projections = ProjectionFactory.GetAllRT90Projections();
            Assert.AreEqual(
                numberOfRTprojections,
                allRT90Projections.Count
            );

            foreach(var rt90Projection in allRT90Projections) {
                var rt90proj = ProjectionFactory.GetRT90ProjectionProjectionByEpsgNumber(rt90Projection.GetEpsgNumber());
                Assert.AreEqual(rt90Projection, rt90proj);
            }
        }    


        // ProjectionEnumExtensions
        [TestMethod]
        public void GetEpsgNumber() {
            Assert.AreEqual(
                epsgNumberForSweref99tm,
                SWEREFProjection.sweref_99_tm.GetEpsgNumber()
            );
        }

    }
}