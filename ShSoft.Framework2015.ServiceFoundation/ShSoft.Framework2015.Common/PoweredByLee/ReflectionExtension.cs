using System;
using System.Reflection;
using System.Text;

namespace ShSoft.Framework2015.Common.PoweredByLee
{
    /// <summary>
    /// 反射扩展工具类
    /// </summary>
    public static class ReflectionExtension
    {
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
                throw new ArgumentNullException("methodInfo", @"方法信息不可为空！");
            }

            #endregion

            string assemblyName = method.DeclaringType.Assembly.GetName().Name;
            string @namespace = method.DeclaringType.Namespace;
            string className = method.DeclaringType.Name;

            StringBuilder pathBuilder = new StringBuilder("/");
            pathBuilder.Append(assemblyName);
            pathBuilder.Append("/");
            pathBuilder.Append(@namespace);
            pathBuilder.Append("/");
            pathBuilder.Append(className);
            pathBuilder.Append("/");
            pathBuilder.Append(method.Name);

            return pathBuilder.ToString();
        }
        #endregion

        public static void Test()
        {
            //Assembly assembly = Assembly.Load("");
            //IEnumerable<Type> types = assembly.GetTypes().Where(x => x.GetCustomAttribute<Attribute>() != null);

            //foreach (var type in types)
            //{
            //    foreach (var method in type.GetMethods())
            //    {
            //        method.DeclaringType.as
            //    }
            //}
        }
    }
}
