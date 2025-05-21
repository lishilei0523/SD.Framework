using Avalonia;
using Avalonia.Media;
using SD.Avalonia.Controls.Shapes;

namespace SD.Infrastructure.Avalonia.Visual2Ds
{
    /// <summary>
    /// 圆
    /// </summary>
    public class CircleVisual2D : Shape
    {
        #region # 字段及构造器

        /// <summary>
        /// 默认边框厚度
        /// </summary>
        public const double DefaultStrokeThickness = 2.0d;

        /// <summary>
        /// 圆心依赖属性
        /// </summary>
        public static readonly StyledProperty<Point> CenterProperty;

        /// <summary>
        /// 半径依赖属性
        /// </summary>
        public static readonly StyledProperty<double> RadiusProperty;

        /// <summary>
        /// 显示圆心依赖属性
        /// </summary>
        public static readonly StyledProperty<bool> ShowCenterProperty;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static CircleVisual2D()
        {
            CenterProperty = AvaloniaProperty.Register<CircleVisual2D, Point>(nameof(Center));
            RadiusProperty = AvaloniaProperty.Register<CircleVisual2D, double>(nameof(Radius));
            ShowCenterProperty = AvaloniaProperty.Register<CircleVisual2D, bool>(nameof(ShowCenter));
        }

        /// <summary>
        /// 默认构造器
        /// </summary>
        public CircleVisual2D()
        {
            base.Fill = new SolidColorBrush(Colors.Transparent);
            base.Stroke = new SolidColorBrush(Colors.Red);
            base.StrokeThickness = DefaultStrokeThickness;
        }

        #endregion

        #region # 属性

        #region 依赖属性 - 圆心 —— Point Center
        /// <summary>
        /// 依赖属性 - 圆心
        /// </summary>
        public Point Center
        {
            get => this.GetValue(CenterProperty);
            set => this.SetValue(CenterProperty, value);
        }
        #endregion

        #region 依赖属性 - 半径 —— double Radius
        /// <summary>
        /// 依赖属性 - 半径
        /// </summary>
        public double Radius
        {
            get => this.GetValue(RadiusProperty);
            set => this.SetValue(RadiusProperty, value);
        }
        #endregion

        #region 依赖属性 - 显示圆心 —— bool ShowCenter
        /// <summary>
        /// 依赖属性 - 显示圆心
        /// </summary>
        public bool ShowCenter
        {
            get => this.GetValue(ShowCenterProperty);
            set => this.SetValue(ShowCenterProperty, value);
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
            EllipseGeometry circleGeometry = new EllipseGeometry
            {
                Center = this.Center,
                RadiusX = this.Radius,
                RadiusY = this.Radius
            };

            GeometryGroup geometryGroup = new GeometryGroup();
            geometryGroup.Children.Add(circleGeometry);
            if (this.ShowCenter)
            {
                EllipseGeometry centerGeometry = new EllipseGeometry
                {
                    Center = this.Center,
                    RadiusX = base.StrokeThickness / 2,
                    RadiusY = base.StrokeThickness / 2
                };
                geometryGroup.Children.Add(centerGeometry);
            }

            return geometryGroup;
        }
        #endregion

        #endregion
    }
}
