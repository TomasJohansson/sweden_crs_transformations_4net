﻿namespace SwedenCrsTransformations {
    public static class CrsProjectionExtensions {

        private const int epsgForSweref99tm = 3006;

        //private const int epsgLowerValueForSwerefLocal = 3007; // the NATIONAL sweref99TM has value 3006 as in the above constant
        //private const int epsgUpperValueForSwerefLocal = 3018;
        private const int epsgLowerValueForSweref = epsgForSweref99tm;
        private const int epsgUpperValueForSweref = 3018;

        private const int epsgLowerValueForRT90 = 3019;
        private const int epsgUpperValueForRT90 = 3024;

        public static int GetEpsgNumber(this CrsProjection crsProjection) { 
            // the EPSG numbers have been used as the values in this enum
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
    }

}