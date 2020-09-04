using Microsoft.AspNet.SignalR.Client;
using SD.Infrastructure.Constants;
using SD.Infrastructure.MessageBase;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
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
        /// Hub连接
        /// </summary>
        private static HubConnection _HubConnection;

        /// <summary>
        /// Hub连接是否已构建
        /// </summary>
        private static bool _HubConnectionBuilt;

        /// <summary>
        /// 发送者Hub代理字典
        /// </summary>
        private static readonly IDictionary<string, IHubProxy> _SenderProxies;

        /// <summary>
        /// 接收者Hub代理字典
        /// </summary>
        private static readonly IDictionary<string, IHubProxy> _ReceiverProxies;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static MessageMediator()
        {
            _HubConnectionBuilt = false;
            _SenderProxies = new ConcurrentDictionary<string, IHubProxy>();
            _ReceiverProxies = new ConcurrentDictionary<string, IHubProxy>();
        }

        #endregion

        #region # 属性

        #region Hub连接 —— static HubConnection HubConnection
        /// <summary>
        /// Hub连接
        /// </summary>
        public static HubConnection HubConnection
        {
            get
            {
                return _HubConnection;
            }
        }
        #endregion

        #region Hub连接是否已构建 —— static bool HubConnectionBuilt
        /// <summary>
        /// Hub连接是否已构建
        /// </summary>
        public static bool HubConnectionBuilt
        {
            get
            {
                return _HubConnectionBuilt;
            }
        }
        #endregion

        #endregion

        #region # 方法

        #region 初始化Hub连接 —— static HubConnection InitHubConnection(string url)
        /// <summary>
        /// 初始化Hub连接
        /// </summary>
        /// <param name="url">连接地址</param>
        /// <returns>Hub连接</returns>
        public static HubConnection InitHubConnection(string url)
        {
            #region # 验证

            if (_HubConnectionBuilt)
            {
                throw new InvalidOperationException("Hub连接已构建，不可重新初始化！");
            }

            #endregion

            _HubConnection = new HubConnection(url);

            return _HubConnection;
        }
        #endregion

        #region 注册发送消息类型 —— static void RegisterMessageTypesToSend(IEnumerable<Type> messageTypes)
        /// <summary>
        /// 注册发送消息类型
        /// </summary>
        /// <param name="messageTypes">消息类型列表</param>
        public static void RegisterMessageTypesToSend(IEnumerable<Type> messageTypes)
        {
            #region # 验证

            if (_HubConnectionBuilt)
            {
                throw new InvalidOperationException("Hub连接已构建，不可注册发送消息类型！");
            }

            #endregion

            messageTypes = messageTypes?.Distinct().ToArray() ?? new Type[0];

            foreach (Type messageType in messageTypes)
            {
                #region # 验证

                if (!typeof(IMessage).IsAssignableFrom(messageType))
                {
                    throw new InvalidOperationException($"类型\"{messageType.Name}\"未实现\"{nameof(IMessage)}\"接口！");
                }

                #endregion

                string hubName = MessageExtension.GetHubName(messageType);
                IHubProxy hubProxy = _HubConnection.CreateHubProxy(hubName);

                _SenderProxies.Add(hubName, hubProxy);
            }
        }
        #endregion

        #region 注册接收消息类型 —— static void RegisterMessageTypesToReceive(IEnumerable<Type> messageTypes)
        /// <summary>
        /// 注册接收消息类型
        /// </summary>
        /// <param name="messageTypes">消息类型列表</param>
        public static void RegisterMessageTypesToReceive(IEnumerable<Type> messageTypes)
        {
            #region # 验证

            if (_HubConnectionBuilt)
            {
                throw new InvalidOperationException("Hub连接已构建，不可注册接收消息类型！");
            }

            #endregion

            messageTypes = messageTypes?.Distinct().ToArray() ?? new Type[0];

            foreach (Type messageType in messageTypes)
            {
                #region # 验证

                if (!typeof(IMessage).IsAssignableFrom(messageType))
                {
                    throw new InvalidOperationException($"类型\"{messageType.Name}\"未实现\"{nameof(IMessage)}\"接口！");
                }

                #endregion

                string hubName = MessageExtension.GetHubName(messageType);
                IHubProxy hubProxy = _HubConnection.CreateHubProxy(hubName);

                _ReceiverProxies.Add(hubName, hubProxy);
            }
        }
        #endregion

        #region 构建Hub连接 —— static void BuildHubConnection()
        /// <summary>
        /// 构建Hub连接
        /// </summary>
        public static void BuildHubConnection()
        {
            #region # 验证

            if (_HubConnectionBuilt)
            {
                throw new InvalidOperationException("Hub连接已构建，不可重新构建！");
            }

            #endregion

            _HubConnection.Start().Wait();
            _HubConnectionBuilt = true;
        }
        #endregion

        #region 发送消息 —— static void Send<T>(T message)
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <typeparam name="T">消息类型</typeparam>
        /// <param name="message">消息</param>
        public static void Send<T>(T message) where T : IMessage
        {
            #region # 验证

            if (!_HubConnectionBuilt)
            {
                throw new InvalidOperationException("Hub连接未构建，不可发送消息！");
            }

            #endregion

            string hubName = MessageExtension.GetHubName<T>();

            #region # 验证

            if (!_SenderProxies.ContainsKey(hubName))
            {
                throw new InvalidOperationException($"要发送的消息类型\"{typeof(T).Name}\"未注册！");
            }

            #endregion

            IHubProxy proxy = _SenderProxies[hubName];
            proxy.Invoke(CommonConstants.ExchangeMethodName, message).Wait();

            //消息已发送事件
            OnMessageSent?.Invoke(message);
        }
        #endregion

        #region 发送消息 —— static async Task SendAsync<T>(T message)
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <typeparam name="T">消息类型</typeparam>
        /// <param name="message">消息</param>
        public static async Task SendAsync<T>(T message) where T : IMessage
        {
            #region # 验证

            if (!_HubConnectionBuilt)
            {
                throw new InvalidOperationException("Hub连接未构建，不可发送消息！");
            }

            #endregion

            string hubName = MessageExtension.GetHubName<T>();

            #region # 验证

            if (!_SenderProxies.ContainsKey(hubName))
            {
                throw new InvalidOperationException($"要发送的消息类型\"{typeof(T).Name}\"未注册！");
            }

            #endregion

            IHubProxy proxy = _SenderProxies[hubName];
            await proxy.Invoke(CommonConstants.ExchangeMethodName, message);

            //消息已发送事件
            OnMessageSent?.Invoke(message);
        }
        #endregion

        #region 接收消息 —— static void Receive<T>(Action<T> onReceive)
        /// <summary>
        /// 接收消息
        /// </summary>
        /// <typeparam name="T">消息类型</typeparam>
        /// <param name="onReceive">消息接收事件</param>
        public static void Receive<T>(Action<T> onReceive) where T : IMessage
        {
            #region # 验证

            if (!_HubConnectionBuilt)
            {
                throw new InvalidOperationException("Hub连接未构建，不可接收消息！");
            }

            #endregion

            string hubName = MessageExtension.GetHubName<T>();

            #region # 验证

            if (!_ReceiverProxies.ContainsKey(hubName))
            {
                throw new InvalidOperationException($"要接收的消息类型\"{typeof(T).Name}\"未注册！");
            }

            #endregion

            IHubProxy proxy = _ReceiverProxies[hubName];
            proxy.On<T>(CommonConstants.ExchangeMethodName, onReceive);
        }
        #endregion 

        #endregion
    }
}
