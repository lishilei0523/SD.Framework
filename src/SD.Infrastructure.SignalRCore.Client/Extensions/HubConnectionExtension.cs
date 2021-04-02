using Microsoft.AspNetCore.Http.Connections.Client;
using Microsoft.AspNetCore.SignalR.Client;
using SD.Infrastructure.Constants;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SD.Infrastructure.SignalRCore.Client.Extensions
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

        #region # 注册公钥 —— static void RegisterPublicKey(this HttpConnectionOptions options...
        /// <summary>
        /// 注册公钥
        /// </summary>
        /// <param name="options">Hub连接选项</param>
        /// <param name="publicKey">公钥</param>
        public static void RegisterPublicKey(this HttpConnectionOptions options, Guid publicKey)
        {
            //添加Header
            options.Headers.Add(SessionKey.CurrentPublicKey, publicKey.ToString());

            //添加QueryString
            string url = options.Url.ToString();
            string urlWithPublicKey = $"{url}?{SessionKey.CurrentPublicKey}={publicKey}";
            options.Url = new Uri(urlWithPublicKey);
        }
        #endregion

        #region # 注册连接关闭处理者 —— static void RegisterClosedHandler(this HubConnection...
        /// <summary>
        /// 注册连接关闭处理者
        /// </summary>
        /// <param name="connection">Hub连接</param>
        public static void RegisterClosedHandler(this HubConnection connection)
        {
            connection.Closed += exception =>
            {
                return Task.Run(() =>
                 {
                     WriteFile(string.Format(_LogPath, DateTime.Today),
                         "----------------------ASP.NET Core SignalR 连接关闭----------------------"
                         + Environment.NewLine + "［连接Id］" + connection.ConnectionId
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

        #region # 注册重连处理者 —— static void RegisterReconnectingHandler(this HubConnection...
        /// <summary>
        /// 注册重连处理者
        /// </summary>
        /// <param name="connection">Hub连接</param>
        public static void RegisterReconnectingHandler(this HubConnection connection)
        {
            connection.Reconnecting += exception =>
            {
                return Task.Run(() =>
                {
                    WriteFile(string.Format(_LogPath, DateTime.Today),
                        "----------------------ASP.NET Core SignalR 正在重连----------------------"
                        + Environment.NewLine + "［连接Id］" + connection.ConnectionId
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

        #region # 注册已重连处理者 —— static void RegisterReconnectedHandler(this HubConnection...
        /// <summary>
        /// 注册已重连处理者
        /// </summary>
        /// <param name="connection">Hub连接</param>
        public static void RegisterReconnectedHandler(this HubConnection connection)
        {
            connection.Reconnected += newConnectionId =>
            {
                return Task.Run(() =>
                 {
                     WriteFile(string.Format(_LogPath, DateTime.Today),
                         "----------------------ASP.NET Core SignalR 已重连----------------------"
                         + Environment.NewLine + "［连接Id］" + connection.ConnectionId
                         + Environment.NewLine + "［新连接Id］" + newConnectionId
                         + Environment.NewLine + "［连接状态］" + connection.State
                         + Environment.NewLine + "［重连时间］" + DateTime.Now
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
