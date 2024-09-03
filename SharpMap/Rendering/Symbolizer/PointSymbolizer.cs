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
        public void Render(MapViewport map, NetTopologySuite.Geometries.IPuntal geometry, Graphics graphics)
        {
            if ( geometry is NetTopologySuite.Geometries.MultiPoint mp )
            {
                foreach ( var geom in mp.Geometries )
                    OnRenderInternal( map, ( NetTopologySuite.Geometries.Point ) geom, graphics );
            }

            else
                OnRenderInternal( map, ( NetTopologySuite.Geometries.Point ) geometry, graphics );
        }

        /// <summary>
        /// Function that does the actual rendering
        /// </summary>
        /// <param name="pt">The point</param>
        /// <param name="g">The graphics object</param>
        protected abstract void OnRenderInternal(MapViewport map, NetTopologySuite.Geometries.Point point, Graphics g);
    }
}
