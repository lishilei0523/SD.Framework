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
        /// 终结数据库
        /// </summary>
        public static void FinalizeDatabase()
        {
            IDbCleaner dbCleaner = ResolveMediator.Resolve<IDbCleaner>();
            dbCleaner.Clean();
        }
    }
}
