using HelixToolkit.Maths;
using HelixToolkit.Wpf.SharpDX;
using System.Numerics;
using System.Windows.Media.Media3D;

namespace SD.Infrastructure.WPF.ThreeDims.Extensions
{
    /// <summary>
    /// 变换扩展
    /// </summary>
    public static class TransformExtension
    {
        #region # 获取位置坐标 —— static Point3D GetLocation(this Matrix3D matrix3D)
        /// <summary>
        /// 获取位置坐标
        /// </summary>
        public static Point3D GetLocation(this Matrix3D matrix3D)
        {
            return new Point3D(matrix3D.OffsetX, matrix3D.OffsetY, matrix3D.OffsetZ);
        }
        #endregion

        #region # 3D元素是否包含点 —— static bool ContainsPoint(this ModelVisual3D visual3D, Point3D point)
        /// <summary>
        /// 3D元素是否包含点
        /// </summary>
        /// <param name="visual3D">3D元素</param>
        /// <param name="point">点</param>
        /// <returns>是否包含</returns>
        public static bool ContainsPoint(this ModelVisual3D visual3D, Point3D point)
        {
            Point3D projectedPoint = point;

            //逆变换
            GeneralTransform3D transformInverse = visual3D.Transform.Inverse;
            if (transformInverse != null)
            {
                projectedPoint = transformInverse.Transform(point);
            }

            bool contained = visual3D.Content.Bounds.Contains(projectedPoint);

            return contained;
        }
        #endregion

        #region # 3D元素是否包含点 —— static bool ContainsPoint(this Element3D element3D, Point3D point)
        /// <summary>
        /// 3D元素是否包含点
        /// </summary>
        /// <param name="element3D">3D元素</param>
        /// <param name="point">点</param>
        /// <returns>是否包含</returns>
        public static bool ContainsPoint(this Element3D element3D, Point3D point)
        {
            Point3D projectedPoint = point;

            //逆变换
            GeneralTransform3D transformInverse = element3D.Transform.Inverse;
            if (transformInverse != null)
            {
                projectedPoint = transformInverse.Transform(point);
            }

            ContainmentType result = element3D.Bounds.Contains(projectedPoint.ToVector3());
            if (result == ContainmentType.Disjoint)
            {
                return false;
            }

            return true;
        }
        #endregion

        #region # 3D元素是否包含点 —— static bool ContainsPoint(this Element3D element3D, Vector3 point)
        /// <summary>
        /// 3D元素是否包含点
        /// </summary>
        /// <param name="element3D">3D元素</param>
        /// <param name="point">点</param>
        /// <returns>是否包含</returns>
        public static bool ContainsPoint(this Element3D element3D, Vector3 point)
        {
            Point3D projectedPoint = point.ToPoint3D();

            //逆变换
            GeneralTransform3D transformInverse = element3D.Transform.Inverse;
            if (transformInverse != null)
            {
                projectedPoint = transformInverse.Transform(projectedPoint);
            }

            ContainmentType result = element3D.Bounds.Contains(projectedPoint.ToVector3());
            if (result == ContainmentType.Disjoint)
            {
                return false;
            }

            return true;
        }
        #endregion
    }
}
