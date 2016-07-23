using System.Collections.Generic;
using SD.IOC.Core.Mediator;
using ShSoft.Infrastructure.RepositoryBase;

// ReSharper disable once CheckNamespace
namespace ShSoft.Infrastructure.Global
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

        /// <summary>
        /// 终结数据库
        /// </summary>
        public static void FlushDb()
        {
            IEnumerable<IDbCleaner> dbCleaners = ResolveMediator.ResolveAll<IDbCleaner>();

            foreach (IDbCleaner dbCleaner in dbCleaners)
            {
                dbCleaner.Flush();
            }
        }
    }
}
