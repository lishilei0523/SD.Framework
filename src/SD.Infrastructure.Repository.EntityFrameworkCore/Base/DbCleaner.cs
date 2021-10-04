using SD.Infrastructure.Repository.EntityFrameworkCore.Base;
using SD.Infrastructure.RepositoryBase;

// ReSharper disable once CheckNamespace
namespace SD.Infrastructure.Repository.EntityFrameworkCore
{
    /// <summary>
    /// 数据库清理者实现
    /// </summary>
    public class DbCleaner : IDbCleaner
    {
        /// <summary>
        /// 清理
        /// </summary>
        public void Clean()
        {
            DbSessionBase.FreeCommandInstanceCall();
            DbSessionBase.FreeQueryInstanceCall();
        }
    }
}
