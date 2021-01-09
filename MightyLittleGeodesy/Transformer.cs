using MightyLittleGeodesy.Positions;
using System;

namespace MightyLittleGeodesy
{

    public class Transformer {
        public const int epsgForWgs84 = 4326;
        private const int epsgForSweref99tm = 3006;

        //private const int epsgLowerValueForSwerefLocal = 3007; // the NATIONAL sweref99TM has value 3006 as in the above constant
        //private const int epsgUpperValueForSwerefLocal = 3018;
        private const int epsgLowerValueForSweref = epsgForSweref99tm;
        private const int epsgUpperValueForSweref = 3018;

        private const int epsgLowerValueForRT90 = 3019;
        private const int epsgUpperValueForRT90 = 3024;

        // TODO use CrsProjection as parameter instead of integer
        // TODO implement extension methods such as isWgs84 for the CrsProjection
        public static CrsCoordinate Transform(CrsCoordinate sourceCoordinate, int targetEpsg) {
            if(sourceCoordinate.epsgNumber == targetEpsg) throw new ArgumentException("Trying to transform from/to the same CRS");
            if(isWgs84(sourceCoordinate.epsgNumber)) { // TODO implement 'isWgs84' as a method in CrsCoordinate or an extension method of CrsProjection
                var wgs84position = new WGS84Position(sourceCoordinate.yLatitude, sourceCoordinate.xLongitude);
                if(isSweref(targetEpsg)) {
                    CrsProjection swerefProjection = CrsProjectionFactory.GetCrsProjectionByEpsgNumber(targetEpsg);
                    var position = new SWEREF99Position(wgs84position, swerefProjection);
                    return CrsCoordinate.CreateCoordinatePoint(targetEpsg, position.Longitude, position.Latitude);
                }
                else if(isRT90(targetEpsg)) {
                    CrsProjection rt90Projection = CrsProjectionFactory.GetCrsProjectionByEpsgNumber(targetEpsg);
                    var position = new RT90Position(wgs84position, rt90Projection);
                    return CrsCoordinate.CreateCoordinatePoint(targetEpsg, position.Longitude, position.Latitude);
                }
            }
            else if(isSweref(sourceCoordinate.epsgNumber)) {
                if(isWgs84(targetEpsg)) {
                    CrsProjection swerefProjection = CrsProjectionFactory.GetCrsProjectionByEpsgNumber(sourceCoordinate.epsgNumber);
                    var sweref99Position = new SWEREF99Position(sourceCoordinate.yLatitude, sourceCoordinate.xLongitude, swerefProjection);
                    var wgs84result = sweref99Position.ToWGS84();
                    return CrsCoordinate.CreateCoordinatePoint(targetEpsg, wgs84result.Longitude, wgs84result.Latitude);
                }
                else {
                    // the only direct transform supported is to/from WGS84, so therefore first transform to wgs84
                    var wgs84coordinate = Transform(sourceCoordinate, epsgForWgs84);
                    return Transform(wgs84coordinate, targetEpsg);
                }
            }
            else if(isRT90(sourceCoordinate.epsgNumber)) {
                if(isWgs84(targetEpsg)) {
                    CrsProjection rt90Projection = CrsProjectionFactory.GetCrsProjectionByEpsgNumber(sourceCoordinate.epsgNumber);
                    var rt90Position = new RT90Position(sourceCoordinate.yLatitude, sourceCoordinate.xLongitude, rt90Projection);
                    var wgs84result = rt90Position.ToWGS84();
                    return CrsCoordinate.CreateCoordinatePoint(targetEpsg, wgs84result.Longitude, wgs84result.Latitude);
                }
                else {
                    // the only direct transform supported is to/from WGS84, so therefore first transform to wgs84
                    var wgs84coordinate = Transform(sourceCoordinate, epsgForWgs84);
                    return Transform(wgs84coordinate, targetEpsg);
                }
            }
            throw new ArgumentException(string.Format("Unhandled source/target EPSG {0} ==> {1}", sourceCoordinate.epsgNumber, targetEpsg));
        }

        private static bool isWgs84(int epsgNumber) {
            return epsgNumber == epsgForWgs84;
        }
        private static bool isSweref(int epsgNumber) {
            return epsgLowerValueForSweref <= epsgNumber && epsgNumber <= epsgUpperValueForSweref;
        }
        private static bool isRT90(int epsgNumber) {
            return epsgLowerValueForRT90 <= epsgNumber && epsgNumber <= epsgUpperValueForRT90;
        }
    }
}
