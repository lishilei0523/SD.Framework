using SD.Infrastructure.Constants;
using SD.Infrastructure.RepositoryBase;
using SD.IOC.Core.Mediators;
using System.Collections.Generic;

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
        /// 清理数据库
        /// </summary>
        public static void CleanDb()
        {
            IEnumerable<IDbCleaner> dbCleaners = ResolveMediator.ResolveAll<IDbCleaner>();
            foreach (IDbCleaner dbCleaner in dbCleaners)
            {
                dbCleaner.Clean();
            }
        }

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
