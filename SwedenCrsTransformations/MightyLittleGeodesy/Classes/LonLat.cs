/*
* Copyright (c) Tomas Johansson , http://www.programmerare.com
* The code in this library is licensed with MIT.
* The library is based on the library 'MightyLittleGeodesy' (https://github.com/bjornsallarp/MightyLittleGeodesy/) 
* which is also released with MIT.
* For more information see the webpage below.
* https://github.com/TomasJohansson/sweden_crs_transformations_4net
*/

namespace MightyLittleGeodesy.Classes {
    // This class was not part of the original 'MightyLittleGeodesy'
    // but the class 'GaussKreuger' has later been changed to return this 'LonLat' instead of array 'double[]'
    internal class LonLat {
        public double LongitudeX { get; private set; }
        public double LatitudeY { get; private set; }
        public LonLat(double xLongitude, double yLatitude) {
            this.LongitudeX = xLongitude;
            this.LatitudeY = yLatitude;
        }
    }
}
