using SD.IOC.Core.Mediators;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SD.Infrastructure.CrontabBase.Factories
{
    /// <summary>
    /// 定时任务执行者工厂
    /// </summary>
    internal static class CrontabExecutorFactory
    {
        #region # 获取定时任务执行者实例列表 —— IEnumerable<ICrontabExecutor<T>> GetCrontabExecutors...
        /// <summary>
        /// 获取定时任务执行者实例列表
        /// </summary>
        /// <typeparam name="T">定时任务类型</typeparam>
        /// <param name="crontab">定时任务</param>
        /// <returns>定时任务执行者列表</returns>
        public static IEnumerable<ICrontabExecutor<T>> GetCrontabExecutorsFor<T>(T crontab) where T : class, ICrontab
        {
            IEnumerable<ICrontabExecutor<T>> schedulers = ResolveMediator.ResolveAll<ICrontabExecutor<T>>();

            return schedulers;
        }
        #endregion

        #region # 获取定时任务执行者实例列表 —— IEnumerable<ICrontabExecutor> GetCrontabExecutors...
        /// <summary>
        /// 获取定时任务执行者实例列表
        /// </summary>
        /// <param name="crontabType">定时任务类型</param>
        /// <returns>定时任务执行者列表</returns>
        public static IEnumerable<ICrontabExecutor> GetCrontabExecutorsFor(Type crontabType)
        {
            #region # 验证类型

            if (!typeof(ICrontab).IsAssignableFrom(crontabType))
            {
                throw new InvalidOperationException($"类型\"{crontabType.FullName}\"不实现定时任务基接口！");
            }

            #endregion

            Type schedulerType = typeof(ICrontabExecutor<>).MakeGenericType(crontabType);
            IEnumerable<object> schedulerInstances = ResolveMediator.ResolveAll(schedulerType);
            IEnumerable<ICrontabExecutor> schedulers = schedulerInstances.Select(handler => (ICrontabExecutor)handler);

            return schedulers;
        }
        #endregion
    }
}
