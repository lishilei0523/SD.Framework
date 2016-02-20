using System;
using System.ServiceModel;

namespace ShSoft.Framework2015.Infrastructure.CustomExceptions
{
    /// <summary>
    /// 应用程序服务层异常基类
    /// </summary>
    [Serializable]
    public class AppServiceException : FaultException
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message">异常消息</param>
        public AppServiceException(string message) : base(message) { }
    }
}
