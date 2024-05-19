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
        public static readonly DependencyProperty CenterProperty = DependencyProperty.Register(nameof(Center), typeof(Vector3), typeof(GridLinesVisual3D), new UIPropertyMetadata(new Vector3(), GeometryChanged));

        /// <summary>
        /// Identifies the <see cref="MinorDistance"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MinorDistanceProperty = DependencyProperty.Register(nameof(MinorDistance), typeof(float), typeof(GridLinesVisual3D), new PropertyMetadata(2.5f, GeometryChanged));

        /// <summary>
        /// Identifies the <see cref="LengthDirection"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LengthDirectionProperty = DependencyProperty.Register(nameof(LengthDirection), typeof(Vector3), typeof(GridLinesVisual3D), new UIPropertyMetadata(new Vector3(1, 0, 0), GeometryChanged));

        /// <summary>
        /// Identifies the <see cref="Length"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LengthProperty = DependencyProperty.Register(nameof(Length), typeof(float), typeof(GridLinesVisual3D), new PropertyMetadata(200.0f, GeometryChanged));

        /// <summary>
        /// Identifies the <see cref="MajorDistance"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MajorDistanceProperty = DependencyProperty.Register(nameof(MajorDistance), typeof(float), typeof(GridLinesVisual3D), new PropertyMetadata(10.0f, GeometryChanged));

        /// <summary>
        /// Identifies the <see cref="Normal"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty NormalProperty = DependencyProperty.Register(nameof(Normal), typeof(Vector3), typeof(GridLinesVisual3D), new UIPropertyMetadata(new Vector3(0, 0, 1), GeometryChanged));

        /// <summary>
        /// Identifies the <see cref="Thickness"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ThicknessProperty = DependencyProperty.Register(nameof(Thickness), typeof(float), typeof(GridLinesVisual3D), new PropertyMetadata(0.08f, GeometryChanged));

        /// <summary>
        /// Identifies the <see cref="Width"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty WidthProperty = DependencyProperty.Register(nameof(Width), typeof(float), typeof(GridLinesVisual3D), new PropertyMetadata(200.0f, GeometryChanged));

        /// <summary>
        /// The length direction.
        /// </summary>
        private Vector3 _lengthDirection;

        /// <summary>
        /// The width direction.
        /// </summary>
        private Vector3 _widthDirection;

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
        public Vector3 Center
        {
            get
            {
                return (Vector3)this.GetValue(GridLinesVisual3D.CenterProperty);
            }

            set
            {
                this.SetValue(GridLinesVisual3D.CenterProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the length of the grid area.
        /// </summary>
        /// <value>The length.</value>
        public float Length
        {
            get
            {
                return (float)this.GetValue(GridLinesVisual3D.LengthProperty);
            }

            set
            {
                this.SetValue(GridLinesVisual3D.LengthProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the length direction of the grid.
        /// </summary>
        /// <value>The length direction.</value>
        public Vector3 LengthDirection
        {
            get
            {
                return (Vector3)this.GetValue(GridLinesVisual3D.LengthDirectionProperty);
            }

            set
            {
                this.SetValue(GridLinesVisual3D.LengthDirectionProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the distance between major grid lines.
        /// </summary>
        /// <value>The distance.</value>
        public float MajorDistance
        {
            get
            {
                return (float)this.GetValue(GridLinesVisual3D.MajorDistanceProperty);
            }

            set
            {
                this.SetValue(GridLinesVisual3D.MajorDistanceProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the distance between minor grid lines.
        /// </summary>
        /// <value>The distance.</value>
        public float MinorDistance
        {
            get
            {
                return (float)this.GetValue(GridLinesVisual3D.MinorDistanceProperty);
            }

            set
            {
                this.SetValue(GridLinesVisual3D.MinorDistanceProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the normal vector of the grid plane.
        /// </summary>
        /// <value>The normal.</value>
        public Vector3 Normal
        {
            get
            {
                return (Vector3)this.GetValue(GridLinesVisual3D.NormalProperty);
            }

            set
            {
                this.SetValue(GridLinesVisual3D.NormalProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the thickness of the grid lines.
        /// </summary>
        /// <value>The thickness.</value>
        public float Thickness
        {
            get
            {
                return (float)this.GetValue(GridLinesVisual3D.ThicknessProperty);
            }

            set
            {
                this.SetValue(GridLinesVisual3D.ThicknessProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the width of the grid area (perpendicular to the length direction).
        /// </summary>
        /// <value>The width.</value>
        public float Width
        {
            get
            {
                return (float)this.GetValue(GridLinesVisual3D.WidthProperty);
            }

            set
            {
                this.SetValue(GridLinesVisual3D.WidthProperty, value);
            }
        }

        /// <summary>
        /// Do the tessellation and return the <see cref="HelixToolkit.SharpDX.Core.MeshGeometry3D" />.
        /// </summary>
        /// <returns>A triangular mesh geometry.</returns>
        protected override MeshGeometry3D Tessellate()
        {
            this._lengthDirection = this.LengthDirection;
            this._lengthDirection.Normalize();

            // #136, chrkon, 2015-03-26
            // if NormalVector and LenghtDirection are not perpendicular then overwrite LengthDirection
            if (Vector3.Dot(this.Normal, this.LengthDirection) != 0.0)
            {
                this._lengthDirection = this.Normal.FindAnyPerpendicular();
                this._lengthDirection.Normalize();
            }

            // create WidthDirection by rotating lengthDirection vector 90° around normal vector
            Vector3D normal = new Vector3D(this.Normal.X, this.Normal.Y, this.Normal.Z);
            Vector3D lengthDirection = new Vector3D(this._lengthDirection.X, this._lengthDirection.Y, this._lengthDirection.Z);
            RotateTransform3D rotate = new RotateTransform3D(new AxisAngleRotation3D(normal, 90.0));
            Vector3D widthDirection = rotate.Transform(lengthDirection);
            this._widthDirection = new Vector3((float)widthDirection.X, (float)widthDirection.Y, (float)widthDirection.Z);
            this._widthDirection.Normalize();
            // #136 

            MeshBuilder mesh = new MeshBuilder(true, false);
            float minX = -this.Width / 2;
            float minY = -this.Length / 2;
            float maxX = this.Width / 2;
            float maxY = this.Length / 2;

            float x = minX;
            float eps = this.MinorDistance / 10;
            while (x <= maxX + eps)
            {
                float t = this.Thickness;
                if (GridLinesVisual3D.IsMultipleOf(x, this.MajorDistance))
                {
                    t *= 2;
                }

                this.AddLineX(mesh, x, minY, maxY, t);
                x += this.MinorDistance;
            }

            float y = minY;
            while (y <= maxY + eps)
            {
                float t = this.Thickness;
                if (GridLinesVisual3D.IsMultipleOf(y, this.MajorDistance))
                {
                    t *= 2;
                }

                this.AddLineY(mesh, y, minX, maxX, t);
                y += this.MinorDistance;
            }

            MeshGeometry3D m = mesh.ToMesh();

            return m;
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
        /// <param name="mesh">The mesh.</param>
        /// <param name="x">The x.</param>
        /// <param name="minY">The min y.</param>
        /// <param name="maxY">The max y.</param>
        /// <param name="thickness">The thickness.</param>
        private void AddLineX(MeshBuilder mesh, float x, float minY, float maxY, float thickness)
        {
            int i0 = mesh.Positions.Count;
            mesh.Positions.Add(this.GetPoint(x - (thickness / 2), minY));
            mesh.Positions.Add(this.GetPoint(x - (thickness / 2), maxY));
            mesh.Positions.Add(this.GetPoint(x + (thickness / 2), maxY));
            mesh.Positions.Add(this.GetPoint(x + (thickness / 2), minY));
            mesh.Normals.Add(this.Normal);
            mesh.Normals.Add(this.Normal);
            mesh.Normals.Add(this.Normal);
            mesh.Normals.Add(this.Normal);
            mesh.TriangleIndices.Add(i0);
            mesh.TriangleIndices.Add(i0 + 1);
            mesh.TriangleIndices.Add(i0 + 2);
            mesh.TriangleIndices.Add(i0 + 2);
            mesh.TriangleIndices.Add(i0 + 3);
            mesh.TriangleIndices.Add(i0);
        }

        /// <summary>
        /// The add line y.
        /// </summary>
        /// <param name="mesh">The mesh.</param>
        /// <param name="y">The y.</param>
        /// <param name="minX">The min x.</param>
        /// <param name="maxX">The max x.</param>
        /// <param name="thickness">The thickness.</param>
        private void AddLineY(MeshBuilder mesh, float y, float minX, float maxX, float thickness)
        {
            int i0 = mesh.Positions.Count;
            mesh.Positions.Add(this.GetPoint(minX, y + (thickness / 2)));
            mesh.Positions.Add(this.GetPoint(maxX, y + (thickness / 2)));
            mesh.Positions.Add(this.GetPoint(maxX, y - (thickness / 2)));
            mesh.Positions.Add(this.GetPoint(minX, y - (thickness / 2)));
            mesh.Normals.Add(this.Normal);
            mesh.Normals.Add(this.Normal);
            mesh.Normals.Add(this.Normal);
            mesh.Normals.Add(this.Normal);
            mesh.TriangleIndices.Add(i0);
            mesh.TriangleIndices.Add(i0 + 1);
            mesh.TriangleIndices.Add(i0 + 2);
            mesh.TriangleIndices.Add(i0 + 2);
            mesh.TriangleIndices.Add(i0 + 3);
            mesh.TriangleIndices.Add(i0);
        }

        /// <summary>
        /// Gets a point on the plane.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <returns>A <see cref="Vector3"/>.</returns>
        private Vector3 GetPoint(float x, float y)
        {
            return this.Center + (this._widthDirection * x) + (this._lengthDirection * y);
        }
    }
}
