using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using ShSoft.Framework2016.Infrastructure.Constants;

namespace ShSoft.Framework2016.Infrastructure.WCF.ServerAuthentication
{
    /// <summary>
    /// 服务端身份认证消息检查器
    /// </summary>
    internal class AuthenticationMessageInspector : IDispatchMessageInspector
    {
        /// <summary>
        /// 接收请求后事件
        /// </summary>
        /// <param name="request">请求消息</param>
        /// <param name="channel">信道</param>
        /// <param name="instanceContext">WCF实例上下文</param>
        /// <returns></returns>
        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            Guid publishKey = OperationContext.Current.IncomingMessageHeaders.GetHeader<Guid>(CacheConstants.WcfAuthHeaderName, CacheConstants.WcfAuthHeaderNs);

            //TODO 读取缓存，判断是否有值

            return null;
        }

        /// <summary>
        /// 响应请求前事件
        /// </summary>
        /// <param name="reply">响应消息</param>
        /// <param name="correlationState">相关状态</param>
        public void BeforeSendReply(ref Message reply, object correlationState) { }
    }
}
