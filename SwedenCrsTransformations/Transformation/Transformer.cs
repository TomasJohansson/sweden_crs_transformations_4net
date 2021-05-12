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
/*
TODO more comments here about why there are different Transform* classes and why they are implemented as they are
in parallell directories doing the same thing but with different implementations ...
('TransformerWithClasses' and 'TransformerWithMethods')
NOTE: The above two mentioned classes are refering to this place i.e. the bottom of the file with the 'Transformer'
so add and keep the explaining comments here...
*/