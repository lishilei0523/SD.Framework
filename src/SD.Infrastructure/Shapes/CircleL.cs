namespace SD.Infrastructure.Shapes
{
    /// <summary>
    /// 圆形
    /// </summary>
    public class CircleL : ShapeL
    {
        #region # 构造器

        #region 00.无参构造器
        /// <summary>
        /// 无参构造器
        /// </summary>
        public CircleL() { }
        #endregion

        #region 01.创建圆形构造器
        /// <summary>
        /// 创建圆形构造器
        /// </summary>
        /// <param name="x">圆心横坐标值</param>
        /// <param name="y">圆心纵坐标值</param>
        /// <param name="radius">半径</param>
        public CircleL(int x, int y, int radius)
            : this()
        {
            this.X = x;
            this.Y = y;
            this.Radius = radius;
        }
        #endregion 

        #region 02.创建圆形构造器
        /// <summary>
        /// 创建圆形构造器
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="x">圆心横坐标值</param>
        /// <param name="y">圆心纵坐标值</param>
        /// <param name="radius">半径</param>
        public CircleL(string name, int x, int y, int radius)
            : this(x, y, radius)
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

        #region 半径 —— int Radius
        /// <summary>
        /// 半径
        /// </summary>
        public int Radius { get; set; }
        #endregion

        #region 只读属性 - 圆心坐标 —— PointL Center
        /// <summary>
        /// 只读属性 - 圆心坐标
        /// </summary>
        public PointL Center
        {
            get => new PointL(this.X, this.Y);
        }
        #endregion

        #region 只读属性 - 空圆形 —— static CircleL Empty
        /// <summary>
        /// 只读属性 - 空圆形
        /// </summary>
        public static CircleL Empty
        {
            get => new CircleL(0, 0, 0);
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
            if (instance is CircleL circle)
            {
                return circle == this;
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
            return this.X.GetHashCode() ^ this.Y.GetHashCode() ^ this.Radius.GetHashCode();
        }
        #endregion

        #region 转换字符串 —— override string ToString()
        /// <summary>
        /// 转换字符串
        /// </summary>
        public override string ToString()
        {
            string circle = $"O({this.X},{this.Y})|r={this.Radius}";

            return circle;
        }
        #endregion

        #region 比较两个圆形是否相等 —— static bool operator ==(CircleL source, CircleL target)
        /// <summary>
        /// 比较两个圆形是否相等
        /// </summary>
        /// <param name="source">源圆形</param>
        /// <param name="target">目标圆形</param>
        /// <returns>是否相等</returns>
        public static bool operator ==(CircleL source, CircleL target)
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
                   source.Radius == target.Radius;
        }
        #endregion

        #region 比较两个圆形是否不等 —— static bool operator !=(CircleL source, CircleL target)
        /// <summary>
        /// 比较两个圆形是否不等
        /// </summary>
        /// <param name="source">源圆形</param>
        /// <param name="target">目标圆形</param>
        /// <returns>是否不等</returns>
        public static bool operator !=(CircleL source, CircleL target)
        {
            return !(source == target);
        }
        #endregion

        #endregion
    }
}
