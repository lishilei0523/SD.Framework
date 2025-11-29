using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SD.Infrastructure.CrontabBase;
using System;

namespace SD.Infrastructure.CrontabStore.Redis.Toolkits
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

        #region # 定时任务序列化JSON —— static string CrontabToJson<T>(this T crontab)
        /// <summary>
        /// 定时任务序列化JSON
        /// </summary>
        /// <typeparam name="T">定时任务类型</typeparam>
        /// <param name="crontab">定时任务实例</param>
        /// <returns>定时任务JSON</returns>
        internal static string CrontabToJson<T>(this T crontab) where T : ICrontab
        {
            #region # 验证

            if (crontab == null)
            {
                throw new ArgumentNullException(nameof(crontab), "定时任务实例不可为null！");
            }

            #endregion

            JsonSerializerSettings settting = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                TypeNameHandling = TypeNameHandling.All
            };
            var typedInstance = new
            {
                Type = crontab.GetType().FullName,
                Assembly = crontab.GetType().Assembly.FullName,
                Instance = JsonConvert.SerializeObject(crontab, Formatting.None, settting)
            };
            string json = JsonConvert.SerializeObject(typedInstance, Formatting.None, settting);

            return json;
        }
        #endregion

        #region # JSON反序列化定时任务 —— static ICrontab JsonToCrontab(this string crontabJson)
        /// <summary>
        /// JSON反序列化定时任务
        /// </summary>
        /// <param name="crontabJson">定时任务JSON</param>
        /// <returns>定时任务</returns>
        internal static ICrontab JsonToCrontab(this string crontabJson)
        {
            #region # 验证

            if (string.IsNullOrWhiteSpace(crontabJson))
            {
                throw new ArgumentNullException(nameof(crontabJson), "JSON字符串不可为空！");
            }

            #endregion

            JObject jObject = (JObject)JsonConvert.DeserializeObject(crontabJson);
            JToken typeProperty = jObject!.GetValue(TypePropertyName);
            JToken assemblyProperty = jObject!.GetValue(AssemblyPropertyName);
            JToken instanceProperty = jObject!.GetValue(InstancePropertyName);
            string typePropertyValue = typeProperty!.Value<string>();
            string assemblyPropertyValue = assemblyProperty!.Value<string>();
            string instancePropertyValue = instanceProperty!.Value<string>();

            Type crontabType = Type.GetType($"{typePropertyValue},{assemblyPropertyValue}");

            #region # 验证

            if (crontabType == null)
            {
                throw new TypeLoadException($"找不到类型\"{typePropertyValue}\"！");
            }
            if (!typeof(ICrontab).IsAssignableFrom(crontabType))
            {
                throw new TypeLoadException($"类型\"{typePropertyValue}\"不是定时任务类型！");
            }

            #endregion

            JsonSerializerSettings settting = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                TypeNameHandling = TypeNameHandling.All
            };
            object instance = JsonConvert.DeserializeObject(instancePropertyValue, crontabType, settting);

            return (ICrontab)instance;
        }
        #endregion
    }
}
