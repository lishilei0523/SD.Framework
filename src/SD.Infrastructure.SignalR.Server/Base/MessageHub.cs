using Microsoft.AspNetCore.SignalR;
using SD.Infrastructure.MessageBase;
using System.Threading.Tasks;

namespace SD.Infrastructure.SignalR.Server.Base
{
    /// <summary>
    /// 消息Hub基类
    /// </summary>
    /// <typeparam name="T">消息类型</typeparam>
    public abstract class MessageHub<T> : Hub, IMessageHub<T> where T : IMessage
    {
        /// <summary>
        /// 交换消息
        /// </summary>
        /// <param name="message">消息</param>
        public virtual async Task Exchange(T message)
        {
            await base.Clients.Others.SendAsync(nameof(this.Exchange), message);
        }
    }
}
