using System;

namespace ShSoft.Framework2015.AOP.Models.Entities
{
    /// <summary>
    /// 运行日志
    /// </summary>
    [Serializable]
    public class RunningLog : BaseLog
    {
        #region 01.方法返回值 —— string ReturnValue
        /// <summary>
        /// 方法返回值
        /// </summary>
        public string ReturnValue { get; set; }
        #endregion

        #region 02.方法返回值类型 —— string ReturnValueType
        /// <summary>
        /// 方法返回值
        /// </summary>
        public string ReturnValueType { get; set; }
        #endregion

        #region 03.操作人Id —— Guid UserId
        /// <summary>
        /// 操作人Id
        /// </summary>
        public Guid UserId { get; set; }
        #endregion

        #region 04.操作人姓名 —— string UserName
        /// <summary>
        /// 操作人姓名
        /// </summary>
        public string UserName { get; set; }
        #endregion

        #region 05.开始时间 —— DateTime StartTime
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public string StartTimeString { get; set; }
        #endregion

        #region 06.结束时间 —— DateTime EndTime
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public string EndTimeString { get; set; }
        #endregion
    }
}
