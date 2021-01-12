using SwedenCrsTransformations.Transformation;

namespace SwedenCrsTransformations {
    public class CrsCoordinate {

        public CrsProjection CrsProjection { get; private set; }
        
        public double XLongitude { get; private set; }
        public double YLatitude { get; private set; }

        private CrsCoordinate(
            CrsProjection crsProjection,
            double xLongitude,
            double yLatitude
        ) {
            this.CrsProjection = crsProjection;
            this.XLongitude = xLongitude;
            this.YLatitude = yLatitude;
        }

        public CrsCoordinate Transform(CrsProjection targetCrsProjection) {
            return Transformer.Transform(this, targetCrsProjection);
        }


        public static CrsCoordinate CreateCoordinatePoint(
            int epsgNumber,
            double xLongitude,
            double yLatitude
        ) {
            CrsProjection crsProjection = CrsProjectionFactory.GetCrsProjectionByEpsgNumber(epsgNumber);
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
