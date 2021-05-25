using NUnit.Framework;
using SwedenCrsTransformations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SwedenCrsTransformationsTests
{
    [TestFixture]
    public class CrsProjectionFactoryTest {

        internal const int epsgNumberForWgs84 = 4326;
        internal const int epsgNumberForSweref99tm = 3006; // https://epsg.org/crs_3006/SWEREF99-TM.html
        internal const int numberOfSweref99projections = 13; // with EPSG numbers 3006-3018
        internal const int numberOfRT90projections = 6; // with EPSG numbers 3019-3024
        internal const int numberOfWgs84Projections = 1; // just to provide semantic instead of using a magic number 1 below
        private const int totalNumberOfProjections = numberOfSweref99projections + numberOfRT90projections + numberOfWgs84Projections;

        private IList<CrsProjection> _allCrsProjections;

        [SetUp]
        public void SetUp() {
            _allCrsProjections = CrsProjectionFactory.GetAllCrsProjections();;
        }


        [Test]
        public void GetCrsProjectionByEpsgNumber() {
            Assert.AreEqual(
                CrsProjection.sweref_99_tm,
                CrsProjectionFactory.GetCrsProjectionByEpsgNumber(epsgNumberForSweref99tm)
            );

            Assert.AreEqual(
                CrsProjection.sweref_99_23_15,
                CrsProjectionFactory.GetCrsProjectionByEpsgNumber(3018) // https://epsg.io/3018
            );

            Assert.AreEqual(
                CrsProjection.rt90_5_0_gon_o,
                CrsProjectionFactory.GetCrsProjectionByEpsgNumber(3024)  // https://epsg.io/3018
            );
        }

        [Test]
        public void VerifyTotalNumberOfProjections() {
            Assert.AreEqual(
                totalNumberOfProjections,
                _allCrsProjections.Count // retrieved with 'GetAllCrsProjections' in the SetUp method
            );
        }    
        [Test]
        public void VerifyNumberOfWgs84Projections() {
            Assert.AreEqual(numberOfWgs84Projections, _allCrsProjections.Where(crs => crs.IsWgs84()).Count());
        }
        [Test]
        public void VerifyNumberOfSweref99Projections() {
            Assert.AreEqual(numberOfSweref99projections, _allCrsProjections.Where(crs => crs.IsSweref()).Count());
        }
        [Test]
        public void VerifyNumberOfRT90Projections() {
            Assert.AreEqual(numberOfRT90projections, _allCrsProjections.Where(crs => crs.IsRT90()).Count());
        }

        [Test]

        public void VerifyThatAllProjectionsCanBeRetrievedByItsEpsgNumber() {
            foreach(var crsProjection in _allCrsProjections) {
                Assert.IsTrue(CrsProjectionFactory.IsEpsgNumberSupported(crsProjection.GetEpsgNumber()), "Projection should be supported: " + crsProjection.GetEpsgNumber());
                var crsProj = CrsProjectionFactory.GetCrsProjectionByEpsgNumber(crsProjection.GetEpsgNumber());
                Assert.AreEqual(crsProjection, crsProj);
            }
        }

        
        [Test]
        public void IsEpsgNumberSupported() {
            // The only 20 values that are supported: 4326 (WGS84) and the 19 swedish projections 3006-3024
            Assert.IsTrue(CrsProjectionFactory.IsEpsgNumberSupported(4326)); // EPSG number for WGS84
            IEnumerable<int> rangeWithSupportedEpsgNumbers = Enumerable.Range(3006, 19); // 3006-3024 is 19 values
            foreach(int epsg in rangeWithSupportedEpsgNumbers) {
                Assert.IsTrue(CrsProjectionFactory.IsEpsgNumberSupported(epsg), "EPSG should be supported: " + epsg);
            }

            // Two examples of EPSG numbers NOT supported:
            Assert.IsFalse(CrsProjectionFactory.IsEpsgNumberSupported(3005));
            Assert.IsFalse(CrsProjectionFactory.IsEpsgNumberSupported(3025));
        }

            [Test]
        public void GetCrsProjectionByEpsgNumber_failing_when_using_incorrect_number() {
            Assert.That(
                () => CrsProjectionFactory.GetCrsProjectionByEpsgNumber(987654321)
                ,
                Throws.Exception // any exception, unless specific exceptions are specified below
                    // testing that the thrown exception type is one of the following:
                    //.TypeOf<ArgumentOutOfRangeException>().Or
                    //.TypeOf<NotSupportedException>().Or
                    //.TypeOf<ArgumentException>().Or
                    //.TypeOf<Exception>()
                    .TypeOf<ArgumentException>()
            );
            // alterntative assertion of exception, see "Assert.Throws" in the below test method
        }
        // The above and below two tests are redundant i.e. none or both should fail 
        // but the reason to include them both here is just for reference i.e. a place to find NUnit examples of both
        [Test]
        public void GetCrsProjectionByEpsgNumber_failing_when_using_incorrect_number_2() {
            // alterntative assertion of exception, see "Assert.That ... Throws.Exception" in the above test method
            var exceptionThrown = Assert.Throws<ArgumentException>(() => {
                CrsProjectionFactory.GetCrsProjectionByEpsgNumber(123456789);
            });
            //Assert.IsNotNull(exceptionThrown); // not needed
        }

        
        [Test]
        public void GetAllCrsProjections_verify_the_sort_order() {
            var allProjections = CrsProjectionFactory.GetAllCrsProjections();
            Assert.AreEqual(allProjections.Count, totalNumberOfProjections); // 20 ( 1 + 13 + 6 )

            // only the first (wgs84, with EPSG 4326) above is "special" but the order of the rest (sweref99 and rt90 projections)
            // is that they should be ordered by EPSG numbers from 3006 to 3024
            // i.e. EPSG 3006 for the second item (after the above wgs84) and EPSG 3024 for the last item in the array
            Assert.AreEqual(CrsProjection.wgs84, allProjections[0]);

            int epsgNumber = 3006; // sweref_99_tm
            for (int i = 1; i < allProjections.Count; i++) {
                var proj = allProjections[i];
                Assert.AreEqual(proj.GetEpsgNumber(), epsgNumber);
                epsgNumber++;
            }
            // The above loop is doing assertions as below (below are the first and last values iterade in the above loop)
            Assert.AreEqual(3006, allProjections[1].GetEpsgNumber()); // 3006 ==> CrsProjection.sweref_99_tm
            Assert.AreEqual(3024, allProjections[allProjections.Count - 1].GetEpsgNumber()); // 3024 ==> CrsProjection.rt90_5_0_gon_o
        }
    }
}