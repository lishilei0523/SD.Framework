using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SD.Infrastructure.WPF.Visual2Ds
{
    /// <summary>
    /// 2D网格线
    /// </summary>
    public class GridLinesVisual2D : Shape
    {
        #region # 字段及构造器

        /// <summary>
        /// 默认行数
        /// </summary>
        public const int DefaultRows = 2000;

        /// <summary>
        /// 默认列数
        /// </summary>
        public const int DefaultCols = 2000;

        /// <summary>
        /// 默认步长
        /// </summary>
        public const int DefaultStepSize = 100;

        /// <summary>
        /// 行数依赖属性
        /// </summary>
        public static readonly DependencyProperty RowsProperty;

        /// <summary>
        /// 列数依赖属性
        /// </summary>
        public static readonly DependencyProperty ColsProperty;

        /// <summary>
        /// 步长依赖属性
        /// </summary>
        public static readonly DependencyProperty StepSizeProperty;

        /// <summary>
        /// 显示中心依赖属性
        /// </summary>
        public static readonly DependencyProperty ShowCenterProperty;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static GridLinesVisual2D()
        {
            RowsProperty = DependencyProperty.Register(nameof(Rows), typeof(int), typeof(GridLinesVisual2D), new FrameworkPropertyMetadata(DefaultRows, FrameworkPropertyMetadataOptions.AffectsRender));
            ColsProperty = DependencyProperty.Register(nameof(Cols), typeof(int), typeof(GridLinesVisual2D), new FrameworkPropertyMetadata(DefaultCols, FrameworkPropertyMetadataOptions.AffectsRender));
            StepSizeProperty = DependencyProperty.Register(nameof(StepSize), typeof(int), typeof(GridLinesVisual2D), new FrameworkPropertyMetadata(DefaultStepSize, FrameworkPropertyMetadataOptions.AffectsRender));
            ShowCenterProperty = DependencyProperty.Register(nameof(ShowCenter), typeof(bool), typeof(GridLinesVisual2D), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));
        }

        /// <summary>
        /// 默认构造器
        /// </summary>
        public GridLinesVisual2D()
        {
            base.Stroke = new SolidColorBrush(Colors.Gray);
        }

        #endregion

        #region # 属性

        #region 依赖属性 - 行数 —— int Rows
        /// <summary>
        /// 依赖属性 - 行数
        /// </summary>
        public int Rows
        {
            get => (int)this.GetValue(RowsProperty);
            set => this.SetValue(RowsProperty, value);
        }
        #endregion

        #region 依赖属性 - 列数 —— int Cols
        /// <summary>
        /// 依赖属性 - 列数
        /// </summary>
        public int Cols
        {
            get => (int)this.GetValue(ColsProperty);
            set => this.SetValue(ColsProperty, value);
        }
        #endregion

        #region 依赖属性 - 步长 —— int StepSize
        /// <summary>
        /// 依赖属性 - 步长
        /// </summary>
        public int StepSize
        {
            get => (int)this.GetValue(StepSizeProperty);
            set => this.SetValue(StepSizeProperty, value);
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

        #region 只读属性 - 几何对象 —— override Geometry DefiningGeometry
        /// <summary>
        /// 只读属性 - 几何对象
        /// </summary>
        protected override Geometry DefiningGeometry
        {
            get
            {
                GeometryGroup gridLines = new GeometryGroup();
                for (int x = 0; x <= this.Cols; x += this.StepSize)
                {
                    LineGeometry verticalLine = new LineGeometry
                    {
                        StartPoint = new Point(x, 0),
                        EndPoint = new Point(x, this.Rows)
                    };
                    gridLines.Children.Add(verticalLine);
                }
                for (int y = 0; y <= this.Rows; y += this.StepSize)
                {
                    LineGeometry horizontalLine = new LineGeometry
                    {
                        StartPoint = new Point(0, y),
                        EndPoint = new Point(this.Cols, y)
                    };
                    gridLines.Children.Add(horizontalLine);
                }

                return gridLines;
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
                //绘制中心点
                Point center = new Point(this.Cols / 2.0, this.Rows / 2.0);
                double thickness = base.StrokeThickness * 2;
                SolidColorBrush brush = new SolidColorBrush(Colors.Red);
                Pen pen = new Pen(brush, thickness);
                drawingContext.DrawEllipse(brush, pen, center, thickness, thickness);
            }
        }
        #endregion

        #endregion
    }
}
