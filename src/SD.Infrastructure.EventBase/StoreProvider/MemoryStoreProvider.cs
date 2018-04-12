using SD.Infrastructure.EventBase;
using SD.Infrastructure.EventBase.Mediator;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;

// ReSharper disable once CheckNamespace
namespace SD.Infrastructure.EventStoreProvider
{
    /// <summary>
    /// 领域事件存储者 - 内存提供者
    /// </summary>
    public class MemoryStoreProvider : IEventStore
    {
        #region # 字段与常量

        /// <summary>
        /// 同步锁
        /// </summary>
        private static readonly object _Sync = new object();

        /// <summary>
        /// 领域事件存储Session键
        /// </summary>
        private const string EventSessionKey = "Event";

        /// <summary>
        /// 领域事件源集合
        /// </summary>
        private IList<Event> _eventSources;

        /// <summary>
        /// 静态构造器
        /// </summary>
        public MemoryStoreProvider()
        {
            this._eventSources = new List<Event>();
        }

        #endregion

        #region # 挂起领域事件 —— void Suspend<T>(T eventSource)
        /// <summary>
        /// 挂起领域事件
        /// </summary>
        /// <typeparam name="T">领域事件源类型</typeparam>
        /// <param name="eventSource">领域事件源</param>
        public void Suspend<T>(T eventSource) where T : class, IEvent
        {
            lock (_Sync)
            {
                //获取线程缓存
                object eventSources = CallContext.GetData(EventSessionKey);

                //如果缓存不为空，则将事件源队列变量赋值为缓存
                if (eventSources != null)
                {
                    this._eventSources = (IList<Event>)eventSources;
                }

                //将新事件源添加到队列
                this._eventSources.Add(eventSource as Event);

                //将新队列添加到缓存
                CallContext.SetData(EventSessionKey, this._eventSources);
            }
        }
        #endregion

        #region # 处理未处理的领域事件 —— void HandleUncompletedEvents()
        /// <summary>
        /// 处理未处理的领域事件
        /// </summary>
        public void HandleUncompletedEvents()
        {
            lock (_Sync)
            {
                //获取线程缓存
                object eventSources = CallContext.GetData(EventSessionKey);

                //如果缓存中没有数据，则终止方法
                if (eventSources == null)
                {
                    return;
                }

                //如果缓存不为空，则将事件源队列变量赋值为缓存
                this._eventSources = (IList<Event>)eventSources;

                //如果有未处理的
                if (this._eventSources.Any(x => !x.Handled))
                {
                    foreach (Event eventSource in this._eventSources.Where(x => !x.Handled))
                    {
                        EventMediator.Handle((IEvent)eventSource);
                        eventSource.Handled = true;
                    }
                }

                //递归
                if (this._eventSources.Any(x => !x.Handled))
                {
                    this.HandleUncompletedEvents();
                }

                //处理完毕后置空缓存
                CallContext.FreeNamedDataSlot(EventSessionKey);
            }
        }
        #endregion

        #region # 清空未处理的领域事件 —— void ClearUncompletedEvents()
        /// <summary>
        /// 清空未处理的领域事件
        /// </summary>
        public void ClearUncompletedEvents()
        {
            lock (_Sync)
            {
                //置空缓存
                CallContext.FreeNamedDataSlot(EventSessionKey);
            }
        }
        #endregion

        #region # 释放资源 —— void Dispose()
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose() { }
        #endregion
    }
}
