using System;

namespace SD.Infrastructure.CustomExceptions
{
    /// <summary>
    /// 无权限异常
    /// </summary>
    [Serializable]
    public class NoPermissionException : InvalidOperationException
    {
        /// <summary>
        /// 无参构造器
        /// </summary>
        public NoPermissionException() { }

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="message">异常消息</param>
        public NoPermissionException(string message)
            : base(message)
        {

        }

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="message">异常消息</param>
        /// <param name="innerException">内部异常</param>
        public NoPermissionException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
