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
using NTS = NetTopologySuite.Geometries;

namespace SharpMap.Rendering.Symbolizer
{
    /// <summary>
    /// Base class for all possible Point symbolizers
    /// </summary>
    [Serializable]
    public abstract class PointSymbolizer : BaseSymbolizer, IPointSymbolizer
    {
        /// <summary>
        /// Function to render the geometry
        /// </summary>
        /// <param name="map">The map object, mainly needed for transformation purposes.</param>
        /// <param name="geometry">The geometry to symbolize.</param>
        /// <param name="graphics">The graphics object to use.</param>
        public void Render(MapViewport map, NTS.IPuntal geometry, Graphics graphics)
        {
            if ( geometry is NTS.MultiPoint mp )
            {
                foreach ( var geom in mp.Geometries )
                    OnRenderInternal( map, ( NTS.Geometry ) geometry, ( NTS.Point ) geom, graphics );
            }

            else
                OnRenderInternal( map, ( NTS.Geometry ) geometry, ( NTS.Point ) geometry, graphics );
        }

        /// <summary>
        /// Method to perform actual rendering
        /// </summary>
        /// <param name="map">The map</param>
        /// <param name="feature">The feature that the point belongs to</param>
        /// <param name="point">The point to render</param>
        /// <param name="g">The graphics object to use</param>
        protected abstract void OnRenderInternal(MapViewport map, NTS.Geometry feature, 
            NTS.Point point, 
            Graphics g);
    }
}
