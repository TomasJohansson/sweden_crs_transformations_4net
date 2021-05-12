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

namespace SwedenCrsTransformations.Transformation.TransformWithClasses {
    // Please note that the three classes (with long names) in this directory which are implementing 'TransformStrategy'
    // (e.g. one of them is this class) can only handle certain parameters.
    // The parameters must be checked first (in the fourth class in this directory i.e. 'TransformerWithClasses')
    // to determine which class to use as the 'TransformStrategy' implementation instance.
    // Regarding the reason for why even using interfaces like this, see the comments at the bottom of the file 'Transformer'.
    internal class TransformStrategy_from_SWEREF99_or_RT90_to_WGS84 : TransformStrategy {
        // Precondition: sourceCoordinate must be CRS SWEREF99 or RT90
        public CrsCoordinate Transform(
            CrsCoordinate sourceCoordinate,
            CrsProjection targetCrsProjection
        ) {
            var gkProjection = GaussKreugerFactory.getInstance().getGaussKreuger(sourceCoordinate.CrsProjection);
            LatLon latLon = gkProjection.grid_to_geodetic(sourceCoordinate.LatitudeY, sourceCoordinate.LongitudeX);
            return CrsCoordinate.CreateCoordinate(targetCrsProjection, latLon.LatitudeY, latLon.LongitudeX);
        }
    }

}