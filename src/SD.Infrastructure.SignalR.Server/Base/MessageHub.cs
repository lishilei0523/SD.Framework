using Microsoft.AspNet.SignalR;
using SD.Infrastructure.MessageBase;

namespace SD.Infrastructure.SignalR.Server.Base
{
    /// <summary>
    /// 消息Hub基类
    /// </summary>
    /// <typeparam name="T">消息类型</typeparam>
    public abstract class MessageHub<T> : Hub<IMessageHub<T>>, IMessageHub<T> where T : IMessage
    {
        /// <summary>
        /// 交换消息
        /// </summary>
        /// <param name="message">消息</param>
        public virtual void Exchange(T message)
        {
            base.Clients.All.Exchange(message);
        }
    }
}
