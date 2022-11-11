using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace SD.Infrastructure.Shapes
{
    /// <summary>
    /// 多边形
    /// </summary>
    public class PolygonL
    {
        #region # 构造器

        #region 00.无参构造器
        /// <summary>
        /// 无参构造器
        /// </summary>
        public PolygonL()
        {
            //默认值
            this.Fill = Color.Transparent;
            this.Stroke = Color.Red;
            this.StrokeThickness = 2;
            this.Tag = null;
            this.Points = new HashSet<Point>();
        }
        #endregion

        #region 01.创建多边形构造器
        /// <summary>
        /// 创建多边形构造器
        /// </summary>
        /// <param name="points">点坐标集</param>
        public PolygonL(IEnumerable<Point> points)
            : this()
        {
            points = points?.ToArray() ?? new Point[0];
            foreach (Point point in points)
            {
                this.Points.Add(point);
            }
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

        #region 层次索引 —— int ZIndex
        /// <summary>
        /// 层次索引
        /// </summary>
        public int ZIndex { get; set; }
        #endregion

        #region 填充颜色 —— Color Fill
        /// <summary>
        /// 填充颜色
        /// </summary>
        public Color Fill { get; set; }
        #endregion

        #region 边框颜色 —— Color Stroke
        /// <summary>
        /// 边框颜色
        /// </summary>
        public Color Stroke { get; set; }
        #endregion

        #region 边框宽度 —— int StrokeThickness
        /// <summary>
        /// 边框宽度
        /// </summary>
        public int StrokeThickness { get; set; }
        #endregion

        #region 自定义标签 —— object Tag
        /// <summary>
        /// 自定义标签
        /// </summary>
        public object Tag { get; set; }
        #endregion

        #region 只读属性 - 空多边形 —— static PolygonL Empty
        /// <summary>
        /// 只读属性 - 空多边形
        /// </summary>
        public static PolygonL Empty
        {
            get => new PolygonL();
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
            if (instance is PolygonL polygon)
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

        #region 比较两个多边形是否相等 —— static bool operator ==(PolygonL source, PolygonL target)
        /// <summary>
        /// 比较两个多边形是否相等
        /// </summary>
        /// <param name="source">源矩形</param>
        /// <param name="target">目标矩形</param>
        /// <returns>是否相等</returns>
        public static bool operator ==(PolygonL source, PolygonL target)
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

        #region 比较两个多边形是否不等 —— static bool operator !=(PolygonL source, PolygonL target)
        /// <summary>
        /// 比较两个多边形是否不等
        /// </summary>
        /// <param name="source">源矩形</param>
        /// <param name="target">目标矩形</param>
        /// <returns>是否不等</returns>
        public static bool operator !=(PolygonL source, PolygonL target)
        {
            return !(source == target);
        }
        #endregion

        #endregion
    }
}
