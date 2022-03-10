using SD.Infrastructure.Global;
using System;
using System.Web;

namespace SD.Infrastructure.AspNet.Server.Modules
{
    /// <summary>
    /// ASP.NET初始化模块
    /// </summary>
    internal class InitializationModule : IHttpModule
    {
        /// <summary>
        /// Initializes a module and prepares it to handle requests.
        /// </summary>
        /// <param name="context">应用程序上下文</param>
        public void Init(HttpApplication context)
        {
            #region # 验证

            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            #endregion

            //初始化SessionId
            Initializer.InitSessionId();

            //初始化数据库
            Initializer.InitDataBase();

            //注册事件
            context.BeginRequest += OnBeginRequest;
        }

        /// <summary>
        /// 请求开始事件
        /// </summary>
        private static void OnBeginRequest(object sender, EventArgs eventArgs)
        {
            //初始化SessionId
            Initializer.InitSessionId();
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {

        }
    }
}