using SD.Infrastructure.Avalonia.Caliburn.Base;

namespace SD.Infrastructure.Avalonia.Caliburn.Extensions
{
    /// <summary>
    /// 事件扩展
    /// </summary>
    public static class EventExtension
    {
        #region # 是否应该处理事件 —— static bool ShouldHandle<T>(this T message, ScreenBase receiver)
        /// <summary>
        /// 是否应该处理事件
        /// </summary>
        public static bool ShouldHandle<T>(this T message, ScreenBase receiver) where T : CaliburnEvent
        {
            //自己发布的事件不处理
            if (message.Publisher == receiver)
            {
                return false;
            }

            //全局事件处理
            if (string.IsNullOrWhiteSpace(message.EventGroup))
            {
                return true;
            }

            //不同事件组不处理
            if (message.EventGroup != receiver.EventGroup)
            {
                return false;
            }

            return true;
        }
        #endregion

        #region # 是否应该处理事件 —— static bool ShouldHandle<T>(this T message, OneActiveConductorBase receiver)
        /// <summary>
        /// 是否应该处理事件
        /// </summary>
        public static bool ShouldHandle<T>(this T message, OneActiveConductorBase receiver) where T : CaliburnEvent
        {
            //自己发布的事件不处理
            if (message.Publisher == receiver)
            {
                return false;
            }

            //全局事件处理
            if (string.IsNullOrWhiteSpace(message.EventGroup))
            {
                return true;
            }

            //不同事件组不处理
            if (message.EventGroup != receiver.EventGroup)
            {
                return false;
            }

            return true;
        }
        #endregion
    }
}
