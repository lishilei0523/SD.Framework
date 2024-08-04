namespace SD.Infrastructure.Shapes
{
    /// <summary>
    /// 点
    /// </summary>
    public class PointL : ShapeL
    {
        #region # 构造器

        #region 00.无参构造器
        /// <summary>
        /// 无参构造器
        /// </summary>
        public PointL()
        {
            //默认值
            this.Thickness = 6;
        }
        #endregion

        #region 01.创建点构造器
        /// <summary>
        /// 创建点构造器
        /// </summary>
        /// <param name="x">横坐标值</param>
        /// <param name="y">纵坐标值</param>
        public PointL(int x, int y)
            : this()
        {
            this.X = x;
            this.Y = y;
        }
        #endregion 

        #region 02.创建点构造器
        /// <summary>
        /// 创建点构造器
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="x">横坐标值</param>
        /// <param name="y">纵坐标值</param>
        public PointL(string name, int x, int y)
            : this(x, y)
        {
            base.Name = name;
        }
        #endregion 

        #endregion

        #region # 属性

        #region 横坐标值 —— int X
        /// <summary>
        /// 横坐标值
        /// </summary>
        public int X { get; set; }
        #endregion

        #region 纵坐标值 —— int Y
        /// <summary>
        /// 纵坐标值
        /// </summary>
        public int Y { get; set; }
        #endregion

        #region 厚度 —— int Thickness
        /// <summary>
        /// 厚度
        /// </summary>
        public int Thickness { get; set; }
        #endregion

        #region 只读属性 - 空点 —— static PointL Empty
        /// <summary>
        /// 只读属性 - 空点
        /// </summary>
        public static PointL Empty
        {
            get => new PointL(0, 0);
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
            if (instance is PointL point)
            {
                return point == this;
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
            return this.X.GetHashCode() ^ this.Y.GetHashCode();
        }
        #endregion

        #region 转换字符串 —— override string ToString()
        /// <summary>
        /// 转换字符串
        /// </summary>
        public override string ToString()
        {
            string point = $"({this.X},{this.Y})";

            return point;
        }
        #endregion

        #region 比较两个点是否相等 —— static bool operator ==(PointL source, PointL target)
        /// <summary>
        /// 比较两个点是否相等
        /// </summary>
        /// <param name="source">源点</param>
        /// <param name="target">目标点</param>
        /// <returns>是否相等</returns>
        public static bool operator ==(PointL source, PointL target)
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

            return source.X == target.X && source.Y == target.Y;
        }
        #endregion

        #region 比较两个点是否不等 —— static bool operator !=(PointL source, PointL target)
        /// <summary>
        /// 比较两个点是否不等
        /// </summary>
        /// <param name="source">源点</param>
        /// <param name="target">目标点</param>
        /// <returns>是否不等</returns>
        public static bool operator !=(PointL source, PointL target)
        {
            return !(source == target);
        }
        #endregion

        #endregion
    }
}
