using System;
using System.ServiceModel;

namespace ShSoft.Framework2015.Infrastructure.WCF
{
    /// <summary>
    /// WCF服务客户端代理
    /// </summary>
    /// <typeparam name="T">服务契约类型</typeparam>
    public sealed class ServiceProxy<T> : IDisposable
    {
        #region # 字段及构造器

        /// <summary>
        /// 信道实例
        /// </summary>
        private T _channel;

        /// <summary>
        /// 同步锁
        /// </summary>
        private static readonly object _Sync;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static ServiceProxy()
        {
            _Sync = new object();
        }

        #endregion

        #region # 只读属性 - 信道 —— T Channel
        /// <summary>
        /// 只读属性 - 信道
        /// </summary>
        public T Channel
        {
            get
            {
                lock (_Sync)
                {
                    if (this._channel != null)
                    {
                        this._channel.CloseChannel();
                    }
                    ChannelFactory<T> factory = ChannelFactoryManager.Current.GetFactory<T>();
                    this._channel = factory.CreateChannel();
                    ((IClientChannel)this._channel).Open();
                    return this._channel;
                }
            }
        }
        #endregion

        #region # 关闭客户端信道 —— void Close()
        /// <summary>
        /// 关闭客户端信道
        /// </summary>
        public void Close()
        {
            if (this._channel != null)
            {
                this._channel.CloseChannel();
            }
        }
        #endregion

        #region # 释放资源 —— void Dispose()
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            this.Close();
        }
        #endregion
    }
}
