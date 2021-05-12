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

using SwedenCrsTransformations.Transformation.TransformWithClasses;
using SwedenCrsTransformations.Transformation.TransformWithMethods;

namespace SwedenCrsTransformations.Transformation {
    internal class Transformer {

        //private static TransformStrategy transformer = new TransformerWithClasses();
        private static TransformStrategy transformer = new TransformerWithMethods();
        // Regarding the reason for the above two classes, see the long comment below at the end of this file.

        public static CrsCoordinate Transform(CrsCoordinate sourceCoordinate, CrsProjection targetCrsProjection) {
            return transformer.Transform(sourceCoordinate, targetCrsProjection);
        }
    }

}
/*
NOTE: Many classes, e.g. the above two classes ("TransformerWithMethods" and "TransformerWithClasses") are refering 
to this place i.e. to the bottom of the file with the 'Transformer' so therefore should keep the explaining comments here below.

What is the reason for the two classes "TransformerWithMethods" and "TransformerWithClasses"?

Well, let me put it this way:

*If* the only purpose of this library would have been to provide an implementation of 
coordinate transformations, then I would only have used the implementation in the class "TransformerWithMethods"
but then modified it by enabling the initial 2-3 lines of code in each of three private Transform methods which are now within comments.
(and those three methods are currently instead using code in three classes, to reuse their code instead of duplicating it)
Also I would have moved that code directly into this class Transformer, and then I would have deleted 
the both subdirectories "TransformWithMethods" and "TransformWithClasses" (including deletion of all four classes in the latter).

*However*, my main purpose of this library is to use it as an appropriately (IMO) sized project 
to be ported to many different programming languages.
In other words, it is a base project for me to use, to maintain/update/achieve skills in different programming languages.
I want the library to have some type hierarchy with a base type and some implementation classes, and in this library it is the 
interface 'TransformStrategy' with three implementation classes (those three with the longest names in the 'TransformWithClasses' directory).
If you have not used a certain programming language in a long time (e.g. a couple of months) then it is easy to forget the syntax
for example how to inherit or implement base types and overriding/implementing methods.
When you know that you have implemented something yourself, but not exactly remember the syntax, I think it is 
quicker to look around in your own code to refresh your memory rather than looking in other's code, 
or reading through tutorials or official documentation pages, or googling or searching at stackoverflow.

The above paragraph explains the reason for why the code in the "TransformWithClasses" directory exists, i.e. 
to provide syntax example of how to define and implement interface in many different programming languages.
But the polymorphic usage of the interface is only correct from a syntax point of view, but not 
correctly used regarding Liskov substitution principle.
The method 'TransformerWithClasses.Transform' is using the parameters (sourceCoordinate and targetCrsProjection) 
to determine which 'TransformStrategy' class to use for then invoking with those parameters.
The reason is that the three implementation classes can only handle certain parameters.
It is actually misleading to use the interface, when the different implementations can only handle certain parameters.
But again, the main purpose of the library is to provide syntax example for different programming languages.

Before 2021-05-12, there were only this Transformer class, which used the three 'TransformStrategy' implementation classes
(i.e. the classes with long names now located in the subdirectory TransformWithClasses, and were used as in the current class "TransformerWithClasses")
but at this date, the code was restructured with the two subdirectories and also with these added comments.
At this time the code had already been ported to some languages (Dart, TypeScript, Python, JVM/Java/Scala/Kotlin)
so those ports do currently (at the mentioned date) NOT contain these two subdirectories.
*/