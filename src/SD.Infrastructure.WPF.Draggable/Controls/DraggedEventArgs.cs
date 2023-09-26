using System.Windows;

namespace SD.Infrastructure.WPF.Draggable.Controls
{
    /// <summary>
    /// 拖拽事件参数
    /// </summary>
    public class DraggedEventArgs : RoutedEventArgs
    {
        /// <summary>
        /// 创建拖拽事件参数构造器
        /// </summary>
        /// <param name="routedEvent">路由事件</param>
        /// <param name="newBound">新边界</param>
        /// <param name="dragTargetElement">拖拽目标元素</param>
        public DraggedEventArgs(RoutedEvent routedEvent, Rect newBound, object dragTargetElement)
            : base(routedEvent)
        {
            this.NewBound = newBound;
            this.DragTargetElement = dragTargetElement;
        }

        /// <summary>
        /// 新边界
        /// </summary>
        public Rect NewBound { get; private set; }

        /// <summary>
        /// 拖拽目标元素
        /// </summary>
        public object DragTargetElement { get; private set; }
    }
}
