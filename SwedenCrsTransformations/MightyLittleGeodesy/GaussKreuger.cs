/*
* Copyright (c) Tomas Johansson , http://www.programmerare.com
* The code in this library is licensed with MIT.
* The library is based on the library 'MightyLittleGeodesy' (https://github.com/bjornsallarp/MightyLittleGeodesy/) 
* which is also released with MIT.
* License information about 'sweden_crs_transformations_4net' and 'MightyLittleGeodesy':
* https://github.com/TomasJohansson/sweden_crs_transformations_4net/blob/csharpe_SwedenCrsTransformations/LICENSE
* For more information see the webpage below.
* https://github.com/TomasJohansson/sweden_crs_transformations_4net
*/

// This project is based on the library [MightyLittleGeodesy](https://github.com/bjornsallarp/MightyLittleGeodesy/)
// It started as a fork, but then most of the original code is gone.
// The main part that is still used is this file with the mathematical calculations i.e. the file "GaussKreuger.cs"
// Although there has been some modifications of this file too, as mentioned below.

// https://github.com/bjornsallarp/MightyLittleGeodesy/blob/83491fc6e7454f5d90d792610b317eca7a332334/MightyLittleGeodesy/Classes/GaussKreuger.cs
// The original version of the below class 'GaussKreuger' is located at the above URL.
// That original version has been modified below in this file below but not in a significant way (e.g. the mathematical calculations has not been modified).
// The modifications:
//      - changed the class from public to internal i.e. "public class GaussKreuger" ==> "internal class GaussKreuger"
//      - a new 'LatLon' class is used as return type from two methods instead of returning an array "double[]"
//              i.e. the two method signatures have changed as below:
//              "public double[] geodetic_to_grid(double latitude, double longitude)"  ==> "public LatLon geodetic_to_grid(double latitude, double longitude)"
//              "public double[] grid_to_geodetic(double x, double y)" ==> "public LatLon grid_to_geodetic(double yLatitude, double xLongitude)"
//      - renamed and changed order of the parameters for the method "grid_to_geodetic" (see the above line)
//      - changed the method "swedish_params" to use an enum as parameter instead of string, i.e. the method signature changed as below:
//              "public void swedish_params(string projection)" ==> "public void swedish_params(CrsProjection projection)"
//      - now the if/else statements in the implementation of the above method "swedish_params" compares with the enum values for CrsProjection instead of comparing with string literals
//      - removed the if/else statements in the above method "swedish_params" which used the projection strings beginning with "bessel_rt90"
//      - updated the GaussKreuger class to be immutable with readonly fields, and the methods (e.g. the above mentioned method "swedish_params") that 
//          previously initialized (mutated) the fields have instead been moved to another class and is provided as a 
//          parameter object to the constructor which copies the values into the readonly fields.   
// 
// For more details about exactly what has changed in this GaussKreuger class, you can also use a git client with "compare" or "blame" features to see the changes)

// ------------------------------------------------------------------------------------------
// The below comment block is kept from the original source file (see the above github URL)
/*
 * MightyLittleGeodesy 
 * RT90, SWEREF99 and WGS84 coordinate transformation library
 * 
 * Read my blog @ http://blog.sallarp.com
 * 
 * 
 * Copyright (C) 2009 Björn Sållarp
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this 
 * software and associated documentation files (the "Software"), to deal in the Software 
 * without restriction, including without limitation the rights to use, copy, modify, 
 * merge, publish, distribute, sublicense, and/or sell copies of the Software, and to 
 * permit persons to whom the Software is furnished to do so, subject to the following 
 * conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all copies or 
 * substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING 
 * BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND 
 * NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, 
 * DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */
// ------------------------------------------------------------------------------------------

using System;

namespace MightyLittleGeodesy {
    /*
     * .NET-implementation of "Gauss Conformal Projection 
     * (Transverse Mercator), Krügers Formulas".
     * - Parameters for SWEREF99 lat-long to/from RT90 and SWEREF99 
     * coordinates (RT90 and SWEREF99 are used in Swedish maps).
     * 
     * The calculations are based entirely on the excellent
     * javscript library by Arnold Andreassons.
     * Source: http://www.lantmateriet.se/geodesi/
     * Source: Arnold Andreasson, 2007. http://mellifica.se/konsult
     * Author: Björn Sållarp. 2009. http://blog.sallarp.com
     * 
     * Some modifications in this file were made 2021 by Tomas Johansson.
     * For details about changes, you should be able to use the github repository to see the git history where you found this source code file.
     */

    internal class GaussKreuger
    {
        // Immutable class with all fields 'readonly'
        private readonly double axis; // Semi-major axis of the ellipsoid.
        private readonly double flattening; // Flattening of the ellipsoid.
        private readonly double central_meridian; // Central meridian for the projection.    
        private readonly double scale; // Scale on central meridian.
        private readonly double false_northing; // Offset for origo.
        private readonly double false_easting; // Offset for origo.
        // The above six fields will simply be copied from the parameter
        // The below fields will be calculated in the constructor
        private readonly double e2, n, a_roof, deg_to_rad, lambda_zero;
        private readonly double n_2, n_3, n_4, e2_2, e2_3, e2_4;
        private readonly double scale_multiplied_with_a_roof;

