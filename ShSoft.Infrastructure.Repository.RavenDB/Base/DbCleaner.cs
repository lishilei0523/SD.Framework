using System.Runtime.Remoting.Messaging;
using ShSoft.Infrastructure.RepositoryBase;

namespace ShSoft.Infrastructure.Repository.RavenDB.Base
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
            if (RavenDbSession.CommandInstance != null)
            {
                RavenDbSession.CommandInstance.Dispose();
            }
            if (RavenDbSession.QueryInstance != null)
            {
                RavenDbSession.QueryInstance.Dispose();
            }
            CallContext.FreeNamedDataSlot(RavenDbSession.CommandInstanceKey);
            CallContext.FreeNamedDataSlot(RavenDbSession.QueryInstanceKey);
        }
    }
}
