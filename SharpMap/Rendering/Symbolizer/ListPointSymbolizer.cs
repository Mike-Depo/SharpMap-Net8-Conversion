﻿// Copyright 2011 - Felix Obermaier (www.ivv-aachen.de)
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
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using NetTopologySuite.Geometries;
using SharpMap.Base;

namespace SharpMap.Rendering.Symbolizer    
{
    /// <summary>
    /// ListPointSymbolizer class
    /// </summary>
    [Serializable]
    public class ListPointSymbolizer : Collection<PointSymbolizer>, IPointSymbolizer, IDisposableEx
    {
        private Size _size;

        #region Collection<T> overrides
        /// <inheritdoc/>
        protected override void ClearItems()
        {
            base.ClearItems();
            _size = new Size();
        }

        /// <inheritdoc/>
        protected override void RemoveItem(int index)
        {
            base.RemoveItem(index);
            _size = new Size();
        }

        /// <inheritdoc/>
        protected override void InsertItem(int index, PointSymbolizer item)
        {
            base.InsertItem(index, item);
            _size = new Size();
        }

        /// <inheritdoc/>
        protected override void SetItem(int index, PointSymbolizer item)
        {
            base.SetItem(index, item);
            _size = new Size();
        }
        #endregion

        /// <summary>
        /// Method to render the Point to the <see cref="Graphics"/> object.
        /// </summary>
        /// <param name="map">The map object</param>
        /// <param name="points">Location where to render the Symbol</param>
        /// <param name="g">The graphics object to use.</param>
        public void Render(MapViewport map, IPuntal points, Graphics g)
        {
            foreach (var pointSymbolizer in Items)
                pointSymbolizer.Render(map, points, g);
        }

        /// <summary>
        /// Gets or sets a value indicating which <see cref="ISymbolizer.SmoothingMode"/> is to be used for rendering
        /// </summary>
        public SmoothingMode SmoothingMode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating which <see cref="ISymbolizer.PixelOffsetMode"/> is to be used for rendering
        /// </summary>
        public PixelOffsetMode PixelOffsetMode { get; set; }

        /// <summary>
        /// Method to indicate that the symbolizer has to be prepared.
        /// </summary>
        /// <param name="g">The graphics object</param>
        /// <param name="map">The map</param>
        /// <param name="aproximateNumberOfGeometries">The approximate number of geometries</param>
        public void Begin(Graphics g, MapViewport map, int aproximateNumberOfGeometries)
        {
        }

        /// <summary>
        /// Method to indicate that the symbolizer should do its symbolizer work.
        /// </summary>
        /// <param name="g">The graphics object</param>
        /// <param name="map">The map</param>
        public void Symbolize(Graphics g, MapViewport map)
        {
        }

        /// <summary>
        /// Method to indicate that the symbolizers work is done and it can clean up.
        /// </summary>
        /// <param name="g">The graphics object</param>
        /// <param name="map">The map</param>
        public void End(Graphics g, MapViewport map)
        {
        }

        #region Implementation of ICloneable

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public object Clone()
        {
            var res = new ListPointSymbolizer();
            foreach (var pointSymbolizer in Items)
                res.Add((PointSymbolizer)pointSymbolizer.Clone());
            return res;
        }

        #endregion

        #region Implementation of IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            if (!IsDisposed)
            {
                foreach (IDisposable pointSymbolizer in Items)
                {
                    pointSymbolizer.Dispose();
                }
                ClearItems();
            }
            IsDisposed = true;
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Implementation of IDisposableEx

        /// <summary>
        /// Gets whether this object was already disposed
        /// </summary>
        public bool IsDisposed { get; private set; }

        #endregion
    }
}
