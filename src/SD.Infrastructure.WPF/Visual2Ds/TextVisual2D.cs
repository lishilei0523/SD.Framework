using System.Globalization;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SD.Infrastructure.WPF.Visual2Ds
{
    /// <summary>
    /// 文本
    /// </summary>
    public class TextVisual2D : Shape
    {
        #region # 字段及构造器

        /// <summary>
        /// 默认边框厚度
        /// </summary>
        public const double DefaultStrokeThickness = 0.1d;

        /// <summary>
        /// 默认字号
        /// </summary>
        public const double DefaultFontSize = 14d;

        /// <summary>
        /// 默认字体
        /// </summary>
        public const string DefaultFontFamily = "Times New Roman,SimSun";

        /// <summary>
        /// X坐标依赖属性
        /// </summary>
        public static readonly DependencyProperty XProperty;

        /// <summary>
        /// Y坐标依赖属性
        /// </summary>
        public static readonly DependencyProperty YProperty;

        /// <summary>
        /// 文本依赖属性
        /// </summary>
        public static readonly DependencyProperty TextProperty;

        /// <summary>
        /// 字号依赖属性
        /// </summary>
        public static readonly DependencyProperty FontSizeProperty;

        /// <summary>
        /// 字体依赖属性
        /// </summary>
        public static readonly DependencyProperty FontFamilyProperty;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static TextVisual2D()
        {
            XProperty = DependencyProperty.Register(nameof(X), typeof(double), typeof(TextVisual2D), new FrameworkPropertyMetadata(0.0d, FrameworkPropertyMetadataOptions.AffectsRender));
            YProperty = DependencyProperty.Register(nameof(Y), typeof(double), typeof(TextVisual2D), new FrameworkPropertyMetadata(0.0d, FrameworkPropertyMetadataOptions.AffectsRender));
            TextProperty = DependencyProperty.Register(nameof(Text), typeof(string), typeof(TextVisual2D), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));
            FontSizeProperty = DependencyProperty.Register(nameof(FontSize), typeof(double), typeof(TextVisual2D), new FrameworkPropertyMetadata(DefaultFontSize, FrameworkPropertyMetadataOptions.AffectsRender));
            FontFamilyProperty = DependencyProperty.Register(nameof(FontFamily), typeof(FontFamily), typeof(TextVisual2D), new FrameworkPropertyMetadata(new FontFamily(DefaultFontFamily), FrameworkPropertyMetadataOptions.AffectsRender));
        }

        /// <summary>
        /// 默认构造器
        /// </summary>
        public TextVisual2D()
        {
            base.Fill = new SolidColorBrush(Colors.Red);
            base.Stroke = base.Fill;
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

        #region 依赖属性 - 文本 —— string Text
        /// <summary>
        /// 依赖属性 - 文本
        /// </summary>
        public string Text
        {
            get => (string)this.GetValue(TextProperty);
            set => this.SetValue(TextProperty, value);
        }
        #endregion

        #region 依赖属性 - 字号 —— double FontSize
        /// <summary>
        /// 依赖属性 - 字号
        /// </summary>
        public double FontSize
        {
            get => (double)this.GetValue(FontSizeProperty);
            set => this.SetValue(FontSizeProperty, value);
        }
        #endregion

        #region 依赖属性 - 字体 —— FontFamily FontFamily
        /// <summary>
        /// 依赖属性 - 字体
        /// </summary>
        public FontFamily FontFamily
        {
            get => (FontFamily)this.GetValue(FontFamilyProperty);
            set => this.SetValue(FontFamilyProperty, value);
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
                Typeface typeface = new Typeface(this.FontFamily, FontStyles.Normal, FontWeights.Thin, FontStretches.Normal);
                FormattedText formattedText = new FormattedText(this.Text, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, typeface, this.FontSize, base.Stroke, new NumberSubstitution(), 1.25);
                Point origin = new Point(this.X, this.Y);
                Geometry textGeometry = formattedText.BuildGeometry(origin);

                return textGeometry;
            }
        }
        #endregion

        #endregion
    }
}
