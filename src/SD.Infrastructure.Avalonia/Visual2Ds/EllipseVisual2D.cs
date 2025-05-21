using Avalonia;
using Avalonia.Media;
using SD.Avalonia.Controls.Shapes;

namespace SD.Infrastructure.Avalonia.Visual2Ds
{
    /// <summary>
    /// 椭圆形
    /// </summary>
    public class EllipseVisual2D : Shape
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
        /// X轴半径依赖属性
        /// </summary>
        public static readonly StyledProperty<double> RadiusXProperty;

        /// <summary>
        /// Y轴半径依赖属性
        /// </summary>
        public static readonly StyledProperty<double> RadiusYProperty;

        /// <summary>
        /// 显示圆心依赖属性
        /// </summary>
        public static readonly StyledProperty<bool> ShowCenterProperty;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static EllipseVisual2D()
        {
            CenterProperty = AvaloniaProperty.Register<EllipseVisual2D, Point>(nameof(Center));
            RadiusXProperty = AvaloniaProperty.Register<EllipseVisual2D, double>(nameof(RadiusX));
            RadiusYProperty = AvaloniaProperty.Register<EllipseVisual2D, double>(nameof(RadiusY));
            ShowCenterProperty = AvaloniaProperty.Register<EllipseVisual2D, bool>(nameof(ShowCenter));
        }

        /// <summary>
        /// 默认构造器
        /// </summary>
        public EllipseVisual2D()
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
            get => (Point)this.GetValue(CenterProperty);
            set => this.SetValue(CenterProperty, value);
        }
        #endregion

        #region 依赖属性 - X轴半径 —— double RadiusX
        /// <summary>
        /// 依赖属性 - X轴半径
        /// </summary>
        public double RadiusX
        {
            get => (double)this.GetValue(RadiusXProperty);
            set => this.SetValue(RadiusXProperty, value);
        }
        #endregion

        #region 依赖属性 - Y轴半径 —— double RadiusY
        /// <summary>
        /// 依赖属性 - Y轴半径
        /// </summary>
        public double RadiusY
        {
            get => (double)this.GetValue(RadiusYProperty);
            set => this.SetValue(RadiusYProperty, value);
        }
        #endregion

        #region 依赖属性 - 显示圆心 —— bool ShowCenter
        /// <summary>
        /// 依赖属性 - 显示圆心
        /// </summary>
        public bool ShowCenter
        {
            get => (bool)this.GetValue(ShowCenterProperty);
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
                RadiusX = this.RadiusX,
                RadiusY = this.RadiusY
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
