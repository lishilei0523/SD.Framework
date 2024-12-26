using SD.Infrastructure.WPF.Extensions;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SD.Infrastructure.WPF.Visual2Ds
{
    /// <summary>
    /// 旋转矩形
    /// </summary>
    public class RotatedRectangleVisual2D : Shape
    {
        #region # 字段及构造器

        /// <summary>
        /// 默认边框厚度
        /// </summary>
        public const double DefaultStrokeThickness = 2.0d;

        /// <summary>
        /// 中心依赖属性
        /// </summary>
        /// <remarks>中心点坐标</remarks>
        public static readonly DependencyProperty CenterProperty;

        /// <summary>
        /// 尺寸依赖属性
        /// </summary>
        public static readonly DependencyProperty SizeProperty;

        /// <summary>
        /// 角度依赖属性
        /// </summary>
        public static readonly DependencyProperty AngleProperty;

        /// <summary>
        /// 标签依赖属性
        /// </summary>
        public static readonly DependencyProperty LabelProperty;

        /// <summary>
        /// 字号依赖属性
        /// </summary>
        public static readonly DependencyProperty FontSizeProperty;

        /// <summary>
        /// 显示中心依赖属性
        /// </summary>
        public static readonly DependencyProperty ShowCenterProperty;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static RotatedRectangleVisual2D()
        {
            CenterProperty = DependencyProperty.Register(nameof(Center), typeof(Point), typeof(RotatedRectangleVisual2D), new FrameworkPropertyMetadata(new Point(50, 50), FrameworkPropertyMetadataOptions.AffectsRender));
            SizeProperty = DependencyProperty.Register(nameof(Size), typeof(Size), typeof(RotatedRectangleVisual2D), new FrameworkPropertyMetadata(new Size(50, 25), FrameworkPropertyMetadataOptions.AffectsRender));
            AngleProperty = DependencyProperty.Register(nameof(Angle), typeof(double), typeof(RotatedRectangleVisual2D), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.AffectsRender));
            LabelProperty = DependencyProperty.Register(nameof(Label), typeof(string), typeof(RotatedRectangleVisual2D), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));
            FontSizeProperty = DependencyProperty.Register(nameof(FontSize), typeof(double), typeof(RotatedRectangleVisual2D), new FrameworkPropertyMetadata(14d, FrameworkPropertyMetadataOptions.AffectsRender));
            ShowCenterProperty = DependencyProperty.Register(nameof(ShowCenter), typeof(bool), typeof(RotatedRectangleVisual2D), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsRender));
        }

        /// <summary>
        /// 默认构造器
        /// </summary>
        public RotatedRectangleVisual2D()
        {
            base.Fill = new SolidColorBrush(Colors.Transparent);
            base.Stroke = new SolidColorBrush(Colors.Red);
            base.StrokeThickness = DefaultStrokeThickness;
        }

        #endregion

        #region # 属性

        #region 依赖属性 - 中心 —— Point Center
        /// <summary>
        /// 依赖属性 - 中心
        /// </summary>
        /// <remarks>中心点坐标</remarks>
        public Point Center
        {
            get => (Point)this.GetValue(CenterProperty);
            set => this.SetValue(CenterProperty, value);
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

        #region 依赖属性 - 角度 —— double Angle
        /// <summary>
        /// 依赖属性 - 角度
        /// </summary>
        public double Angle
        {
            get => (double)this.GetValue(AngleProperty);
            set => this.SetValue(AngleProperty, value);
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

        #region 依赖属性 - 显示中心 —— bool ShowCenter
        /// <summary>
        /// 依赖属性 - 显示中心
        /// </summary>
        public bool ShowCenter
        {
            get => (bool)this.GetValue(ShowCenterProperty);
            set => this.SetValue(ShowCenterProperty, value);
        }
        #endregion

        #region 只读属性 - 点A —— Point A
        /// <summary>
        /// 只读属性 - 点A
        /// </summary>
        /// <remarks>左上</remarks>
        public Point A
        {
            get => this.RotateTransform.Transform(this.Location);
        }
        #endregion

        #region 只读属性 - 点B —— Point B
        /// <summary>
        /// 只读属性 - 点B
        /// </summary>
        /// <remarks>左下</remarks>
        public Point B
        {
            get
            {
                Point leftBottom = new Point(this.Location.X, this.Location.Y + this.Size.Height);
                return this.RotateTransform.Transform(leftBottom);
            }
        }
        #endregion

        #region 只读属性 - 点C —— Point C
        /// <summary>
        /// 只读属性 - 点C
        /// </summary>
        /// <remarks>右下</remarks>
        public Point C
        {
            get
            {
                Point rightBottom = new Point(this.Location.X + this.Size.Width, this.Location.Y + this.Size.Height);
                return this.RotateTransform.Transform(rightBottom);
            }
        }
        #endregion

        #region 只读属性 - 点D —— Point D
        /// <summary>
        /// 只读属性 - 点D
        /// </summary>
        /// <remarks>右上</remarks>
        public Point D
        {
            get
            {
                Point rightTop = new Point(this.Location.X + this.Size.Width, this.Location.Y);
                return this.RotateTransform.Transform(rightTop);
            }
        }
        #endregion

        #region 只读属性 - 位置 —— Point Location
        /// <summary>
        /// 只读属性 - 位置
        /// </summary>
        /// <remarks>左上角坐标</remarks>
        public Point Location
        {
            get => new Point(this.Center.X - this.Size.Width / 2, this.Center.Y - this.Size.Height / 2);
        }
        #endregion

        #region 只读属性 - 旋转变换 —— RotateTransform RotateTransform
        /// <summary>
        /// 只读属性 - 旋转变换
        /// </summary>
        public RotateTransform RotateTransform
        {
            get => new RotateTransform(this.Angle, this.Center.X, this.Center.Y);
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
                rectangleGeometry.Transform = this.RotateTransform;

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
                FontFamily fontFamily = new FontFamily("Times New Roman,SimSun");
                Typeface typeface = new Typeface(fontFamily, FontStyles.Normal, FontWeights.Thin, FontStretches.Normal);
                FormattedText formattedText = new FormattedText(this.Label, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, typeface, this.FontSize, base.Stroke, new NumberSubstitution(), 1.25);
                Point origin = new Point(this.Location.X + 1, this.Location.Y - this.FontSize - this.StrokeThickness);
                Geometry textGeometry = formattedText.BuildGeometry(origin);
                textGeometry.Transform = this.RotateTransform;

                //绘制文本背景
                double backgroundXMin = this.Location.X - this.StrokeThickness / 2;
                double backgroundYMin = this.Location.Y - this.FontSize - this.StrokeThickness / 2;
                double backgroundXMax = backgroundXMin + textGeometry.Bounds.Width + 4;
                double backgroundYMax = this.Location.Y - this.StrokeThickness / 2 + 0.3;
                double backgroundWidth = backgroundXMax - backgroundXMin;
                double backgroundHeight = backgroundYMax - backgroundYMin;
                Rect backgroundBox = new Rect(backgroundXMin, backgroundYMin, backgroundWidth, backgroundHeight);
                Geometry backgroundGeometry = new RectangleGeometry(backgroundBox);
                backgroundGeometry.Transform = this.RotateTransform;
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

                //绘制中心
                if (this.ShowCenter)
                {
                    double radius = base.StrokeThickness * 1.5;
                    drawingContext.DrawEllipse(base.Stroke, null, this.Center, radius, radius);
                }
            }
        }
        #endregion

        #endregion
    }
}
