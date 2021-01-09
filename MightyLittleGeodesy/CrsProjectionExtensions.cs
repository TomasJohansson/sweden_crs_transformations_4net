namespace MightyLittleGeodesy
{
    public static class CrsProjectionExtensions {
        public static int GetEpsgNumber(this CrsProjection crsProjection) { 
            // the EPSG numbers have been used as the values in this enum (which will replace SWEREFProjection and RT90Projection)
            return (int)crsProjection;
        }
    }
}