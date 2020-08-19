using Microsoft.AspNet.SignalR.Client;
using SD.Infrastructure.Constants;
using System;

namespace SD.Infrastructure.SignalR.Client.Extensions
{
    /// <summary>
    /// Hub连接扩展方法
    /// </summary>
    public static class HubConnectionExtension
    {
        /// <summary>
        /// 注册公钥
        /// </summary>
        /// <param name="connection">Hub连接</param>
        /// <param name="publicKey">公钥</param>
        public static void RegisterPublicKey(this HubConnection connection, Guid publicKey)
        {
            connection.Headers.Add(SessionKey.CurrentPublicKey, publicKey.ToString());
        }
    }
}
