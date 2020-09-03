using SD.Infrastructure.Constants;
using System;

namespace SD.Infrastructure.MessageBase
{
    /// <summary>
    /// 消息扩展方法
    /// </summary>
    public static class MessageExtension
    {
        #region # 获取Hub名称 —— static string GetHubName<T>()
        /// <summary>
        /// 获取Hub名称
        /// </summary>
        /// <typeparam name="T">消息类型</typeparam>
        /// <returns>Hub名称</returns>
        /// <remarks>
        /// 此处为约定，所有Hub应遵守“MessageTypeName + Hub”命名方式
        /// </remarks>
        public static string GetHubName<T>() where T : IMessage
        {
            string typeName = typeof(T).Name;
            string hubName = $"{typeName}{CommonConstants.HubSuffix}";

            return hubName;
        }
        #endregion

        #region # 获取Hub名称 —— static string GetHubName(Type messageType)
        /// <summary>
        /// 获取Hub名称
        /// </summary>
        /// <param name="messageType">消息类型</param>
        /// <returns>Hub名称</returns>
        /// <remarks>
        /// 此处为约定，所有Hub应遵守“MessageTypeName + Hub”命名方式
        /// </remarks>
        public static string GetHubName(Type messageType)
        {
            #region # 验证

            if (!typeof(IMessage).IsAssignableFrom(messageType))
            {
                throw new InvalidOperationException($"类型\"{messageType.Name}\"未实现\"{nameof(IMessage)}\"接口！");
            }

            #endregion

            string typeName = messageType.Name;
            string hubName = $"{typeName}{CommonConstants.HubSuffix}";

            return hubName;
        }
        #endregion
    }
}
