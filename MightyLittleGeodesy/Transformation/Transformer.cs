using System;

namespace MightyLittleGeodesy {
    internal class Transformer {

        // Implementations of transformations from WGS84:
        private static TransformStrategy _transformStrategy_from_WGS84_to_SWEREF99_or_RT90 = new TransformStrategy_from_WGS84_to_SWEREF99_or_RT90();

        // Implementations of transformations to WGS84:
        private static TransformStrategy _transformStrategy_from_SWEREF99_or_RT90_to_WGS84 = new TransformStrategy_from_SWEREF99_or_RT90_to_WGS84();

        // Implementation first transforming to WGS84 and then to the real target:
        private static TransformStrategy _transFormStrategy_From_Sweref99OrRT90_to_WGS84_andThenToRealTarget  = new TransFormStrategy_From_Sweref99OrRT90_to_WGS84_andThenToRealTarget();

        public static CrsCoordinate Transform(CrsCoordinate sourceCoordinate, CrsProjection targetCrsProjection) {
            if(sourceCoordinate.crsProjection == targetCrsProjection) throw new ArgumentException("Trying to transform from/to the same CRS");

            TransformStrategy _transFormStrategy = null;

            // Transform FROM wgs84:
            if(
                sourceCoordinate.crsProjection.isWgs84()
                &&
                ( targetCrsProjection.isSweref() || targetCrsProjection.isRT90() )
            ) {
                _transFormStrategy = _transformStrategy_from_WGS84_to_SWEREF99_or_RT90;
            }

            // Transform TO wgs84:
            else if(
                targetCrsProjection.isWgs84()
                &&
                ( sourceCoordinate.crsProjection.isSweref() || sourceCoordinate.crsProjection.isRT90() )
            ) {
                _transFormStrategy = _transformStrategy_from_SWEREF99_or_RT90_to_WGS84;
            }

            // Transform between two non-wgs84:
            else if(
                ( sourceCoordinate.crsProjection.isSweref() || sourceCoordinate.crsProjection.isRT90() )
                &&
                ( targetCrsProjection.isSweref() || targetCrsProjection.isRT90() )
            ) {
                // the only direct transform supported is to/from WGS84, so therefore first transform to wgs84
                _transFormStrategy = _transFormStrategy_From_Sweref99OrRT90_to_WGS84_andThenToRealTarget;
            }
            
            if(_transFormStrategy != null) {
                return _transFormStrategy.Transform(sourceCoordinate, targetCrsProjection);
            }

            throw new ArgumentException(string.Format("Unhandled source/target EPSG {0} ==> {1}", sourceCoordinate.epsgNumber, targetCrsProjection));
        }

    }

}