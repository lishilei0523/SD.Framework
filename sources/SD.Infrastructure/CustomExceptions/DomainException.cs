using System;

namespace SD.Infrastructure.CustomExceptions
{
    /// <summary>
    /// 领域层异常基类
    /// </summary>
    [Serializable]
    public class DomainException : ApplicationException
    {
        /// <summary>
        /// 无参构造器
        /// </summary>
        public DomainException() { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message">异常消息</param>
        public DomainException(string message) : base(message) { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message">异常消息</param>
        /// <param name="innerException">内部异常</param>
        public DomainException(string message, Exception innerException) : base(message, innerException) { }
    }
}
