﻿using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using Application = System.Windows.Application;
// TODO Menu is no longer supported. Use ToolStripDropDown instead. For more details see https://docs.microsoft.com/en-us/dotnet/core/compatibility/winforms#removed-controls
using MenuItem = System.Windows.Controls.MenuItem;
namespace WPFSamples
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            //NetTopologySuiteBootstrapper.Bootstrap();
            InitializeComponent();

            var gss = NtsGeometryServices.Instance;
            var css = new SharpMap.CoordinateSystems.CoordinateSystemServices(
                new ProjNet.CoordinateSystems.CoordinateSystemFactory(),
                new ProjNet.CoordinateSystems.Transformations.CoordinateTransformationFactory(),
                SharpMap.Converters.WellKnownText.SpatialReference.GetAllReferenceSystems());

            NtsGeometryServices.Instance = gss;
            SharpMap.Session.Instance
                .SetGeometryServices(gss)
                .SetCoordinateSystemServices(css)
                .SetCoordinateSystemRepository(css);
        }

        private void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void BgOSM_OnClick(object sender, RoutedEventArgs e)
        {
            WpfMap.BackgroundLayer = new SharpMap.Layers.TileAsyncLayer(BruTile.Predefined.KnownTileSources.Create(), "OSM");

            // TODO Menu is no longer supported. Use ToolStripDropDown instead. For more details see https://docs.microsoft.com/en-us/dotnet/core/compatibility/winforms#removed-controls
            foreach (var menuItem in Menu.Items.OfType<MenuItem>())
            {
                menuItem.IsChecked = false;
            }
            BgOsm.IsChecked = true;

            WpfMap.ZoomToExtents();
            e.Handled = true;
        }

        private void BgStamenWaterColor_Click(object sender, RoutedEventArgs e)
        {
            WpfMap.BackgroundLayer = new SharpMap.Layers.TileAsyncLayer(
              BruTile.Predefined.KnownTileSources.Create(
                BruTile.Predefined.KnownTileSource.StamenWatercolor), "Stamen Watercolor");

            // TODO Menu is no longer supported. Use ToolStripDropDown instead. For more details see https://docs.microsoft.com/en-us/dotnet/core/compatibility/winforms#removed-controls
            foreach (var menuItem in Menu.Items.OfType<MenuItem>())
            {
                menuItem.IsChecked = false;
            }
            BgStamenWaterColor.IsChecked = true;

            WpfMap.ZoomToExtents();
            e.Handled = true;
        }

        private void AddShapeLayer_OnClick(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = @"Shapefiles (*.shp)|*.shp";
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var ds = new SharpMap.Data.Providers.ShapeFile(ofd.FileName);
                var lay = new SharpMap.Layers.VectorLayer(System.IO.Path.GetFileNameWithoutExtension(ofd.FileName), ds);
                if (ds.CoordinateSystem != null)
                {
                    ProjNet.CoordinateSystems.Transformations.CoordinateTransformationFactory fact =
                        new ProjNet.CoordinateSystems.Transformations.CoordinateTransformationFactory();

                    lay.CoordinateTransformation = fact.CreateFromCoordinateSystems(ds.CoordinateSystem,
                        ProjNet.CoordinateSystems.ProjectedCoordinateSystem.WebMercator);
                    lay.ReverseCoordinateTransformation = fact.CreateFromCoordinateSystems(ProjNet.CoordinateSystems.ProjectedCoordinateSystem.WebMercator,
                        ds.CoordinateSystem);
                }
                WpfMap.MapLayers.Add(lay);
                if (WpfMap.MapLayers.Count == 1)
                {
                    Envelope env = lay.Envelope;
                    WpfMap.ZoomToEnvelope(env);
                }
            }
            e.Handled = true;
        }

        private void Rotation_OnClick(object sender, RoutedEventArgs e)
        {
            // TODO MenuItem is no longer supported. Use ToolStripMenuItem instead. For more details see https://docs.microsoft.com/en-us/dotnet/core/compatibility/winforms#removed-controls
            if (!(sender is MenuItem mi))
                return;

            WpfMap.MapRotation = float.Parse(mi.Name.Substring(3), NumberStyles.Integer);
            // TODO MenuItem is no longer supported. Use ToolStripMenuItem instead. For more details see https://docs.microsoft.com/en-us/dotnet/core/compatibility/winforms#removed-controls
            foreach (MenuItem tmp in ((MenuItem)mi.Parent).Items)
                tmp.IsChecked = tmp == mi;

        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
