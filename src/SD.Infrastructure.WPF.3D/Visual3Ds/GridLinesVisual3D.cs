using SharpDX;
using System;
using System.Windows;
using System.Windows.Media.Media3D;
#if NET462_OR_GREATER
using HelixToolkit.Wpf.SharpDX;
using MeshGeometry3D = HelixToolkit.Wpf.SharpDX.MeshGeometry3D;
#endif
#if NET6_0_OR_GREATER
using HelixToolkit.SharpDX.Core;
using HelixToolkit.Wpf.SharpDX;
using MeshGeometry3D = HelixToolkit.SharpDX.Core.MeshGeometry3D;
#endif

namespace SD.Infrastructure.WPF.ThreeDims.Visual3Ds
{
    /// <summary>
    /// A visual element that shows a set of grid lines.
    /// </summary>
    public class GridLinesVisual3D : MeshElement3D
    {
        /// <summary>
        /// Identifies the <see cref="Center"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CenterProperty = DependencyProperty.Register(nameof(Center), typeof(Vector3D), typeof(GridLinesVisual3D), new UIPropertyMetadata(new Vector3D(), GeometryChanged));

        /// <summary>
        /// Identifies the <see cref="Normal"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty NormalProperty = DependencyProperty.Register(nameof(Normal), typeof(Vector3D), typeof(GridLinesVisual3D), new UIPropertyMetadata(new Vector3D(0, 0, 1), GeometryChanged));

        /// <summary>
        /// Identifies the <see cref="LengthDirection"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LengthDirectionProperty = DependencyProperty.Register(nameof(LengthDirection), typeof(Vector3D), typeof(GridLinesVisual3D), new UIPropertyMetadata(new Vector3D(1, 0, 0), GeometryChanged));

        /// <summary>
        /// Identifies the <see cref="Length"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LengthProperty = DependencyProperty.Register(nameof(Length), typeof(float), typeof(GridLinesVisual3D), new PropertyMetadata(200.0f, GeometryChanged));

        /// <summary>
        /// Identifies the <see cref="Width"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty WidthProperty = DependencyProperty.Register(nameof(Width), typeof(float), typeof(GridLinesVisual3D), new PropertyMetadata(200.0f, GeometryChanged));

        /// <summary>
        /// Identifies the <see cref="Thickness"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ThicknessProperty = DependencyProperty.Register(nameof(Thickness), typeof(float), typeof(GridLinesVisual3D), new PropertyMetadata(0.08f, GeometryChanged));

        /// <summary>
        /// Identifies the <see cref="MinorDistance"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MinorDistanceProperty = DependencyProperty.Register(nameof(MinorDistance), typeof(float), typeof(GridLinesVisual3D), new PropertyMetadata(2.5f, GeometryChanged));

        /// <summary>
        /// Identifies the <see cref="MajorDistance"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MajorDistanceProperty = DependencyProperty.Register(nameof(MajorDistance), typeof(float), typeof(GridLinesVisual3D), new PropertyMetadata(10.0f, GeometryChanged));

        /// <summary>
        /// The length direction.
        /// </summary>
        private Vector3D _lengthDirection;

        /// <summary>
        /// The width direction.
        /// </summary>
        private Vector3D _widthDirection;

        /// <summary>
        /// Initializes a new instance of the <see cref = "GridLinesVisual3D" /> class.
        /// </summary>
        public GridLinesVisual3D()
        {

        }

        /// <summary>
        /// Gets or sets the center of the grid.
        /// </summary>
        /// <value>The center.</value>
        public Vector3D Center
        {
            get => (Vector3D)this.GetValue(CenterProperty);
            set => this.SetValue(CenterProperty, value);
        }

        /// <summary>
        /// Gets or sets the normal vector of the grid plane.
        /// </summary>
        /// <value>The normal.</value>
        public Vector3D Normal
        {
            get => (Vector3D)this.GetValue(NormalProperty);
            set => this.SetValue(NormalProperty, value);
        }

        /// <summary>
        /// Gets or sets the length direction of the grid.
        /// </summary>
        /// <value>The length direction.</value>
        public Vector3D LengthDirection
        {
            get => (Vector3D)this.GetValue(LengthDirectionProperty);
            set => this.SetValue(LengthDirectionProperty, value);
        }

        /// <summary>
        /// Gets or sets the length of the grid area.
        /// </summary>
        /// <value>The length.</value>
        public float Length
        {
            get => (float)this.GetValue(LengthProperty);
            set => this.SetValue(LengthProperty, value);
        }

        /// <summary>
        /// Gets or sets the width of the grid area (perpendicular to the length direction).
        /// </summary>
        /// <value>The width.</value>
        public float Width
        {
            get => (float)this.GetValue(WidthProperty);
            set => this.SetValue(WidthProperty, value);
        }

        /// <summary>
        /// Gets or sets the thickness of the grid lines.
        /// </summary>
        /// <value>The thickness.</value>
        public float Thickness
        {
            get => (float)this.GetValue(ThicknessProperty);
            set => this.SetValue(ThicknessProperty, value);
        }

        /// <summary>
        /// Gets or sets the distance between minor grid lines.
        /// </summary>
        /// <value>The distance.</value>
        public float MinorDistance
        {
            get => (float)this.GetValue(MinorDistanceProperty);
            set => this.SetValue(MinorDistanceProperty, value);
        }

        /// <summary>
        /// Gets or sets the distance between major grid lines.
        /// </summary>
        /// <value>The distance.</value>
        public float MajorDistance
        {
            get => (float)this.GetValue(MajorDistanceProperty);
            set => this.SetValue(MajorDistanceProperty, value);
        }

