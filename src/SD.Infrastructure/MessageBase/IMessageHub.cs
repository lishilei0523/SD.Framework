﻿using System.Threading.Tasks;

namespace SD.Infrastructure.MessageBase
{
    /// <summary>
    /// 消息交换者接口
    /// </summary>
    /// <typeparam name="T">消息类型</typeparam>
    public interface IMessageHub<in T> where T : IMessage
    {
        #region # 交换消息 —— Task Exchange(T message)
        /// <summary>
        /// 交换消息
        /// </summary>
        /// <param name="message">消息</param>
        Task Exchange(T message);
        #endregion
    }
}
