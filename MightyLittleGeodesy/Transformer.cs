using MightyLittleGeodesy.Classes;
using System;

namespace MightyLittleGeodesy {

    internal class Transformer {

        // Implementations of transformations from WGS84:
        private static TransformStrategy _transformStrategy_from_WGS84_to_SWEREF99      = new TransformStrategy_from_WGS84_to_SWEREF99();
        private static TransformStrategy _transFormStrategy_from_WGS84_to_RT90          = new TransformStrategy_from_WGS84_to_RT90();

        // Implementations of transformations to WGS84:
        private static TransformStrategy _transformStrategy_from_SWEREF99_to_WGS84      = new TransformStrategy_from_SWEREF99_to_WGS84();
        private static TransformStrategy _transformStrategy_from_RT90_to_WGS84          = new TransformStrategy_from_RT90_to_WGS84();

        // Implementation first transforming to WGS84 and then to the real target:
        private static TransformStrategy _transFormStrategy_From_Sweref99OrRT90_to_WGS84_andThenToRealTarget  = new TransFormStrategy_From_Sweref99OrRT90_to_WGS84_andThenToRealTarget();

        public static CrsCoordinate Transform(CrsCoordinate sourceCoordinate, CrsProjection targetCrsProjection) {
            if(sourceCoordinate.crsProjection == targetCrsProjection) throw new ArgumentException("Trying to transform from/to the same CRS");

            TransformStrategy _transFormStrategy = null;

            if(sourceCoordinate.crsProjection.isWgs84()) {
                if(targetCrsProjection.isSweref()) {
                    _transFormStrategy = _transformStrategy_from_WGS84_to_SWEREF99;
                }
                else if(targetCrsProjection.isRT90()) {
                    _transFormStrategy = _transFormStrategy_from_WGS84_to_RT90;
                }
            }
            else if(sourceCoordinate.crsProjection.isSweref()) {
                if(targetCrsProjection.isWgs84()) {
                    _transFormStrategy = _transformStrategy_from_SWEREF99_to_WGS84;
                }
                else {
                    // the only direct transform supported is to/from WGS84, so therefore first transform to wgs84
                    _transFormStrategy = _transFormStrategy_From_Sweref99OrRT90_to_WGS84_andThenToRealTarget;
                }
            }
            else if(sourceCoordinate.crsProjection.isRT90()) {
                if(targetCrsProjection.isWgs84()) {
                    _transFormStrategy = _transformStrategy_from_RT90_to_WGS84;
                }
                else {
                    // the only direct transform supported is to/from WGS84, so therefore first transform to wgs84
                    _transFormStrategy = _transFormStrategy_From_Sweref99OrRT90_to_WGS84_andThenToRealTarget;
                }
            }
            if(_transFormStrategy != null) {
                return _transFormStrategy.Transform(sourceCoordinate, targetCrsProjection);
            }
            throw new ArgumentException(string.Format("Unhandled source/target EPSG {0} ==> {1}", sourceCoordinate.epsgNumber, targetCrsProjection));
        }

    }

    internal interface TransformStrategy {
        CrsCoordinate Transform(
            CrsCoordinate sourceCoordinate,
            CrsProjection targetCrsProjection
        );
    }

    internal class TransformStrategy_from_WGS84_to_SWEREF99 : TransformStrategy {
        // Precondition: sourceCoordinate must be CRS WGS84
        public CrsCoordinate Transform(
            CrsCoordinate sourceCoordinate,
            CrsProjection targetCrsProjection
        ) {
            var gkProjection = new GaussKreuger();
            gkProjection.swedish_params(targetCrsProjection);
            LonLat lonLat = gkProjection.geodetic_to_grid(sourceCoordinate.yLatitude, sourceCoordinate.xLongitude);
            return CrsCoordinate.CreateCoordinatePoint(targetCrsProjection, lonLat.xLongitude, lonLat.yLatitude);
        }
    }

    
    internal class TransformStrategy_from_WGS84_to_RT90 : TransformStrategy {
        // Precondition: sourceCoordinate must be CRS WGS84
        public CrsCoordinate Transform(
            CrsCoordinate sourceCoordinate,
            CrsProjection targetCrsProjection
        ) {
            var gkProjection = new GaussKreuger();
            gkProjection.swedish_params(targetCrsProjection);
            LonLat lonLat = gkProjection.geodetic_to_grid(sourceCoordinate.yLatitude, sourceCoordinate.xLongitude);
            return CrsCoordinate.CreateCoordinatePoint(targetCrsProjection, lonLat.xLongitude, lonLat.yLatitude);
        }
    }

    internal class TransformStrategy_from_SWEREF99_to_WGS84 : TransformStrategy {
        // Precondition: sourceCoordinate must be CRS SWEREF99
        public CrsCoordinate Transform(
            CrsCoordinate sourceCoordinate,
            CrsProjection targetCrsProjection
        ) {
            var gkProjection = new GaussKreuger();
            gkProjection.swedish_params(sourceCoordinate.crsProjection);
            LonLat lonLat = gkProjection.grid_to_geodetic(sourceCoordinate.yLatitude, sourceCoordinate.xLongitude); 
            return CrsCoordinate.CreateCoordinatePoint(targetCrsProjection, lonLat.xLongitude, lonLat.yLatitude);
        }
    }
    internal class TransformStrategy_from_RT90_to_WGS84 : TransformStrategy {
        // Precondition: sourceCoordinate must be CRS RT90
        public CrsCoordinate Transform(
            CrsCoordinate sourceCoordinate,
            CrsProjection targetCrsProjection
        ) {
            var gkProjection = new GaussKreuger();
            gkProjection.swedish_params(sourceCoordinate.crsProjection);
            LonLat lonLat = gkProjection.grid_to_geodetic(sourceCoordinate.yLatitude, sourceCoordinate.xLongitude);
            return CrsCoordinate.CreateCoordinatePoint(targetCrsProjection, lonLat.xLongitude, lonLat.yLatitude);
        }
    }

    internal class TransFormStrategy_From_Sweref99OrRT90_to_WGS84_andThenToRealTarget : TransformStrategy {
        // Precondition: sourceCoordinate must be CRS SWEREF99 or RT90
        public CrsCoordinate Transform(
            CrsCoordinate sourceCoordinate,
            CrsProjection targetCrsProjection
        ) {
            var wgs84coordinate = Transformer.Transform(sourceCoordinate, CrsProjection.wgs84);
            return Transformer.Transform(wgs84coordinate, targetCrsProjection);
        }
    }

}
