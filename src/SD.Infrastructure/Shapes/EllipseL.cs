using System.Drawing;

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
        /// <param name="x">圆心横坐标值</param>
        /// <param name="y">圆心纵坐标值</param>
        /// <param name="radiusX">X轴半径</param>
        /// <param name="radiusY">Y轴半径</param>
        public EllipseL(int x, int y, int radiusX, int radiusY)
            : this()
        {
            this.X = x;
            this.Y = y;
            this.RadiusX = radiusX;
            this.RadiusY = radiusY;
        }
        #endregion

        #region 02.创建椭圆形构造器
        /// <summary>
        /// 创建椭圆形构造器
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="x">圆心横坐标值</param>
        /// <param name="y">圆心纵坐标值</param>
        /// <param name="radiusX">X轴半径</param>
        /// <param name="radiusY">Y轴半径</param>
        public EllipseL(string name, int x, int y, int radiusX, int radiusY)
            : this(x, y, radiusX, radiusY)
        {
            base.Name = name;
        }
        #endregion

        #endregion

        #region # 属性

        #region 圆心横坐标值 —— int X
        /// <summary>
        /// 圆心横坐标值
        /// </summary>
        public int X { get; set; }
        #endregion

        #region 圆心纵坐标值 —— int Y
        /// <summary>
        /// 圆心纵坐标值
        /// </summary>
        public int Y { get; set; }
        #endregion

        #region X轴半径 —— int RadiusX
        /// <summary>
        /// X轴半径
        /// </summary>
        public int RadiusX { get; set; }
        #endregion

        #region Y轴半径 —— int RadiusY
        /// <summary>
        /// Y轴半径
        /// </summary>
        public int RadiusY { get; set; }
        #endregion

        #region 只读属性 - 圆心坐标 —— Point Center
        /// <summary>
        /// 只读属性 - 圆心坐标
        /// </summary>
        public Point Center
        {
            get => new Point(this.X, this.Y);
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
            return this.X.GetHashCode() ^ this.Y.GetHashCode() ^ this.RadiusX.GetHashCode() ^ this.RadiusY.GetHashCode();
        }
        #endregion

        #region 转换字符串 —— override string ToString()
        /// <summary>
        /// 转换字符串
        /// </summary>
        public override string ToString()
        {
            string ellipse = $"O({this.X},{this.Y})|rx={this.RadiusX},ry={this.RadiusY}";

            return ellipse;
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
                   source.RadiusX == target.RadiusX &&
                   source.RadiusY == target.RadiusY;
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
