using HelixToolkit.Wpf.SharpDX;
using System.Windows;
using MeshGeometry3D = HelixToolkit.Wpf.SharpDX.MeshGeometry3D;

namespace SD.Infrastructure.WPF.ThreeDims.Visual3Ds
{
    /// <summary>
    /// Represents a base class for elements that contain one <see cref="T:System.Windows.Media.Media3D.GeometryModel3D" />.
    /// </summary>
    /// <remarks>
    /// Derived classes should override the Tessellate method to generate the geometry.
    /// </remarks>
    public abstract class MeshElement3D : MeshGeometryModel3D
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:HelixToolkit.Wpf.MeshElement3D" /> class.
        /// </summary>
        protected MeshElement3D()
        {
            this.UpdateModel();
        }

        /// <summary>
        /// Forces an update of the geometry.
        /// </summary>
        public void UpdateModel()
        {
            this.OnGeometryChanged();
        }

        /// <summary>
        /// The geometry was changed.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The event arguments.</param>
        protected static void GeometryChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((MeshElement3D)d).OnGeometryChanged();
        }

        /// <summary>
        /// Handles changes in geometry or visible state.
        /// </summary>
        protected virtual void OnGeometryChanged()
        {
            this.Geometry = this.Visible ? this.Tessellate() : null;
        }

        /// <summary>
        /// Do the tessellation and return the <see cref="T:System.Windows.Media.Media3D.MeshGeometry3D" />.
        /// </summary>
        /// <returns>A triangular mesh geometry.</returns>
        protected abstract MeshGeometry3D Tessellate();
    }
}
