using System;

namespace ShSoft.Framework2016.Infrastructure.IDomainEvent
{
    /// <summary>
    /// 领域事件存储者接口
    /// </summary>
    public interface IDomainEventStorer : IDisposable
    {
        #region # 初始化存储 —— void InitStore()
        /// <summary>
        /// 初始化存储
        /// </summary>
        void InitStore();
        #endregion

        #region # 挂起领域事件 —— void Suspend<T>(T domainSource)
        /// <summary>
        /// 挂起领域事件
        /// </summary>
        /// <typeparam name="T">领域事件源类型</typeparam>
        /// <param name="domainSource">领域事件源</param>
        void Suspend<T>(T domainSource) where T : class, IDomainEvent;
        #endregion

        #region # 处理未处理的领域事件 —— void HandleUncompletedEvents()
        /// <summary>
        /// 处理未处理的领域事件
        /// </summary>
        void HandleUncompletedEvents();
        #endregion

        #region # 清空未处理的领域事件 —— void ClearUncompletedEvents()
        /// <summary>
        /// 清空未处理的领域事件
        /// </summary>
        void ClearUncompletedEvents();
        #endregion
    }
}
