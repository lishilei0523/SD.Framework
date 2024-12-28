using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SD.Infrastructure.WPF.Visual2Ds
{
    /// <summary>
    /// 线段
    /// </summary>
    public class LineVisual2D : Shape
    {
        #region # 字段及构造器

        /// <summary>
        /// 默认边框厚度
        /// </summary>
        public const double DefaultStrokeThickness = 2.0d;

        /// <summary>
        /// 起始点依赖属性
        /// </summary>
        public static readonly DependencyProperty StartPointProperty;

        /// <summary>
        /// 终止点依赖属性
        /// </summary>
        public static readonly DependencyProperty EndPointProperty;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static LineVisual2D()
        {
            StartPointProperty = DependencyProperty.Register(nameof(StartPoint), typeof(Point), typeof(RectangleVisual2D), new FrameworkPropertyMetadata(new Point(0, 0), FrameworkPropertyMetadataOptions.AffectsRender));
            EndPointProperty = DependencyProperty.Register(nameof(EndPoint), typeof(Point), typeof(RectangleVisual2D), new FrameworkPropertyMetadata(new Point(10, 10), FrameworkPropertyMetadataOptions.AffectsRender));
        }

        /// <summary>
        /// 默认构造器
        /// </summary>
        public LineVisual2D()
        {
            base.Fill = new SolidColorBrush(Colors.Transparent);
            base.Stroke = new SolidColorBrush(Colors.Red);
            base.StrokeThickness = DefaultStrokeThickness;
        }

        #endregion

        #region # 属性

        #region 依赖属性 - 起始点 —— Point StartPoint
        /// <summary>
        /// 依赖属性 - 起始点
        /// </summary>
        public Point StartPoint
        {
            get => (Point)this.GetValue(StartPointProperty);
            set => this.SetValue(StartPointProperty, value);
        }
        #endregion

        #region 依赖属性 - 终止点 —— Point EndPoint
        /// <summary>
        /// 依赖属性 - 终止点
        /// </summary>
        public Point EndPoint
        {
            get => (Point)this.GetValue(EndPointProperty);
            set => this.SetValue(EndPointProperty, value);
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
                LineGeometry lineGeometry = new LineGeometry(this.StartPoint, this.EndPoint);

                return lineGeometry;
            }
        }
        #endregion

        #endregion
    }
}
