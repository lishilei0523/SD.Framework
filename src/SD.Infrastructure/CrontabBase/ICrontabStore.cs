using System;
using System.Collections.Generic;

namespace SD.Infrastructure.CrontabBase
{
    /// <summary>
    /// 定时任务存储接口
    /// </summary>
    public interface ICrontabStore : IDisposable
    {
        #region # 保存定时任务 —— void Store(ICrontab crontab) 
        /// <summary>
        /// 挂起定时任务
        /// </summary>
        /// <param name="crontab">定时任务</param>
        void Store(ICrontab crontab);
        #endregion

        #region # 删除定时任务 —— void Remove(ICrontab crontab)
        /// <summary>
        /// 删除定时任务
        /// </summary>
        /// <param name="crontab">定时任务</param>
        void Remove(ICrontab crontab);
        #endregion

        #region # 清空任务 —— void Clear()
        /// <summary>
        /// 清空任务
        /// </summary>
        void Clear();
        #endregion

        #region # 获取全部定时任务列表 —— IList<ICrontab> FindAll()
        /// <summary>
        /// 获取全部定时任务列表
        /// </summary>
        /// <returns>定时任务列表</returns>
        IList<ICrontab> FindAll();
        #endregion
    }
}
