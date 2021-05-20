using SD.Infrastructure.WPF.Interfaces;

namespace SD.Infrastructure.WPF.Extensions
{
    /// <summary>
    /// 繁忙扩展
    /// </summary>
    public static class BusyExtension
    {
        /// <summary>
        /// 同步锁
        /// </summary>
        private static readonly object _Sync = new object();

        /// <summary>
        /// 挂起繁忙状态
        /// </summary>
        public static void Busy(this IBusy busy)
        {
            lock (_Sync)
            {
                busy.IsBusy = true;
            }
        }

        /// <summary>
        /// 释放繁忙状态
        /// </summary>
        public static void Idle(this IBusy busy)
        {
            lock (_Sync)
            {
                busy.IsBusy = false;
            }
        }
    }
}
