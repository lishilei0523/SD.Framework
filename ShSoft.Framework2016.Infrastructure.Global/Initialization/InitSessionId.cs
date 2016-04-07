using System;
using System.Runtime.Remoting.Messaging;
using ShSoft.Framework2016.Infrastructure.Constants;

namespace ShSoft.Framework2016.Infrastructure.Global
{
    /// <summary>
    /// 初始化会话Id
    /// </summary>
    public static class InitSessionId
    {
        /// <summary>
        /// 注册
        /// </summary>
        public static void Register()
        {
            CallContext.FreeNamedDataSlot(CacheConstants.SessionIdKey);
            CallContext.SetData(CacheConstants.SessionIdKey, Guid.NewGuid());
        }
    }
}
