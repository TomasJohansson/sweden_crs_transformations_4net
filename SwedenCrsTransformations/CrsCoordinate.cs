using SwedenCrsTransformations.Transformation;
using System;
using System.Collections.Generic;

namespace SwedenCrsTransformations {
    public class CrsCoordinate : IEquatable<CrsCoordinate> {

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

        // ----------------------------------------------------------------------------------------------------------------------
        // These five methods below was generated with Visual Studio 2019
        public override bool Equals(object obj) {
            return Equals(obj as CrsCoordinate);
        }

        public bool Equals(CrsCoordinate other) {
            return other != null &&
                   CrsProjection == other.CrsProjection &&
                   XLongitude == other.XLongitude &&
                   YLatitude == other.YLatitude;
        }

        public override int GetHashCode() {
            int hashCode = 1147467376;
            hashCode = hashCode * -1521134295 + CrsProjection.GetHashCode();
            hashCode = hashCode * -1521134295 + XLongitude.GetHashCode();
            hashCode = hashCode * -1521134295 + YLatitude.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(CrsCoordinate left, CrsCoordinate right) {
            return EqualityComparer<CrsCoordinate>.Default.Equals(left, right);
        }

        public static bool operator !=(CrsCoordinate left, CrsCoordinate right) {
            return !(left == right);
        }

        // These five methods above was generated with Visual Studio 2019
        // ----------------------------------------------------------------------------------------------------------------------
    }
}
