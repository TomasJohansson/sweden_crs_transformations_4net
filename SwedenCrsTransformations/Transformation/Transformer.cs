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

using SwedenCrsTransformations.Transformation.TransformWithClasses;
using SwedenCrsTransformations.Transformation.TransformWithMethods;

namespace SwedenCrsTransformations.Transformation {
    internal class Transformer {

        //private static TransformStrategy transformer = new TransformerWithClasses();
        private static TransformStrategy transformer = new TransformerWithMethods();

        public static CrsCoordinate Transform(CrsCoordinate sourceCoordinate, CrsProjection targetCrsProjection) {
            return transformer.Transform(sourceCoordinate, targetCrsProjection);
       }

    }

}