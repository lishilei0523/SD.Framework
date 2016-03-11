using System.Collections.Generic;
using System.Linq;
using ShSoft.Framework2015.Infrastructure.IDomainEvent;

// ReSharper disable once CheckNamespace
namespace ShSoft.Framework2015.Infrastructure.DomainEventTests
{
    /// <summary>
    /// 领域事件存储者桩实现
    /// </summary>
    public class StubDomainEventStorer : IDomainEventStorer
    {
        /// <summary>
        /// 领域事件源集合
        /// </summary>
        private static readonly ICollection<IDomainEvent.IDomainEvent> _EventSources;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static StubDomainEventStorer()
        {
            _EventSources = new HashSet<IDomainEvent.IDomainEvent>();
        }

        /// <summary>
        /// 初始化存储
        /// </summary>
        public void InitStore()
        {
            _EventSources.Clear();
        }

        /// <summary>
        /// 挂起领域事件
        /// </summary>
        /// <typeparam name="T">领域事件源类型</typeparam>
        /// <param name="domainSource">领域事件源</param>
        public void Suspend<T>(T domainSource) where T : class, IDomainEvent.IDomainEvent
        {
            _EventSources.Add(domainSource);
        }

        /// <summary>
        /// 处理未处理的领域事件
        /// </summary>
        public void HandleUncompletedEvents()
        {
            IEnumerable<IDomainEvent.IDomainEvent> eventSources = _EventSources.Where(x => !x.Handled);

            //如果有未处理的
            if (eventSources.Any())
            {
                foreach (IDomainEvent.IDomainEvent eventSource in eventSources)
                {
                    eventSource.Handle();
                }
            }

            //递归
            if (_EventSources.Any(x => !x.Handled))
            {
                this.HandleUncompletedEvents();
            }
        }

        /// <summary>
        /// 清空未处理的领域事件
        /// </summary>
        public void ClearUncompletedEvents()
        {
            List<IDomainEvent.IDomainEvent> eventSources = _EventSources.Where(x => !x.Handled).ToList();

            foreach (IDomainEvent.IDomainEvent eventSource in eventSources)
            {
                _EventSources.Remove(eventSource);
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {

        }
    }
}
