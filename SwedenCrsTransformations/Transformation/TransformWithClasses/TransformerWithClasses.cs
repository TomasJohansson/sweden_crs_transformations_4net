/*
* Copyright (c) Tomas Johansson , http://www.programmerare.com
* The code in this library is licensed with MIT.
* The library is based on the library 'MightyLittleGeodesy' (https://github.com/bjornsallarp/MightyLittleGeodesy/) 
* which is also released with MIT.
* License information about 'sweden_crs_transformations_4net' and 'MightyLittleGeodesy':
* https://github.com/TomasJohansson/sweden_crs_transformations_4net/blob/csharpe_SwedenCrsTransformations/LICENSE
* For more information see the webpage below.
* https://github.com/TomasJohansson/sweden_crs_transformations_4net
*/
using System;

namespace SwedenCrsTransformations.Transformation.TransformWithClasses {
    // Regarding the purpose of all 'Transform*' types, see the comments at the bottom of the file with the 'Transformer'
    internal class TransformerWithClasses : TransformStrategy {

        // Implementations of transformations from WGS84:
        private static readonly TransformStrategy _transformStrategy_from_WGS84_to_SWEREF99_or_RT90 = new TransformStrategy_from_WGS84_to_SWEREF99_or_RT90();

        // Implementations of transformations to WGS84:
        private static readonly TransformStrategy _transformStrategy_from_SWEREF99_or_RT90_to_WGS84 = new TransformStrategy_from_SWEREF99_or_RT90_to_WGS84();

        // Implementation first transforming to WGS84 and then to the real target:
        private static readonly TransformStrategy _transFormStrategy_From_Sweref99OrRT90_to_WGS84_andThenToRealTarget = new TransFormStrategy_From_Sweref99OrRT90_to_WGS84_andThenToRealTarget();


        public CrsCoordinate Transform(
            CrsCoordinate sourceCoordinate,
            CrsProjection targetCrsProjection
        ) {
            if (sourceCoordinate.CrsProjection == targetCrsProjection) return sourceCoordinate;

            TransformStrategy _transFormStrategy = null;

            // Transform FROM wgs84:
            if (
                sourceCoordinate.CrsProjection.IsWgs84()
                &&
                (targetCrsProjection.IsSweref() || targetCrsProjection.IsRT90())
            )
            {
                _transFormStrategy = _transformStrategy_from_WGS84_to_SWEREF99_or_RT90;
            }

            // Transform TO wgs84:
            else if (
                targetCrsProjection.IsWgs84()
                &&
                (sourceCoordinate.CrsProjection.IsSweref() || sourceCoordinate.CrsProjection.IsRT90())
            )
            {
                _transFormStrategy = _transformStrategy_from_SWEREF99_or_RT90_to_WGS84;
            }

            // Transform between two non-wgs84:
            else if (
                (sourceCoordinate.CrsProjection.IsSweref() || sourceCoordinate.CrsProjection.IsRT90())
                &&
                (targetCrsProjection.IsSweref() || targetCrsProjection.IsRT90())
            )
            {
                // the only direct transform supported is to/from WGS84, so therefore first transform to wgs84
                _transFormStrategy = _transFormStrategy_From_Sweref99OrRT90_to_WGS84_andThenToRealTarget;
            }

            if (_transFormStrategy != null)
            {
                // Please note that this is NOT a proper way of handling polymorphic interfaces 
                // as with the Liskov substitution principle.
                // The different classes implementing the interface "TransformStrategy" can only
                // handle certain parameters and the parameters have above been used for choosing
                // which "_transFormStrategy" to be used below.
                // Then why is it implemented like this? Well, the reason is explained in the bottom of the file for 'Transformer'
                return _transFormStrategy.Transform(sourceCoordinate, targetCrsProjection);
            }

            throw new ArgumentException(string.Format("Unhandled source/target projection transformation: {0} ==> {1}", sourceCoordinate.CrsProjection, targetCrsProjection));
        }
    }
}
