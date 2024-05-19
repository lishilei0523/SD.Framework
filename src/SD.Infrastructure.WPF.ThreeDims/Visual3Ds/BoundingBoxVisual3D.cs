using HelixToolkit.Wpf.SharpDX;
using System.Windows;
using System.Windows.Media.Media3D;
using MeshGeometry3D = HelixToolkit.Wpf.SharpDX.MeshGeometry3D;

namespace SD.Infrastructure.WPF.ThreeDims.Visual3Ds
{
    /// <summary>
    /// A visual element that shows a wireframe for the specified bounding box.
    /// </summary>
    public class BoundingBoxVisual3D : MeshElement3D
    {
        /// <summary>
        /// Identifies the <see cref="BoundingBox"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty BoundingBoxProperty;

        /// <summary>
        /// Identifies the <see cref="Diameter"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DiameterProperty;

        /// <summary>
        /// Static constructor
        /// </summary>
        static BoundingBoxVisual3D()
        {
            DiameterProperty = DependencyProperty.Register(nameof(Diameter), typeof(double), typeof(BoundingBoxVisual3D), new UIPropertyMetadata(0.1, GeometryChanged));
            BoundingBoxProperty = DependencyProperty.Register(nameof(BoundingBox), typeof(Rect3D), typeof(BoundingBoxVisual3D), new UIPropertyMetadata(new Rect3D(0, 0, 0, 1, 1, 1), GeometryChanged));
        }

        /// <summary>
        /// Gets or sets the bounding box.
        /// </summary>
        /// <value> The bounding box. </value>
        public Rect3D BoundingBox
        {
            get
            {
                return (Rect3D)this.GetValue(BoundingBoxVisual3D.BoundingBoxProperty);
            }

            set
            {
                this.SetValue(BoundingBoxVisual3D.BoundingBoxProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the diameter.
        /// </summary>
        /// <value> The diameter. </value>
        public double Diameter
        {
            get
            {
                return (double)this.GetValue(BoundingBoxVisual3D.DiameterProperty);
            }

            set
            {
                this.SetValue(BoundingBoxVisual3D.DiameterProperty, value);
            }
        }

        /// <summary>
        /// Do the tessellation and return the <see cref="T:System.Windows.Media.Media3D.MeshGeometry3D" />.
        /// </summary>
        /// <returns>A triangular mesh geometry.</returns>
        protected override MeshGeometry3D Tessellate()
        {
            MeshBuilder meshBuilder = new MeshBuilder(false, false);
            meshBuilder.AddBoundingBox(this.BoundingBox, this.Diameter);

            return meshBuilder.ToMesh();
        }
    }
}
