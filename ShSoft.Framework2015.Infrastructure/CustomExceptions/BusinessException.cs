using System;

namespace ShSoft.Framework2015.Infrastructure.CustomExceptions
{
    /// <summary>
    /// Business层异常基类
    /// </summary>
    [Serializable]
    public class BusinessException : ApplicationException
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message">异常消息</param>
        public BusinessException(string message) : base(message) { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message">异常消息</param>
        /// <param name="innerException">内部异常</param>
        public BusinessException(string message, Exception innerException) : base(message, innerException) { }
    }
}
