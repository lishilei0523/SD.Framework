using SD.Infrastructure.Repository.Redis.Base;
using SD.Infrastructure.RepositoryBase;
using System.Runtime.Remoting.Messaging;

// ReSharper disable once CheckNamespace
namespace SD.Infrastructure.Repository.Redis
{
    /// <summary>
    /// Redis数据库清理者实现
    /// </summary>
    public class DbCleaner : IDbCleaner
    {
        /// <summary>
        /// 清理
        /// </summary>
        public void Clean()
        {
            if (RedisSession.Current != null)
            {
                RedisSession.Current.Dispose();
            }

            CallContext.FreeNamedDataSlot(RedisSession.CurrentInstanceKey);
        }
    }
}
