using SharpDX;
using System;
using System.Collections.Generic;
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
    /// A visual element that shows a 3D rectangle defined by origin, normal, length and width.
    /// </summary>
    public class RectangleVisual3D : MeshElement3D
    {
        /// <summary>
        /// Identifies the <see cref="DivLength"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DivLengthProperty = DependencyProperty.Register(nameof(DivLength), typeof(int), typeof(RectangleVisual3D), new UIPropertyMetadata(10, GeometryChanged, CoerceDivValue));

        /// <summary>
        /// Identifies the <see cref="DivWidth"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DivWidthProperty = DependencyProperty.Register(nameof(DivWidth), typeof(int), typeof(RectangleVisual3D), new UIPropertyMetadata(10, GeometryChanged, CoerceDivValue));

        /// <summary>
        /// Identifies the <see cref="LengthDirection"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LengthDirectionProperty = DependencyProperty.Register(nameof(LengthDirection), typeof(Vector3), typeof(RectangleVisual3D), new PropertyMetadata(new Vector3(1, 0, 0), GeometryChanged));

        /// <summary>
        /// Identifies the <see cref="Length"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LengthProperty = DependencyProperty.Register(nameof(Length), typeof(float), typeof(RectangleVisual3D), new PropertyMetadata(10.0f, GeometryChanged));

        /// <summary>
        /// Identifies the <see cref="Normal"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty NormalProperty = DependencyProperty.Register(nameof(Normal), typeof(Vector3), typeof(RectangleVisual3D), new PropertyMetadata(new Vector3(0, 0, 1), GeometryChanged));

        /// <summary>
        /// Identifies the <see cref="Origin"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OriginProperty = DependencyProperty.Register(nameof(Origin), typeof(Vector3), typeof(RectangleVisual3D), new PropertyMetadata(new Vector3(0, 0, 0), GeometryChanged));

        /// <summary>
        /// Identifies the <see cref="Width"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty WidthProperty = DependencyProperty.Register(nameof(Width), typeof(float), typeof(RectangleVisual3D), new PropertyMetadata(10.0f, GeometryChanged));

        /// <summary>
        /// Gets or sets the number of divisions in the 'length' direction.
        /// </summary>
        /// <value>The number of divisions.</value>
        public int DivLength
        {
            get
            {
                return (int)this.GetValue(RectangleVisual3D.DivLengthProperty);
            }

            set
            {
                this.SetValue(RectangleVisual3D.DivLengthProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the number of divisions in the 'width' direction.
        /// </summary>
        /// <value>The number of divisions.</value>
        public int DivWidth
        {
            get
            {
                return (int)this.GetValue(RectangleVisual3D.DivWidthProperty);
            }

            set
            {
                this.SetValue(RectangleVisual3D.DivWidthProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the length.
        /// </summary>
        /// <value>The length.</value>
        public float Length
        {
            get
            {
                return (float)this.GetValue(RectangleVisual3D.LengthProperty);
            }

            set
            {
                this.SetValue(RectangleVisual3D.LengthProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the length direction.
        /// </summary>
        /// <value>The length direction.</value>
        public Vector3 LengthDirection
        {
            get
            {
                return (Vector3)this.GetValue(RectangleVisual3D.LengthDirectionProperty);
            }

            set
            {
                this.SetValue(RectangleVisual3D.LengthDirectionProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the normal vector of the plane.
        /// </summary>
        /// <value>The normal.</value>
        public Vector3 Normal
        {
            get
            {
                return (Vector3)this.GetValue(RectangleVisual3D.NormalProperty);
            }

            set
            {
                this.SetValue(RectangleVisual3D.NormalProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the center point of the plane.
        /// </summary>
        /// <value>The origin.</value>
        public Vector3 Origin
        {
            get
            {
                return (Vector3)this.GetValue(RectangleVisual3D.OriginProperty);
            }

            set
            {
                this.SetValue(RectangleVisual3D.OriginProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>The width.</value>
        public float Width
        {
            get
            {
                return (float)this.GetValue(RectangleVisual3D.WidthProperty);
            }

            set
            {
                this.SetValue(RectangleVisual3D.WidthProperty, value);
            }
        }

        /// <summary>
        /// Do the tessellation and return the <see cref="System.Windows.Media.Media3D.MeshGeometry3D"/>.
        /// </summary>
        /// <returns>A triangular mesh geometry.</returns>
        protected override MeshGeometry3D Tessellate()
        {
            Vector3 u = this.LengthDirection;
            Vector3 w = this.Normal;
            Vector3 v = Vector3.Cross(w, u);
            u = Vector3.Cross(v, w);

            u.Normalize();
            v.Normalize();
            w.Normalize();

            float le = this.Length;
            float wi = this.Width;

            List<Vector3> pts = new List<Vector3>();
            for (int i = 0; i < this.DivLength; i++)
            {
                float fi = -0.5f + ((float)i / (this.DivLength - 1));
                for (int j = 0; j < this.DivWidth; j++)
                {
                    float fj = -0.5f + ((float)j / (this.DivWidth - 1));
                    pts.Add(this.Origin + (u * le * fi) + (v * wi * fj));
                }
            }

            MeshBuilder builder = new MeshBuilder(false, true);
            builder.AddRectangularMesh(pts, this.DivWidth);

            return builder.ToMesh();
        }

        /// <summary>
        /// Coerces the division value.
        /// </summary>
        /// <param name="d">The sender.</param>
        /// <param name="baseValue">The base value.</param>
        /// <returns>A value not less than 2.</returns>
        private static object CoerceDivValue(DependencyObject d, object baseValue)
        {
            return Math.Max(2, (int)baseValue);
        }
    }
}