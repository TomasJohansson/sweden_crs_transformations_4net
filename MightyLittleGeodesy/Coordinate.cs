namespace MightyLittleGeodesy
{
    public class Coordinate {
        public readonly int epsgNumber;
        public readonly double xLongitude;
        public readonly double yLatitude;
        public Coordinate(
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
