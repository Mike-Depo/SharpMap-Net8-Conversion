﻿// Copyright 2012 - Felix Obermaier (www.ivv-aachen.de)
//
// This file is part of SharpMap.Data.Providers.FileGdb.
// SharpMap.Data.Providers.FileGdb is free software; you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
// 
// SharpMap.Data.Providers.FileGdb is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public License
// along with SharpMap; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA 


using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using Envelope = NetTopologySuite.Geometries.Envelope;


using EsriExtent = Esri.FileGDB.Envelope;

using EsriShapeBuffer = Esri.FileGDB.ShapeBuffer;
using EsriPointShapeBuffer = Esri.FileGDB.PointShapeBuffer;
using EsriMultiPointShapeBuffer = Esri.FileGDB.MultiPointShapeBuffer;
using EsriMultiPartShapeBuffer = Esri.FileGDB.MultiPartShapeBuffer;

using EsriGeometryType = Esri.FileGDB.GeometryType;
using EsriShapeType = Esri.FileGDB.ShapeType;
using EsriShapeModifiers = Esri.FileGDB.ShapeModifiers;
using Esri.FileGDB;
using System.Reflection.PortableExecutable;
using NetTopologySuite.Triangulate.QuadEdge;
using NUnit.Framework;



namespace SharpMap.Data.Providers.Converter
{

    internal class FileGdbGeometryConverter
    {

        private static readonly GeometryFactory geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory();


        internal static EsriExtent ToEsriExtent(Envelope bbox)
        {
            return new EsriExtent(bbox.MinX, bbox.MinY, bbox.MaxX, bbox.MaxY);
        }

        internal static EsriGeometryType EsriGeometryType(EsriShapeBuffer buffer)
        {
            return (EsriGeometryType)buffer.geometryType;
        }

        private static EsriShapeType EsriShapeType(EsriShapeBuffer buffer)
        {
            return (EsriShapeType)buffer.shapeType;
        }

        internal static Geometry ToSharpMapGeometry(EsriShapeBuffer buffer)
        {
            if (buffer == null || buffer.IsEmpty)
                return null;

            var geometryType = EsriGeometryType(buffer);

            switch (geometryType)
            {
                case Esri.FileGDB.GeometryType.Null:
                    return null;

                case Esri.FileGDB.GeometryType.Point:
                    return ToSharpMapPoint(buffer);

                case Esri.FileGDB.GeometryType.Multipoint:
                    return ToSharpMapMultiPoint(buffer);

                case Esri.FileGDB.GeometryType.Polyline:
                    return ToSharpMapMultiLineString(buffer);

                case Esri.FileGDB.GeometryType.Polygon:
                    return ToSharpMapMultiPolygon(buffer);

                default:
                    return null;
            }
        }

        private static Geometry ToSharpMapMultiLineString(EsriShapeBuffer shapeBuffer)
        {
            if (shapeBuffer == null)
                return null;

            var multiPartShapeBuffer = shapeBuffer as EsriMultiPartShapeBuffer;
            if (multiPartShapeBuffer == null)
            {
                Envelope box;
                return FromShapeFilePolyLine(shapeBuffer, out box);
            }

            var hasZ = EsriShapeBuffer.HasZs(shapeBuffer.shapeType);
            var lines = new List<LineString>();

            var offset = 0;
            for (var i = 0; i < multiPartShapeBuffer.NumParts; i++)
            {
                var vertices = new List<Coordinate>(multiPartShapeBuffer.Parts[i]);
                for (var j = 0; j < multiPartShapeBuffer.Parts[i]; j++)
                {
                    var index = offset + j;
                    var point = multiPartShapeBuffer.Points[index];
                    var verticesCoord = new Coordinate(point.x, point.y);
                    if (hasZ)
                    {
                        verticesCoord.Z = multiPartShapeBuffer.Zs[index];
                    }

                    vertices.Add(verticesCoord);
                }
                lines.Add(new LineString(vertices.ToArray()));
                offset += multiPartShapeBuffer.Parts[i];
            }

            if (lines.Count == 1)
                return lines[0];

            return new MultiLineString(lines.ToArray());

        }

