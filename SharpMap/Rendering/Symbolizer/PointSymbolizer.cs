// Copyright 2011 - Felix Obermaier (www.ivv-aachen.de)
//
// This file is part of SharpMap.
// SharpMap is free software; you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
// 
// SharpMap is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public License
// along with SharpMap; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA 

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using NetTopologySuite.Geometries;

namespace SharpMap.Rendering.Symbolizer
{
    /// <summary>
    /// Base class for all possible Point symbolizers
    /// </summary>
    [Serializable]
    public abstract class PointSymbolizer : BaseSymbolizer, IPointSymbolizerEx
    {
        /// <summary>
        /// The calculated rectangle enclosing the extent of this symbol
        /// </summary>
        public RectangleF CanvasArea { get; protected set; } = RectangleF.Empty;

        /// <summary>
        /// Function to render the symbol
        /// </summary>
        /// <param name="map">The map</param>
        /// <param name="point">The point to symbolize</param>
        /// <param name="g">The graphics object</param>
        protected void RenderPoint(MapViewport map, Coordinate point, Graphics g)
        {
            if (point != null)
                OnRenderInternal(map, point, g);
        }

        /// <summary>
        /// Function that does the actual rendering
        /// </summary>
        /// <param name="pt">The point</param>
        /// <param name="g">The graphics object</param>
        protected abstract void OnRenderInternal(MapViewport map, Coordinate point, Graphics g);

        /// <summary>
        /// Function to render the geometry
        /// </summary>
        /// <param name="map">The map object, mainly needed for transformation purposes.</param>
        /// <param name="geometry">The geometry to symbolize.</param>
        /// <param name="graphics">The graphics object to use.</param>
        public void Render(MapViewport map, IPuntal geometry, Graphics graphics)
        {
            var mp = geometry as MultiPoint;
            if (mp != null)
            {
                var combinedArea = RectangleF.Empty;
                foreach (var point in mp.Coordinates)
                {
                    RenderPoint(map, point, graphics);
                    combinedArea = CanvasArea.ExpandToInclude(combinedArea);
                }
                CanvasArea = combinedArea;
                return;
            }
            RenderPoint(map, ((NetTopologySuite.Geometries.Point)geometry).Coordinate, graphics);
        }
    }
}
