namespace SwedenCrsTransformations {
    public static class CrsProjectionExtensions {

        private const int epsgForSweref99tm = 3006;

        //private const int epsgLowerValueForSwerefLocal = 3007; // the NATIONAL sweref99TM has value 3006 as in the above constant
        //private const int epsgUpperValueForSwerefLocal = 3018;
        private const int epsgLowerValueForSweref = epsgForSweref99tm;
        private const int epsgUpperValueForSweref = 3018;

        private const int epsgLowerValueForRT90 = 3019;
        private const int epsgUpperValueForRT90 = 3024;

        public static int GetEpsgNumber(this CrsProjection crsProjection) { 
            // the EPSG numbers have been used as the values in this enum (which will replace SWEREFProjection and RT90Projection)
            return (int)crsProjection;
        }

        public static bool IsWgs84(this CrsProjection crsProjection) {
            return crsProjection == CrsProjection.wgs84;
        }
        public static bool IsSweref(this CrsProjection crsProjection) {
            int epsgNumber = crsProjection.GetEpsgNumber();
            return epsgLowerValueForSweref <= epsgNumber && epsgNumber <= epsgUpperValueForSweref;
        }
        public static bool IsRT90(this CrsProjection crsProjection) {
            int epsgNumber = crsProjection.GetEpsgNumber();
            return epsgLowerValueForRT90 <= epsgNumber && epsgNumber <= epsgUpperValueForRT90;
        }
        public static CrsGrid Grid(this CrsProjection crsProjection) {
            if(crsProjection.IsWgs84()) {
                return CrsGrid.WGS84;
            }
            else if(crsProjection.IsSweref()) {
                return CrsGrid.SWEREF99;
            }
            else if(crsProjection.IsRT90()) {
                return CrsGrid.RT90;
            }
            else {
                // TODO throw some appropriate exception instead ... or maybe even delete this extension method
                // and delete the CrsGrid since it is redundant i.e you can get the same information from it as you can by using the three is-methods above.
                return CrsGrid.WGS84;
            }
        }
    }

}