using System.Drawing;

namespace SD.Infrastructure.Shapes
{
    /// <summary>
    /// 线段
    /// </summary>
    public class LineL : ShapeL
    {
        #region # 构造器

        #region 00.无参构造器
        /// <summary>
        /// 无参构造器
        /// </summary>
        public LineL() { }
        #endregion

        #region 01.创建线段构造器
        /// <summary>
        /// 创建线段构造器
        /// </summary>
        /// <param name="a">点A</param>
        /// <param name="b">点B</param>
        public LineL(Point a, Point b)
            : this()
        {
            this.A = a;
            this.B = b;
        }
        #endregion 

        #region 02.创建线段构造器
        /// <summary>
        /// 创建线段构造器
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="a">点A</param>
        /// <param name="b">点B</param>
        public LineL(string name, Point a, Point b)
            : this(a, b)
        {
            base.Name = name;
        }
        #endregion 

        #endregion

        #region # 属性

        #region 点B —— Point A
        /// <summary>
        /// 点A
        /// </summary>
        public Point A { get; set; }
        #endregion

        #region 点B —— Point B
        /// <summary>
        /// 点B
        /// </summary>
        public Point B { get; set; }
        #endregion

        #region 只读属性 - 空线段 —— static LineL Empty
        /// <summary>
        /// 只读属性 - 空线段
        /// </summary>
        public static LineL Empty
        {
            get => new LineL(Point.Empty, Point.Empty);
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
            if (instance is LineL line)
            {
                return line == this;
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
            return this.A.X.GetHashCode() ^ this.A.Y.GetHashCode() ^ this.B.X.GetHashCode() ^ this.B.Y.GetHashCode();
        }
        #endregion

        #region 转换字符串 —— override string ToString()
        /// <summary>
        /// 转换字符串
        /// </summary>
        public override string ToString()
        {
            string line = $"A({this.A.X},{this.A.Y})|B({this.B.X},{this.B.Y})";

            return line;
        }
        #endregion

        #region 比较两个线段是否相等 —— static bool operator ==(LineL source, LineL target)
        /// <summary>
        /// 比较两个线段是否相等
        /// </summary>
        /// <param name="source">源线段</param>
        /// <param name="target">目标线段</param>
        /// <returns>是否相等</returns>
        public static bool operator ==(LineL source, LineL target)
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

            return source.A.X == target.A.X &&
                   source.A.Y == target.A.Y &&
                   source.B.X == target.B.X &&
                   source.B.Y == target.B.Y;
        }
        #endregion

        #region 比较两个线段是否不等 —— static bool operator !=(LineL source, LineL target)
        /// <summary>
        /// 比较两个线段是否不等
        /// </summary>
        /// <param name="source">源线段</param>
        /// <param name="target">目标线段</param>
        /// <returns>是否不等</returns>
        public static bool operator !=(LineL source, LineL target)
        {
            return !(source == target);
        }
        #endregion

        #endregion
    }
}
