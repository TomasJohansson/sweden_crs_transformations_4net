namespace MightyLittleGeodesy.Classes {
    // This class was not part of the original 'MightyLittleGeodesy'
    // but the class 'GaussKreuger' has later been changed to return this 'LonLat' instead of array 'double[]'
    internal class LonLat {
        public double xLongitude { get; private set; }
        public double yLatitude { get; private set; }
        public LonLat(double xLongitude, double yLatitude) {
            this.xLongitude = xLongitude;
            this.yLatitude = yLatitude;
        }
    }
}
