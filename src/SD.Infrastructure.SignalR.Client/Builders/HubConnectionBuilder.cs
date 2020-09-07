using Microsoft.AspNet.SignalR.Client;
using SD.Infrastructure.MessageBase;
using SD.Infrastructure.SignalR.Client.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace SD.Infrastructure.SignalR.Client.Builders
{
    /// <summary>
    /// Hub连接建造者
    /// </summary>
    public sealed class HubConnectionBuilder : IDisposable
    {
        #region # 字段及构造器

        #region 00.字段

        /// <summary>
        /// 发送者Hub代理字典
        /// </summary>
        private readonly IDictionary<string, IHubProxy> _senderProxies;

        /// <summary>
        /// 接收者Hub代理字典
        /// </summary>
        private readonly IDictionary<string, IHubProxy> _receiverProxies;

        #endregion

        #region 01.创建Hub连接建造者构造器
        /// <summary>
        /// 创建Hub连接建造者构造器
        /// </summary>
        /// <param name="url">连接地址</param>
        public HubConnectionBuilder(string url)
        {
            this.HubConnection = new HubConnection(url);
            this.HubConnectionBuilt = false;
            this._senderProxies = new ConcurrentDictionary<string, IHubProxy>();
            this._receiverProxies = new ConcurrentDictionary<string, IHubProxy>();
        }
        #endregion

        #endregion

        #region # 属性

        #region Hub连接 —— HubConnection HubConnection
        /// <summary>
        /// Hub连接
        /// </summary>
        public HubConnection HubConnection { get; private set; }
        #endregion

        #region Hub连接是否已构建 —— bool HubConnectionBuilt
        /// <summary>
        /// Hub连接是否已构建
        /// </summary>
        public bool HubConnectionBuilt { get; private set; }
        #endregion

        #region 发送者Hub代理字典 —— IDictionary<string, IHubProxy> SenderProxies
        /// <summary>
        /// 发送者Hub代理字典
        /// </summary>
        public IDictionary<string, IHubProxy> SenderProxies
        {
            get
            {
                return this._senderProxies;
            }
        }
        #endregion

        #region 接收者Hub代理字典 —— IDictionary<string, IHubProxy> ReceiverProxies
        /// <summary>
        /// 接收者Hub代理字典
        /// </summary>
        public IDictionary<string, IHubProxy> ReceiverProxies
        {
            get
            {
                return this._receiverProxies;
            }
        }
        #endregion

        #endregion

        #region # 方法

        #region 注册发送消息类型 —— void RegisterMessagesToSend(IEnumerable<Type> messageTypes)
        /// <summary>
        /// 注册发送消息类型
        /// </summary>
        /// <param name="messageTypes">消息类型列表</param>
        public void RegisterMessagesToSend(IEnumerable<Type> messageTypes)
        {
            #region # 验证

            if (this.HubConnectionBuilt)
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
                IHubProxy hubProxy = this.HubConnection.CreateHubProxy(hubName);

                this._senderProxies.Add(hubName, hubProxy);
            }
        }
        #endregion

        #region 注册接收消息类型 —— void RegisterMessagesToReceive(IEnumerable<Type> messageTypes)
        /// <summary>
        /// 注册接收消息类型
        /// </summary>
        /// <param name="messageTypes">消息类型列表</param>
        public void RegisterMessagesToReceive(IEnumerable<Type> messageTypes)
        {
            #region # 验证

            if (this.HubConnectionBuilt)
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
                IHubProxy hubProxy = this.HubConnection.CreateHubProxy(hubName);

                this._receiverProxies.Add(hubName, hubProxy);
            }
        }
        #endregion

        #region 注册公钥 —— void RegisterPublicKey(Guid publicKey)
        /// <summary>
        /// 注册公钥
        /// </summary>
        /// <param name="publicKey">公钥</param>
        public void RegisterPublicKey(Guid publicKey)
        {
            #region # 验证

            if (this.HubConnectionBuilt)
            {
                throw new InvalidOperationException("Hub连接已构建，不可注册公钥！");
            }

            #endregion

            this.HubConnection.RegisterPublicKey(publicKey);
        }
        #endregion

        #region 注册异常处理者 —— void RegisterExceptionHandler()
        /// <summary>
        /// 注册异常处理者
        /// </summary>
        public void RegisterExceptionHandler()
        {
            #region # 验证

            if (this.HubConnectionBuilt)
            {
                throw new InvalidOperationException("Hub连接已构建，不可注册异常处理者！");
            }

            #endregion

            this.HubConnection.RegisterExceptionHandler();
        }
        #endregion

        #region 注册保持断线重连 —— void RegisterKeepReconnecting(...
        /// <summary>
        /// 注册保持断线重连
        /// </summary>
        /// <param name="delay">延迟时间（单位：毫秒）</param>
        /// <param name="timeout">超时时间（单位：毫秒）</param>
        public void RegisterKeepReconnecting(int delay = 5000, int timeout = 5000)
        {
            #region # 验证

            if (this.HubConnectionBuilt)
            {
                throw new InvalidOperationException("Hub连接已构建，不可注册保持断线重连！");
            }

            #endregion

            this.HubConnection.RegisterKeepReconnecting(delay, timeout);
        }
        #endregion

        #region 注册状态变更处理者 —— void RegisterStateChangedHandler()
        /// <summary>
        /// 注册状态变更处理者
        /// </summary>
        public void RegisterStateChangedHandler()
        {
            #region # 验证

            if (this.HubConnectionBuilt)
            {
                throw new InvalidOperationException("Hub连接已构建，不可注册状态变更处理者！");
            }

            #endregion

            this.HubConnection.RegisterStateChangedHandler();
        }
        #endregion

        #region 构建Hub连接 —— HubConnection Build()
        /// <summary>
        /// 构建Hub连接
        /// </summary>
        /// <returns>Hub连接</returns>
        public HubConnection Build()
        {
            #region # 验证

            if (this.HubConnectionBuilt)
            {
                throw new InvalidOperationException("Hub连接已构建，不可重新构建！");
            }

            #endregion

            this.HubConnection.Start().Wait();
            this.HubConnectionBuilt = true;

            return this.HubConnection;
        }
        #endregion

        #region 释放资源 —— void Dispose()
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            this.HubConnection.Dispose();
            this.HubConnectionBuilt = false;
        }
        #endregion

        #endregion
    }
}
