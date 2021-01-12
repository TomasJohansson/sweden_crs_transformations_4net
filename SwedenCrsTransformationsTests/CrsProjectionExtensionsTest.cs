using NUnit.Framework;
using SwedenCrsTransformations;

namespace SwedenCrsTransformationsTests {
    
    [TestFixture]
    public class CrsProjectionExtensionsTest {

        // TODO add more tests for all enum values
        [Test]
        public void isWgs84() {
            Assert.IsTrue(CrsProjection.wgs84.isWgs84());
            Assert.IsFalse(CrsProjection.sweref_99_12_00.isWgs84());
            Assert.IsFalse(CrsProjection.rt90_0_0_gon_v.isWgs84());
        }

        [Test]
        public void isSweref() {
            Assert.IsFalse(CrsProjection.wgs84.isSweref());
            Assert.IsTrue(CrsProjection.sweref_99_12_00.isSweref());
            Assert.IsFalse(CrsProjection.rt90_0_0_gon_v.isSweref());
        }

        [Test]
        public void isRT90() {
            Assert.IsFalse(CrsProjection.wgs84.isRT90());
            Assert.IsFalse(CrsProjection.sweref_99_12_00.isRT90());
            Assert.IsTrue(CrsProjection.rt90_0_0_gon_v.isRT90());
        }

        [Test]
        public void Grid() {
            Assert.AreEqual(CrsGrid.WGS84,      CrsProjection.wgs84.Grid());
            Assert.AreEqual(CrsGrid.SWEREF99,   CrsProjection.sweref_99_12_00.Grid());
            Assert.AreEqual(CrsGrid.RT90,       CrsProjection.rt90_0_0_gon_v.Grid());
        }
    }
}