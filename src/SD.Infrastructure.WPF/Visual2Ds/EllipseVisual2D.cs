using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SD.Infrastructure.WPF.Visual2Ds
{
    /// <summary>
    /// 椭圆形
    /// </summary>
    public class EllipseVisual2D : Shape
    {
        #region # 字段及构造器

        /// <summary>
        /// 圆心依赖属性
        /// </summary>
        public static readonly DependencyProperty CenterProperty;

        /// <summary>
        /// X轴半径依赖属性
        /// </summary>
        public static readonly DependencyProperty RadiusXProperty;

        /// <summary>
        /// Y轴半径依赖属性
        /// </summary>
        public static readonly DependencyProperty RadiusYProperty;

        /// <summary>
        /// 显示圆心依赖属性
        /// </summary>
        public static readonly DependencyProperty ShowCenterProperty;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static EllipseVisual2D()
        {
            CenterProperty = DependencyProperty.Register(nameof(Center), typeof(Point), typeof(EllipseVisual2D), new FrameworkPropertyMetadata(new Point(0, 0), FrameworkPropertyMetadataOptions.AffectsRender));
            RadiusXProperty = DependencyProperty.Register(nameof(RadiusX), typeof(double), typeof(EllipseVisual2D), new FrameworkPropertyMetadata(50.0d, FrameworkPropertyMetadataOptions.AffectsRender));
            RadiusYProperty = DependencyProperty.Register(nameof(RadiusY), typeof(double), typeof(EllipseVisual2D), new FrameworkPropertyMetadata(25.0d, FrameworkPropertyMetadataOptions.AffectsRender));
            ShowCenterProperty = DependencyProperty.Register(nameof(ShowCenter), typeof(bool), typeof(EllipseVisual2D), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));
        }

        /// <summary>
        /// 默认构造器
        /// </summary>
        public EllipseVisual2D()
        {
            base.Fill = new SolidColorBrush(Colors.Transparent);
            base.Stroke = new SolidColorBrush(Colors.Red);
            base.StrokeThickness = 2;
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

        #region 只读属性 - 几何对象 —— override Geometry DefiningGeometry
        /// <summary>
        /// 只读属性 - 几何对象
        /// </summary>
        protected override Geometry DefiningGeometry
        {
            get
            {
                EllipseGeometry ellipseGeometry = new EllipseGeometry(this.Center, this.RadiusX, this.RadiusY);

                return ellipseGeometry;
            }
        }
        #endregion

        #endregion

        #region # 方法

        #region 渲染事件 —— override void OnRender(DrawingContext drawingContext)
        /// <summary>
        ///  渲染事件
        /// </summary>
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            if (this.ShowCenter)
            {
                //绘制圆心
                double thickness = base.StrokeThickness;
                Pen pen = new Pen(base.Stroke, thickness);
                drawingContext.DrawEllipse(base.Stroke, pen, this.Center, thickness, thickness);
            }
        }
        #endregion

        #endregion
    }
}
