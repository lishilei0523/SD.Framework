using SharpDX;
using System.Windows;
#if NET462_OR_GREATER
using HelixToolkit.Wpf.SharpDX;
using MeshGeometry3D = HelixToolkit.Wpf.SharpDX.MeshGeometry3D;
#endif
#if NET6_0_OR_GREATER
using HelixToolkit.SharpDX.Core;
using MeshGeometry3D = HelixToolkit.SharpDX.Core.MeshGeometry3D;
#endif

namespace SD.Infrastructure.WPF.ThreeDims.Visual3Ds
{
    /// <summary>
    /// A visual element that shows a sphere defined by center and radius.
    /// </summary>
    public class SphereVisual3D : MeshElement3D
    {
        /// <summary>
        /// Identifies the <see cref="Center"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CenterProperty = DependencyProperty.Register(
            nameof(Center),
            typeof(Vector3),
            typeof(SphereVisual3D),
            new PropertyMetadata(new Vector3(0, 0, 0), GeometryChanged));

        /// <summary>
        /// Identifies the <see cref="PhiDiv"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PhiDivProperty = DependencyProperty.Register(
            nameof(PhiDiv), typeof(int), typeof(SphereVisual3D), new PropertyMetadata(30, GeometryChanged));

        /// <summary>
        /// Identifies the <see cref="Radius"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty RadiusProperty = DependencyProperty.Register(
            nameof(Radius), typeof(double), typeof(SphereVisual3D), new PropertyMetadata(1.0, GeometryChanged));

        /// <summary>
        /// Identifies the <see cref="ThetaDiv"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ThetaDivProperty = DependencyProperty.Register(
            nameof(ThetaDiv), typeof(int), typeof(SphereVisual3D), new PropertyMetadata(60, GeometryChanged));

        /// <summary>
        /// Gets or sets the center of the sphere.
        /// </summary>
        /// <value>The center.</value>
        public Vector3 Center
        {
            get
            {
                return (Vector3)this.GetValue(SphereVisual3D.CenterProperty);
            }

            set
            {
                this.SetValue(SphereVisual3D.CenterProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the number of divisions in the phi direction (from "top" to "bottom").
        /// </summary>
        /// <value>The phi div.</value>
        public int PhiDiv
        {
            get
            {
                return (int)this.GetValue(SphereVisual3D.PhiDivProperty);
            }

            set
            {
                this.SetValue(SphereVisual3D.PhiDivProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the radius of the sphere.
        /// </summary>
        /// <value>The radius.</value>
        public double Radius
        {
            get
            {
                return (double)this.GetValue(SphereVisual3D.RadiusProperty);
            }

            set
            {
                this.SetValue(SphereVisual3D.RadiusProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the number of divisions in the theta direction (around the sphere).
        /// </summary>
        /// <value>The theta div.</value>
        public int ThetaDiv
        {
            get
            {
                return (int)this.GetValue(SphereVisual3D.ThetaDivProperty);
            }

            set
            {
                this.SetValue(SphereVisual3D.ThetaDivProperty, value);
            }
        }

        /// <summary>
        /// Do the tessellation and return the <see cref="HelixToolkit.Wpf.SharpDX.MeshGeometry3D"/>.
        /// </summary>
        /// <returns>A triangular mesh geometry.</returns>
        protected override MeshGeometry3D Tessellate()
        {
            MeshBuilder builder = new MeshBuilder(true, true);
            builder.AddSphere(this.Center, this.Radius, this.ThetaDiv, this.PhiDiv);
            return builder.ToMesh();
        }
    }
}
