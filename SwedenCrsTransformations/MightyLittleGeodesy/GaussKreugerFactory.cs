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
            GaussKreuger gaussKreuger = GaussKreuger.create(new GaussKreugerParameterObject(crsProjection));
            return gaussKreuger;
        }

    }
}