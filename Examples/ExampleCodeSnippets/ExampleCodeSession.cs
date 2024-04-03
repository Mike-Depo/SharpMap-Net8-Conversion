using NetTopologySuite;

namespace ExampleCodeSnippets
{
    [NUnit.Framework.SetUpFixture]
    public class ExampleCodeSession
    {
        [NUnit.Framework.SetUp]
        public void SetUp()
        {
            var gss = new NtsGeometryServices();
            var css = new SharpMap.CoordinateSystems.CoordinateSystemServices(
                new ProjNet.CoordinateSystems.CoordinateSystemFactory(),
                new ProjNet.CoordinateSystems.Transformations.CoordinateTransformationFactory(),
                SharpMap.Converters.WellKnownText.SpatialReference.GetAllReferenceSystems());

            NtsGeometryServices.Instance = gss;
            SharpMap.Session.Instance
                .SetGeometryServices(gss)
                .SetCoordinateSystemServices(css)
                .SetCoordinateSystemRepository(css);
        }
    }
}
