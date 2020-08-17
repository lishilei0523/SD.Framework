using Microsoft.AspNet.SignalR.Client;
using SD.Infrastructure.Constants;
using SD.Infrastructure.MessageBase;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SD.Infrastructure.SignalR.Client.Mediators
{
    /// <summary>
    /// 消息中介者
    /// </summary>
    public static class MessageMediator
    {
        #region # 事件、字段及构造器

        /// <summary>
        /// 消息已发送事件
        /// </summary>
        public static event Action<IMessage> OnMessageSent;

        /// <summary>
        /// Hub代理字典
        /// </summary>
        private static readonly IDictionary<string, IHubProxy> _HubProxies;

        static MessageMediator()
        {
            _HubProxies = new ConcurrentDictionary<string, IHubProxy>();
        }

        #endregion

        #region # 发送消息 —— static void Send<T>(HubConnection connection, T message)
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <typeparam name="T">消息类型</typeparam>
        /// <param name="connection">Hub连接</param>
        /// <param name="message">消息</param>
        public static void Send<T>(HubConnection connection, T message) where T : IMessage
        {
            string hubName = MessageExtension.GetHubName<T>();
            IHubProxy proxy = GetHubProxy(connection, hubName);

            proxy.Invoke(CommonConstants.ExchangeMethodName, message).Wait();

            //消息已发送事件
            OnMessageSent?.Invoke(message);
        }
        #endregion

        #region # 发送消息 —— static async Task SendAsync<T>(HubConnection connection, T message)
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <typeparam name="T">消息类型</typeparam>
        /// <param name="connection">Hub连接</param>
        /// <param name="message">消息</param>
        public static async Task SendAsync<T>(HubConnection connection, T message) where T : IMessage
        {
            string hubName = MessageExtension.GetHubName<T>();
            IHubProxy proxy = GetHubProxy(connection, hubName);

            await proxy.Invoke(CommonConstants.ExchangeMethodName, message);

            //消息已发送事件
            OnMessageSent?.Invoke(message);
        }
        #endregion

        #region # 接收消息 —— static void Receive<T>(HubConnection connection, Action<T> onReceive)
        /// <summary>
        /// 接收消息
        /// </summary>
        /// <typeparam name="T">消息类型</typeparam>
        /// <param name="connection">Hub连接</param>
        /// <param name="onReceive">消息接收事件</param>
        public static void Receive<T>(HubConnection connection, Action<T> onReceive) where T : IMessage
        {
            string hubName = MessageExtension.GetHubName<T>();
            IHubProxy proxy = GetHubProxy(connection, hubName);

            proxy.On<T>(CommonConstants.ExchangeMethodName, onReceive);
        }
        #endregion


        //Private 

        #region # 获取Hub代理 —— static IHubProxy GetHubProxy(HubConnection connection, string hubName)
        /// <summary>
        /// 获取Hub代理
        /// </summary>
        /// <param name="connection">Hub连接</param>
        /// <param name="hubName">Hub名称</param>
        /// <returns>Hub代理</returns>
        private static IHubProxy GetHubProxy(HubConnection connection, string hubName)
        {
            string connectionId = string.IsNullOrWhiteSpace(connection.ConnectionId) ? string.Empty : connection.ConnectionId;
            IHubProxy proxy;

            if (!_HubProxies.ContainsKey(connectionId))
            {
                proxy = connection.CreateHubProxy(hubName);
                connection.Start().Wait();

                if (!_HubProxies.ContainsKey(connection.ConnectionId))
                {
                    _HubProxies.Add(connection.ConnectionId, proxy);
                }
            }
            else
            {
                proxy = _HubProxies[connectionId];
            }

            return proxy;
        }
        #endregion
    }
}
