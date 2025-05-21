using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Metadata;
using SD.Avalonia.Controls.Shapes;

namespace SD.Infrastructure.Avalonia.Visual2Ds
{
    /// <summary>
    /// 形状容器
    /// </summary>
    public sealed class ShapeVisual2D : Shape
    {
        #region # 字段及构造器

        /// <summary>
        /// 内容依赖属性
        /// </summary>
        public static readonly StyledProperty<Shape> ContentProperty;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static ShapeVisual2D()
        {
            ContentProperty = AvaloniaProperty.Register<ShapeVisual2D, Shape>(nameof(Content));
            ContentProperty.Changed.AddClassHandler<ShapeVisual2D>(OnContentChanged);
        }

        #endregion

        #region # 属性

        #region 依赖属性 - 内容 —— Shape Content
        /// <summary>
        /// 依赖属性 - 内容
        /// </summary>
        [Content]
        public Shape Content
        {
            get => this.GetValue(ContentProperty);
            set => this.SetValue(ContentProperty, value);
        }
        #endregion

        #endregion

        #region # 事件

        #region 创建几何图形 —— override Geometry CreateDefiningGeometry()
        /// <summary>
        /// 创建几何图形
        /// </summary>
        /// <returns>几何图形</returns>
        protected override Geometry CreateDefiningGeometry()
        {
            return new StreamGeometry();
        }
        #endregion

        #region 内容改变事件 —— static void OnContentChanged(ShapeVisual2D shapeVisual2D...
        /// <summary>
        /// 内容改变事件
        /// </summary>
        private static void OnContentChanged(ShapeVisual2D shapeVisual2D, AvaloniaPropertyChangedEventArgs eventArgs)
        {
            Shape oldShape = eventArgs.OldValue as Shape;
            Shape newShape = eventArgs.NewValue as Shape;
            if (shapeVisual2D.Parent is Canvas canvas)
            {
                if (oldShape != null && canvas.Children.Contains(oldShape))
                {
                    canvas.Children.Remove(oldShape);
                }
                if (newShape != null && !canvas.Children.Contains(newShape))
                {
                    canvas.Children.Add(newShape);
                }
            }
        }
        #endregion

        #endregion
    }
}
