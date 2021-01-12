using NUnit.Framework;
using SwedenCrsTransformations;
using static SwedenCrsTransformationsTests.CrsProjectionFactoryTest; // to be able to use constants such as epsgNumberForSweref99tm and epsgNumberForWgs84
namespace SwedenCrsTransformationsTests {
    
    [TestFixture]
    public class CrsProjectionExtensionsTest {

        [Test]
        public void GetEpsgNumber() {
            Assert.AreEqual(
                epsgNumberForSweref99tm, // constant defined in CrsProjectionFactoryTest
                CrsProjection.sweref_99_tm.GetEpsgNumber()
            );

            Assert.AreEqual(
                epsgNumberForWgs84, // constant defined in CrsProjectionFactoryTest
                CrsProjection.wgs84.GetEpsgNumber()
            );
        }


        // TODO add more tests for all enum values
        [Test]
        public void isWgs84() {
            Assert.IsTrue(CrsProjection.wgs84.IsWgs84());
            Assert.IsFalse(CrsProjection.sweref_99_12_00.IsWgs84());
            Assert.IsFalse(CrsProjection.rt90_0_0_gon_v.IsWgs84());
        }

        [Test]
        public void isSweref() {
            Assert.IsFalse(CrsProjection.wgs84.IsSweref());
            Assert.IsTrue(CrsProjection.sweref_99_12_00.IsSweref());
            Assert.IsFalse(CrsProjection.rt90_0_0_gon_v.IsSweref());
        }

        [Test]
        public void isRT90() {
            Assert.IsFalse(CrsProjection.wgs84.IsRT90());
            Assert.IsFalse(CrsProjection.sweref_99_12_00.IsRT90());
            Assert.IsTrue(CrsProjection.rt90_0_0_gon_v.IsRT90());
        }

        [Test]
        public void Grid() {
            Assert.AreEqual(CrsGrid.WGS84,      CrsProjection.wgs84.Grid());
            Assert.AreEqual(CrsGrid.SWEREF99,   CrsProjection.sweref_99_12_00.Grid());
            Assert.AreEqual(CrsGrid.RT90,       CrsProjection.rt90_0_0_gon_v.Grid());
        }
    }
}