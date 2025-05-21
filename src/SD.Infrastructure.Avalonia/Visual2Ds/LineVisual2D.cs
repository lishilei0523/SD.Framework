using Avalonia;
using Avalonia.Media;
using SD.Avalonia.Controls.Shapes;

namespace SD.Infrastructure.Avalonia.Visual2Ds
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
        public static readonly StyledProperty<Point> StartPointProperty;

        /// <summary>
        /// 终止点依赖属性
        /// </summary>
        public static readonly StyledProperty<Point> EndPointProperty;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static LineVisual2D()
        {
            EndPointProperty = AvaloniaProperty.Register<LineVisual2D, Point>(nameof(EndPoint));
            StartPointProperty = AvaloniaProperty.Register<LineVisual2D, Point>(nameof(StartPoint));
            AffectsGeometry<LineVisual2D>(StartPointProperty, EndPointProperty);
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
            get => this.GetValue(StartPointProperty);
            set => this.SetValue(StartPointProperty, value);
        }
        #endregion

        #region 依赖属性 - 终止点 —— Point EndPoint
        /// <summary>
        /// 依赖属性 - 终止点
        /// </summary>
        public Point EndPoint
        {
            get => this.GetValue(EndPointProperty);
            set => this.SetValue(EndPointProperty, value);
        }
        #endregion

        #endregion

        #region # 方法

        #region 创建几何图形 —— override Geometry CreateDefiningGeometry()
        /// <summary>
        /// 创建几何图形
        /// </summary>
        /// <returns>几何图形</returns>
        protected override Geometry CreateDefiningGeometry()
        {
            LineGeometry lineGeometry = new LineGeometry(this.StartPoint, this.EndPoint);

            return lineGeometry;
        }
        #endregion

        #endregion
    }
}
