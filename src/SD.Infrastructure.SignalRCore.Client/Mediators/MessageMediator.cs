using Microsoft.AspNetCore.SignalR.Client;
using SD.Infrastructure.Constants;
using SD.Infrastructure.MessageBase;
using System;
using System.Threading.Tasks;

namespace SD.Infrastructure.SignalRCore.Client.Mediators
{
    /// <summary>
    /// 消息中介者
    /// </summary>
    public static class MessageMediator
    {
        #region # 事件

        /// <summary>
        /// 消息已发送事件
        /// </summary>
        public static event Action<IMessage> OnMessageSent;

        #endregion

        #region # 方法

        #region 发送消息 —— static void Send<T>(HubConnection hubConnection...
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <typeparam name="T">消息类型</typeparam>
        /// <param name="hubConnection">Hub连接</param>
        /// <param name="message">消息</param>
        public static void Send<T>(HubConnection hubConnection, T message) where T : IMessage
        {
            hubConnection.InvokeAsync(CommonConstants.ExchangeMethodName, message).Wait();

            //消息已发送事件
            OnMessageSent?.Invoke(message);
        }
        #endregion

        #region 发送消息 —— static async Task SendAsync<T>(HubConnection hubConnection...
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <typeparam name="T">消息类型</typeparam>
        /// <param name="hubConnection">Hub连接</param>
        /// <param name="message">消息</param>
        public static async Task SendAsync<T>(HubConnection hubConnection, T message) where T : IMessage
        {
            await hubConnection.InvokeAsync(CommonConstants.ExchangeMethodName, message);

            //消息已发送事件
            OnMessageSent?.Invoke(message);
        }
        #endregion

        #region 接收消息 —— static void Receive<T>(HubConnection hubConnection...
        /// <summary>
        /// 接收消息
        /// </summary>
        /// <typeparam name="T">消息类型</typeparam>
        /// <param name="hubConnection">Hub连接</param>
        /// <param name="onReceive">消息接收事件</param>
        public static void Receive<T>(HubConnection hubConnection, Action<T> onReceive) where T : IMessage
        {
            hubConnection.On<T>(CommonConstants.ExchangeMethodName, onReceive);
        }
        #endregion 

        #endregion
    }
}
