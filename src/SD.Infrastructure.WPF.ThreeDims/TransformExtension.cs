﻿using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using System.Windows.Media.Media3D;

namespace SD.Infrastructure.WPF.ThreeDims
{
    /// <summary>
    /// 变换扩展
    /// </summary>
    public static class TransformExtension
    {
        #region # Matrix3D转换Matrix —— static Matrix<double> ToMatrix(this Matrix3D matrix3D)
        /// <summary>
        /// Matrix3D转换Matrix
        /// </summary>
        public static Matrix<double> ToMatrix(this Matrix3D matrix3D)
        {
            Matrix<double> transpose = DenseMatrix.Create(4, 4, 0);
            transpose[0, 0] = matrix3D.M11;
            transpose[1, 0] = matrix3D.M21;
            transpose[2, 0] = matrix3D.M31;
            transpose[3, 0] = matrix3D.OffsetX;
            transpose[0, 1] = matrix3D.M12;
            transpose[1, 1] = matrix3D.M22;
            transpose[2, 1] = matrix3D.M32;
            transpose[3, 1] = matrix3D.OffsetY;
            transpose[0, 2] = matrix3D.M13;
            transpose[1, 2] = matrix3D.M23;
            transpose[2, 2] = matrix3D.M33;
            transpose[3, 2] = matrix3D.OffsetZ;
            transpose[0, 3] = matrix3D.M14;
            transpose[1, 3] = matrix3D.M24;
            transpose[2, 3] = matrix3D.M34;
            transpose[3, 3] = matrix3D.M44;
            Matrix<double> matrix = transpose.Transpose();

            return matrix;
        }
        #endregion

        #region # Matrix转换Matrix3D —— static Matrix3D ToMatrix3D(this Matrix<double> matrix)
        /// <summary>
        /// Matrix转换Matrix3D
        /// </summary>
        public static Matrix3D ToMatrix3D(this Matrix<double> matrix)
        {
            Matrix<double> transpose = matrix.Transpose();
            double m11 = transpose[0, 0];
            double m12 = transpose[0, 1];
            double m13 = transpose[0, 2];
            double m14 = transpose[0, 3];
            double m21 = transpose[1, 0];
            double m22 = transpose[1, 1];
            double m23 = transpose[1, 2];
            double m24 = transpose[1, 3];
            double m31 = transpose[2, 0];
            double m32 = transpose[2, 1];
            double m33 = transpose[2, 2];
            double m34 = transpose[2, 3];
            double m41 = transpose[3, 0];
            double m42 = transpose[3, 1];
            double m43 = transpose[3, 2];
            double m44 = transpose[3, 3];
            Matrix3D matrix3D = new Matrix3D(m11, m12, m13, m14, m21, m22, m23, m24, m31, m32, m33, m34, m41, m42, m43, m44);

            return matrix3D;
        }
        #endregion
    }
}
