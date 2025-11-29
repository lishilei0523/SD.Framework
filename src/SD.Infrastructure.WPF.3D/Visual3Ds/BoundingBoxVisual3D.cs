using HelixToolkit.SharpDX;
using HelixToolkit.Wpf.SharpDX;
using System.Windows;
using System.Windows.Media.Media3D;

namespace SD.Infrastructure.WPF.ThreeDims.Visual3Ds
{
    /// <summary>
    /// A visual element that shows a wireframe for the specified bounding box.
    /// </summary>
    public class BoundingBoxVisual3D : LineGeometryModel3D
    {
        /// <summary>
        /// Identifies the <see cref="BoundingBox"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty BoundingBoxProperty;

        /// <summary>
        /// Static constructor
        /// </summary>
        static BoundingBoxVisual3D()
        {
            BoundingBoxProperty = DependencyProperty.Register(nameof(BoundingBox), typeof(Rect3D), typeof(BoundingBoxVisual3D), new UIPropertyMetadata(new Rect3D(0, 0, 0, 1, 1, 1), GeometryChanged));
        }

        /// <summary>
        /// Gets or sets the bounding box.
        /// </summary>
        /// <value> The bounding box. </value>
        public Rect3D BoundingBox
        {
            get => (Rect3D)this.GetValue(BoundingBoxProperty);
            set => this.SetValue(BoundingBoxProperty, value);
        }

        /// <summary>
        /// Updates the geometry.
        /// </summary>
        protected void UpdateGeometry()
        {
            this.Geometry = LineBuilder.GenerateBoundingBox(this.BoundingBox.ToBoundingBox());
        }

        /// <summary>
        /// Called when geometry properties have changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        protected static void GeometryChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ((BoundingBoxVisual3D)sender).UpdateGeometry();
        }
    }
}
