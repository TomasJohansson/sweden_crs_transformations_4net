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

using System;
using System.Collections.Generic;
using System.Linq;

namespace SwedenCrsTransformations {

    /// <summary>
    /// Class with methods for getting all projections, and for getting one projection by its EPSG number.
    /// (since such custom methods can not be located within the CrsProjection enum type itself)
    /// </summary>
    /// See also <see cref="CrsProjection"/>
    public static class CrsProjectionFactory {

        private readonly static IList<CrsProjection> sortedListWithAllProjections;

        private readonly static IDictionary<int, CrsProjection>
            mapWithAllCrsProjections = new Dictionary<int, CrsProjection>();

        /// <summary>
        /// Static constructor
        /// </summary>
        static CrsProjectionFactory() {
            sortedListWithAllProjections = CreateListWithAllCrsProjectionsSorted();

            IList<CrsProjection> crsProjections = sortedListWithAllProjections;
            foreach(CrsProjection crsProjection in crsProjections) {
                mapWithAllCrsProjections.Add(crsProjection.GetEpsgNumber(), crsProjection);
            }
        }

        /// <summary>
        /// Factory method creating an enum 'CrsProjection' by its number (EPSG) value.
        /// </summary>
        /// <param name="epsg">
        /// An EPSG number.
        /// https://en.wikipedia.org/wiki/EPSG_Geodetic_Parameter_Dataset
        /// https://epsg.org
        /// https://epsg.io
        /// </param>
        /// See also <see cref="CrsProjection"/>        
        public static CrsProjection GetCrsProjectionByEpsgNumber(int epsg) {
            if(mapWithAllCrsProjections.ContainsKey(epsg)) {
                return mapWithAllCrsProjections[epsg];
            }
            throw new ArgumentException("Could not find CrsProjection for EPSG " + epsg);
        }

        /// <summary>
        /// Returning a List with all supported projections.
        /// The order is: The very first item is the projection WGS84, 
        /// and after that they are increased by EPSG number,
        /// i.e.the first(after wgs84) is sweref_99_tm(EPSG 3006)
        /// and the last is rt90_5_0_gon_o(EPSG 3024)
        /// </summary>
        public static IList<CrsProjection> GetAllCrsProjections() {
            return sortedListWithAllProjections;
        }

        /// <summary>
        /// Private method called once from the static constructor.
        /// </summary>
        /// <returns>
        /// a list sorted in the order to be returned by the public method 'GetAllCrsProjections'
        /// </returns>
        private static IList<CrsProjection> CreateListWithAllCrsProjectionsSorted() {
            List<CrsProjection> allCrsProjections = ((CrsProjection[])Enum.GetValues(typeof(CrsProjection))).ToList();
            int wgs84Value = CrsProjection.wgs84.GetEpsgNumber(); // the special value which should be sorted to be the first in the returned list
            // the other values (i.e all but wgs84) should be sorted by EPSG number as documented by the public method 'GetAllCrsProjections'
            allCrsProjections.Sort((crs1, crs2) => {
                int epsg1 = crs1.GetEpsgNumber();
                int epsg2 = crs2.GetEpsgNumber();
                int diff = epsg1 - epsg2;
                if (diff != 0) {
                    if (epsg1 == wgs84Value) return -1;
                    if (epsg2 == wgs84Value) return 1;
                }
                return diff;
            });
            return allCrsProjections;
        }
    }
}