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
        /// 保存定时任务
        /// </summary>
        /// <param name="crontab">定时任务</param>
        void Store(ICrontab crontab);
        #endregion

        #region # 获取定时任务 —— T Get<T>(string crontabId)
        /// <summary>
        /// 获取定时任务
        /// </summary>
        /// <typeparam name="T">定时任务类型</typeparam>
        /// <param name="crontabId">定时任务Id</param>
        /// <returns>定时任务</returns>
        T Get<T>(string crontabId) where T : ICrontab;
        #endregion

        #region # 删除定时任务 —— void Remove(ICrontab crontab)
        /// <summary>
        /// 删除定时任务
        /// </summary>
        /// <param name="crontab">定时任务</param>
        void Remove(ICrontab crontab);
        #endregion

        #region # 删除定时任务 —— void Remove(string crontabId)
        /// <summary>
        /// 删除定时任务
        /// </summary>
        /// <param name="crontabId">定时任务Id</param>
        void Remove(string crontabId);
        #endregion

        #region # 清空定时任务 —— void Clear()
        /// <summary>
        /// 清空定时任务
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
