using Caliburn.Micro;

namespace SD.Infrastructure.Avalonia.Caliburn.Base
{
    /// <summary>
    /// Caliburn事件
    /// </summary>
    public abstract class CaliburnEvent
    {
        /// <summary>
        /// 事件发布者
        /// </summary>
        public Screen Publisher { get; set; }
    }
}
