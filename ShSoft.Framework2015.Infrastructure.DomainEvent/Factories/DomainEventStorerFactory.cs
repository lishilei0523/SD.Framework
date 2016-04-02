﻿using SD.IOC.Core.Mediator;
using ShSoft.Framework2015.Infrastructure.IDomainEvent;

namespace ShSoft.Framework2015.Infrastructure.DomainEvent.Factories
{
    /// <summary>
    /// 领域事件存储者工厂
    /// </summary>
    internal static class DomainEventStorerFactory
    {
        #region # 获取领域事件存储者实例 —— IDomainEventStorer GetEventEventStorer()
        /// <summary>
        /// 获取领域事件存储者实例
        /// </summary>
        /// <returns>领域事件存储者实例</returns>
        public static IDomainEventStorer GetEventEventStorer()
        {
            return ResolveMediator.Resolve<IDomainEventStorer>();
        }
        #endregion
    }
}
