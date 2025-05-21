using Avalonia;
using Avalonia.Media;
using SD.Avalonia.Controls.Shapes;
using System.Globalization;

namespace SD.Infrastructure.Avalonia.Visual2Ds
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
        public static readonly StyledProperty<double> XProperty;

        /// <summary>
        /// Y坐标依赖属性
        /// </summary>
        public static readonly StyledProperty<double> YProperty;

        /// <summary>
        /// 文本依赖属性
        /// </summary>
        public static readonly StyledProperty<string> TextProperty;

        /// <summary>
        /// 字号依赖属性
        /// </summary>
        public static readonly StyledProperty<double> FontSizeProperty;

        /// <summary>
        /// 字体依赖属性
        /// </summary>
        public static readonly StyledProperty<FontFamily> FontFamilyProperty;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static TextVisual2D()
        {
            XProperty = AvaloniaProperty.Register<TextVisual2D, double>(nameof(X));
            YProperty = AvaloniaProperty.Register<TextVisual2D, double>(nameof(Y));
            TextProperty = AvaloniaProperty.Register<TextVisual2D, string>(nameof(Text));
            FontSizeProperty = AvaloniaProperty.Register<TextVisual2D, double>(nameof(FontSize), DefaultFontSize);
            FontFamilyProperty = AvaloniaProperty.Register<TextVisual2D, FontFamily>(nameof(FontFamily), new FontFamily(DefaultFontFamily));
            AffectsGeometry<TextVisual2D>(TextProperty, FontSizeProperty, FontFamilyProperty);
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
            get => this.GetValue(XProperty);
            set => this.SetValue(XProperty, value);
        }
        #endregion

        #region 依赖属性 - Y坐标 —— double Y
        /// <summary>
        /// 依赖属性 - Y坐标
        /// </summary>
        public double Y
        {
            get => this.GetValue(YProperty);
            set => this.SetValue(YProperty, value);
        }
        #endregion

        #region 依赖属性 - 文本 —— string Text
        /// <summary>
        /// 依赖属性 - 文本
        /// </summary>
        public string Text
        {
            get => this.GetValue(TextProperty);
            set => this.SetValue(TextProperty, value);
        }
        #endregion

        #region 依赖属性 - 字号 —— double FontSize
        /// <summary>
        /// 依赖属性 - 字号
        /// </summary>
        public double FontSize
        {
            get => this.GetValue(FontSizeProperty);
            set => this.SetValue(FontSizeProperty, value);
        }
        #endregion

        #region 依赖属性 - 字体 —— FontFamily FontFamily
        /// <summary>
        /// 依赖属性 - 字体
        /// </summary>
        public FontFamily FontFamily
        {
            get => this.GetValue(FontFamilyProperty);
            set => this.SetValue(FontFamilyProperty, value);
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
            Typeface typeface = new Typeface(this.FontFamily, FontStyle.Normal, FontWeight.Thin, FontStretch.Normal);
            FormattedText formattedText = new FormattedText(this.Text, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, typeface, this.FontSize, base.Stroke);
            Point origin = new Point(this.X, this.Y);
            Geometry textGeometry = formattedText.BuildGeometry(origin);

            return textGeometry;
        }
        #endregion

        #endregion
    }
}
