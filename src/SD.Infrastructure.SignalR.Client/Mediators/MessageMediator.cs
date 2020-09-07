using Microsoft.AspNet.SignalR.Client;
using SD.Infrastructure.Constants;
using SD.Infrastructure.MessageBase;
using SD.Infrastructure.SignalR.Client.Builders;
using System;
using System.Threading.Tasks;

namespace SD.Infrastructure.SignalR.Client.Mediators
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

        #region 发送消息 —— static void Send<T>(HubConnectionBuilder hubConnectionBuilder...
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <typeparam name="T">消息类型</typeparam>
        /// <param name="hubConnectionBuilder">Hub连接建造者</param>
        /// <param name="message">消息</param>
        public static void Send<T>(HubConnectionBuilder hubConnectionBuilder, T message) where T : IMessage
        {
            #region # 验证

            if (!hubConnectionBuilder.HubConnectionBuilt)
            {
                throw new InvalidOperationException("Hub连接未构建，不可发送消息！");
            }

            #endregion

            string hubName = MessageExtension.GetHubName<T>();

            #region # 验证

            if (!hubConnectionBuilder.SenderProxies.ContainsKey(hubName))
            {
                throw new InvalidOperationException($"要发送的消息类型\"{typeof(T).Name}\"未注册！");
            }

            #endregion

            IHubProxy proxy = hubConnectionBuilder.SenderProxies[hubName];
            proxy.Invoke(CommonConstants.ExchangeMethodName, message).Wait();

            //消息已发送事件
            OnMessageSent?.Invoke(message);
        }
        #endregion

        #region 发送消息 —— static async Task SendAsync<T>(HubConnectionBuilder hubConnectionBuilder...
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <typeparam name="T">消息类型</typeparam>
        /// <param name="hubConnectionBuilder">Hub连接建造者</param>
        /// <param name="message">消息</param>
        public static async Task SendAsync<T>(HubConnectionBuilder hubConnectionBuilder, T message) where T : IMessage
        {
            #region # 验证

            if (!hubConnectionBuilder.HubConnectionBuilt)
            {
                throw new InvalidOperationException("Hub连接未构建，不可发送消息！");
            }

            #endregion

            string hubName = MessageExtension.GetHubName<T>();

            #region # 验证

            if (!hubConnectionBuilder.SenderProxies.ContainsKey(hubName))
            {
                throw new InvalidOperationException($"要发送的消息类型\"{typeof(T).Name}\"未注册！");
            }

            #endregion

            IHubProxy proxy = hubConnectionBuilder.SenderProxies[hubName];
            await proxy.Invoke(CommonConstants.ExchangeMethodName, message);

            //消息已发送事件
            OnMessageSent?.Invoke(message);
        }
        #endregion

        #region 接收消息 —— static void Receive<T>(HubConnectionBuilder hubConnectionBuilder...
        /// <summary>
        /// 接收消息
        /// </summary>
        /// <typeparam name="T">消息类型</typeparam>
        /// <param name="hubConnectionBuilder">Hub连接建造者</param>
        /// <param name="onReceive">消息接收事件</param>
        public static void Receive<T>(HubConnectionBuilder hubConnectionBuilder, Action<T> onReceive) where T : IMessage
        {
            #region # 验证

            if (!hubConnectionBuilder.HubConnectionBuilt)
            {
                throw new InvalidOperationException("Hub连接未构建，不可接收消息！");
            }

            #endregion

            string hubName = MessageExtension.GetHubName<T>();

            #region # 验证

            if (!hubConnectionBuilder.ReceiverProxies.ContainsKey(hubName))
            {
                throw new InvalidOperationException($"要接收的消息类型\"{typeof(T).Name}\"未注册！");
            }

            #endregion

            IHubProxy proxy = hubConnectionBuilder.ReceiverProxies[hubName];
            proxy.On<T>(CommonConstants.ExchangeMethodName, onReceive);
        }
        #endregion 

        #endregion
    }
}
