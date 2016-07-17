using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using RabbitMQ.Client;

namespace ShSoft.Infrastructure.CommandBase.QueueProvider
{
    /// <summary>
    /// RabbitMQ命令队列提供者
    /// </summary>
    public class RabbitMQQueueProvider : ICommandQueue
    {
        #region # 静态字段及静态构造器

        /// <summary>
        /// 二进制序列化器
        /// </summary>
        private static readonly BinaryFormatter _BinaryFormatter;

        /// <summary>
        /// RabbitMQ连接工厂
        /// </summary>
        private static readonly ConnectionFactory _ConnectionFactory;


        /// <summary>
        /// 静态构造函数
        /// </summary>
        static RabbitMQQueueProvider()
        {
            _BinaryFormatter = new BinaryFormatter();
            _ConnectionFactory = new ConnectionFactory();
            _ConnectionFactory.HostName = "192.168.8.210";
            _ConnectionFactory.UserName = "admin";
            _ConnectionFactory.Password = "123456";
        }

        #endregion

        #region # 字段及构造器

        /// <summary>
        /// RabbitMQ连接
        /// </summary>
        private readonly IConnection _connection;

        /// <summary>
        /// RabbitMQ信道
        /// </summary>
        private readonly IModel _channel;

        /// <summary>
        /// 构造器
        /// </summary>
        public RabbitMQQueueProvider()
        {
            this._connection = _ConnectionFactory.CreateConnection();
            this._channel = this._connection.CreateModel();
        }

        #endregion

        #region # 挂起命令 —— void Suspend<T>(T command)
        /// <summary>
        /// 挂起命令
        /// </summary>
        /// <typeparam name="T">命令类型</typeparam>
        /// <param name="command">命令对象</param>
        public void Suspend<T>(T command) where T : class, ICommand
        {
            //声明队列
            this._channel.QueueDeclare(command.Id.ToString(), false, false, false, null);

            //序列化消息
            byte[] messageBody = this.ToBuffer(command);

            //发布
            this._channel.BasicPublish(string.Empty, command.Id.ToString(), null, messageBody);
        }
        #endregion

        #region # 处理未处理的命令 —— void HandleUncompletedCommands()
        /// <summary>
        /// 处理未处理的命令
        /// </summary>
        public void HandleUncompletedCommands()
        {
            //this._channel.
        }
        #endregion


        public void Dispose()
        {

        }

        #region # object序列化为byte数组 —— byte[] ToBuffer(object instance)
        /// <summary>
        /// object序列化为byte数组
        /// </summary>
        /// <param name="instance">object及其子类</param>
        /// <returns>byte数组</returns>
        /// <exception cref="ArgumentNullException">源对象为空</exception>
        /// <exception cref="SerializationException">对象类型未标记"Serializable"特性</exception>
        private byte[] ToBuffer(object instance)
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
    }
}
