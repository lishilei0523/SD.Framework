using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SD.Infrastructure.WPF.Visual2Ds
{
    /// <summary>
    /// 形状容器
    /// </summary>
    [ContentProperty(nameof(Content))]
    public sealed class ShapeVisual2D : Shape
    {
        #region # 字段及构造器

        /// <summary>
        /// 内容依赖属性
        /// </summary>
        public static readonly DependencyProperty ContentProperty;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static ShapeVisual2D()
        {
            ContentProperty = DependencyProperty.Register(nameof(Content), typeof(Shape), typeof(ShapeVisual2D), new PropertyMetadata(null, OnContentChanged));
        }

        #endregion

        #region # 属性

        #region 依赖属性 - 内容 —— Shape Content
        /// <summary>
        /// 依赖属性 - 内容
        /// </summary>
        public Shape Content
        {
            get => (Shape)this.GetValue(ContentProperty);
            set => this.SetValue(ContentProperty, value);
        }
        #endregion

        #region 只读属性 - 几何对象 —— override Geometry DefiningGeometry
        /// <summary>
        /// 只读属性 - 几何对象
        /// </summary>
        protected override Geometry DefiningGeometry
        {
            get => new StreamGeometry();
        }
        #endregion

        #endregion

        #region # 事件

        #region 内容改变事件 —— static void OnContentChanged(DependencyObject dependencyObject...
        /// <summary>
        /// 内容改变事件
        /// </summary>
        private static void OnContentChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            ShapeVisual2D shapeVisual2D = (ShapeVisual2D)dependencyObject;
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
