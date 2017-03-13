using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SD.Infrastructure.Constants;
using SD.Infrastructure.EventBase;
using SD.Infrastructure.EventBase.Mediator;

// ReSharper disable once CheckNamespace
namespace SD.Infrastructure.EventStoreProvider
{
    /// <summary>
    /// 领域事件存储 - RabbitMQ提供者
    /// </summary>
    public class RabbitStoreProvider : IEventStore
    {
        #region # 字段及构造器

        /// <summary>
        /// 二进制序列化器
        /// </summary>
        private static readonly BinaryFormatter _BinaryFormatter;

        /// <summary>
        /// RabbitMQ连接工厂
        /// </summary>
        private static readonly ConnectionFactory _ConnectionFactory;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static RabbitStoreProvider()
        {
            _BinaryFormatter = new BinaryFormatter();
            _ConnectionFactory = new ConnectionFactory
            {
                HostName = RabbitConfiguration.Setting.HostName,
                UserName = RabbitConfiguration.Setting.UserName,
                Password = RabbitConfiguration.Setting.Password
            };
        }


        /// <summary>
        /// 会话Id
        /// </summary>
        private readonly string _sessionId;

        /// <summary>
        /// RabbitMQ连接
        /// </summary>
        private readonly IConnection _connection;

        /// <summary>
        /// RabbitMQ连接信道
        /// </summary>
        private readonly IModel _channel;

        /// <summary>
        /// RabbitMQ队列
        /// </summary>
        private readonly QueueDeclareOk _queue;

        /// <summary>
        /// 构造器
        /// </summary>
        public RabbitStoreProvider()
        {
            this._sessionId = WebConfigSetting.CurrentSessionId.ToString();
            this._connection = _ConnectionFactory.CreateConnection();
            this._channel = this._connection.CreateModel();
            this._queue = this._channel.QueueDeclare(this._sessionId, true, false, true, null);
        }

        #endregion


        //Implements

        #region # 挂起领域事件 —— void Suspend<T>(T eventSource)
        /// <summary>
        /// 挂起领域事件
        /// </summary>
        /// <typeparam name="T">领域事件源类型</typeparam>
        /// <param name="eventSource">领域事件源</param>
        public void Suspend<T>(T eventSource) where T : class, IEvent
        {
            byte[] messageBody = this.ToByteArray(eventSource);

            this._channel.BasicPublish(string.Empty, this._sessionId, null, messageBody);
        }
        #endregion

        #region # 处理未处理的领域事件 —— void HandleUncompletedEvents()
        /// <summary>
        /// 处理未处理的领域事件
        /// </summary>
        public void HandleUncompletedEvents()
        {
            QueueingBasicConsumer consumer = new QueueingBasicConsumer(this._channel);

            this.HandleRecursively(consumer);
        }
        #endregion

        #region # 清空未处理的领域事件 —— void ClearUncompletedEvents()
        /// <summary>
        /// 清空未处理的领域事件
        /// </summary>
        public void ClearUncompletedEvents()
        {
            this._channel.QueueDelete(this._queue);
        }
        #endregion

        #region # 释放资源 —— void Dispose()
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (this._connection != null)
            {
                this._connection.Close();
                this._connection.Dispose();
            }
            if (this._channel != null)
            {
                this._channel.Close();
                this._channel.Dispose();
            }
        }
        #endregion


        //private

        #region # object序列化为byte数组扩展方法 —— byte[] ToByteArray(object instance)
        /// <summary>
        /// object序列化为byte数组扩展方法
        /// </summary>
        /// <param name="instance">object及其子类</param>
        /// <returns>byte数组</returns>
        /// <exception cref="ArgumentNullException">源对象为空</exception>
        /// <exception cref="SerializationException">对象类型未标记"Serializable"特性</exception>
        private byte[] ToByteArray(object instance)
        {
            #region # 验证参数

            if (instance == null)
            {
                throw new ArgumentNullException("instance", @"源对象不可为空！");
            }

            #endregion

            using (MemoryStream stream = new MemoryStream())
            {
                try
                {
                    _BinaryFormatter.Serialize(stream, instance);
                    return stream.ToArray();
                }
                catch (SerializationException)
                {
                    throw new SerializationException(string.Format("给定对象类型\"{0}\"未标记\"Serializable\"特性！", instance.GetType().Name));
                }
            }
        }
        #endregion

        #region # byte数组反序列化为对象扩展方法 —— T ToObject<T>(byte[] buffer)
        /// <summary>
        /// byte数组反序列化为对象扩展方法
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="buffer">byte数组</param>
        /// <returns>对象</returns>
        public T ToObject<T>(byte[] buffer)
        {
            #region # 验证参数

            if (buffer == null)
            {
                throw new ArgumentNullException("buffer", @"byte数组不可为null！");
            }

            #endregion

            using (MemoryStream stream = new MemoryStream(buffer))
            {
                object instance = _BinaryFormatter.Deserialize(stream);
                return (T)instance;
            }
        }
        #endregion

        #region # 递归处理领域事件 —— void HandleRecursively(QueueingBasicConsumer consumer)
        /// <summary>
        /// 递归处理领域事件
        /// </summary>
        /// <param name="consumer">消息消费者</param>
        private void HandleRecursively(QueueingBasicConsumer consumer)
        {
            this._channel.BasicConsume(this._sessionId, false, consumer);

            BasicDeliverEventArgs eventArg;

            if (consumer.Queue.Dequeue(1000, out eventArg))
            {
                IEvent eventSource = this.ToObject<IEvent>(eventArg.Body);

                EventMediator.Handle(eventSource);

                this._channel.BasicAck(eventArg.DeliveryTag, false);

                this.HandleRecursively(consumer);
            }
        }
        #endregion
    }
}
