using Microsoft.AspNet.SignalR.Client;
using SD.Infrastructure.Constants;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SD.Infrastructure.SignalR.Client.Extensions
{
    /// <summary>
    /// Hub连接扩展方法
    /// </summary>
    public static class HubConnectionExtension
    {
        #region # 常量、字段及构造器

        /// <summary>
        /// 同步锁
        /// </summary>
        private static readonly object _Sync;

        /// <summary>
        /// 日志路径
        /// </summary>
        private static readonly string _LogPath;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static HubConnectionExtension()
        {
            _LogPath = $"{AppDomain.CurrentDomain.BaseDirectory}\\SignalR.Logs\\{{0:yyyy-MM-dd}}.txt";
            _Sync = new object();
        }

        #endregion

        #region # 注册公钥 —— static void RegisterPublicKey(this HubConnection connection...
        /// <summary>
        /// 注册公钥
        /// </summary>
        /// <param name="connection">Hub连接</param>
        /// <param name="publicKey">公钥</param>
        public static void RegisterPublicKey(this HubConnection connection, Guid publicKey)
        {
            connection.Headers.Add(SessionKey.CurrentPublicKey, publicKey.ToString());
        }
        #endregion

        #region # 注册异常处理者 —— static void RegisterExceptionHandler(this HubConnection...
        /// <summary>
        /// 注册异常处理者
        /// </summary>
        /// <param name="connection">Hub连接</param>
        public static void RegisterExceptionHandler(this HubConnection connection)
        {
            connection.Error += exception =>
            {
                Task.Run(() =>
                {
                    WriteFile(string.Format(_LogPath, DateTime.Today),
                        "----------------------ASP.NET SignalR 运行异常----------------------"
                        + Environment.NewLine + "［连接Id］" + connection.ConnectionId
                        + Environment.NewLine + "［连接URL］" + connection.Url
                        + Environment.NewLine + "［连接状态］" + connection.State
                        + Environment.NewLine + "［异常时间］" + DateTime.Now
                        + Environment.NewLine + "［异常消息］" + exception.Message
                        + Environment.NewLine + "［异常明细］" + exception
                        + Environment.NewLine + "［内部异常］" + exception.InnerException
                        + Environment.NewLine + "［堆栈信息］" + exception.StackTrace
                        + Environment.NewLine + Environment.NewLine);
                });
            };
        }
        #endregion

        #region # 注册保持断线重连 —— static void RegisterKeepReconnecting(this HubConnection...
        /// <summary>
        /// 注册保持断线重连
        /// </summary>
        /// <param name="connection">Hub连接</param>
        public static void RegisterKeepReconnecting(this HubConnection connection)
        {
            connection.Closed += () =>
            {
                Thread.Sleep(5000);
                connection.Start().Wait(new TimeSpan(0, 0, 0, 5));
            };
        }
        #endregion

        #region # 注册状态变更处理者 —— static void RegisterStateChangedHandler(this HubConnection...
        /// <summary>
        /// 注册状态变更处理者
        /// </summary>
        /// <param name="connection">Hub连接</param>
        public static void RegisterStateChangedHandler(this HubConnection connection)
        {
            connection.StateChanged += stateChange =>
            {
                Task.Run(() =>
                {
                    WriteFile(string.Format(_LogPath, DateTime.Today),
                        "----------------------ASP.NET SignalR 状态变更----------------------"
                        + Environment.NewLine + "［连接Id］" + connection.ConnectionId
                        + Environment.NewLine + "［连接URL］" + connection.Url
                        + Environment.NewLine + "［旧状态］" + stateChange.OldState
                        + Environment.NewLine + "［新状态］" + stateChange.NewState
                        + Environment.NewLine + "［变更时间］" + DateTime.Now
                        + Environment.NewLine + Environment.NewLine);
                });
            };
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
