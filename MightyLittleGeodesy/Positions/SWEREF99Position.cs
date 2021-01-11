﻿/*
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

using MightyLittleGeodesy.Classes;

namespace MightyLittleGeodesy.Positions
{
    public class SWEREF99Position : Position
    {

        /// <summary>
        /// Create a Sweref99 position from double values with 
        /// Sweref 99 TM as default projection.
        /// </summary>
        /// <param name="n"></param>
        /// <param name="e"></param>
        public SWEREF99Position(double n, double e)
            : base(n, e, Grid.SWEREF99) // n=north=y/Latitude   e=east=x/Longitude
        {
            Projection = CrsProjection.sweref_99_tm;
        }

        /// <summary>
        /// Create a Sweref99 position from double values. Supply the projection
        /// for values other than Sweref 99 TM
        /// </summary>
        /// <param name="n"></param>
        /// <param name="e"></param>
        /// <param name="projection"></param>
        public SWEREF99Position(double n, double e, CrsProjection projection)
            : base(n, e, Grid.SWEREF99) // n=north=y/Latitude   e=east=x/Longitude
        {
            Projection = projection;
        }

                /// <summary>
        /// Create a RT90 position by converting a WGS84 position
        /// </summary>
        /// <param name="position">WGS84 position to convert</param>
        /// <param name="rt90projection">Projection to convert to</param>
        public SWEREF99Position(WGS84Position position, CrsProjection projection)
            : base(Grid.SWEREF99)
        {
            GaussKreuger gkProjection = new GaussKreuger();
            gkProjection.swedish_params(projection);
            LonLat lonLat = gkProjection.geodetic_to_grid(position.Latitude, position.Longitude);
            Latitude = lonLat.yLatitude;    // lat_lon[0];
            Longitude = lonLat.xLongitude;  // lat_lon[1];
            Projection = projection;
        }

        /// <summary>
        /// Convert the position to WGS84 format
        /// </summary>
        /// <returns></returns>
        public WGS84Position ToWGS84()
        {
            GaussKreuger gkProjection = new GaussKreuger();
            gkProjection.swedish_params(Projection);
            LonLat lonLat = gkProjection.grid_to_geodetic(Latitude, Longitude); 

            WGS84Position newPos = new WGS84Position()
            {
                Latitude = lonLat.yLatitude, //  lat_lon[0],
                Longitude = lonLat.xLongitude, // lat_lon[1],
                GridFormat = Grid.WGS84
            };

            return newPos;
        }

        public CrsProjection Projection { get; set; }

        public override string ToString()
        {
            return string.Format("N: {0} E: {1} Projection: {2}", Latitude, Longitude, Projection);
        }
    }
}
