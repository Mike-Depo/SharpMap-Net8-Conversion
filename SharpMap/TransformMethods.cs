using NetTopologySuite.CoordinateSystems.Transformations;
using NetTopologySuite.Geometries;
using ProjNet.CoordinateSystems.Transformations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpMap
{
    public static class TransformMethods
    {
        public static Func<Coordinate, MathTransform,                  Coordinate> TransformCoordinate { get; set; } = GeometryTransform.TransformCoordinate;
        public static Func<Envelope  , MathTransform,                  Envelope  > TransformBox        { get; set; } = GeometryTransform.TransformBox       ;
        public static Func<Geometry  , MathTransform, GeometryFactory, Geometry  > TransformGeometry   { get; set; } = GeometryTransform.TransformGeometry  ;
    }
}
