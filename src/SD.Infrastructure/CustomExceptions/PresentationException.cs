using System;

namespace SD.Infrastructure.CustomExceptions
{
    /// <summary>
    /// 表现层异常
    /// </summary>
    [Serializable]
    public class PresentationException : ApplicationException
    {
        /// <summary>
        /// 无参构造器
        /// </summary>
        public PresentationException() { }

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="message">异常消息</param>
        public PresentationException(string message)
            : base(message)
        {

        }

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="message">异常消息</param>
        /// <param name="innerException">内部异常</param>
        public PresentationException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
