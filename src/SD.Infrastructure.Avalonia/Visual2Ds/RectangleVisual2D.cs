using Avalonia;
using Avalonia.Media;
using SD.Avalonia.Controls.Shapes;
using SD.Infrastructure.Avalonia.Extensions;
using System.Globalization;

namespace SD.Infrastructure.Avalonia.Visual2Ds
{
    /// <summary>
    /// 矩形
    /// </summary>
    public class RectangleVisual2D : Shape
    {
        #region # 字段及构造器

        /// <summary>
        /// 默认边框厚度
        /// </summary>
        public const double DefaultStrokeThickness = 2.0d;

        /// <summary>
        /// 位置依赖属性
        /// </summary>
        /// <remarks>左上点坐标</remarks>
        public static readonly StyledProperty<Point> LocationProperty;

        /// <summary>
        /// 尺寸依赖属性
        /// </summary>
        public static readonly StyledProperty<Size> SizeProperty;

        /// <summary>
        /// 标签依赖属性
        /// </summary>
        public static readonly StyledProperty<string> LabelProperty;

        /// <summary>
        /// 字号依赖属性
        /// </summary>
        public static readonly StyledProperty<double> FontSizeProperty;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static RectangleVisual2D()
        {
            LocationProperty = AvaloniaProperty.Register<RectangleVisual2D, Point>(nameof(Location));
            SizeProperty = AvaloniaProperty.Register<RectangleVisual2D, Size>(nameof(Size), new Size(50, 25));
            LabelProperty = AvaloniaProperty.Register<RectangleVisual2D, string>(nameof(Label));
            FontSizeProperty = AvaloniaProperty.Register<RectangleVisual2D, double>(nameof(FontSize), 14d);
        }

        /// <summary>
        /// 默认构造器
        /// </summary>
        public RectangleVisual2D()
        {
            base.Fill = new SolidColorBrush(Colors.Transparent);
            base.Stroke = new SolidColorBrush(Colors.Red);
            base.StrokeThickness = DefaultStrokeThickness;
        }

        #endregion

        #region # 属性

        #region 依赖属性 - 位置 —— Point Location
        /// <summary>
        /// 依赖属性 - 位置
        /// </summary>
        /// <remarks>左上点坐标</remarks>
        public Point Location
        {
            get => this.GetValue(LocationProperty);
            set => this.SetValue(LocationProperty, value);
        }
        #endregion

        #region 依赖属性 - 尺寸 —— Size Size
        /// <summary>
        /// 依赖属性 - 尺寸
        /// </summary>
        public Size Size
        {
            get => this.GetValue(SizeProperty);
            set => this.SetValue(SizeProperty, value);
        }
        #endregion

        #region 依赖属性 - 标签 —— string Label
        /// <summary>
        /// 依赖属性 - 标签
        /// </summary>
        public string Label
        {
            get => this.GetValue(LabelProperty);
            set => this.SetValue(LabelProperty, value);
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

        #endregion

        #region # 方法

        #region 创建几何图形 —— override Geometry CreateDefiningGeometry()
        /// <summary>
        /// 创建几何图形
        /// </summary>
        /// <returns>几何图形</returns>
        protected override Geometry CreateDefiningGeometry()
        {
            Rect rect = new Rect(this.Location, this.Size);
            RectangleGeometry rectangleGeometry = new RectangleGeometry(rect);

            return rectangleGeometry;
        }
        #endregion

        #region 渲染事件 —— override void Render(DrawingContext drawingContext)
        /// <summary>
        ///  渲染事件
        /// </summary>
        public override void Render(DrawingContext drawingContext)
        {
            base.Render(drawingContext);

            if (!string.IsNullOrWhiteSpace(this.Label))
            {
                //定义文本形状
                FontFamily fontFamily = new FontFamily("Times New Roman,SimSun");
                Typeface typeface = new Typeface(fontFamily, FontStyle.Normal, FontWeight.Thin, FontStretch.Normal);
                FormattedText formattedText = new FormattedText(this.Label, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, typeface, this.FontSize, base.Stroke);
                Point origin = new Point(this.Location.X + 1, this.Location.Y - this.FontSize - this.StrokeThickness);
                Geometry textGeometry = formattedText.BuildGeometry(origin);

                //绘制文本背景
                double backgroundXMin = this.Location.X - this.StrokeThickness / 2;
                double backgroundYMin = this.Location.Y - this.FontSize - this.StrokeThickness / 2;
                double backgroundXMax = backgroundXMin + textGeometry!.Bounds.Width + 4;
                double backgroundYMax = this.Location.Y - this.StrokeThickness / 2 + 0.3;
                double backgroundWidth = backgroundXMax - backgroundXMin;
                double backgroundHeight = backgroundYMax - backgroundYMin;
                Rect backgroundBox = new Rect(backgroundXMin, backgroundYMin, backgroundWidth, backgroundHeight);
                Geometry backgroundGeometry = new RectangleGeometry(backgroundBox);
                drawingContext.DrawGeometry(base.Stroke, null, backgroundGeometry);

                //定义文本画刷
                Brush textBrush;
                if (base.Stroke is SolidColorBrush solidColorBrush)
                {
                    Color invertColor = solidColorBrush.Color.Invert();
                    textBrush = new SolidColorBrush(invertColor);
                }
                else
                {
                    textBrush = new SolidColorBrush(Colors.White);
                }

                //绘制文本
                Pen pen = new Pen(base.Stroke, 0.1);
                drawingContext.DrawGeometry(textBrush, pen, textGeometry);
            }
        }
        #endregion

        #endregion
    }
}
