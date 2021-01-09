﻿using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using MightyLittleGeodesy;

namespace SwedenCrsTransformationsTests.CoordinateFiles {
    
    [TestClass]
    public class TransformingCoordinatesFromFileTest {

        internal const string columnSeparator = "|";
        
        // the below file "swedish_crs_transformations.csv" was copied from: https://github.com/TomasJohansson/crsTransformations/blob/a1da6c74daf040a521beb32f9f395124ffe76aa6/crs-transformation-adapter-test/src/test/resources/generated/swedish_crs_coordinates.csv
        // and it was generated with a method "createFileWithTransformationResultsForCoordinatesInSweden()" at https://github.com/TomasJohansson/crsTransformations/blob/a1da6c74daf040a521beb32f9f395124ffe76aa6/crs-transformation-adapter-test/src/test/java/com/programmerare/com/programmerare/testData/CoordinateTestDataGeneratedFromEpsgDatabaseTest.java
        private const string relativePathForFileWith_swedish_crs_transformations = "CoordinateFiles/data/swedish_crs_coordinates.csv";
        // the project file should use "CopyToOutputDirectory" for the above file


        [TestMethod]
        public void AssertThatTransformationsDoNotDifferTooMuchFromExpectedResultInFile() {
            string directory = GetPathToOutputDirectoryWhereTheDataFileShouldBeCopiedToAutomatically();
            string absolutePathToFile = Path.Combine(directory, relativePathForFileWith_swedish_crs_transformations).Replace('/', Path.DirectorySeparatorChar);
            FileInfo file = new FileInfo(absolutePathToFile);
            Assert.IsTrue(file.Exists, "Try the build action 'CopyToOutputDirectory' ! . The file could not be found: " + file.FullName);

            IList<string> problemTransformationResults = new List<string>();
            IList<string> lines = File.ReadAllLines(file.FullName);
            // The first two lines of the input file (the header row, and a data row):
                // EPSG 4326 (WGS84)Longitude for WGS84 (EPSG 4326)|Latitude for WGS84 (EPSG 4326)|EPSG 3006|X for EPSG 3006|Y for EPSG 3006|EPSG 3007-3024|X for EPSG 3007-3024|Y for EPSG 3007-3024|Implementation count for EPSG 3006 transformation|Implementation count for EPSG 3007-3024 transformation
                // 4326|12.146151472138385|58.46573396912418|3006|333538.2957000149|6484098.2550872|3007|158529.85136620898|6483166.205771873|6|6
            // The last two columns can be ignored here, but the first nine columns are in three pairs with three columns each:
            // an epsg number, and then the longitude(x) and latitude(y) for that coordinate.
            // All three coordinates in one row represents the same location but in different coordinate reference systems.
            // The first two, of the three, coordinates are for the same coordinate reference systems, WGS84 and SWEREF99TM, 
            // but the third is different for all rows (18 data rows for the local swedish CRS systems, RT90 and SWEREF99, with EPSG codes 3007-3024).
            
            // The below loop iterates all lines and makes transformations between (to and from) the three coordinate reference systems
            // and verifies the expected result according to the file, and asserts with an error if the difference is too big.
            // Note that the expected coordinates have been calculated in another project, by using a median value for 6 different implementations.
            // (and the number 6 is actually what the last columns means i.e. how many implementations were used to create the data file)
            IList<Coordinates> listOfCoordinates = lines.Select(line => new Coordinates(line)).Skip(1).ToList();
            Assert.AreEqual(18, listOfCoordinates.Count);
            int numberOfTransformations = 0;
            foreach(var listOfCoordinatesWhichRepresentTheSameLocation in listOfCoordinates) {
                IList<CrsCoordinate> coordinates = listOfCoordinatesWhichRepresentTheSameLocation.coordinateList;
                for(int i=0; i<coordinates.Count-1; i++) {
                    for(int j=i+1; j<coordinates.Count; j++) {
                        Transform(coordinates[i], coordinates[j], problemTransformationResults);
                        Transform(coordinates[j], coordinates[i], problemTransformationResults);
                        numberOfTransformations += 2;
                    }
                }

            }
            if (problemTransformationResults.Count > 0) {
                foreach (string s in problemTransformationResults) {
                    Console.WriteLine(s);
                }
            }
            Assert.AreEqual(0, problemTransformationResults.Count, "For further details see the Console output");
            
            const int expectedNumberOfTransformations = 108; // for an explanation, see the lines below:
            // Each line in the input file "swedish_crs_coordinates.csv" has three coordinates (and let's below call then A B C)
            // and then for each line we should have done six number of transformations:
            // A ==> B
            // A ==> C
            // B ==> C
            // (and three more in the opposite directions)
            // And there are 18 local CRS for sweden (i.e number of data rows in the file)
            // Thus the total number of transformations should be 18 * 6 = 108
            Assert.AreEqual(expectedNumberOfTransformations, numberOfTransformations);
        }



