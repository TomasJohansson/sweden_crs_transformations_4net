# SwedenCrsTransformations
'SwedenCrsTransformations' is a C#.NET library for transforming geographic coordinates between the following three kind of CRS (Coordinate Reference Systems): WGS84, SWEREF99 and RT90.
(13 versions of SWEREF99, and 6 versions of RT90)

The library is based on [MightyLittleGeodesy](https://github.com/bjornsallarp/MightyLittleGeodesy/) which in turn is based on a [javascript library by Arnold Andreasson](http://latlong.mellifica.se/).

The main part of 'MightyLittleGeodesy' which has been kept is the mathematical calculations in the class 'GaussKreuger.cs'.
(although some modifications have been done in the class [GaussKreuger](https://github.com/TomasJohansson/sweden_crs_transformations/blob/master_MightyLittleGeodesy/SwedenCrsTransformations/MightyLittleGeodesy/Classes/GaussKreuger.cs))

# NuGet release

No.  
I have currently not released it anywhere except from here at this github repository, and currently I have no plans of releasing it at NuGet neither.  

# Code example
```C#
using SwedenCrsTransformations;
using System;
using System.Collections.Generic;

...

const double stockholmCentralStation_WGS84_latitude = 59.330231; // https://kartor.eniro.se/m/XRCfh
const double stockholmCentralStation_WGS84_longitude = 18.059196;

CrsCoordinate stockholmWGS84 = CrsCoordinate.CreateCoordinate(
    CrsProjection.wgs84,
    stockholmCentralStation_WGS84_longitude,
    stockholmCentralStation_WGS84_latitude
);

CrsCoordinate stockholmSweref99tm = stockholmWGS84.Transform(CrsProjection.sweref_99_tm);
Console.WriteLine("stockholmSweref99tm X: " + stockholmSweref99tm.LongitudeX);
Console.WriteLine("stockholmSweref99tm Y: " + stockholmSweref99tm.LatitudeY);
Console.WriteLine("stockholmSweref99tm 'ToString': " + stockholmSweref99tm.ToString());
// Output from the above:
//stockholmSweref99tm X: 674032.357
//stockholmSweref99tm Y: 6580821.991
//stockholmSweref99tm 'ToString': CrsCoordinate [ X: 674032.357 , Y: 6580821.991 , CRS: SWEREF_99_TM ]

IList<CrsProjection> allProjections = CrsProjectionFactory.GetAllCrsProjections();
foreach(var crsProjection in allProjections) {
    Console.WriteLine(stockholmWGS84.Transform(crsProjection));
}
// Output from the above loop:
//CrsCoordinate [ X: 674032.357 , Y: 6580821.991 , CRS: SWEREF_99_TM ]
//CrsCoordinate [ X: 494604.69 , Y: 6595151.116 , CRS: SWEREF_99_12_00 ]
//CrsCoordinate [ X: 409396.217 , Y: 6588340.147 , CRS: SWEREF_99_13_30 ]
//CrsCoordinate [ X: 324101.998 , Y: 6583455.373 , CRS: SWEREF_99_15_00 ]
//CrsCoordinate [ X: 238750.424 , Y: 6580494.921 , CRS: SWEREF_99_16_30 ]
//CrsCoordinate [ X: 153369.673 , Y: 6579457.649 , CRS: SWEREF_99_18_00 ]
//CrsCoordinate [ X: 366758.045 , Y: 6585657.12 , CRS: SWEREF_99_14_15 ]
//CrsCoordinate [ X: 281431.616 , Y: 6581734.696 , CRS: SWEREF_99_15_45 ]
//CrsCoordinate [ X: 196061.94 , Y: 6579735.93 , CRS: SWEREF_99_17_15 ]
//CrsCoordinate [ X: 110677.129 , Y: 6579660.051 , CRS: SWEREF_99_18_45 ]
//CrsCoordinate [ X: 25305.238 , Y: 6581507.028 , CRS: SWEREF_99_20_15 ]
//CrsCoordinate [ X: -60025.629 , Y: 6585277.577 , CRS: SWEREF_99_21_45 ]
//CrsCoordinate [ X: -145287.219 , Y: 6590973.148 , CRS: SWEREF_99_23_15 ]
//CrsCoordinate [ X: 1884004.1 , Y: 6598325.639 , CRS: RT90_7_5_GON_V ]
//CrsCoordinate [ X: 1756244.287 , Y: 6587493.237 , CRS: RT90_5_0_GON_V ]
//CrsCoordinate [ X: 1628293.886 , Y: 6580994.18 , CRS: RT90_2_5_GON_V ]
//CrsCoordinate [ X: 1500248.374 , Y: 6578822.84 , CRS: RT90_0_0_GON_V ]
//CrsCoordinate [ X: 1372202.721 , Y: 6580977.349 , CRS: RT90_2_5_GON_O ]
//CrsCoordinate [ X: 1244251.702 , Y: 6587459.595 , CRS: RT90_5_0_GON_O ]
//CrsCoordinate [ Longitude: 18.059196 , Latitude: 59.330231 , CRS: WGS84 ]
```

# License

MIT.   
'SwedenCrsTransformations' started as a fork of 'MightyLittleGeodesy' and it is licensed with the MIT license just like 'MightyLittleGeodesy' (see below).

# License for the forked repository [MightyLittleGeodesy](https://github.com/bjornsallarp/MightyLittleGeodesy/)

The text below has been copied from the above webpage (the forked repository):
> The calculations in this library is based on the excellent javascript library by Arnold Andreasson which is published under the Creative Commons license. However, as agreed with mr Andreasson, MightyLittleGeodesy is now licensed under the MIT license.

The text below has been copied from [one of the source files for MightyLittleGeodesy](https://github.com/bjornsallarp/MightyLittleGeodesy/blob/83491fc6e7454f5d90d792610b317eca7a332334/MightyLittleGeodesy/Classes/GaussKreuger.cs).
```C#
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
 ```