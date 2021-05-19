using NUnit.Framework;

namespace SwedenCrsTransformations.Transformation {

    [TestFixture]
    public class TransformerTest {

        // The tests below are using coordinates from the csv file "SwedenCrsTransformationsTests/CoordinateFiles/data/swedish_crs_coordinates.csv" :
        // 4326|15.816797928880426|61.291598649254105|3006|543770.853060645|6795541.286404101|3021|1500627.951348714|6797357.300170688|6|6

        // The three coordinate systems in the above row from the CSV file are: EPSG 4326,3006,3021
        // EPSG 4326 ==> wgs84
        // EPSG 3006 ==> sweref_99_tm
        // EPSG 3021 ==> rt90_2_5_gon_v

        private const double wgs84_longitude = 15.816797928880426;
        private const double wgs84_latitude = 61.291598649254105;
        private const double sweref_99_tm_x = 543770.853060645;
        private const double sweref_99_tm_y = 6795541.286404101;
        private const double rt90_2_5_gon_v_x = 1500627.951348714;
        private const double rt90_2_5_gon_v_y = 6797357.300170688;

        private CrsCoordinate coordinateWgs, coordinateSweref, coordinateRT;
        
        private CrsProjection crsWgs, crsSweref, crsRT;

        [SetUp]
        public void setUp() {
            coordinateWgs = CrsCoordinate.CreateCoordinate(CrsProjection.wgs84, wgs84_latitude, wgs84_longitude);
            coordinateSweref = CrsCoordinate.CreateCoordinate(CrsProjection.sweref_99_tm, sweref_99_tm_y, sweref_99_tm_x);
            coordinateRT = CrsCoordinate.CreateCoordinate(CrsProjection.rt90_2_5_gon_v, rt90_2_5_gon_v_y, rt90_2_5_gon_v_x);

            crsWgs = coordinateWgs.CrsProjection;
            crsSweref = coordinateSweref.CrsProjection;
            crsRT = coordinateRT.CrsProjection;
        }


        // ------------------------------------------------------------------
        // Transformations between Wgs84 and Sweref99
        [Test]
        public void Transform_from_Wgs84_to_Sweref99tm() {
            CrsCoordinate result = Transformer.Transform(coordinateWgs, crsSweref);
            Assert.AreEqual(crsSweref, result.CrsProjection);
            const double delta = 0.001;
            Assert.AreEqual(coordinateSweref.LatitudeY, result.LatitudeY, delta);
            Assert.AreEqual(coordinateSweref.LongitudeX, result.LongitudeX, delta);
        }
        [Test]
        public void Transform_from_Sweref99tm_to_Wgs84() {
            CrsCoordinate result = Transformer.Transform(coordinateSweref, crsWgs);
            Assert.AreEqual(crsWgs, result.CrsProjection);
            const double delta = 0.0000000001;
            Assert.AreEqual(coordinateWgs.LatitudeY, result.LatitudeY, delta);
            Assert.AreEqual(coordinateWgs.LongitudeX, result.LongitudeX, delta);
        }
        // ------------------------------------------------------------------
        // Transformations between Wgs84 and RT90
        [Test]
        public void Transform_from_Wgs84_to_RT90() {
            CrsCoordinate result = Transformer.Transform(coordinateWgs, crsRT);
            Assert.AreEqual(crsRT, result.CrsProjection);
            const double delta = 0.1;
            Assert.AreEqual(coordinateRT.LatitudeY, result.LatitudeY, delta);
            Assert.AreEqual(coordinateRT.LongitudeX, result.LongitudeX, delta);
        }
        [Test]
        public void Transform_from_RT90_to_Wgs84()
        {
            CrsCoordinate result = Transformer.Transform(coordinateRT, crsWgs);
            Assert.AreEqual(crsWgs, result.CrsProjection);
            const double delta = 0.000001;
            Assert.AreEqual(coordinateWgs.LatitudeY, result.LatitudeY, delta);
            Assert.AreEqual(coordinateWgs.LongitudeX, result.LongitudeX, delta);
        }
        // ------------------------------------------------------------------
        // Transformations between Sweref99 and RT90
        [Test]
        public void Transform_from_Sweref99tm_toRT90() {
            CrsCoordinate result = Transformer.Transform(coordinateSweref, crsRT);
            Assert.AreEqual(crsRT, result.CrsProjection);
            const double delta = 0.1;
            Assert.AreEqual(coordinateRT.LatitudeY, result.LatitudeY, delta);
            Assert.AreEqual(coordinateRT.LongitudeX, result.LongitudeX, delta);
        }
        [Test]
        public void Transform_from_RT90_to_Sweref99tm() {
            CrsCoordinate result = Transformer.Transform(coordinateRT, crsSweref);
            Assert.AreEqual(crsSweref, result.CrsProjection);
            const double delta = 0.1;
            Assert.AreEqual(coordinateSweref.LatitudeY, result.LatitudeY, delta);
            Assert.AreEqual(coordinateSweref.LongitudeX, result.LongitudeX, delta);
        }
        // ------------------------------------------------------------------
    }
}
