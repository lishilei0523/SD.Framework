using Avalonia;
using Avalonia.Media;
using SD.Avalonia.Controls.Shapes;
using System.Globalization;

namespace SD.Infrastructure.Avalonia.Visual2Ds
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
        public const double DefaultStrokeThickness = 2.0d;

        /// <summary>
        /// X坐标依赖属性
        /// </summary>
        public static readonly StyledProperty<double> XProperty;

        /// <summary>
        /// Y坐标依赖属性
        /// </summary>
        public static readonly StyledProperty<double> YProperty;

        /// <summary>
        /// 厚度依赖属性
        /// </summary>
        public static readonly StyledProperty<double> ThicknessProperty;

        /// <summary>
        /// 标签依赖属性
        /// </summary>
        public static readonly StyledProperty<string> LabelProperty;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static PointVisual2D()
        {
            XProperty = AvaloniaProperty.Register<PointVisual2D, double>(nameof(X));
            YProperty = AvaloniaProperty.Register<PointVisual2D, double>(nameof(Y));
            ThicknessProperty = AvaloniaProperty.Register<PointVisual2D, double>(nameof(Thickness), DefaultThickness);
            LabelProperty = AvaloniaProperty.Register<PointVisual2D, string>(nameof(Label));
            AffectsGeometry<PointVisual2D>(ThicknessProperty);
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

        #endregion

        #region # 方法

        #region 创建几何图形 —— override Geometry CreateDefiningGeometry()
        /// <summary>
        /// 创建几何图形
        /// </summary>
        /// <returns>几何图形</returns>
        protected override Geometry CreateDefiningGeometry()
        {
            Point point = new Point(this.X, this.Y);
            double radius = this.Thickness / 2;
            EllipseGeometry circleGeometry = new EllipseGeometry
            {
                Center = point,
                RadiusX = radius,
                RadiusY = radius
            };

            return circleGeometry;
        }
        #endregion

        #region 渲染事件 —— override void OnRender(DrawingContext drawingContext)
        /// <summary>
        ///  渲染事件
        /// </summary>
        public override void Render(DrawingContext drawingContext)
        {
            base.Render(drawingContext);

            if (!string.IsNullOrWhiteSpace(this.Label))
            {
                //绘制文本
                const int fontSize = 10;
                FontFamily fontFamily = new FontFamily("Times New Roman,SimSun");
                Typeface typeface = new Typeface(fontFamily, FontStyle.Normal, FontWeight.Thin, FontStretch.Normal);
                FormattedText formattedText = new FormattedText(this.Label, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, typeface, fontSize, base.Stroke);
                Point origin = new Point(this.X + base.StrokeThickness, this.Y + base.StrokeThickness);
                Geometry textGeometry = formattedText.BuildGeometry(origin);

                Pen pen = new Pen(base.Stroke, 0.1);
                drawingContext.DrawGeometry(base.Stroke, pen, textGeometry!);
            }
        }
        #endregion

        #endregion
    }
}
