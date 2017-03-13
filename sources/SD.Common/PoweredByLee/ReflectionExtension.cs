using System;
using System.Reflection;
using System.Text;

namespace SD.Common.PoweredByLee
{
    /// <summary>
    /// 反射扩展工具类
    /// </summary>
    public static class ReflectionExtension
    {
        #region # 常量

        /// <summary>
        /// 分隔符
        /// </summary>
        private const string Separator = "/";

        #endregion

        #region # 获取方法路径 —— string GetMethodPath(this MethodBase method)
        /// <summary>
        /// 获取方法路径
        /// </summary>
        /// <param name="method">方法</param>
        /// <returns>方法路径</returns>
        public static string GetMethodPath(this MethodBase method)
        {
            #region # 验证参数

            if (method == null)
            {
                throw new ArgumentNullException("method", @"方法信息不可为空！");
            }

            #endregion

            string assemblyName = method.DeclaringType.Assembly.GetName().Name;
            string @namespace = method.DeclaringType.Namespace;
            string className = method.DeclaringType.Name;

            StringBuilder pathBuilder = new StringBuilder(Separator);
            pathBuilder.Append(assemblyName);
            pathBuilder.Append(Separator);
            pathBuilder.Append(@namespace);
            pathBuilder.Append(Separator);
            pathBuilder.Append(className);
            pathBuilder.Append(Separator);
            pathBuilder.Append(method.Name);

            return pathBuilder.ToString();
        }
        #endregion
    }
}
