using System;
using System.Activities;

namespace SD.Infrastructure.Workflow.Toolkits
{
    /// <summary>
    /// 工作流活动工厂
    /// </summary>
    public static class ActivityFactory
    {
        #region # 创建工作流活动 —— static Activity CreateActivity(string assemblyName, string typeName)
        /// <summary>
        /// 创建工作流活动
        /// </summary>
        /// <param name="assemblyName">程序集名称</param>
        /// <param name="typeName">类型名称</param>
        /// <returns>工作流活动</returns>
        public static Activity CreateActivity(string assemblyName, string typeName)
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

            #endregion

            Type activityType = Type.GetType($"{typeName},{assemblyName}");

            #region # 验证

            if (activityType == null)
            {
                throw new NullReferenceException("给定类型的工作流活动不存在！");
            }
            if (!activityType.IsSubclassOf(typeof(Activity)))
            {
                throw new ArgumentOutOfRangeException(nameof(typeName), "给定类型不是工作流活动类型");
            }

            #endregion

            object instance = Activator.CreateInstance(activityType);

            #region # 验证

            if (instance == null)
            {
                throw new NullReferenceException("给定类型的工作流活动不存在！");
            }

            #endregion

            Activity activity = (Activity)instance;

            return activity;
        }
        #endregion
    }
}
