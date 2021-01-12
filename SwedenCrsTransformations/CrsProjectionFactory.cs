using System;
using System.Collections.Generic;
using System.Linq;

namespace SwedenCrsTransformations {

    /// <summary>
    /// Class with methods for getting all projections, and for getting one projection by its EPSG number.
    /// </summary>
    public static class CrsProjectionFactory {
        public static CrsProjection GetCrsProjectionByEpsgNumber(int epsg) {
            var values = GetAllCrsProjections();
            foreach(CrsProjection value in values) {
                if(value.GetEpsgNumber() == epsg) {
                    return value;
                }
            }
            throw new ArgumentException("Could not find CrsProjection for EPSG " + epsg);
        }

        public static IList<CrsProjection> GetAllCrsProjections() {
            return ((CrsProjection[])Enum.GetValues(typeof(CrsProjection))).ToList();
        }
    }
}