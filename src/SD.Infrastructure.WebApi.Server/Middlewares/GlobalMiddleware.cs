using Microsoft.Owin;
using SD.Infrastructure.Global;
using SD.IOC.Core.Mediators;
using System.Threading.Tasks;

namespace SD.Infrastructure.WebApi.Server.Middlewares
{
    /// <summary>
    /// 全局中间件
    /// </summary>
    public class GlobalMiddleware : OwinMiddleware
    {
        /// <summary>
        /// 默认构造器
        /// </summary>
        public GlobalMiddleware(OwinMiddleware next)
            : base(next)
        {
            //初始化SessionId
            Initializer.InitSessionId();

            //初始化数据库
            Initializer.InitDataBase();
        }

        /// <summary>
        /// 执行中间件
        /// </summary>
        public override async Task Invoke(IOwinContext context)
        {
            try
            {
                //初始化SessionId
                Initializer.InitSessionId();

                await base.Next.Invoke(context);
            }
            finally
            {
                //清理依赖注入范围容器
                ResolveMediator.Dispose();
            }
        }
    }
}
