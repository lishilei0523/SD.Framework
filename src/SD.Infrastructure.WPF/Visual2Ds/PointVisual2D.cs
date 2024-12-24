using System.Globalization;
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
        /// 默认厚度
        /// </summary>
        public const double DefaultThickness = 6.0d;

        /// <summary>
        /// 默认边框厚度
        /// </summary>
        public const double DefaultStrokeThickness = 3.0d;

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
        /// 标签依赖属性
        /// </summary>
        public static readonly DependencyProperty LabelProperty;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static PointVisual2D()
        {
            XProperty = DependencyProperty.Register(nameof(X), typeof(double), typeof(PointVisual2D), new FrameworkPropertyMetadata(0.0d, FrameworkPropertyMetadataOptions.AffectsRender));
            YProperty = DependencyProperty.Register(nameof(Y), typeof(double), typeof(PointVisual2D), new FrameworkPropertyMetadata(0.0d, FrameworkPropertyMetadataOptions.AffectsRender));
            ThicknessProperty = DependencyProperty.Register(nameof(Thickness), typeof(double), typeof(PointVisual2D), new FrameworkPropertyMetadata(DefaultThickness, FrameworkPropertyMetadataOptions.AffectsRender));
            LabelProperty = DependencyProperty.Register(nameof(Label), typeof(string), typeof(PointVisual2D), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));
        }

        /// <summary>
        /// 默认构造器
        /// </summary>
        public PointVisual2D()
        {
            base.Fill = new SolidColorBrush(Colors.Black);
            base.Stroke = new SolidColorBrush(Colors.Red);
            base.StrokeThickness = DefaultStrokeThickness;
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

        #region 依赖属性 - 标签 —— string Label
        /// <summary>
        /// 依赖属性 - 标签
        /// </summary>
        public string Label
        {
            get => (string)this.GetValue(LabelProperty);
            set => this.SetValue(LabelProperty, value);
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
                double radius = this.Thickness / 2;
                EllipseGeometry circleGeometry = new EllipseGeometry(point, radius, radius);

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

            if (!string.IsNullOrWhiteSpace(this.Label))
            {
                //绘制文本
                const int fontSize = 10;
                FontFamily fontFamily = new FontFamily("Times New Roman,SimSun");
                Typeface typeface = new Typeface(fontFamily, FontStyles.Normal, FontWeights.Thin, FontStretches.Normal);
                FormattedText formattedText = new FormattedText(this.Label, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, typeface, fontSize, base.Stroke, new NumberSubstitution(), 1.25);
                Point origin = new Point(this.X + base.StrokeThickness, this.Y + base.StrokeThickness);
                Geometry textGeometry = formattedText.BuildGeometry(origin);
                drawingContext.DrawGeometry(base.Stroke, null, textGeometry);
            }
        }
        #endregion

        #endregion
    }
}