        private static Geometry ToSharpMapMultiPolygon(EsriShapeBuffer shapeBuffer)
        {
            if (shapeBuffer == null)
                return null;

            var multiPartShapeBuffer = shapeBuffer as EsriMultiPartShapeBuffer;
            if (multiPartShapeBuffer == null)
            {
                Envelope box;
                return FromShapeFilePolygon(shapeBuffer, out box);
            }

            var hasZ = EsriShapeBuffer.HasZs(shapeBuffer.shapeType);
            IList<Polygon> polygons = new List<Polygon>();
            //IPolygon poly = null;
            LinearRing shell = null;
            IList<LinearRing> holes = new List<LinearRing>();
            var offset = 0;
            for (var i = 0; i < multiPartShapeBuffer.NumParts; i++)
            {
                var vertices = new List<Coordinate>(multiPartShapeBuffer.Parts[i]);
                for (var j = 0; j < multiPartShapeBuffer.Parts[i]; j++)
                {
                    var index = offset + j;
                    var point = multiPartShapeBuffer.Points[index];
                    var verticesCoord = new Coordinate(point.x, point.y);
                    if (hasZ)
                    {
                        verticesCoord.Z = multiPartShapeBuffer.Zs[index];
                    }

                    vertices.Add(verticesCoord);
                }

                var ring = geometryFactory.CreateLinearRing(vertices.ToArray());
                if (shell == null || !ring.IsCCW())
                {
                    shell = ring;
                    //poly = new Polygon(ring);
                    //polygons.Add(poly);
                }
                else
                {
                    holes.Add(ring);
                    //poly.InteriorRings.Add(ring);
                }

                offset += multiPartShapeBuffer.Parts[i];

                polygons.Add(geometryFactory.CreatePolygon(shell, holes.ToArray()));

            }

            if (polygons.Count == 1)
                return polygons[0];

            return new MultiPolygon(polygons.ToArray());
        }

        private static Geometry FromShapeFilePolygon(EsriShapeBuffer shapeBuffer, out Envelope box)
        {
            box = null;
            if (shapeBuffer == null)
                return null;

            var hasZ = EsriShapeBuffer.HasZs(shapeBuffer.shapeType);
            var hasM = EsriShapeBuffer.HasMs(shapeBuffer.shapeType);
            using (var reader = new BinaryReader(new MemoryStream(shapeBuffer.shapeBuffer)))
            {
                var type = reader.ReadInt32();
                if (!(type == 5 || type == 15 || type == 25))
                    throw new InvalidOperationException();

                box = createEnvelope(reader);

                var numParts = reader.ReadInt32();
                var numPoints = reader.ReadInt32();
                var allVertices = new List<Coordinate>(numPoints);

                var parts = new int[numParts + 1];
                for (var i = 0; i < numParts; i++)
                    parts[i] = reader.ReadInt32();
                parts[numParts] = numPoints;

                //IPolygon poly = null;
                LinearRing shell = null;
                IList<LinearRing> holes = new List<LinearRing>();
                for (var i = 0; i < numParts; i++)
                {
                    var count = parts[i + 1] - parts[i];
                    var vertices = new List<Coordinate>(count);
                    for (var j = 0; j < count; j++)
                    {
                        var vertex = new Coordinate(reader.ReadDouble(), reader.ReadDouble());
                        if (hasZ)
                        {
                            vertex.Z = double.NaN;
                        }

                        vertices.Add(vertex);
                        allVertices.Add(vertex);
                    }

                    var ring = geometryFactory.CreateLinearRing(vertices.ToArray());
                    if (shell == null || !ring.IsCCW())
                    {
                        shell = ring;
                        //poly = new Polygon(ring);
                        //res.Polygons.Add(poly);
                    }
                    else
                    {
                        holes.Add(ring);
                        //poly.InteriorRings.Add(ring);
                    }
                }


                if (hasZ)
                {
                    var minZ = reader.ReadDouble();
                    var maxZ = reader.ReadDouble();
                    for (var i = 0; i < numPoints; i++)
                        allVertices[i].Z = reader.ReadDouble();
                }

                return geometryFactory.CreatePolygon(shell, holes.ToArray());


            }
        }


        private static Geometry FromShapeFilePolyLine(EsriShapeBuffer shapeBuffer, out Envelope box)
        {
            box = null;
            if (shapeBuffer == null)
                return null;

            var hasZ = EsriShapeBuffer.HasZs(shapeBuffer.shapeType);
            var hasM = EsriShapeBuffer.HasMs(shapeBuffer.shapeType);
            using (var reader = new BinaryReader(new MemoryStream(shapeBuffer.shapeBuffer)))
            {
                var type = reader.ReadInt32();
                if (!(type == 3 || type == 13 || type == 23))
                    throw new InvalidOperationException();

                box = createEnvelope(reader);

                var numParts = reader.ReadInt32();
                var numPoints = reader.ReadInt32();
                var allVertices = new List<Coordinate>(numPoints);

                var parts = new int[numParts + 1];
                for (var i = 0; i < numParts; i++)
                    parts[i] = reader.ReadInt32();
                parts[numParts] = numPoints;

                var lines = new List<LineString>();

                for (var i = 0; i < numParts; i++)
                {
                    var count = parts[i + 1] - parts[i];
                    var vertices = new List<Coordinate>(count);
                    for (var j = 0; j < count; j++)
                    {
                        var vertex = new Coordinate(reader.ReadDouble(), reader.ReadDouble());
                        if (hasZ)
                        {
                            vertex.Z = double.NaN;
                        }

                        vertices.Add(vertex);
                        allVertices.Add(vertex);
                    }

                    lines.Add(geometryFactory.CreateLineString(vertices.ToArray()));
                }

                if (hasZ)
                {
                    var minZ = reader.ReadDouble();
                    var maxZ = reader.ReadDouble();
                    for (var i = 0; i < numPoints; i++)
                        allVertices[i].Z = reader.ReadDouble();
                }

                if (lines.Count == 1)
                    return lines[0];

                return geometryFactory.CreateMultiLineString(lines.ToArray());

            }
        }

