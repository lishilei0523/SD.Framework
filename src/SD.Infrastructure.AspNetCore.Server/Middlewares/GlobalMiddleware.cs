using Microsoft.AspNetCore.Http;
using SD.Infrastructure.Constants;
using SD.Infrastructure.RepositoryBase;
using SD.IOC.Core.Mediators;
using System.Threading.Tasks;

namespace SD.Infrastructure.AspNetCore.Server.Middlewares
{
    /// <summary>
    /// 全局中间件
    /// </summary>
    public class GlobalMiddleware
    {
        /// <summary>
        /// 同步锁
        /// </summary>
        private static readonly object _Sync = new object();

        /// <summary>
        /// 请求委托
        /// </summary>
        private readonly RequestDelegate _next;

        /// <summary>
        /// 依赖注入构造器
        /// </summary>
        public GlobalMiddleware(RequestDelegate next)
        {
            this._next = next;

            //初始化SessionId
            lock (_Sync)
            {
                GlobalSetting.FreeCurrentSessionId();
                GlobalSetting.InitCurrentSessionId();
            }

            //初始化数据库
            IDataInitializer initializer = ResolveMediator.Resolve<IDataInitializer>();
            initializer.Initialize();
        }

        /// <summary>
        /// 执行中间件
        /// </summary>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                //初始化SessionId
                lock (_Sync)
                {
                    GlobalSetting.FreeCurrentSessionId();
                    GlobalSetting.InitCurrentSessionId();
                }

                await this._next.Invoke(context);
            }
            finally
            {
                //清理依赖注入范围容器
                ResolveMediator.Dispose();
            }
        }
    }
}
