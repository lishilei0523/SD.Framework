﻿using SD.Infrastructure.Constants;

// ReSharper disable once CheckNamespace
namespace SD.Infrastructure.Global
{
    /// <summary>
    /// 终结器
    /// </summary>
    public static class Finalizer
    {
        /// <summary>
        /// 同步锁
        /// </summary>
        private static readonly object _Sync = new object();

        /// <summary>
        /// 清理会话Id
        /// </summary>
        public static void CleanSessionId()
        {
            lock (_Sync)
            {
                GlobalSetting.FreeCurrentSessionId();
            }
        }
    }
}
