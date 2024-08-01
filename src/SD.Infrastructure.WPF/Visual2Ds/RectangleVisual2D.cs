using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using Size = System.Windows.Size;

namespace SD.Infrastructure.WPF.Visual2Ds
{
    /// <summary>
    /// 矩形
    /// </summary>
    public class RectangleVisual2D : Shape
    {
        #region # 字段及构造器

        /// <summary>
        /// 位置依赖属性
        /// </summary>
        /// <remarks>左上点坐标</remarks>
        public static readonly DependencyProperty LocationProperty;

        /// <summary>
        /// 尺寸依赖属性
        /// </summary>
        public static readonly DependencyProperty SizeProperty;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static RectangleVisual2D()
        {
            LocationProperty = DependencyProperty.Register(nameof(Location), typeof(Point), typeof(RectangleVisual2D), new FrameworkPropertyMetadata(new Point(0, 0), FrameworkPropertyMetadataOptions.AffectsRender));
            SizeProperty = DependencyProperty.Register(nameof(Size), typeof(Size), typeof(RectangleVisual2D), new FrameworkPropertyMetadata(new Size(0, 0), FrameworkPropertyMetadataOptions.AffectsRender));
        }

        /// <summary>
        /// 默认构造器
        /// </summary>
        public RectangleVisual2D()
        {
            base.Fill = new SolidColorBrush(Colors.Transparent);
            base.Stroke = new SolidColorBrush(Colors.Red);
            base.StrokeThickness = 2;
        }

        #endregion

        #region # 属性

        #region 依赖属性 - 位置 —— Point Location
        /// <summary>
        /// 依赖属性 - 位置
        /// </summary>
        /// <remarks>左上点坐标</remarks>
        public Point Location
        {
            get => (Point)this.GetValue(LocationProperty);
            set => this.SetValue(LocationProperty, value);
        }
        #endregion

        #region 依赖属性 - 尺寸 —— Size Size
        /// <summary>
        /// 依赖属性 - 尺寸
        /// </summary>
        public Size Size
        {
            get => (Size)this.GetValue(SizeProperty);
            set => this.SetValue(SizeProperty, value);
        }
        #endregion

        #region 只读属性 - 几何对象 —— override Geometry DefiningGeometry
        /// <summary>
        /// 只读属性 - 几何对象
        /// </summary>
        protected override Geometry DefiningGeometry
        {
            get
            {
                Rect rect = new Rect(this.Location, this.Size);
                RectangleGeometry rectangleGeometry = new RectangleGeometry(rect);

                return rectangleGeometry;
            }
        }
        #endregion

        #endregion
    }
}
