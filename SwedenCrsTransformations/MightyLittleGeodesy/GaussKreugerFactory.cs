using SwedenCrsTransformations;
using System;
using System.Collections.Generic;

namespace MightyLittleGeodesy {
    internal class GaussKreugerFactory {

        private static readonly GaussKreugerFactory _gaussKreugerFactory = new GaussKreugerFactory();
    
        internal static GaussKreugerFactory getInstance() {
            return _gaussKreugerFactory;
        }

        private readonly IDictionary<CrsProjection, GaussKreuger>
            mapWithAllGaussKreugers = new Dictionary<CrsProjection, GaussKreuger>();

        private GaussKreugerFactory() {
            IList<CrsProjection> crsProjections = CrsProjectionFactory.GetAllCrsProjections();
            foreach(CrsProjection crsProjection in crsProjections) {
                if(crsProjection.IsWgs84()) continue; // WGS84 can not be used as GaussKreuger parameter, and will throw an exception if trying to use it as parameter below
                GaussKreugerParameterObject gaussKreugerParameterObject = new GaussKreugerParameterObject(crsProjection);
                GaussKreuger gaussKreuger = GaussKreuger.create(gaussKreugerParameterObject);
                mapWithAllGaussKreugers.Add(crsProjection, gaussKreuger);
            }        
        }
    
        internal GaussKreuger getGaussKreuger(CrsProjection crsProjection) {
            if(mapWithAllGaussKreugers.ContainsKey(crsProjection)) {
                return mapWithAllGaussKreugers[crsProjection];
            }
            throw new ArgumentException("Could not find GaussKreuger for crsProjection " + crsProjection);
        }

    }
}