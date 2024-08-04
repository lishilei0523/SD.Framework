using SD.Infrastructure.WPF.Enums;
using System.Windows.Controls.Primitives;

namespace SD.Infrastructure.WPF.Models
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
