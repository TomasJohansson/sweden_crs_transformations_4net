namespace SwedenCrsTransformations {
    
    public enum CrsProjection {
            wgs84 = 4326,

            sweref_99_tm = 3006, // national sweref99 CRS
            // local sweref99 systems:
            sweref_99_12_00 = 3007,
            sweref_99_13_30 = 3008,
            sweref_99_15_00 = 3009,
            sweref_99_16_30 = 3010,
            sweref_99_18_00 = 3011,
            sweref_99_14_15 = 3012,
            sweref_99_15_45 = 3013,
            sweref_99_17_15 = 3014,
            sweref_99_18_45 = 3015,
            sweref_99_20_15 = 3016,
            sweref_99_21_45 = 3017,
            sweref_99_23_15 = 3018,

            rt90_7_5_gon_v = 3019,
            rt90_5_0_gon_v = 3020,
            rt90_2_5_gon_v = 3021,
            rt90_0_0_gon_v = 3022,
            rt90_2_5_gon_o = 3023,
            rt90_5_0_gon_o = 3024
    }
}
