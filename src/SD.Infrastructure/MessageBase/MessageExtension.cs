using SD.Infrastructure.Constants;

namespace SD.Infrastructure.MessageBase
{
    /// <summary>
    /// 消息扩展方法
    /// </summary>
    public static class MessageExtension
    {
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
    }
}