        private static Geometry FromShapeFileMultiPoint(EsriShapeBuffer shapeBuffer, out Envelope box)
        {
            box = null;
            if (shapeBuffer == null)
                return null;

            var hasZ = EsriShapeBuffer.HasZs(shapeBuffer.shapeType);
            var hasM = EsriShapeBuffer.HasMs(shapeBuffer.shapeType);
            using (var reader = new BinaryReader(new MemoryStream(shapeBuffer.shapeBuffer)))
            {
                var type = reader.ReadInt32();
                if (!(type == 8 || type == 18 || type == 28))
                    throw new InvalidOperationException();

                box = createEnvelope(reader);

                var numPoints = reader.ReadInt32();

                IList<NetTopologySuite.Geometries.Point> points = new List<NetTopologySuite.Geometries.Point>();

                for (var i = 0; i < numPoints; i++)
                {
                    var vertex = new NetTopologySuite.Geometries.Point(reader.ReadDouble(), reader.ReadDouble());
                    if (hasZ)
                    {
                        vertex.Z = double.NaN;
                    }

                    points.Add(vertex);
                }

                if (hasZ)
                {
                    var minZ = reader.ReadDouble();
                    var maxZ = reader.ReadDouble();
                    for (var i = 0; i < numPoints; i++)
                        points[i].Z = reader.ReadDouble();
                }

                if (points.Count == 1)
                    return points[0];

                return geometryFactory.CreateMultiPoint(points.ToArray());

            }
        }

        private static Geometry FromShapeFilePoint(EsriShapeBuffer shapeBuffer, out Envelope box)
        {
            box = null;
            if (shapeBuffer == null)
                return null;

            var hasZ = EsriShapeBuffer.HasZs(shapeBuffer.shapeType);
            var hasM = EsriShapeBuffer.HasMs(shapeBuffer.shapeType);
            using (var reader = new BinaryReader(new MemoryStream(shapeBuffer.shapeBuffer)))
            {
                var type = reader.ReadInt32();
                if (!(type == 1 || type == 11 || type == 21))
                    throw new InvalidOperationException();

                var vertex = new NetTopologySuite.Geometries.Point(reader.ReadDouble(), reader.ReadDouble());
                if (hasZ)
                {
                    vertex.Z = reader.ReadDouble();
                }

                return vertex;
            }
        }

        private static Geometry ToSharpMapMultiPoint(EsriShapeBuffer shapeBuffer)
        {
            var multiPointShapeBuffer = shapeBuffer as EsriMultiPointShapeBuffer;
            if (multiPointShapeBuffer == null)
            {
                Envelope box;
                return FromShapeFileMultiPoint(shapeBuffer, out box);
            }

            var hasZ = EsriShapeBuffer.HasZs(multiPointShapeBuffer.shapeType);
            IList<NetTopologySuite.Geometries.Point> points = new List<NetTopologySuite.Geometries.Point>();
            var offset = 0;
            foreach (var point in multiPointShapeBuffer.Points)
            {
                var vertex = new NetTopologySuite.Geometries.Point(point.x, point.y);
                if (hasZ)
                {
                    vertex.Z = multiPointShapeBuffer.Zs[offset++];
                }

                points.Add(vertex);
            }

            return geometryFactory.CreateMultiPoint(points.ToArray());
        }

        private static Geometry ToSharpMapPoint(EsriShapeBuffer shapeBuffer)
        {
            var pointShapeBuffer = shapeBuffer as EsriPointShapeBuffer;
            if (pointShapeBuffer == null)
            {
                Envelope box;
                return FromShapeFilePoint(shapeBuffer, out box);
            }

            var pt = pointShapeBuffer.point;
            var vertex = new NetTopologySuite.Geometries.Point(pt.x, pt.y);
            if (EsriShapeBuffer.HasZs(pointShapeBuffer.shapeType))
            {
                vertex.Z = pointShapeBuffer.Z;
            }

            return vertex;
        }

        private static Envelope createEnvelope(BinaryReader reader)
        {
            double minX = reader.ReadDouble();
            double minY = reader.ReadDouble();
            double maxX = reader.ReadDouble();
            double maxY = reader.ReadDouble();

            Envelope box = new Envelope(minX, maxX, minY, maxY);
            return box;
        }


    }
}
