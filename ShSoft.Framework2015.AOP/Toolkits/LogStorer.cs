using System;
using ShSoft.Framework2015.AOP.Models.Entities;

namespace ShSoft.Framework2015.AOP.Toolkits
{
    /// <summary>
    /// 日志持久化帮助类
    /// </summary>
    internal static class LogStorer
    {
        #region # 将异常日志插入数据库 —— static Guid InsertIntoDb(ExceptionLog log)
        /// <summary>
        /// 将异常日志插入数据库
        /// </summary>
        /// <param name="log">异常日志</param>
        /// <returns>日志Id</returns>
        public static Guid InsertIntoDb(ExceptionLog log)
        {
            //初始化数据表
            LogWriter.InitTable();

            //记录日志
            return LogWriter.Error(log);
        }
        #endregion

        #region # 将运行日志插入数据库 —— static Guid InsertIntoDb(RunningLog log)
        /// <summary>
        /// 将运行日志插入数据库
        /// </summary>
        /// <param name="log">运行日志</param>
        /// <returns>日志Id</returns>
        public static Guid InsertIntoDb(RunningLog log)
        {
            //初始化数据表
            LogWriter.InitTable();

            //记录日志
            return LogWriter.Info(log);
        }
        #endregion
    }
}
