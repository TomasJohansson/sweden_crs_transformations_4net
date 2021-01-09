using MightyLittleGeodesy.Positions;
using System;

namespace MightyLittleGeodesy {

    public class Transformer {

        public static CrsCoordinate Transform(CrsCoordinate sourceCoordinate, CrsProjection targetCrsProjection) {
            int targetEpsg = targetCrsProjection.GetEpsgNumber();
            if(sourceCoordinate.epsgNumber == targetEpsg) throw new ArgumentException("Trying to transform from/to the same CRS");
            if(sourceCoordinate.crsProjection.isWgs84()) {
                var wgs84position = new WGS84Position(sourceCoordinate.yLatitude, sourceCoordinate.xLongitude);
                if(targetCrsProjection.isSweref()) {
                    CrsProjection swerefProjection = CrsProjectionFactory.GetCrsProjectionByEpsgNumber(targetEpsg);
                    var position = new SWEREF99Position(wgs84position, swerefProjection);
                    return CrsCoordinate.CreateCoordinatePoint(targetEpsg, position.Longitude, position.Latitude);
                }
                else if(targetCrsProjection.isRT90()) {
                    CrsProjection rt90Projection = CrsProjectionFactory.GetCrsProjectionByEpsgNumber(targetEpsg);
                    var position = new RT90Position(wgs84position, rt90Projection);
                    return CrsCoordinate.CreateCoordinatePoint(targetEpsg, position.Longitude, position.Latitude);
                }
            }
            else if(sourceCoordinate.crsProjection.isSweref()) {
                if(targetCrsProjection.isWgs84()) {
                    CrsProjection swerefProjection = CrsProjectionFactory.GetCrsProjectionByEpsgNumber(sourceCoordinate.epsgNumber);
                    var sweref99Position = new SWEREF99Position(sourceCoordinate.yLatitude, sourceCoordinate.xLongitude, swerefProjection);
                    var wgs84result = sweref99Position.ToWGS84();
                    return CrsCoordinate.CreateCoordinatePoint(targetEpsg, wgs84result.Longitude, wgs84result.Latitude);
                }
                else {
                    // the only direct transform supported is to/from WGS84, so therefore first transform to wgs84
                    var wgs84coordinate = Transform(sourceCoordinate, CrsProjection.wgs84);
                    return Transform(wgs84coordinate, targetCrsProjection);
                }
            }
            else if(sourceCoordinate.crsProjection.isRT90()) {
                if(targetCrsProjection.isWgs84()) {
                    CrsProjection rt90Projection = CrsProjectionFactory.GetCrsProjectionByEpsgNumber(sourceCoordinate.epsgNumber);
                    var rt90Position = new RT90Position(sourceCoordinate.yLatitude, sourceCoordinate.xLongitude, rt90Projection);
                    var wgs84result = rt90Position.ToWGS84();
                    return CrsCoordinate.CreateCoordinatePoint(targetEpsg, wgs84result.Longitude, wgs84result.Latitude);
                }
                else {
                    // the only direct transform supported is to/from WGS84, so therefore first transform to wgs84
                    var wgs84coordinate = Transform(sourceCoordinate, CrsProjection.wgs84);
                    return Transform(wgs84coordinate, targetCrsProjection);
                }
            }
            throw new ArgumentException(string.Format("Unhandled source/target EPSG {0} ==> {1}", sourceCoordinate.epsgNumber, targetEpsg));
        }

    }
}
