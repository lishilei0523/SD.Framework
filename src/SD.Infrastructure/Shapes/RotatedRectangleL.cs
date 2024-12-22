using System;
using System.Numerics;
using System.Runtime.Serialization;

namespace SD.Infrastructure.Shapes
{
    /// <summary>
    /// 旋转矩形
    /// </summary>
    [Serializable]
    [DataContract]
    public class RotatedRectangleL : ShapeL
    {
        #region # 构造器

        #region 00.无参构造器
        /// <summary>
        /// 无参构造器
        /// </summary>
        public RotatedRectangleL() { }
        #endregion

        #region 01.创建旋转矩形构造器
        /// <summary>
        /// 创建旋转矩形构造器
        /// </summary>
        /// <param name="x">顶点横坐标值</param>
        /// <param name="y">顶点纵坐标值</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        /// <param name="angle">角度</param>
        public RotatedRectangleL(int x, int y, int width, int height, float angle)
            : this()
        {
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
            this.Angle = angle;
        }
        #endregion

        #region 02.创建旋转矩形构造器
        /// <summary>
        /// 创建旋转矩形构造器
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="x">顶点横坐标值</param>
        /// <param name="y">顶点纵坐标值</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        /// <param name="angle">角度</param>
        public RotatedRectangleL(string name, int x, int y, int width, int height, float angle)
            : this(x, y, width, height, angle)
        {
            base.Name = name;
        }
        #endregion

        #endregion

        #region # 属性

        #region 顶点横坐标值 —— int X
        /// <summary>
        /// 顶点横坐标值
        /// </summary>
        [DataMember]
        public int X { get; set; }
        #endregion

        #region 顶点纵坐标值 —— int Y
        /// <summary>
        /// 顶点纵坐标值
        /// </summary>
        [DataMember]
        public int Y { get; set; }
        #endregion

        #region 宽度 —— int Width
        /// <summary>
        /// 宽度
        /// </summary>
        [DataMember]
        public int Width { get; set; }
        #endregion

        #region 高度 —— int Height
        /// <summary>
        /// 高度
        /// </summary>
        [DataMember]
        public int Height { get; set; }
        #endregion

        #region 角度 —— float Angle
        /// <summary>
        /// 角度
        /// </summary>
        [DataMember]
        public float Angle { get; set; }
        #endregion

        #region 只读属性 - 左上顶点坐标 —— PointL TopLeft
        /// <summary>
        /// 只读属性 - 左上顶点坐标
        /// </summary>
        /// <remarks>A点</remarks>
        public PointL TopLeft
        {
            get
            {
                Vector2 point = new Vector2(this.X, this.Y);
                Vector2 transformedpoint = Vector2.Transform(point, this.TransformMatrix);
                int x = (int)Math.Ceiling(transformedpoint.X);
                int y = (int)Math.Ceiling(transformedpoint.Y);

                return new PointL(x, y);
            }
        }
        #endregion

        #region 只读属性 - 右上顶点坐标 —— PointL TopRight
        /// <summary>
        /// 只读属性 - 右上顶点坐标
        /// </summary>
        /// <remarks>D点</remarks>
        public PointL TopRight
        {
            get
            {
                Vector2 point = new Vector2(this.X + this.Width, this.Y);
                Vector2 transformedpoint = Vector2.Transform(point, this.TransformMatrix);
                int x = (int)Math.Ceiling(transformedpoint.X);
                int y = (int)Math.Ceiling(transformedpoint.Y);

                return new PointL(x, y);
            }
        }
        #endregion

        #region 只读属性 - 左下顶点坐标 —— PointL BottomLeft
        /// <summary>
        /// 只读属性 - 左下顶点坐标
        /// </summary>
        /// <remarks>B点</remarks>
        public PointL BottomLeft
        {
            get
            {
                Vector2 point = new Vector2(this.X, this.Y + this.Height);
                Vector2 transformedpoint = Vector2.Transform(point, this.TransformMatrix);
                int x = (int)Math.Ceiling(transformedpoint.X);
                int y = (int)Math.Ceiling(transformedpoint.Y);

                return new PointL(x, y);
            }
        }
        #endregion

