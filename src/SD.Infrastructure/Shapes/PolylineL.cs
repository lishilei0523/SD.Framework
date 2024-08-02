using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace SD.Infrastructure.Shapes
{
    /// <summary>
    /// 折线段
    /// </summary>
    public class PolylineL : ShapeL
    {
        #region # 构造器

        #region 00.无参构造器
        /// <summary>
        /// 无参构造器
        /// </summary>
        public PolylineL()
        {
            //默认值
            this.Points = new HashSet<Point>();
        }
        #endregion

        #region 01.创建折线段构造器
        /// <summary>
        /// 创建折线段构造器
        /// </summary>
        /// <param name="points">点坐标集</param>
        public PolylineL(IEnumerable<Point> points)
            : this()
        {
            points = points?.ToArray() ?? new Point[0];
            foreach (Point point in points)
            {
                this.Points.Add(point);
            }
        }
        #endregion

        #region 02.创建折线段构造器
        /// <summary>
        /// 创建折线段构造器
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="points">点坐标集</param>
        public PolylineL(string name, IEnumerable<Point> points)
            : this(points)
        {
            base.Name = name;
        }
        #endregion

        #endregion

        #region # 属性

        #region 点坐标列表 —— ICollection<Point> Points
        /// <summary>
        /// 点坐标列表
        /// </summary>
        public ICollection<Point> Points { get; set; }
        #endregion

        #region 只读属性 - 空折线段 —— static PolylineL Empty
        /// <summary>
        /// 只读属性 - 空折线段
        /// </summary>
        public static PolylineL Empty
        {
            get => new PolylineL();
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
            if (instance is PolylineL polygon)
            {
                return polygon == this;
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
            int hashCode = 0;
            foreach (Point point in this.Points)
            {
                hashCode = hashCode ^ point.X ^ point.Y;
            }

            return hashCode;
        }
        #endregion

        #region 转换字符串 —— override string ToString()
        /// <summary>
        /// 转换字符串
        /// </summary>
        public override string ToString()
        {
            StringBuilder locationPoints = new StringBuilder();
            foreach (Point point in this.Points)
            {
                locationPoints.Append($"({point.X},{point.Y})");
                locationPoints.Append(",");
            }

            return locationPoints.ToString().Substring(0, locationPoints.Length - 1);
        }
        #endregion

        #region 比较两个折线段是否相等 —— static bool operator ==(PolylineL source, PolylineL target)
        /// <summary>
        /// 比较两个折线段是否相等
        /// </summary>
        /// <param name="source">源折线段</param>
        /// <param name="target">目标折线段</param>
        /// <returns>是否相等</returns>
        public static bool operator ==(PolylineL source, PolylineL target)
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
            if (!source.Points.Any() && !target.Points.Any())
            {
                return true;
            }
            if (source.Points.Count != target.Points.Count)
            {
                return false;
            }
            if (source.Points.Except(target.Points).Any())
            {
                return false;
            }

            return true;
        }
        #endregion

        #region 比较两个折线段是否不等 —— static bool operator !=(PolylineL source, PolylineL target)
        /// <summary>
        /// 比较两个折线段是否不等
        /// </summary>
        /// <param name="source">源折线段</param>
        /// <param name="target">目标折线段</param>
        /// <returns>是否不等</returns>
        public static bool operator !=(PolylineL source, PolylineL target)
        {
            return !(source == target);
        }
        #endregion

        #endregion
    }
}