        /// <summary>
        /// Do the tessellation and return the <see cref="MeshGeometry3D" />.
        /// </summary>
        /// <returns>A triangular mesh geometry.</returns>
        protected override MeshGeometry3D Tessellate()
        {
            this._lengthDirection = this.LengthDirection;
            this._lengthDirection.Normalize();

            // #136, chrkon, 2015-03-26
            // if NormalVector and LenghtDirection are not perpendicular then overwrite LengthDirection
            if (Vector3D.DotProduct(this.Normal, this.LengthDirection) != 0.0)
            {
                this._lengthDirection = this.Normal.ToVector3().FindAnyPerpendicular().ToVector3D();
                this._lengthDirection.Normalize();
            }

            //Create WidthDirection by rotating lengthDirection vector 90° around normal vector
            AxisAngleRotation3D axisAngleRotation3D = new AxisAngleRotation3D(this.Normal, 90.0);
            RotateTransform3D rotateTransform3D = new RotateTransform3D(axisAngleRotation3D);
            this._widthDirection = rotateTransform3D.Transform(this._lengthDirection);
            this._widthDirection.Normalize();
            // #136 

            MeshBuilder meshBuilder = new MeshBuilder();
            float minX = -this.Width / 2;
            float minY = -this.Length / 2;
            float maxX = this.Width / 2;
            float maxY = this.Length / 2;

            float x = minX;
            float eps = this.MinorDistance / 10.0f;
            while (x <= maxX + eps)
            {
                float t = this.Thickness;
                if (IsMultipleOf(x, this.MajorDistance))
                {
                    t *= 2;
                }

                this.AddLineX(meshBuilder, x, minY, maxY, t);
                x += this.MinorDistance;
            }

            float y = minY;
            while (y <= maxY + eps)
            {
                float t = this.Thickness;
                if (IsMultipleOf(y, this.MajorDistance))
                {
                    t *= 2;
                }

                this.AddLineY(meshBuilder, y, minX, maxX, t);
                y += this.MinorDistance;
            }

            MeshGeometry3D meshGeometry3D = meshBuilder.ToMesh();

            return meshGeometry3D;
        }

        /// <summary>
        /// Determines whether y is a multiple of d.
        /// </summary>
        /// <param name="y">The y.</param>
        /// <param name="d">The d.</param>
        /// <returns>The is multiple of.</returns>
        private static bool IsMultipleOf(double y, double d)
        {
            double y2 = d * (int)(y / d);
            return Math.Abs(y - y2) < 1e-3;
        }

        /// <summary>
        /// The add line x.
        /// </summary>
        /// <param name="meshBuilder">The mesh.</param>
        /// <param name="x">The x.</param>
        /// <param name="minY">The min y.</param>
        /// <param name="maxY">The max y.</param>
        /// <param name="thickness">The thickness.</param>
        private void AddLineX(MeshBuilder meshBuilder, float x, float minY, float maxY, float thickness)
        {
            Vector3 normal = this.Normal.ToVector3();
            int i0 = meshBuilder.Positions.Count;
            meshBuilder.Positions.Add(this.GetPoint(x - (thickness / 2), minY));
            meshBuilder.Positions.Add(this.GetPoint(x - (thickness / 2), maxY));
            meshBuilder.Positions.Add(this.GetPoint(x + (thickness / 2), maxY));
            meshBuilder.Positions.Add(this.GetPoint(x + (thickness / 2), minY));
            meshBuilder.Normals.Add(normal);
            meshBuilder.Normals.Add(normal);
            meshBuilder.Normals.Add(normal);
            meshBuilder.Normals.Add(normal);
            meshBuilder.TriangleIndices.Add(i0);
            meshBuilder.TriangleIndices.Add(i0 + 1);
            meshBuilder.TriangleIndices.Add(i0 + 2);
            meshBuilder.TriangleIndices.Add(i0 + 2);
            meshBuilder.TriangleIndices.Add(i0 + 3);
            meshBuilder.TriangleIndices.Add(i0);
        }

        /// <summary>
        /// The add line y.
        /// </summary>
        /// <param name="meshBuilder">The mesh.</param>
        /// <param name="y">The y.</param>
        /// <param name="minX">The min x.</param>
        /// <param name="maxX">The max x.</param>
        /// <param name="thickness">The thickness.</param>
        private void AddLineY(MeshBuilder meshBuilder, float y, float minX, float maxX, float thickness)
        {
            Vector3 normal = this.Normal.ToVector3();
            int i0 = meshBuilder.Positions.Count;
            meshBuilder.Positions.Add(this.GetPoint(minX, y + (thickness / 2)));
            meshBuilder.Positions.Add(this.GetPoint(maxX, y + (thickness / 2)));
            meshBuilder.Positions.Add(this.GetPoint(maxX, y - (thickness / 2)));
            meshBuilder.Positions.Add(this.GetPoint(minX, y - (thickness / 2)));
            meshBuilder.Normals.Add(normal);
            meshBuilder.Normals.Add(normal);
            meshBuilder.Normals.Add(normal);
            meshBuilder.Normals.Add(normal);
            meshBuilder.TriangleIndices.Add(i0);
            meshBuilder.TriangleIndices.Add(i0 + 1);
            meshBuilder.TriangleIndices.Add(i0 + 2);
            meshBuilder.TriangleIndices.Add(i0 + 2);
            meshBuilder.TriangleIndices.Add(i0 + 3);
            meshBuilder.TriangleIndices.Add(i0);
        }

        /// <summary>
        /// Gets a point on the plane.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <returns>A <see cref="Vector3"/>.</returns>
        private Vector3 GetPoint(float x, float y)
        {
            Vector3D point = this.Center + (this._widthDirection * x) + (this._lengthDirection * y);

            return point.ToVector3();
        }
    }
}