        #region 只读属性 - 右下顶点坐标 —— PointL BottomRight
        /// <summary>
        /// 只读属性 - 右下顶点坐标
        /// </summary>
        /// <remarks>C点</remarks>
        public PointL BottomRight
        {
            get
            {
                Vector2 point = new Vector2(this.X + this.Width, this.Y + this.Height);
                Vector2 transformedpoint = Vector2.Transform(point, this.TransformMatrix);
                int x = (int)Math.Ceiling(transformedpoint.X);
                int y = (int)Math.Ceiling(transformedpoint.Y);

                return new PointL(x, y);
            }
        }
        #endregion

        #region 只读属性 - 变换矩阵 —— Matrix3x2 TransformMatrix
        /// <summary>
        /// 只读属性 - 变换矩阵
        /// </summary>
        public Matrix3x2 TransformMatrix
        {
            get
            {
                float radians = (float)(Math.PI / 180f * this.Angle);
                float centerX = this.X + this.Width / 2.0f;
                float centerY = this.Y + this.Height / 2.0f;
                Matrix3x2 matrix = Matrix3x2.CreateRotation(radians, new Vector2(centerX, centerY));

                return matrix;
            }
        }
        #endregion

        #region 只读属性 - 空旋转矩形 —— static RotatedRectangleL Empty
        /// <summary>
        /// 只读属性 - 空旋转矩形
        /// </summary>
        public static RotatedRectangleL Empty
        {
            get => new RotatedRectangleL(0, 0, 0, 0, 0);
        }
        #endregion

        #endregion

        #region # 方法

        #region 是否相等 —— override bool Equals(object instance)
        /// <summary>
        /// 是否相等
        /// </summary>
        public override bool Equals(object instance)
        {
            if (instance is RotatedRectangleL rectangle)
            {
                return rectangle == this;
            }

            return false;
        }
        #endregion

        #region 获取哈希码 —— override int GetHashCode()
        /// <summary>
        /// 获取哈希码
        /// </summary>
        public override int GetHashCode()
        {
            return this.X.GetHashCode() ^ this.Y.GetHashCode() ^ this.Width.GetHashCode() ^ this.Height.GetHashCode() ^ this.Angle.GetHashCode();
        }
        #endregion

        #region 转换字符串 —— override string ToString()
        /// <summary>
        /// 转换字符串
        /// </summary>
        public override string ToString()
        {
            string rectangle = $"A({this.X},{this.Y})|{this.Width}*{this.Height}|{this.Angle:F2}";

            return rectangle;
        }
        #endregion

        #region 比较两个旋转矩形是否相等 —— static bool operator ==(RotatedRectangleL source, RotatedRectangleL target)
        /// <summary>
        /// 比较两个旋转矩形是否相等
        /// </summary>
        /// <param name="source">源旋转矩形</param>
        /// <param name="target">目标旋转矩形</param>
        /// <returns>是否相等</returns>
        public static bool operator ==(RotatedRectangleL source, RotatedRectangleL target)
        {
            if (source is null && target is null)
            {
                return true;
            }
            if (source is null)
            {
                return false;
            }
            if (target is null)
            {
                return false;
            }

            return source.X == target.X &&
                   source.Y == target.Y &&
                   source.Width == target.Width &&
                   source.Height == target.Height &&
                   source.Angle.Equals(target.Angle);
        }
        #endregion

        #region 比较两个旋转矩形是否不等 —— static bool operator !=(RotatedRectangleL source, RotatedRectangleL target)
        /// <summary>
        /// 比较两个旋转矩形是否不等
        /// </summary>
        /// <param name="source">源旋转矩形</param>
        /// <param name="target">目标旋转矩形</param>
        /// <returns>是否不等</returns>
        public static bool operator !=(RotatedRectangleL source, RotatedRectangleL target)
        {
            return !(source == target);
        }
        #endregion

        #endregion
    }
}
