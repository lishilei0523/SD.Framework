using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SD.Infrastructure.WPF.Visual2Ds
{
    /// <summary>
    /// 圆形
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
        public static readonly DependencyProperty CenterProperty;

        /// <summary>
        /// 半径依赖属性
        /// </summary>
        public static readonly DependencyProperty RadiusProperty;

        /// <summary>
        /// 显示圆心依赖属性
        /// </summary>
        public static readonly DependencyProperty ShowCenterProperty;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static CircleVisual2D()
        {
            CenterProperty = DependencyProperty.Register(nameof(Center), typeof(Point), typeof(CircleVisual2D), new FrameworkPropertyMetadata(new Point(0, 0), FrameworkPropertyMetadataOptions.AffectsRender));
            RadiusProperty = DependencyProperty.Register(nameof(Radius), typeof(double), typeof(CircleVisual2D), new FrameworkPropertyMetadata(50.0d, FrameworkPropertyMetadataOptions.AffectsRender));
            ShowCenterProperty = DependencyProperty.Register(nameof(ShowCenter), typeof(bool), typeof(CircleVisual2D), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));
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
            get => (Point)this.GetValue(CenterProperty);
            set => this.SetValue(CenterProperty, value);
        }
        #endregion

        #region 依赖属性 - 半径 —— double Radius
        /// <summary>
        /// 依赖属性 - 半径
        /// </summary>
        public double Radius
        {
            get => (double)this.GetValue(RadiusProperty);
            set => this.SetValue(RadiusProperty, value);
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
                EllipseGeometry circleGeometry = new EllipseGeometry(this.Center, this.Radius, this.Radius);

                return circleGeometry;
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
