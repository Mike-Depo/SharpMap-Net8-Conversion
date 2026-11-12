using System;

namespace SharpMap.Data.Providers
{
    /// <summary>
    /// Interface for all classes that create a provider
    /// </summary>
    public interface IProviderConfiguration
    {
        /// <summary>
        /// Create the provider provider
        /// </summary>
        /// <returns>The created provider</returns>
        IProvider Create();
    }

    /// <summary>
    /// Shapefile provider configuration class
    /// </summary>
    [Serializable]
    public class ShapeFileProviderConfiguration : IProviderConfiguration
    {
        /// <summary>
        /// Gets or sets the filename of the ShapeFile
        /// </summary>
        public string Filename { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a spatial index should be reused
        /// </summary>
        public bool UseFilebasedIndex { get; set; }

        /// <summary>
        /// Gets or sets a value if the shapefile should be used as a <see cref="System.IO.MemoryMappedFiles.MemoryMappedFile"/>
        /// </summary>
        public bool UseMemoryCache { get; set; }

        /// <summary>
        /// Gets or sets a value indicating how to create 
        /// </summary>
        public ShapeFile.SpatialIndexCreation SpatialIndexCreationOption { get; set; }

        /// <summary>
        /// Creates a Shapefile provider
        /// </summary>
        /// <returns></returns>
        public IProvider Create()
        {
#pragma warning disable 618
            ShapeFile.SpatialIndexCreationOption = SpatialIndexCreationOption;
#pragma warning restore 618
            return new ShapeFile(Filename, UseFilebasedIndex, UseMemoryCache);
        }
    }
}
