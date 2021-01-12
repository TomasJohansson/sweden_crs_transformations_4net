using NUnit.Framework;
using SwedenCrsTransformations;

namespace SwedenCrsTransformationsTests {
    
    [TestFixture]
    public class CrsProjectionExtensionsTest {

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