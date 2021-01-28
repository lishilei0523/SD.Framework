using SD.Infrastructure.Global;
using System;
using System.Web;

namespace SD.Infrastructure.WebApi.WebHost.Server
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
            context.BeginRequest += OnBeginRequest;
            context.EndRequest += OnEndRequest;
        }

        /// <summary>
        /// 请求开始事件
        /// </summary>
        private void OnBeginRequest(object sender, EventArgs e)
        {
            //初始化SessionId
            Initializer.InitSessionId();
        }

        /// <summary>
        /// 请求结束事件
        /// </summary>
        private void OnEndRequest(object sender, EventArgs e)
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