        private void Transform(
            CrsCoordinate sourceCoordinate,
            CrsCoordinate targetCoordinateExpected,
            IList<string> problemTransformationResults
        ) {
            int targetEpsg = targetCoordinateExpected.epsgNumber;
            CrsCoordinate targetCoordinate = Transformer.Transform(sourceCoordinate, targetEpsg);
            bool isTargetEpsgWgs84 = targetEpsg == Transformer.epsgForWgs84;
            // double maxDifference = isTargetEpsgWgs84 ? 0.000002 : 0.2;   // fails, Epsg 3022 ==> 4326 , diffLongitude 2.39811809521484E-06
            // double maxDifference = isTargetEpsgWgs84 ? 000003 : 0.1;     // fails, Epsg 4326 ==> 3022 , diffLongitude 0.117090131156147
            double maxDifference = isTargetEpsgWgs84 ? 000003 : 0.2; // the other (i.e. non-WGS84) are using meter as unit, so 0.2 is just two decimeters difference
            double diffLongitude = Math.Abs((targetCoordinate.xLongitude - targetCoordinateExpected.xLongitude));
            double diffLatitude = Math.Abs((targetCoordinate.yLatitude - targetCoordinateExpected.yLatitude));

            if (diffLongitude > maxDifference || diffLatitude > maxDifference) {
                string problem = string.Format(
                    "Epsg {0} ==> {1} , diffLongitude {2}  , diffLatitude {3}"
                    + "sourceCoordinate xLongitude/yLatitude: {4}/{5}" 
                    + "targetCoordinate xLongitude/yLatitude: {6}/{7}" 
                    + "targetCoordinateExpected xLongitude/yLatitude: {8}/{9}",
                    sourceCoordinate.epsgNumber, targetCoordinateExpected.epsgNumber,
                    diffLongitude, diffLatitude,
                    sourceCoordinate.xLongitude, sourceCoordinate.yLatitude,
                    targetCoordinate.xLongitude, targetCoordinate.yLatitude,
                    targetCoordinateExpected.xLongitude, targetCoordinateExpected.yLatitude
                );
                problemTransformationResults.Add(problem);
            }
        }

        private string GetPathToOutputDirectoryWhereTheDataFileShouldBeCopiedToAutomatically() {
            string pathToOutputDirectory= Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            // e.g. a path ending with something like this: "... SwedenCrsTransformationsTests\bin\Debug\netcoreapp2.1"
            return pathToOutputDirectory;
        }
   }

    internal class Coordinates {
        internal readonly List<CrsCoordinate> coordinateList;
        internal Coordinates(
            string lineFromFile
        ) {
            var array = lineFromFile.Split(TransformingCoordinatesFromFileTest.columnSeparator);
            coordinateList = new List<CrsCoordinate> {
                CrsCoordinate.CreateCoordinatePoint(int.Parse(array[0]), double.Parse(array[1]), double.Parse(array[2])),
                CrsCoordinate.CreateCoordinatePoint(int.Parse(array[3]), double.Parse(array[4]), double.Parse(array[5])),
                CrsCoordinate.CreateCoordinatePoint(int.Parse(array[6]), double.Parse(array[7]), double.Parse(array[8]))
            };
        }
    }

}