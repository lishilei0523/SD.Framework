using Avalonia;
using System.Numerics;

namespace SD.Infrastructure.Avalonia.Extensions
{
    /// <summary>
    /// 变换扩展
    /// </summary>
    public static class TransformExtension
    {
        #region # Matrix3x2转Matrix —— static Matrix ToMatrix(this Matrix3x2 matrix3x2)
        /// <summary>
        /// Matrix3x2转Matrix
        /// </summary>
        public static Matrix ToMatrix(this Matrix3x2 matrix3x2)
        {
            Matrix matrix = new Matrix(matrix3x2.M11, matrix3x2.M12, matrix3x2.M21, matrix3x2.M22, matrix3x2.M31, matrix3x2.M32);

            return matrix;
        }
        #endregion

        #region # Matrix转Matrix3x2 —— static Matrix3x2 ToMatrix3x2(this Matrix matrix)
        /// <summary>
        /// Matrix转Matrix3x2
        /// </summary>
        public static Matrix3x2 ToMatrix3x2(this Matrix matrix)
        {
            Matrix3x2 matrix3x2 = new Matrix3x2((float)matrix.M11, (float)matrix.M12, (float)matrix.M21, (float)matrix.M22, (float)matrix.M31, (float)matrix.M32);

            return matrix3x2;
        }
        #endregion

        #region # 在指定点缩放 —— static Matrix ScaleAt(this Matrix matrix, double scaleX...
        /// <summary>
        /// 在指定点缩放
        /// </summary>
        public static Matrix ScaleAt(this Matrix matrix, double scaleX, double scaleY, double centerX, double centerY)
        {
            Matrix3x2 scaledMatrix32 = Matrix3x2.CreateScale((float)scaleX, (float)scaleY, new Vector2((float)centerX, (float)centerY));
            Matrix scaledMatrix = scaledMatrix32.ToMatrix();
            matrix = scaledMatrix * matrix;

            return matrix;
        }
        #endregion
    }
}
