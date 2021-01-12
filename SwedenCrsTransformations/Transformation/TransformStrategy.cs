namespace SwedenCrsTransformations.Transformation {
    internal interface TransformStrategy {
        CrsCoordinate Transform(
            CrsCoordinate sourceCoordinate,
            CrsProjection targetCrsProjection
        );
    }

}
