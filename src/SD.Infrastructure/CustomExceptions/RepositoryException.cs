using System;

namespace SD.Infrastructure.CustomExceptions
{
    /// <summary>
    /// 仓储层异常基类
    /// </summary>
    [Serializable]
    public class RepositoryException : ApplicationException
    {
        /// <summary>
        /// 无参构造器
        /// </summary>
        public RepositoryException() { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message">异常消息</param>
        public RepositoryException(string message) : base(message) { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message">异常消息</param>
        /// <param name="innerException">内部异常</param>
        public RepositoryException(string message, Exception innerException) : base(message, innerException) { }
    }
}
