using Avalonia;
using Avalonia.Data;
using Avalonia.Media;
using SD.Avalonia.Controls.Shapes;
using System.Collections.Generic;

namespace SD.Infrastructure.Avalonia.Visual2Ds
{
    /// <summary>
    /// 折线段
    /// </summary>
    public class PolylineVisual2D : Shape
    {
        #region # 字段及构造器

        /// <summary>
        /// 默认边框厚度
        /// </summary>
        public const double DefaultStrokeThickness = 2.0d;

        /// <summary>
        /// 点集依赖属性
        /// </summary>
        public static readonly StyledProperty<IList<Point>> PointsProperty;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static PolylineVisual2D()
        {
            PointsProperty = AvaloniaProperty.Register<PolylineVisual2D, IList<Point>>(nameof(Points));
            AffectsGeometry<PolylineVisual2D>(PointsProperty);
        }

        /// <summary>
        /// 默认构造器
        /// </summary>
        public PolylineVisual2D()
        {
            base.Fill = new SolidColorBrush(Colors.Transparent);
            base.Stroke = new SolidColorBrush(Colors.Red);
            base.StrokeThickness = DefaultStrokeThickness;
            this.SetValue(PointsProperty, new Points(), BindingPriority.Template);
        }

        #endregion

        #region # 属性

        #region 依赖属性 - 点集 —— IList<Point> Points
        /// <summary>
        /// 依赖属性 - 点集
        /// </summary>
        public IList<Point> Points
        {
            get => this.GetValue(PointsProperty);
            set => this.SetValue(PointsProperty, value);
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
            PolylineGeometry polylineGeometry = new PolylineGeometry
            {
                Points = this.Points,
                IsFilled = false
            };

            return polylineGeometry;
        }
        #endregion

        #endregion
    }
}
