using System;

namespace ShSoft.Framework2015.AOP.Models.ValueObjects
{
    /// <summary>
    /// 异常消息
    /// </summary>
    [Serializable]
    public struct ExceptionMessage
    {
        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="errorMessage">错误消息</param>
        /// <param name="logId">日志Id</param>
        public ExceptionMessage(string errorMessage, Guid logId)
        {
            this.ErrorMessage = errorMessage;
            this.LogId = logId;
        }

        /// <summary>
        /// 异常消息
        /// </summary>
        public string ErrorMessage;

        /// <summary>
        /// 错误日志Id
        /// </summary>
        public Guid LogId;
    }
}
