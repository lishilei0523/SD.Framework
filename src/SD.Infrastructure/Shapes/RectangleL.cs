using System.Drawing;

namespace SD.Infrastructure.Shapes
{
    /// <summary>
    /// 矩形
    /// </summary>
    public class RectangleL
    {
        #region # 构造器

        #region 00.无参构造器
        /// <summary>
        /// 无参构造器
        /// </summary>
        public RectangleL()
        {
            //默认值
            this.Fill = Color.Transparent;
            this.Stroke = Color.Red;
            this.StrokeThickness = 2;
            this.Tag = null;
        }
        #endregion

        #region 01.创建矩形构造器
        /// <summary>
        /// 创建矩形构造器
        /// </summary>
        /// <param name="x">顶点横坐标值</param>
        /// <param name="y">顶点纵坐标值</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        public RectangleL(int x, int y, int width, int height)
            : this()
        {
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
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

        #region 只读属性 - 空矩形 —— static RectangleL Empty
        /// <summary>
        /// 只读属性 - 空矩形
        /// </summary>
        public static RectangleL Empty
        {
            get => new RectangleL(0, 0, 0, 0);
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
            if (instance is RectangleL rectangle)
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
            double ax = this.TopLeft.X;
            double ay = this.TopLeft.Y;
            double bx = this.BottomLeft.X;
            double by = this.BottomLeft.Y;
            double cx = this.BottomRight.X;
            double cy = this.BottomRight.Y;
            double dx = this.TopRight.X;
            double dy = this.TopRight.Y;

            string locationPoints = $"A({ax},{ay}), B({bx},{by}), C({cx},{cy}), D({dx},{dy})";

            return locationPoints;
        }
        #endregion

        #region 比较两个矩形是否相等 —— static bool operator ==(RectangleL source, RectangleL target)
        /// <summary>
        /// 比较两个矩形是否相等
        /// </summary>
        /// <param name="source">源矩形</param>
        /// <param name="target">目标矩形</param>
        /// <returns>是否相等</returns>
        public static bool operator ==(RectangleL source, RectangleL target)
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

        #region 比较两个矩形是否不等 —— static bool operator !=(RectangleL source, RectangleL target)
        /// <summary>
        /// 比较两个矩形是否不等
        /// </summary>
        /// <param name="source">源矩形</param>
        /// <param name="target">目标矩形</param>
        /// <returns>是否不等</returns>
        public static bool operator !=(RectangleL source, RectangleL target)
        {
            return !(source == target);
        }
        #endregion

        #endregion
    }
}
