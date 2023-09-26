using System.Windows.Controls.Primitives;

namespace SD.Infrastructure.WPF.Draggable.Controls
{
    /// <summary>
    /// 可伸缩拖动控件
    /// </summary>
    public class StretchableThumb : Thumb
    {
        /// <summary>
        /// 拖拽方向
        /// </summary>
        public DragDirection DragDirection { get; set; }
    }
}
