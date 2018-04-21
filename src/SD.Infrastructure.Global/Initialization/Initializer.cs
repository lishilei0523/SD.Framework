using SD.Infrastructure.Constants;
using SD.Infrastructure.RepositoryBase;
using SD.IOC.Core.Mediator;

namespace SD.Infrastructure.Global
{
    /// <summary>
    /// 初始化器
    /// </summary>
    public static class Initializer
    {
        /// <summary>
        /// 同步锁
        /// </summary>
        private static readonly object _Sync = new object();

        /// <summary>
        /// 初始化数据库
        /// </summary>
        public static void InitDataBase()
        {
            IDataInitializer initializer = ResolveMediator.Resolve<IDataInitializer>();
            initializer.Initialize();
        }

        /// <summary>
        /// 初始化会话Id
        /// </summary>
        public static void InitSessionId()
        {
            lock (_Sync)
            {
                GlobalSetting.FreeCurrentSessionId();
                GlobalSetting.InitCurrentSessionId();
            }
        }
    }
}
