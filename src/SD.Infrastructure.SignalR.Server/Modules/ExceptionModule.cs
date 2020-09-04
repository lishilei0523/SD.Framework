using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SD.Infrastructure.SignalR.Server.Modules
{
    /// <summary>
    /// 异常处理模块
    /// </summary>
    public class ExceptionModule : HubPipelineModule
    {
        #region # 常量、字段及构造器

        /// <summary>
        /// 同步锁
        /// </summary>
        private static readonly object _Sync;

        /// <summary>
        /// 异常日志路径
        /// </summary>
        private static readonly string _ExceptionLogPath;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static ExceptionModule()
        {
            _ExceptionLogPath = $"{AppDomain.CurrentDomain.BaseDirectory}\\SignalR.Logs\\{{0:yyyy-MM-dd}}.txt";
            _Sync = new object();
        }

        #endregion

        #region # 发生异常 —— override void OnIncomingError(ExceptionContext exceptionContext...
        /// <summary>
        /// 发生异常
        /// </summary>
        protected override void OnIncomingError(ExceptionContext exceptionContext, IHubIncomingInvokerContext invokerContext)
        {
            Exception exception = exceptionContext.Error;
            Task.Run(() =>
            {
                WriteFile(string.Format(_ExceptionLogPath, DateTime.Today),
                    "----------------------ASP.NET SignalR 运行异常----------------------"
                    + Environment.NewLine + "［当前Hub］" + invokerContext.Hub.GetType().FullName
                    + Environment.NewLine + "［连接Id］" + invokerContext.Hub.Context.ConnectionId
                    + Environment.NewLine + "［异常时间］" + DateTime.Now
                    + Environment.NewLine + "［异常消息］" + exception.Message
                    + Environment.NewLine + "［异常明细］" + exception
                    + Environment.NewLine + "［内部异常］" + exception.InnerException
                    + Environment.NewLine + "［堆栈信息］" + exception.StackTrace
                    + Environment.NewLine + Environment.NewLine);
            });

            base.OnIncomingError(exceptionContext, invokerContext);
        }
        #endregion


        //Private

        #region # 写入文件方法 —— static void WriteFile(string path, string content)
        /// <summary>
        /// 写入文件方法
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="content">内容</param>
        private static void WriteFile(string path, string content)
        {
            lock (_Sync)
            {
                FileInfo file = new FileInfo(path);
                StreamWriter writer = null;

                try
                {
                    //获取文件目录并判断是否存在
                    string directory = Path.GetDirectoryName(path);

                    if (string.IsNullOrEmpty(directory))
                    {
                        throw new ArgumentNullException(nameof(path), "目录不可为空！");
                    }
                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }

                    writer = file.AppendText();
                    writer.Write(content);
                }
                finally
                {
                    writer?.Dispose();
                }
            }
        }
        #endregion
    }
}
