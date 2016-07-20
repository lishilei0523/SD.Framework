// ReSharper disable once CheckNamespace
namespace ShSoft.Infrastructure.EventStoreProvider
{
    /// <summary>
    /// 领域事件存储 - RabbitMQ提供者
    /// </summary>
    public class RabbitMQStoreProvider
    {

    }
}



//using System;
//using System.Linq;
//using System.Messaging;
//using System.Runtime.Remoting.Messaging;
//using ShSoft.Framework2016.Infrastructure.Constants;
//using ShSoft.Framework2016.Infrastructure.IEvent;

//namespace ShSoft.Framework2016.Infrastructure.Event.MSMQStorer.Provider
//{
//    /// <summary>
//    /// 领域事件存储者 - MSMQ提供者
//    /// </summary>
//    public class MSMQStorerProvider : IEventStorer
//    {
//        #region # 字段及构造器

//        /// <summary>
//        /// 消息路径
//        /// </summary>
//        private readonly string _path;

//        /// <summary>
//        /// 消息对象
//        /// </summary>
//        private readonly MessageQueue _messageQueue;

//        /// <summary>
//        /// 构造器
//        /// </summary>
//        public MSMQStorerProvider()
//        {
//            #region # SessionId处理

//            object sessionIdCache = CallContext.GetData(CacheConstants.SessionIdKey);

//            if (sessionIdCache == null)
//            {
//                throw new ApplicationException("SessionId未设置，请检查程序！");
//            }

//            Guid sessionId = (Guid)sessionIdCache;

//            #endregion

//            this._path = @".\Private$\" + sessionId;

//            this._messageQueue = !MessageQueue.Exists(this._path) ? MessageQueue.Create(this._path) : new MessageQueue(this._path);

//            this._messageQueue.Formatter = new BinaryMessageFormatter();
//        }

//        #endregion

//        #region # 初始化存储 —— void InitStore()
//        /// <summary>
//        /// 初始化存储
//        /// </summary>
//        public void InitStore()
//        {

//        }
//        #endregion

//        #region # 挂起领域事件 —— void Suspend<T>(T eventSource)
//        /// <summary>
//        /// 挂起领域事件
//        /// </summary>
//        /// <typeparam name="T">领域事件源类型</typeparam>
//        /// <param name="eventSource">领域事件源</param>
//        public void Suspend<T>(T eventSource) where T : class, IEvent.IEvent
//        {
//            this._messageQueue.Send(eventSource);
//        }
//        #endregion

//        #region # 处理未处理的领域事件 —— void HandleUncompletedEvents()
//        /// <summary>
//        /// 处理未处理的领域事件
//        /// </summary>
//        public void HandleUncompletedEvents()
//        {
//            while (this._messageQueue.GetAllMessages().Any())
//            {
//                Message message = this._messageQueue.Receive();

//                if (message != null)
//                {
//                    IEvent.IEvent eventSource = (IEvent.Event)message.Body;

//                    eventSource.Handle();
//                }
//            }

//            this.Clear();
//        }
//        #endregion

//        #region # 清空未处理的领域事件 —— void ClearUncompletedEvents()
//        /// <summary>
//        /// 清空未处理的领域事件
//        /// </summary>
//        public void ClearUncompletedEvents()
//        {
//            this._messageQueue.Purge();
//            this.Clear();
//        }
//        #endregion

//        #region # 释放资源 —— void Dispose()
//        /// <summary>
//        /// 释放资源
//        /// </summary>
//        public void Dispose()
//        {
//            this._messageQueue.Close();
//            this._messageQueue.Dispose();
//        }
//        #endregion

//        #region # 清理队列 —— void Clear()
//        /// <summary>
//        /// 清理队列
//        /// </summary>
//        private void Clear()
//        {
//            if (MessageQueue.Exists(this._path))
//            {
//                MessageQueue.Delete(this._path);
//            }
//        }
//        #endregion
//    }
//}

