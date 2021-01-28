using Microsoft.Owin;
using SD.Infrastructure.Global;
using System.Threading.Tasks;

namespace SD.Infrastructure.WebApi.SelfHost.Server.Middlewares
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
            //初始化SessionId
            Initializer.InitSessionId();

            await base.Next.Invoke(context);

            //清理数据库
            Finalizer.CleanDb();
        }
    }
}
