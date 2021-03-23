using SwedenCrsTransformations;

namespace MightyLittleGeodesy {
    internal class GaussKreugerFactory {

        private static readonly GaussKreugerFactory _gaussKreugerFactory = new GaussKreugerFactory();
    
        internal static GaussKreugerFactory getInstance() {
            return _gaussKreugerFactory;
        }

        private GaussKreugerFactory() {
        }

    
        internal GaussKreuger getGaussKreuger(CrsProjection crsProjection) {
            // TODO cache the 'GaussKreuger' instances instead of creating new instances every time in this method

            GaussKreuger gaussKreuger = new GaussKreuger();
            // TODO make the 'GaussKreuger' immutable, e.g. provide the projection as parameter to the above constructor instead of the below method
            gaussKreuger.swedish_params(crsProjection);
        
            return gaussKreuger;
        }

    }
}