using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpMap.Base
{
    public static class GeometryExtension
    {
        public static Geometry Clone(this Geometry geometry)
        {
            return geometry.Factory.CreateGeometry(geometry);
        }
    }
}
