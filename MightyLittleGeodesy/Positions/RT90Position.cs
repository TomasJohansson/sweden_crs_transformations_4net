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

using MightyLittleGeodesy.Classes;

namespace MightyLittleGeodesy.Positions
{
    public class RT90Position : Position
    {

        /// <summary>
        /// Create a new position using default projection (2.5 gon v);
        /// </summary>
        /// <param name="yLatitude">X value</param>
        /// <param name="xLongitude">Y value</param>
        public RT90Position(double yLatitude, double xLongitude)
            :base(yLatitude, xLongitude, Grid.RT90)
        {
            Projection = CrsProjection.rt90_2_5_gon_v;
        }

        /// <summary>
        /// Create a new position
        /// </summary>
        /// <param name="yLatitude"></param>
        /// <param name="xLongitude"></param>
        /// <param name="projection"></param>
        public RT90Position(double yLatitude, double xLongitude, CrsProjection projection)
            : base(yLatitude, xLongitude, Grid.RT90)
        {
            Projection = projection;
        }

        /// <summary>
        /// Create a RT90 position by converting a WGS84 position
        /// </summary>
        /// <param name="position">WGS84 position to convert</param>
        /// <param name="rt90projection">Projection to convert to</param>
        public RT90Position(WGS84Position position, CrsProjection rt90projection)
            : base(Grid.RT90)
        {
            GaussKreuger gkProjection = new GaussKreuger();
            gkProjection.swedish_params(rt90projection);
            LonLat lonLat = gkProjection.geodetic_to_grid(position.Latitude, position.Longitude);
            Latitude = lonLat.yLatitude;    // lat_lon[0];
            Longitude = lonLat.xLongitude;  // lat_lon[1];
            Projection = rt90projection;
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
                Latitude = lonLat.yLatitude,
                Longitude = lonLat.xLongitude,
                GridFormat = Grid.WGS84
            };

            return newPos;
        }

        public CrsProjection Projection { get; set; }

        public override string ToString()
        {
            return string.Format("X: {0} Y: {1} Projection: {2}", Latitude, Longitude, Projection);
        }
    }
}
