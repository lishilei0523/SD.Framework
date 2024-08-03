using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SD.Infrastructure.WPF.Visual2Ds
{
    /// <summary>
    /// 点
    /// </summary>
    public class PointVisual2D : Shape
    {
        #region # 字段及构造器

        /// <summary>
        /// X坐标依赖属性
        /// </summary>
        public static readonly DependencyProperty XProperty;

        /// <summary>
        /// Y坐标依赖属性
        /// </summary>
        public static readonly DependencyProperty YProperty;

        /// <summary>
        /// 厚度依赖属性
        /// </summary>
        public static readonly DependencyProperty ThicknessProperty;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static PointVisual2D()
        {
            XProperty = DependencyProperty.Register(nameof(X), typeof(double), typeof(PointVisual2D), new FrameworkPropertyMetadata(0.0d, FrameworkPropertyMetadataOptions.AffectsRender));
            YProperty = DependencyProperty.Register(nameof(Y), typeof(double), typeof(PointVisual2D), new FrameworkPropertyMetadata(0.0d, FrameworkPropertyMetadataOptions.AffectsRender));
            ThicknessProperty = DependencyProperty.Register(nameof(Thickness), typeof(double), typeof(PointVisual2D), new FrameworkPropertyMetadata(5.0d, FrameworkPropertyMetadataOptions.AffectsRender));
        }

        /// <summary>
        /// 默认构造器
        /// </summary>
        public PointVisual2D()
        {
            base.Fill = new SolidColorBrush(Colors.Black);
            base.Stroke = new SolidColorBrush(Colors.Red);
            base.StrokeThickness = 3;
        }

        #endregion

        #region # 属性

        #region 依赖属性 - X坐标 —— double X
        /// <summary>
        /// 依赖属性 - X坐标
        /// </summary>
        public double X
        {
            get => (double)this.GetValue(XProperty);
            set => this.SetValue(XProperty, value);
        }
        #endregion

        #region 依赖属性 - Y坐标 —— double Y
        /// <summary>
        /// 依赖属性 - Y坐标
        /// </summary>
        public double Y
        {
            get => (double)this.GetValue(YProperty);
            set => this.SetValue(YProperty, value);
        }
        #endregion

        #region 依赖属性 - 厚度 —— double Thickness
        /// <summary>
        /// 依赖属性 - 厚度
        /// </summary>
        public double Thickness
        {
            get => (double)this.GetValue(ThicknessProperty);
            set => this.SetValue(ThicknessProperty, value);
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
                Point point = new Point(this.X, this.Y);
                EllipseGeometry circleGeometry = new EllipseGeometry(point, this.Thickness, this.Thickness);

                return circleGeometry;
            }
        }
        #endregion

        #endregion
    }
}
