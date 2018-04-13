using SD.Infrastructure.Global;
using SD.IOC.Integration.WebApi;
using System;
using System.Web;

namespace SD.Infrastructure.WebApi.Server
{
    /// <summary>
    /// 初始化HttpModule
    /// </summary>
    internal class InitializationHttpModule : IHttpModule
    {
        /// <summary>
        /// Initializes a module and prepares it to handle requests.
        /// </summary>
        /// <param name="context">应用程序上下文</param>
        public void Init(HttpApplication context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            //初始化SessionId
            Initializer.InitSessionId();

            //初始化数据库
            Initializer.InitDataBase();

            //注册事件
            WebApiDependencyResolver.OnGetInstance += this.WebApiDependencyResolver_OnGetInstance;
            WebApiDependencyResolver.OnReleaseInstance += this.WebApiDependencyResolver_OnReleaseInstance;
        }

        /// <summary>
        /// 获取服务实例事件
        /// </summary>
        private void WebApiDependencyResolver_OnGetInstance()
        {
            //初始化SessionId
            Initializer.InitSessionId();
        }

        /// <summary>
        /// 销毁服务实例事件
        /// </summary>
        private void WebApiDependencyResolver_OnReleaseInstance()
        {
            //清理数据库
            Finalizer.CleanDb();
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose() { }
    }
}