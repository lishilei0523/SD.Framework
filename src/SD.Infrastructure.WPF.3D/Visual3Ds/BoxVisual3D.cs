using SharpDX;
using System.Windows;
#if NET462_OR_GREATER
using HelixToolkit.Wpf.SharpDX;
#endif
#if NET6_0_OR_GREATER
using HelixToolkit.SharpDX.Core;
#endif

namespace SD.Infrastructure.WPF.ThreeDims.Visual3Ds
{
    /// <summary>
    /// A visual element that renders a box.
    /// </summary>
    /// <remarks>
    /// The box is aligned with the local X, Y and Z coordinate system
    /// Use a transform to orient the box in other directions.
    /// </remarks>
    public class BoxVisual3D : MeshElement3D
    {
        /// <summary>
        /// Identifies the <see cref="Center"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CenterProperty = DependencyProperty.Register(nameof(Center), typeof(Vector3), typeof(BoxVisual3D), new UIPropertyMetadata(new Vector3(), GeometryChanged));

        /// <summary>
        /// Identifies the <see cref="Length"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LengthProperty = DependencyProperty.Register(nameof(Length), typeof(double), typeof(BoxVisual3D), new UIPropertyMetadata(1.0, GeometryChanged));

        /// <summary>
        /// Identifies the <see cref="Width"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty WidthProperty = DependencyProperty.Register(nameof(Width), typeof(double), typeof(BoxVisual3D), new UIPropertyMetadata(1.0, GeometryChanged));

        /// <summary>
        /// Identifies the <see cref="Height"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HeightProperty = DependencyProperty.Register(nameof(Height), typeof(double), typeof(BoxVisual3D), new UIPropertyMetadata(1.0, GeometryChanged));

        /// <summary>
        /// Identifies the <see cref="TopFace"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TopFaceProperty = DependencyProperty.Register(nameof(TopFace), typeof(bool), typeof(BoxVisual3D), new UIPropertyMetadata(true, GeometryChanged));

        /// <summary>
        /// Identifies the <see cref="BottomFace"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty BottomFaceProperty = DependencyProperty.Register(nameof(BottomFace), typeof(bool), typeof(BoxVisual3D), new UIPropertyMetadata(true, GeometryChanged));

        /// <summary>
        /// Gets or sets the center of the box.
        /// </summary>
        /// <value>The center.</value>
        public Vector3 Center
        {
            get => (Vector3)this.GetValue(CenterProperty);
            set => this.SetValue(CenterProperty, value);
        }

        /// <summary>
        /// Gets or sets the length of the box (along local x-axis).
        /// </summary>
        /// <value>The length.</value>
        public double Length
        {
            get => (double)this.GetValue(LengthProperty);
            set => this.SetValue(LengthProperty, value);
        }

        /// <summary>
        /// Gets or sets the width of the box (along local y-axis).
        /// </summary>
        /// <value>The width.</value>
        public double Width
        {
            get => (double)this.GetValue(WidthProperty);
            set => this.SetValue(WidthProperty, value);
        }

        /// <summary>
        /// Gets or sets the height (along local z-axis).
        /// </summary>
        /// <value>The height.</value>
        public double Height
        {
            get => (double)this.GetValue(HeightProperty);
            set => this.SetValue(HeightProperty, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether to include the top face.
        /// </summary>
        public bool TopFace
        {
            get => (bool)this.GetValue(TopFaceProperty);
            set => this.SetValue(TopFaceProperty, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether to include the bottom face.
        /// </summary>
        public bool BottomFace
        {
            get => (bool)this.GetValue(BottomFaceProperty);
            set => this.SetValue(BottomFaceProperty, value);
        }

        /// <summary>
        /// Do the tessellation and return the <see cref="MeshGeometry3D"/>.
        /// </summary>
        /// <returns>The mesh geometry.</returns>
        protected override MeshGeometry3D Tessellate()
        {
            MeshBuilder meshBuilder = new MeshBuilder();
            meshBuilder.AddCubeFace(this.Center, new Vector3(-1, 0, 0), new Vector3(0, 0, 1), this.Length, this.Width, this.Height);
            meshBuilder.AddCubeFace(this.Center, new Vector3(1, 0, 0), new Vector3(0, 0, -1), this.Length, this.Width, this.Height);
            meshBuilder.AddCubeFace(this.Center, new Vector3(0, -1, 0), new Vector3(0, 0, 1), this.Width, this.Length, this.Height);
            meshBuilder.AddCubeFace(this.Center, new Vector3(0, 1, 0), new Vector3(0, 0, -1), this.Width, this.Length, this.Height);
            if (this.TopFace)
            {
                meshBuilder.AddCubeFace(this.Center, new Vector3(0, 0, 1), new Vector3(0, -1, 0), this.Height, this.Length, this.Width);
            }
            if (this.BottomFace)
            {
                meshBuilder.AddCubeFace(this.Center, new Vector3(0, 0, -1), new Vector3(0, 1, 0), this.Height, this.Length, this.Width);
            }

            return meshBuilder.ToMesh();
        }
    }
}
