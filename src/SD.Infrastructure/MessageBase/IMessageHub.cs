namespace SD.Infrastructure.MessageBase
{
    /// <summary>
    /// 消息交换者接口
    /// </summary>
    /// <typeparam name="T">消息类型</typeparam>
    public interface IMessageHub<in T> where T : IMessage
    {
        /// <summary>
        /// 交换消息
        /// </summary>
        /// <param name="message">消息</param>
        void Exchange(T message);
    }
}
