using HelixToolkit;
using HelixToolkit.SharpDX;
using HelixToolkit.Wpf.SharpDX;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Windows;
using System.Windows.Media.Media3D;

namespace SD.Infrastructure.WPF.ThreeDims.Visual3Ds
{
    /// <summary>
    /// A visual element that contains a set of points. The size of the points is defined in screen space.
    /// </summary>
    public class PointsVisual3D : PointGeometryModel3D
    {
        /// <summary>
        /// Identifies the <see cref="Points"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PointsProperty = DependencyProperty.Register(nameof(Points), typeof(Point3DCollection), typeof(PointsVisual3D), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsMeasure, GeometryChanged));

        /// <summary>
        /// Initializes a new instance of the <see cref = "PointsVisual3D" /> class.
        /// </summary>
        public PointsVisual3D()
        {
            this.Points = new Point3DCollection();
        }

        /// <summary>
        /// Gets or sets the points collection.
        /// </summary>
        /// <value>The points collection.</value>
        public Point3DCollection Points
        {
            get => (Point3DCollection)this.GetValue(PointsProperty);
            set => this.SetValue(PointsProperty, value);
        }

        /// <summary>
        /// Updates the geometry.
        /// </summary>
        protected void UpdateGeometry()
        {
            if (this.Points == null)
            {
                return;
            }

            IEnumerable<Vector3> vectors = this.Points.Select(point => new Vector3((float)point.X, (float)point.Y, (float)point.Z));
            this.Geometry = new PointGeometry3D
            {
                Positions = new Vector3Collection(vectors)
            };
        }

        /// <summary>
        /// Called when geometry properties have changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        protected static void GeometryChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ((PointsVisual3D)sender).UpdateGeometry();
        }
    }
}
