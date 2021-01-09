namespace MightyLittleGeodesy
{
    public class CrsCoordinate {
        public readonly int epsgNumber;
        public readonly double xLongitude;
        public readonly double yLatitude;
        public CrsCoordinate(
            int epsgNumber,
            double xLongitude,
            double yLatitude
        ) {
            this.epsgNumber = epsgNumber;
            this.xLongitude = xLongitude;
            this.yLatitude = yLatitude;
        }
    }
}
