using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SD.Infrastructure.EventBase;
using System;

namespace SD.Infrastructure.EventStore.Redis.Toolkits
{
    /// <summary>
    /// JSON工具
    /// </summary>
    internal static class JsonUtil
    {
        #region # 常量

        /// <summary>
        /// 类型属性名
        /// </summary>
        private const string TypePropertyName = "Type";

        /// <summary>
        /// 程序集属性名
        /// </summary>
        private const string AssemblyPropertyName = "Assembly";

        /// <summary>
        /// 实例属性名
        /// </summary>
        private const string InstancePropertyName = "Instance";

        #endregion

        #region # 领域事件序列化JSON —— static string EventToJson<T>(this T eventSource)
        /// <summary>
        /// 领域事件序列化JSON
        /// </summary>
        /// <typeparam name="T">领域事件类型</typeparam>
        /// <param name="eventSource">领域事件实例</param>
        /// <returns>领域事件JSON</returns>
        internal static string EventToJson<T>(this T eventSource) where T : IEvent
        {
            #region # 验证

            if (eventSource == null)
            {
                throw new ArgumentNullException(nameof(eventSource), "领域事件实例不可为null！");
            }

            #endregion

            JsonSerializerSettings settting = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            var typedInstance = new
            {
                Type = eventSource.GetType().FullName,
                Assembly = eventSource.GetType().Assembly.FullName,
                Instance = JsonConvert.SerializeObject(eventSource, Formatting.None, settting)
            };

            return JsonConvert.SerializeObject(typedInstance, Formatting.None, settting);
        }
        #endregion

        #region # JSON反序列化领域事件 —— static IEvent JsonToEvent(this string eventSourceJson)
        /// <summary>
        /// JSON反序列化领域事件
        /// </summary>
        /// <param name="eventSourceJson">领域事件JSON</param>
        /// <returns>领域事件</returns>
        internal static IEvent JsonToEvent(this string eventSourceJson)
        {
            #region # 验证

            if (string.IsNullOrWhiteSpace(eventSourceJson))
            {
                throw new ArgumentNullException(nameof(eventSourceJson), @"JSON字符串不可为空！");
            }

            #endregion

            JObject jObject = (JObject)JsonConvert.DeserializeObject(eventSourceJson);

            JToken typeProperty = jObject.GetValue(TypePropertyName);
            JToken assemblyProperty = jObject.GetValue(AssemblyPropertyName);
            JToken instanceProperty = jObject.GetValue(InstancePropertyName);

            string typePropertyValue = typeProperty.Value<string>();
            string assemblyPropertyValue = assemblyProperty.Value<string>();
            string instancePropertyValue = instanceProperty.Value<string>();

            Type eventType = Type.GetType($"{typePropertyValue},{assemblyPropertyValue}");

            #region # 验证类型

            if (!typeof(IEvent).IsAssignableFrom(eventType))
            {
                throw new TypeLoadException("给定类型不是领域事件类型！");
            }

            #endregion

            object instance = JsonConvert.DeserializeObject(instancePropertyValue, eventType);

            return (IEvent)instance;
        }
        #endregion
    }
}
