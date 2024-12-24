using SD.Infrastructure.Shapes;
using System.Windows;
using System.Windows.Shapes;

namespace SD.Infrastructure.WPF.Models
{
    /// <summary>
    /// 形状事件参数
    /// </summary>
    public class ShapeEventArgs : RoutedEventArgs
    {
        /// <summary>
        /// 创建形状事件参数构造器
        /// </summary>
        /// <param name="routedEvent">路由事件</param>
        /// <param name="shape">形状</param>
        /// <param name="shapeL">形状数据</param>
        public ShapeEventArgs(RoutedEvent routedEvent, Shape shape, ShapeL shapeL)
            : base(routedEvent)
        {
            this.Shape = shape;
            this.ShapeL = shapeL;
        }

        /// <summary>
        /// 形状
        /// </summary>
        public Shape Shape { get; set; }

        /// <summary>
        /// 形状数据
        /// </summary>
        public ShapeL ShapeL { get; set; }
    }
}
