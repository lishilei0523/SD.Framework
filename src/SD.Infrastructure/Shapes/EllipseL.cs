﻿using System.Drawing;

namespace SD.Infrastructure.Shapes
{
    /// <summary>
    /// 椭圆形
    /// </summary>
    public class EllipseL : ShapeL
    {
        #region # 构造器

        #region 00.无参构造器
        /// <summary>
        /// 无参构造器
        /// </summary>
        public EllipseL() { }
        #endregion

        #region 01.创建椭圆形构造器
        /// <summary>
        /// 创建椭圆形构造器
        /// </summary>
        /// <param name="x">顶点横坐标值</param>
        /// <param name="y">顶点纵坐标值</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        public EllipseL(int x, int y, int width, int height)
            : this()
        {
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
        }
        #endregion

        #region 02.创建椭圆形构造器
        /// <summary>
        /// 创建椭圆形构造器
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="x">顶点横坐标值</param>
        /// <param name="y">顶点纵坐标值</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        public EllipseL(string name, int x, int y, int width, int height)
            : this(x, y, width, height)
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
        public int X { get; set; }
        #endregion

        #region 顶点纵坐标值 —— int Y
        /// <summary>
        /// 顶点纵坐标值
        /// </summary>
        public int Y { get; set; }
        #endregion

        #region 宽度 —— int Width
        /// <summary>
        /// 宽度
        /// </summary>
        public int Width { get; set; }
        #endregion

        #region 高度 —— int Height
        /// <summary>
        /// 高度
        /// </summary>
        public int Height { get; set; }
        #endregion

        #region 只读属性 - 左上顶点坐标 —— Point TopLeft
        /// <summary>
        /// 只读属性 - 左上顶点坐标
        /// </summary>
        /// <remarks>A点</remarks>
        public Point TopLeft
        {
            get => new Point(this.X, this.Y);
        }
        #endregion

        #region 只读属性 - 右上顶点坐标 —— Point TopRight
        /// <summary>
        /// 只读属性 - 右上顶点坐标
        /// </summary>
        /// <remarks>D点</remarks>
        public Point TopRight
        {
            get => new Point(this.X + this.Width, this.Y);
        }
        #endregion

        #region 只读属性 - 左下顶点坐标 —— Point BottomLeft
        /// <summary>
        /// 只读属性 - 左下顶点坐标
        /// </summary>
        /// <remarks>B点</remarks>
        public Point BottomLeft
        {
            get => new Point(this.X, this.Y + this.Height);
        }
        #endregion

        #region 只读属性 - 右下顶点坐标 —— Point BottomRight
        /// <summary>
        /// 只读属性 - 右下顶点坐标
        /// </summary>
        /// <remarks>C点</remarks>
        public Point BottomRight
        {
            get => new Point(this.X + this.Width, this.Y + this.Height);
        }
        #endregion

        #region 只读属性 - 空椭圆形 —— static EllipseL Empty
        /// <summary>
        /// 只读属性 - 空椭圆形
        /// </summary>
        public static EllipseL Empty
        {
            get => new EllipseL(0, 0, 0, 0);
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
            if (instance is EllipseL ellipse)
            {
                return ellipse == this;
            }

            return false;
        }
        #endregion

        #region 获取哈希码 —— override int GetHashCode()
        /// <summary>
        /// 获取哈希码
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this.X.GetHashCode() ^ this.Y.GetHashCode() ^ this.Width.GetHashCode() ^ this.Height.GetHashCode();
        }
        #endregion

        #region 转换字符串 —— override string ToString()
        /// <summary>
        /// 转换字符串
        /// </summary>
        public override string ToString()
        {
            string locationPoints = $"Ø({this.X},{this.Y})|{this.Width}*{this.Height}";

            return locationPoints;
        }
        #endregion

        #region 比较两个椭圆形是否相等 —— static bool operator ==(EllipseL source, EllipseL target)
        /// <summary>
        /// 比较两个椭圆形是否相等
        /// </summary>
        /// <param name="source">源椭圆形</param>
        /// <param name="target">目标椭圆形</param>
        /// <returns>是否相等</returns>
        public static bool operator ==(EllipseL source, EllipseL target)
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
                   source.Height == target.Height;
        }
        #endregion

        #region 比较两个椭圆形是否不等 —— static bool operator !=(EllipseL source, EllipseL target)
        /// <summary>
        /// 比较两个椭圆形是否不等
        /// </summary>
        /// <param name="source">源椭圆形</param>
        /// <param name="target">目标椭圆形</param>
        /// <returns>是否不等</returns>
        public static bool operator !=(EllipseL source, EllipseL target)
        {
            return !(source == target);
        }
        #endregion

        #endregion
    }
}
