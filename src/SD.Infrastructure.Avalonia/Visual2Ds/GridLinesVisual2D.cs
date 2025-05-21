using Avalonia;
using Avalonia.Media;
using SD.Avalonia.Controls.Shapes;

namespace SD.Infrastructure.Avalonia.Visual2Ds
{
    /// <summary>
    /// 2D网格线
    /// </summary>
    public class GridLinesVisual2D : Shape
    {
        #region # 字段及构造器

        /// <summary>
        /// 默认边框厚度
        /// </summary>
        public const double DefaultStrokeThickness = 1d;

        /// <summary>
        /// 行数依赖属性
        /// </summary>
        public static readonly StyledProperty<int> RowsProperty;

        /// <summary>
        /// 列数依赖属性
        /// </summary>
        public static readonly StyledProperty<int> ColsProperty;

        /// <summary>
        /// 步长依赖属性
        /// </summary>
        public static readonly StyledProperty<int> StepSizeProperty;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static GridLinesVisual2D()
        {
            RowsProperty = AvaloniaProperty.Register<GridLinesVisual2D, int>(nameof(Rows), 2000);
            ColsProperty = AvaloniaProperty.Register<GridLinesVisual2D, int>(nameof(Cols), 2000);
            StepSizeProperty = AvaloniaProperty.Register<GridLinesVisual2D, int>(nameof(StepSize), 100);
        }

        /// <summary>
        /// 默认构造器
        /// </summary>
        public GridLinesVisual2D()
        {
            base.Stroke = new SolidColorBrush(Colors.Gray);
            base.StrokeThickness = DefaultStrokeThickness;
        }

        #endregion

        #region # 属性

        #region 依赖属性 - 行数 —— int Rows
        /// <summary>
        /// 依赖属性 - 行数
        /// </summary>
        public int Rows
        {
            get => this.GetValue(RowsProperty);
            set => this.SetValue(RowsProperty, value);
        }
        #endregion

        #region 依赖属性 - 列数 —— int Cols
        /// <summary>
        /// 依赖属性 - 列数
        /// </summary>
        public int Cols
        {
            get => this.GetValue(ColsProperty);
            set => this.SetValue(ColsProperty, value);
        }
        #endregion

        #region 依赖属性 - 步长 —— int StepSize
        /// <summary>
        /// 依赖属性 - 步长
        /// </summary>
        public int StepSize
        {
            get => this.GetValue(StepSizeProperty);
            set => this.SetValue(StepSizeProperty, value);
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
            GeometryGroup gridLines = new GeometryGroup();
            for (int y = 0; y <= this.Rows; y += this.StepSize)
            {
                LineGeometry horizontalLine = new LineGeometry
                {
                    StartPoint = new Point(0, y),
                    EndPoint = new Point(this.Cols, y)
                };
                gridLines.Children.Add(horizontalLine);
            }
            for (int x = 0; x <= this.Cols; x += this.StepSize)
            {
                LineGeometry verticalLine = new LineGeometry
                {
                    StartPoint = new Point(x, 0),
                    EndPoint = new Point(x, this.Rows)
                };
                gridLines.Children.Add(verticalLine);
            }

            return gridLines;
        }
        #endregion

        #endregion
    }
}
