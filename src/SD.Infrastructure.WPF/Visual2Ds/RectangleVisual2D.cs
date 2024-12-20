using SD.Infrastructure.WPF.Extensions;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SD.Infrastructure.WPF.Visual2Ds
{
    /// <summary>
    /// 矩形
    /// </summary>
    public class RectangleVisual2D : Shape
    {
        #region # 字段及构造器

        /// <summary>
        /// 位置依赖属性
        /// </summary>
        /// <remarks>左上点坐标</remarks>
        public static readonly DependencyProperty LocationProperty;

        /// <summary>
        /// 尺寸依赖属性
        /// </summary>
        public static readonly DependencyProperty SizeProperty;

        /// <summary>
        /// 标签依赖属性
        /// </summary>
        public static readonly DependencyProperty LabelProperty;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static RectangleVisual2D()
        {
            LocationProperty = DependencyProperty.Register(nameof(Location), typeof(Point), typeof(RectangleVisual2D), new FrameworkPropertyMetadata(new Point(0, 0), FrameworkPropertyMetadataOptions.AffectsRender));
            SizeProperty = DependencyProperty.Register(nameof(Size), typeof(Size), typeof(RectangleVisual2D), new FrameworkPropertyMetadata(new Size(50, 25), FrameworkPropertyMetadataOptions.AffectsRender));
            LabelProperty = DependencyProperty.Register(nameof(Label), typeof(string), typeof(RectangleVisual2D), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));
        }

        /// <summary>
        /// 默认构造器
        /// </summary>
        public RectangleVisual2D()
        {
            base.Fill = new SolidColorBrush(Colors.Transparent);
            base.Stroke = new SolidColorBrush(Colors.Red);
            base.StrokeThickness = 2;
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
            get => (Point)this.GetValue(LocationProperty);
            set => this.SetValue(LocationProperty, value);
        }
        #endregion

        #region 依赖属性 - 尺寸 —— Size Size
        /// <summary>
        /// 依赖属性 - 尺寸
        /// </summary>
        public Size Size
        {
            get => (Size)this.GetValue(SizeProperty);
            set => this.SetValue(SizeProperty, value);
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
                Rect rect = new Rect(this.Location, this.Size);
                RectangleGeometry rectangleGeometry = new RectangleGeometry(rect);

                return rectangleGeometry;
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
                //定义文本形状
                const int fontSize = 14;
                FontFamily fontFamily = new FontFamily("Times New Roman,SimSun");
                Typeface typeface = new Typeface(fontFamily, FontStyles.Normal, FontWeights.Thin, FontStretches.Normal);
                FormattedText formattedText = new FormattedText(this.Label, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, typeface, fontSize, base.Stroke, new NumberSubstitution(), 1.25);
                Point origin = new Point(this.Location.X, this.Location.Y - fontSize - this.StrokeThickness);
                Geometry textGeometry = formattedText.BuildGeometry(origin);

                //绘制文本背景
                double backgroundX = this.Location.X - this.StrokeThickness / 2;
                double backgroundY = this.Location.Y - fontSize - this.StrokeThickness / 2;
                double backgroundWidth = textGeometry.Bounds.Width + this.StrokeThickness + this.StrokeThickness / 2;
                double backgroundHeight = this.DefiningGeometry.Bounds.Y - textGeometry.Bounds.Y + this.StrokeThickness;
                Rect backgroundBox = new Rect(backgroundX, backgroundY, backgroundWidth, backgroundHeight);
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
                drawingContext.DrawGeometry(textBrush, null, textGeometry);
            }
        }
        #endregion

        #endregion
    }
}
