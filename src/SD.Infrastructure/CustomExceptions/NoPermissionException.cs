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
    }
}
