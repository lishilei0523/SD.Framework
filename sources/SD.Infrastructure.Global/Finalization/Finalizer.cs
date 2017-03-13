using System.Collections.Generic;
using SD.IOC.Core.Mediator;
using SD.Infrastructure.RepositoryBase;

// ReSharper disable once CheckNamespace
namespace SD.Infrastructure.Global
{

    /// <summary>
    /// 终结器
    /// </summary>
    public static class Finalizer
    {
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
    }
}
