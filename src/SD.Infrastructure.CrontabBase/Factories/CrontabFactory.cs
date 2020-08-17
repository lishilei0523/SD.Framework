using System;

namespace SD.Infrastructure.CrontabBase.Factories
{
    /// <summary>
    /// 定时任务工厂
    /// </summary>
    public static class CrontabFactory
    {
        #region # 创建定时任务 —— static ICrontab CreateCrontab(string assemblyName...
        /// <summary>
        /// 创建定时任务
        /// </summary>
        /// <param name="assemblyName">程序集名称</param>
        /// <param name="typeName">类型名称</param>
        /// <param name="strategy">执行策略</param>
        /// <returns>定时任务</returns>
        public static ICrontab CreateCrontab(string assemblyName, string typeName, ExecutionStrategy strategy)
        {
            #region # 验证

            if (string.IsNullOrWhiteSpace(assemblyName))
            {
                throw new ArgumentNullException(nameof(assemblyName), "程序集名称不可为空！");
            }
            if (string.IsNullOrWhiteSpace(typeName))
            {
                throw new ArgumentNullException(nameof(typeName), "类型名称不可为空！");
            }
            if (strategy == null)
            {
                throw new ArgumentNullException(nameof(strategy), "执行策略不可为空！");
            }

            #endregion

            Type crontabType = Type.GetType($"{typeName},{assemblyName}");

            #region # 验证

            if (crontabType == null)
            {
                throw new NullReferenceException("给定类型的定时任务不存在！");
            }
            if (!typeof(ICrontab).IsAssignableFrom(crontabType))
            {
                throw new ArgumentOutOfRangeException(nameof(typeName), "给定类型不是定时任务类型！");
            }

            #endregion

            object instance = Activator.CreateInstance(crontabType, strategy);

            #region # 验证

            if (instance == null)
            {
                throw new NullReferenceException("给定类型的定时任务不存在！");
            }

            #endregion

            ICrontab crontab = (ICrontab)instance;

            return crontab;
        }
        #endregion
    }
}
