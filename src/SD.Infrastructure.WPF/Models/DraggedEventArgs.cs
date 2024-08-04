using SD.Infrastructure.WPF.Enums;
using System.Windows;

namespace SD.Infrastructure.WPF.Models
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
        /// <param name="oldBound">原边界</param>
        /// <param name="newBound">新边界</param>
        /// <param name="dragMode">拖拽模式</param>
        /// <param name="dragDirection">拖拽方向</param>
        /// <param name="dragTargetElement">拖拽目标元素</param>
        public DraggedEventArgs(RoutedEvent routedEvent, Rect oldBound, Rect newBound, DragMode dragMode, DragDirection dragDirection, object dragTargetElement)
            : base(routedEvent)
        {
            this.RoutedEvent = routedEvent;
            this.OldBound = oldBound;
            this.NewBound = newBound;
            this.DragMode = dragMode;
            this.DragDirection = dragDirection;
            this.DragTargetElement = dragTargetElement;
        }

        /// <summary>
        /// 原边界
        /// </summary>
        public Rect OldBound { get; private set; }

        /// <summary>
        /// 新边界
        /// </summary>
        public Rect NewBound { get; private set; }

        /// <summary>
        /// 拖拽模式
        /// </summary>
        public DragMode DragMode { get; private set; }

        /// <summary>
        /// 拖拽方向
        /// </summary>
        public DragDirection DragDirection { get; private set; }

        /// <summary>
        /// 拖拽目标元素
        /// </summary>
        public object DragTargetElement { get; private set; }
    }
}