        private readonly double geodetic_A, geodetic_B, geodetic_C, geodetic_D, geodetic_beta1, geodetic_beta2, geodetic_beta3, geodetic_beta4;
        private readonly double grid_delta1, grid_delta2, grid_delta3, grid_delta4, grid_Astar, grid_Bstar, grid_Cstar, grid_Dstar;

        private GaussKreuger(GaussKreugerParameterObject gaussKreugerParameterObject) {
            this.axis = gaussKreugerParameterObject.axis;
            this.flattening = gaussKreugerParameterObject.flattening;
            this.central_meridian = gaussKreugerParameterObject.central_meridian;
            this.scale = gaussKreugerParameterObject.scale;
            this.false_northing = gaussKreugerParameterObject.false_northing;
            this.false_easting = gaussKreugerParameterObject.false_easting;


            // These fields below are always needed by both transform methods (i.e. regardless of the direction of the transformation)
            // and therefore the code duplication (i.e. the same calculation) have been reduced by moving the code here
            // and also if the GaussKreuger is reused then these values need not be calculated again since they do not depend on the method parameters
            e2 = flattening * (2.0 - flattening);
            n = flattening / (2.0 - flattening);
            a_roof = axis / (1.0 + n) * (1.0 + n * n / 4.0 + n * n * n * n / 64.0);
            deg_to_rad = Math.PI / 180.0;
            lambda_zero = central_meridian * deg_to_rad;
            n_2 = n * n;
            n_3 = n * n_2;
            n_4 = n * n_3;
            e2_2 = e2 * e2;
            e2_3 = e2_2 * e2;
            e2_4 = e2_3 * e2;
            scale_multiplied_with_a_roof = scale * a_roof;
            // The above (about 12) calculated fields are used by both transform methods 
            // i.e. the methods 'geodetic_to_grid' and 'grid_to_geodetic'.
            // Below are some other 16 fields (8 per transform method) which are calculated here at construction since they 
            // do not depend on a method parameter (i.e. the specific coordinate to be transformed).
            // If only one of the two transform methods will be used with the created 'GaussKreuger' instance,
            // then 8 of these 16 calculations below were unneedingly calculated.
            // However, if only used once, those calculations are fast anyway, and there 
            // will be more benefit to win if lots of transformations are done, when calculations 
            // not depending on the coordinates have been moved out from the transform methods.
            // Another option for the implementation (instead of simply calculating all 16 fields here as currently)
            // would be to use lazy loading of these values from each of the transform methods.
            // But thread-safe lazy loading is maybe not easily implemented with all programming languages 
            // which this library is intended to become ported to, and therefore it maybe is a bit overkill
            // to care too much about these 8 potentially unneeded calculations below.

            // These below variables with the prefix 'geodetic_' are specific for this method 'geodetic_to_grid'
            // (unlike many other calculated values in the constructor which are common for both transform methods)
            geodetic_A = e2;
            geodetic_B = (5.0 * e2_2 - e2_3) / 6.0;
            geodetic_C = (104.0 * e2_3 - 45.0 * e2_4) / 120.0;
            geodetic_D = (1237.0 * e2_4) / 1260.0;
            geodetic_beta1 = n / 2.0 - 2.0 * n_2 / 3.0 + 5.0 * n_3 / 16.0 + 41.0 * n_4 / 180.0;
            geodetic_beta2 = 13.0 * n_2 / 48.0 - 3.0 * n_3 / 5.0 + 557.0 * n_4 / 1440.0;
            geodetic_beta3 = 61.0 * n_3 / 240.0 - 103.0 * n_4 / 140.0;
            geodetic_beta4 = 49561.0 * n_4 / 161280.0;

            // These below variables with the prefix 'grid_' are specific for this method 'grid_to_geodetic'
            // (unlike many other calculated values in the constructor which are common for both transform methods)
            grid_delta1 = n / 2.0 - 2.0 * n_2 / 3.0 + 37.0 * n_3 / 96.0 - n_4 / 360.0;
            grid_delta2 = n_2 / 48.0 + n_3 / 15.0 - 437.0 * n_4 / 1440.0;
            grid_delta3 = 17.0 * n_3 / 480.0 - 37 * n_4 / 840.0;
            grid_delta4 = 4397.0 * n_4 / 161280.0;
            grid_Astar = e2 + e2_2 + e2_3 + e2_4;
            grid_Bstar = -(7.0 * e2_2 + 17.0 * e2_3 + 30.0 * e2_4) / 6.0;
            grid_Cstar = (224.0 * e2_3 + 889.0 * e2_4) / 120.0;
            grid_Dstar = -(4279.0 * e2_4) / 1260.0;
        }
        public static GaussKreuger create(GaussKreugerParameterObject gaussKreugerParameterObject) {
            GaussKreuger gaussKreuger = new GaussKreuger(gaussKreugerParameterObject);
            return gaussKreuger;
        }

