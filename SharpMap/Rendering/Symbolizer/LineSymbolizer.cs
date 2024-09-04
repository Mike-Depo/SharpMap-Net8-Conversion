using System.Drawing;
using System.Drawing.Drawing2D;
using NTS = NetTopologySuite.Geometries;

//using KnownColor = System.Drawing.KnownColor
namespace SharpMap.Rendering.Symbolizer
{
    /// <summary>
    /// Abstract base class for all line symbolizer classes
    /// </summary>
    public abstract class LineSymbolizer : BaseSymbolizer, ILineSymbolizer
    {
#if !NETSTANDARD2_0
        /// <summary>
        /// Creates an instance of this class. <see cref="Line"/> is set to a random
        /// <see cref="KnownColor"/>.
        /// </summary>
#else
        /// <summary>
        /// Creates an instance of this class. <see cref="Line"/> is set to a random
        /// <see cref="SharpMap.Drawing.KnownColor"/>.
        /// </summary>
#endif
        protected LineSymbolizer()
        {
            Line = new Pen(Utility.RandomKnownColor(), 1);
        }

        /// <summary>
        /// Releases managed resources
        /// </summary>
        protected override void ReleaseManagedResources()
        {
            CheckDisposed();

            if (Line != null) 
                Line.Dispose();
            
            base.ReleaseManagedResources();
        }

        /// <summary>
        /// Method to render a LineString to the <see cref="Graphics"/> object.
        /// </summary>
        /// <param name="map">The map object</param>
        /// <param name="geometry">Linestring to symbolize</param>
        /// <param name="graphics">The graphics object to use.</param>
        public void Render(MapViewport map, NTS.ILineal geometry, Graphics graphics)
        {
            if ( geometry is NTS.MultiLineString m )
            {
                foreach ( var geom in m.Geometries )
                    OnRenderInternal( map, ( NTS.Geometry ) geometry, ( NTS.LineString ) geom, graphics );
            }

            else
                OnRenderInternal( map, ( NTS.Geometry ) geometry, ( NTS.LineString ) geometry, graphics );
        }

        /// <summary>
        /// Method to perform actual rendering
        /// </summary>
        /// <param name="map">The map</param>
        /// <param name="feature">The feature that the line string belongs to</param>
        /// <param name="lineString">The line string to render</param>
        /// <param name="g">The graphics object to use</param>
        protected abstract void OnRenderInternal(MapViewport map, NTS.Geometry feature, 
            NTS.LineString lineString, 
            Graphics g);

        /// <summary>
        /// Function to transform a linestring to a graphics path for further processing
        /// </summary>
        /// <param name="lineString">The Linestring</param>
        /// <param name="map">The map</param>
        /// <!--<param name="useClipping">A value indicating whether clipping should be applied or not</param>-->
        /// <returns>A GraphicsPath</returns>
        public static GraphicsPath LineStringToPath( NTS.LineString lineString, MapViewport map)
        {
            var gp = new GraphicsPath(FillMode.Alternate);
            gp.AddLines(NTS.GeoAPIEx.TransformToImage(lineString, map));
            return gp;
        }

        /// <summary>
        /// Gets or sets the <see cref="Pen"/> to render the LineString
        /// </summary>
        public Pen Line { get; set; }

        #region ISymbolizer implementation

        /// <summary>
        /// Method to perform symbolization
        /// </summary>
        /// <param name="g">The graphics object to symbolize upon</param>
        /// <param name="map">The map</param>
        public override void Symbolize(Graphics g, MapViewport map)
        {
        }

        #endregion
    }
}
