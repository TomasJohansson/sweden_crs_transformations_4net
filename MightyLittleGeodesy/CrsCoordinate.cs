namespace MightyLittleGeodesy
{
    public class CrsCoordinate {

        public CrsProjection crsProjection { get; private set; }
        
        [System.Obsolete("Instead use CrsProjection")]
        public int epsgNumber { 
            get {
                return crsProjection.GetEpsgNumber();
            }
        }

        public double xLongitude { get; private set; }
        public double yLatitude { get; private set; }

        private CrsCoordinate(
            CrsProjection crsProjection,
            double xLongitude,
            double yLatitude
        ) {
            this.crsProjection = crsProjection;
            this.xLongitude = xLongitude;
            this.yLatitude = yLatitude;
        }

        public static CrsCoordinate CreateCoordinatePoint(
            int epsgNumber,
            double xLongitude,
            double yLatitude
        ) {
            CrsProjection crsProjection = ProjectionFactory.GetCrsProjectionByEpsgNumber(epsgNumber);
            return CreateCoordinatePoint(crsProjection, xLongitude, yLatitude);
        }

        public static CrsCoordinate CreateCoordinatePoint(
            CrsProjection crsProjection,
            double xLongitude,
            double yLatitude
        ) {
            return new CrsCoordinate(crsProjection, xLongitude, yLatitude);
        }
    }
}