        // Conversion from geodetic coordinates to grid coordinates.
        public LatLon geodetic_to_grid(double yLatitude, double xLongitude)
        {
            double phi = yLatitude * deg_to_rad;
            double lambda = xLongitude * deg_to_rad;

            double sin_phi = Math.Sin(phi);
            double phi_star = phi - sin_phi * Math.Cos(phi) * (geodetic_A +
                            geodetic_B * Math.Pow(sin_phi, 2) +
                            geodetic_C * Math.Pow(sin_phi, 4) +
                            geodetic_D * Math.Pow(sin_phi, 6));
            double delta_lambda = lambda - lambda_zero;
            double xi_prim = Math.Atan(Math.Tan(phi_star) / Math.Cos(delta_lambda));
            double eta_prim = math_atanh(Math.Cos(phi_star) * Math.Sin(delta_lambda));
            double x = scale_multiplied_with_a_roof * (xi_prim +
                            geodetic_beta1 * Math.Sin(2.0 * xi_prim) * math_cosh(2.0 * eta_prim) +
                            geodetic_beta2 * Math.Sin(4.0 * xi_prim) * math_cosh(4.0 * eta_prim) +
                            geodetic_beta3 * Math.Sin(6.0 * xi_prim) * math_cosh(6.0 * eta_prim) +
                            geodetic_beta4 * Math.Sin(8.0 * xi_prim) * math_cosh(8.0 * eta_prim)) +
                            false_northing;
            double y = scale_multiplied_with_a_roof * (eta_prim +
                            geodetic_beta1 * Math.Cos(2.0 * xi_prim) * math_sinh(2.0 * eta_prim) +
                            geodetic_beta2 * Math.Cos(4.0 * xi_prim) * math_sinh(4.0 * eta_prim) +
                            geodetic_beta3 * Math.Cos(6.0 * xi_prim) * math_sinh(6.0 * eta_prim) +
                            geodetic_beta4 * Math.Cos(8.0 * xi_prim) * math_sinh(8.0 * eta_prim)) +
                            false_easting;
            return new LatLon(
                Math.Round(x * 1000.0) / 1000.0
                ,
                Math.Round(y * 1000.0) / 1000.0
            );
        }

        // Conversion from grid coordinates to geodetic coordinates.
        public LatLon grid_to_geodetic(double yLatitude, double xLongitude)
        {
            double xi = (yLatitude - false_northing) / (scale_multiplied_with_a_roof);
            double eta = (xLongitude - false_easting) / (scale_multiplied_with_a_roof);
            double xi_prim = xi -
                            grid_delta1 * Math.Sin(2.0 * xi) * math_cosh(2.0 * eta) -
                            grid_delta2 * Math.Sin(4.0 * xi) * math_cosh(4.0 * eta) -
                            grid_delta3 * Math.Sin(6.0 * xi) * math_cosh(6.0 * eta) -
                            grid_delta4 * Math.Sin(8.0 * xi) * math_cosh(8.0 * eta);
            double eta_prim = eta -
                            grid_delta1 * Math.Cos(2.0 * xi) * math_sinh(2.0 * eta) -
                            grid_delta2 * Math.Cos(4.0 * xi) * math_sinh(4.0 * eta) -
                            grid_delta3 * Math.Cos(6.0 * xi) * math_sinh(6.0 * eta) -
                            grid_delta4 * Math.Cos(8.0 * xi) * math_sinh(8.0 * eta);
            double phi_star = Math.Asin(Math.Sin(xi_prim) / math_cosh(eta_prim));
            double delta_lambda = Math.Atan(math_sinh(eta_prim) / Math.Cos(xi_prim));
            double lon_radian = lambda_zero + delta_lambda;
            double sin_phi_star = Math.Sin(phi_star);
            double lat_radian = phi_star + sin_phi_star * Math.Cos(phi_star) *
                            (grid_Astar +
                             grid_Bstar * Math.Pow(sin_phi_star, 2) +
                             grid_Cstar * Math.Pow(sin_phi_star, 4) +
                             grid_Dstar * Math.Pow(sin_phi_star, 6));
            return new LatLon(
                lat_radian * 180.0 / Math.PI
                ,
                lon_radian * 180.0 / Math.PI
            );
        }


        private double math_sinh(double value) {
            return 0.5 * (Math.Exp(value) - Math.Exp(-value));
        }
        private double math_cosh(double value) {
            return 0.5 * (Math.Exp(value) + Math.Exp(-value));
        }
        private double math_atanh(double value) {
            return 0.5 * Math.Log((1.0 + value) / (1.0 - value));
        }

    }
}

