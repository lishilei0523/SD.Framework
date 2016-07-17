using System;

namespace ShSoft.Infrastructure.DomainEventBase
{
    /// <summary>
    /// 领域事件存储接口
    /// </summary>
    public interface IDomainEventStore : IDisposable
    {
        #region # 初始化存储 —— void Init()
        /// <summary>
        /// 初始化存储
        /// </summary>
        void Init();
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
