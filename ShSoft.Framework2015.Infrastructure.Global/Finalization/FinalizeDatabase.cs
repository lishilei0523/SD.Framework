using SD.IOC.Core.Mediator;
using ShSoft.Framework2015.Infrastructure.IRepository;

namespace ShSoft.Framework2015.Infrastructure.Global.Finalization
{
    /// <summary>
    /// 清理数据库
    /// </summary>
    public static class FinalizeDatabase
    {
        /// <summary>
        /// 注册
        /// </summary>
        public static void Register()
        {
            //清理数据库
            IDbCleaner dbCleaner = ResolveMediator.Resolve<IDbCleaner>();
            dbCleaner.Clean();
        }
    }
}
