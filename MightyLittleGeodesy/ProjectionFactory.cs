using System;
using System.Collections.Generic;
using System.Linq;
using static MightyLittleGeodesy.Positions.RT90Position;
using static MightyLittleGeodesy.Positions.SWEREF99Position;

namespace MightyLittleGeodesy {
    
    /// <summary>
    /// Class with methods for getting all projections, and for getting one projection by its EPSG number.
    /// </summary>
    public static class ProjectionFactory {
        public static CrsProjection GetCrsProjectionByEpsgNumber(int epsg) {
            var values = GetAllCrsProjections();
            foreach(CrsProjection value in values) {
                if(value.GetEpsgNumber() == epsg) {
                    return value;
                }
            }
            throw new ArgumentException("Could not find RT90Projection for EPSG " + epsg);
        }

        [System.Obsolete("RT90Projection and SWEREF99Position will become replaced with CrsProjection")]
        public static SWEREFProjection GetSwerefProjectionByEpsgNumber(int epsg) {
            var values = GetAllSwerefProjections();
            foreach(SWEREFProjection value in values) {
                if(value.GetEpsgNumber() == epsg) {
                    return value;
                }
            }
            throw new ArgumentException("Could not find SWEREFProjection for EPSG " + epsg);
        }
        [System.Obsolete("RT90Projection and SWEREF99Position will become replaced with CrsProjection")]
        public static RT90Projection GetRT90ProjectionProjectionByEpsgNumber(int epsg) {
            var values = GetAllRT90Projections();
            foreach(RT90Projection value in values) {
                if(value.GetEpsgNumber() == epsg) {
                    return value;
                }
            }
            throw new ArgumentException("Could not find RT90Projection for EPSG " + epsg);
        }

        
        public static IList<CrsProjection> GetAllCrsProjections() {
            return ((CrsProjection[])Enum.GetValues(typeof(CrsProjection))).ToList();
        }
        public static IList<SWEREFProjection> GetAllSwerefProjections() {
            return ((SWEREFProjection[])Enum.GetValues(typeof(SWEREFProjection))).ToList();
        }
        public static IList<RT90Projection> GetAllRT90Projections() {
            return ((RT90Projection[])Enum.GetValues(typeof(RT90Projection))).ToList();
        }
    }

    public static class ProjectionEnumExtensions {
        public static int GetEpsgNumber(this CrsProjection crsProjection) { 
            // the EPSG numbers have been used as the values in this enum (which will replace SWEREFProjection and RT90Projection)
            return (int)crsProjection;
        }

        [System.Obsolete("RT90Projection and SWEREF99Position will become replaced with CrsProjection")]
        public static int GetEpsgNumber(this SWEREFProjection swerefProjection) {
            // The values in the enum happen to be defined in the same order as the EPSG number i.e. the enum values:
            //sweref_99_tm = 0,
            //sweref_99_12_00 = 1,
            //...
            //sweref_99_21_45 = 11,
            //sweref_99_23_15 = 12

            // while the correspondign EPSG values are (in the same order) 3006-3018, so therefore add 3006 to the value
            int theEnumValue = (int)swerefProjection;
            return theEnumValue + 3006;
        }

        [System.Obsolete("RT90Projection and SWEREF99Position will become replaced with CrsProjection")]
        public static int GetEpsgNumber(this RT90Projection rt90Projection) {
            // The values in the enum happen to be defined in the same order as the EPSG number i.e. the enum values:
            //rt90_7_5_gon_v = 0,
            // ...
            //rt90_5_0_gon_o = 5

            // while the correspondign EPSG values are (in the same order) 3019-3024, so therefore add 3019 to the value
            int theEnumValue = (int)rt90Projection;
            return theEnumValue + 3019;
        }
    }

}
