using MightyLittleGeodesy.Positions;
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

    // TODO modify the below implementations to use GaussKreuger directly instead of using it through the classes SWEREF99Position and RT90Position
    internal class TransformStrategy_from_WGS84_to_SWEREF99 : TransformStrategy {
        // Precondition: sourceCoordinate must be CRS WGS84
        public CrsCoordinate Transform(
            CrsCoordinate sourceCoordinate,
            CrsProjection targetCrsProjection
        ) {
            var wgs84position = new WGS84Position(sourceCoordinate.yLatitude, sourceCoordinate.xLongitude);
            var position = new SWEREF99Position(wgs84position, targetCrsProjection);
            return CrsCoordinate.CreateCoordinatePoint(targetCrsProjection, position.Longitude, position.Latitude);
        }
    }

    
    internal class TransformStrategy_from_WGS84_to_RT90 : TransformStrategy {
        // Precondition: sourceCoordinate must be CRS WGS84
        public CrsCoordinate Transform(
            CrsCoordinate sourceCoordinate,
            CrsProjection targetCrsProjection
        ) {
            var wgs84position = new WGS84Position(sourceCoordinate.yLatitude, sourceCoordinate.xLongitude);
            var position = new RT90Position(wgs84position, targetCrsProjection);
            return CrsCoordinate.CreateCoordinatePoint(targetCrsProjection, position.Longitude, position.Latitude);
        }
    }

    internal class TransformStrategy_from_SWEREF99_to_WGS84 : TransformStrategy {
        // Precondition: sourceCoordinate must be CRS SWEREF99
        public CrsCoordinate Transform(
            CrsCoordinate sourceCoordinate,
            CrsProjection targetCrsProjection
        ) {
            var sweref99Position = new SWEREF99Position(sourceCoordinate.yLatitude, sourceCoordinate.xLongitude, sourceCoordinate.crsProjection);
            var wgs84result = sweref99Position.ToWGS84();
            return CrsCoordinate.CreateCoordinatePoint(targetCrsProjection, wgs84result.Longitude, wgs84result.Latitude);
        }
    }
    internal class TransformStrategy_from_RT90_to_WGS84 : TransformStrategy {
        // Precondition: sourceCoordinate must be CRS RT90
        public CrsCoordinate Transform(
            CrsCoordinate sourceCoordinate,
            CrsProjection targetCrsProjection
        ) {
            var rt90Position = new RT90Position(sourceCoordinate.yLatitude, sourceCoordinate.xLongitude, sourceCoordinate.crsProjection);
            var wgs84result = rt90Position.ToWGS84();
            return CrsCoordinate.CreateCoordinatePoint(targetCrsProjection, wgs84result.Longitude, wgs84result.Latitude);
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
