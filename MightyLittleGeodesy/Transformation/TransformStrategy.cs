namespace MightyLittleGeodesy {
    internal interface TransformStrategy {
        CrsCoordinate Transform(
            CrsCoordinate sourceCoordinate,
            CrsProjection targetCrsProjection
        );
    }

}
