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
using MightyLittleGeodesy;
using System;

namespace SwedenCrsTransformations.Transformation.Transformer2 {
    internal class Transformer2 : TransformStrategy {

        public CrsCoordinate Transform(
            CrsCoordinate sourceCoordinate,
            CrsProjection targetCrsProjection
        ) {
            if (sourceCoordinate.CrsProjection == targetCrsProjection) return sourceCoordinate;

            // Transform FROM wgs84:
            if (
                sourceCoordinate.CrsProjection.IsWgs84()
                &&
                (targetCrsProjection.IsSweref() || targetCrsProjection.IsRT90())
            )
            {
                return method_transformStrategy_from_WGS84_to_SWEREF99_or_RT90(sourceCoordinate, targetCrsProjection);
            }

            // Transform TO wgs84:
            else if (
                targetCrsProjection.IsWgs84()
                &&
                (sourceCoordinate.CrsProjection.IsSweref() || sourceCoordinate.CrsProjection.IsRT90())
            )
            {
                return method_transformStrategy_from_SWEREF99_or_RT90_to_WGS84(sourceCoordinate, targetCrsProjection);
            }

            // Transform between two non-wgs84:
            else if (
                (sourceCoordinate.CrsProjection.IsSweref() || sourceCoordinate.CrsProjection.IsRT90())
                &&
                (targetCrsProjection.IsSweref() || targetCrsProjection.IsRT90())
            )
            {
                // the only direct transform supported is to/from WGS84, so therefore first transform to wgs84
                return method_transFormStrategy_From_Sweref99OrRT90_to_WGS84_andThenToRealTarget(sourceCoordinate, targetCrsProjection);
            }

            throw new ArgumentException(string.Format("Unhandled source/target projection transformation: {0} ==> {1}", sourceCoordinate.CrsProjection, targetCrsProjection));
        }

        private CrsCoordinate method_transformStrategy_from_WGS84_to_SWEREF99_or_RT90(
            CrsCoordinate sourceCoordinate,
            CrsProjection targetCrsProjection
        )
        {
            var gkProjection = GaussKreugerFactory.getInstance().getGaussKreuger(targetCrsProjection);
            LatLon latLon = gkProjection.geodetic_to_grid(sourceCoordinate.LatitudeY, sourceCoordinate.LongitudeX);
            return CrsCoordinate.CreateCoordinate(targetCrsProjection, latLon.LatitudeY, latLon.LongitudeX);
        }

        private CrsCoordinate method_transformStrategy_from_SWEREF99_or_RT90_to_WGS84(
            CrsCoordinate sourceCoordinate,
            CrsProjection targetCrsProjection
        )
        {
            var gkProjection = GaussKreugerFactory.getInstance().getGaussKreuger(sourceCoordinate.CrsProjection);
            LatLon latLon = gkProjection.grid_to_geodetic(sourceCoordinate.LatitudeY, sourceCoordinate.LongitudeX);
            return CrsCoordinate.CreateCoordinate(targetCrsProjection, latLon.LatitudeY, latLon.LongitudeX);
        }

        private CrsCoordinate method_transFormStrategy_From_Sweref99OrRT90_to_WGS84_andThenToRealTarget(
            CrsCoordinate sourceCoordinate,
            CrsProjection targetCrsProjection
        )
        {
            var wgs84coordinate = Transformer.Transform(sourceCoordinate, CrsProjection.wgs84);
            return Transformer.Transform(wgs84coordinate, targetCrsProjection);
        }
    }
}
