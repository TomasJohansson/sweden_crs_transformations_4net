namespace MightyLittleGeodesy
{
    public class CrsCoordinate {
        public int epsgNumber { get; private set; }
        public double xLongitude { get; private set; }
        public double yLatitude { get; private set; }

        private CrsCoordinate(
            int epsgNumber,
            double xLongitude,
            double yLatitude
        ) {
            this.epsgNumber = epsgNumber;
            this.xLongitude = xLongitude;
            this.yLatitude = yLatitude;
        }

        public static CrsCoordinate CreateCoordinatePoint(
            int epsgNumber,
            double xLongitude,
            double yLatitude
        ) {
            return new CrsCoordinate(epsgNumber, xLongitude, yLatitude);
        }
    }
}
