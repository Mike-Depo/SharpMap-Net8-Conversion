﻿using ProjNet.CoordinateSystems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpMap.Base.GeoAPI.CoordinateSystems
{
    /// <summary>
	/// Base interface for all coordinate systems
	/// </summary>
	/// <remarks>
	/// <para>A coordinate system is a mathematical space, where the elements of the space are called
	/// positions. Each position is described by a list of numbers. The length of the list corresponds
	/// to the dimension of the coordinate system. So in a 2D coordinate system each position is 
	/// described by a list containing 2 numbers.</para>
	/// <para>
	/// However, in a coordinate system, not all lists of numbers correspond to a position - 
	/// some lists may be outside the domain of the coordinate system. For example, in a 2D Lat/Lon
	/// coordinate system, the list (91,91) does not correspond to a position.</para>
	/// <para>
	/// Some coordinate systems also have a mapping from the mathematical space into locations
	/// in the real world. So in a Lat/Lon coordinate system, the mathematical position (lat, long) 
	/// corresponds to a location on the surface of the Earth. This mapping from the mathematical 
	/// space into real-world locations is called a Datum.</para>
	/// </remarks>
    public interface ICoordinateSystem : IInfo
    {

        /// <summary>
        /// Dimension of the coordinate system.
        /// </summary>
        int Dimension { get; }

        /// <summary>
        /// Gets axis details for dimension within coordinate system.
        /// </summary>
        /// <param name="dimension">Dimension</param>
        /// <returns>Axis info</returns>
        AxisInfo GetAxis(int dimension);

        /// <summary>
        /// Gets units for dimension within coordinate system.
        /// </summary>
        IUnit GetUnits(int dimension);

        /// <summary>
        /// Gets default envelope of coordinate system.
        /// </summary>
        /// <remarks>
        /// Gets default envelope of coordinate system. Coordinate systems 
        /// which are bounded should return the minimum bounding box of their 
        /// domain. Unbounded coordinate systems should return a box which is 
        /// as large as is likely to be used. For example, a (lon,lat) 
        /// geographic coordinate system in degrees should return a box from 
        /// (-180,-90) to (180,90), and a geocentric coordinate system could 
        /// return a box from (-r,-r,-r) to (+r,+r,+r) where r is the 
        /// approximate radius of the Earth.
        /// </remarks>
        double[] DefaultEnvelope { get; }
    }
}
