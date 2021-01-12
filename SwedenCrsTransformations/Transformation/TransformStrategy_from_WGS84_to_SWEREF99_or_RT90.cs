using MightyLittleGeodesy.Classes;

namespace SwedenCrsTransformations.Transformation {
    internal class TransformStrategy_from_WGS84_to_SWEREF99_or_RT90 : TransformStrategy {
        // Precondition: sourceCoordinate must be CRS WGS84
        public CrsCoordinate Transform(
            CrsCoordinate sourceCoordinate,
            CrsProjection targetCrsProjection
        ) {
            var gkProjection = new GaussKreuger();
            gkProjection.swedish_params(targetCrsProjection);
            LonLat lonLat = gkProjection.geodetic_to_grid(sourceCoordinate.LatitudeY, sourceCoordinate.LongitudeX);
            return CrsCoordinate.CreateCoordinate(targetCrsProjection, lonLat.LongitudeX, lonLat.LatitudeY);
        }
    }

}