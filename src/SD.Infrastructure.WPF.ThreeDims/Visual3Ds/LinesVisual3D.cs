using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media.Media3D;
using SharpDX;
#if NET462_OR_GREATER
using HelixToolkit.Wpf.SharpDX;
#endif
#if NET6_0_OR_GREATER
using HelixToolkit.Wpf.SharpDX;
using HelixToolkit.SharpDX.Core;
#endif

namespace SD.Infrastructure.WPF.ThreeDims.Visual3Ds
{
    /// <summary>
    /// A visual element that contains a set of line segments. The thickness of the lines is defined in screen space.
    /// </summary>
    public class LinesVisual3D : LineGeometryModel3D
    {
        /// <summary>
        /// Identifies the <see cref="IsClosed"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsClosedProperty = DependencyProperty.Register(nameof(IsClosed), typeof(bool), typeof(LinesVisual3D), new UIPropertyMetadata(false, GeometryChanged));

        /// <summary>
        /// Identifies the <see cref="Points"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PointsProperty = DependencyProperty.Register(nameof(Points), typeof(Point3DCollection), typeof(LinesVisual3D), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsMeasure, GeometryChanged));

        /// <summary>
        /// Initializes a new instance of the <see cref = "LinesVisual3D" /> class.
        /// </summary>
        public LinesVisual3D()
        {
            this.Points = new Point3DCollection();
        }

        /// <summary>
        /// Gets or sets if the lines is closed.
        /// </summary>
        /// <value>Is closed.</value>
        public bool IsClosed
        {
            get => (bool)this.GetValue(IsClosedProperty);
            set => this.SetValue(IsClosedProperty, value);
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
            LineBuilder lineBuilder = new LineBuilder();
            lineBuilder.Add(this.IsClosed, vectors.ToArray());
            this.Geometry = lineBuilder.ToLineGeometry3D();
        }

        /// <summary>
        /// Called when geometry properties have changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        protected static void GeometryChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ((LinesVisual3D)sender).UpdateGeometry();
        }
    }
}
