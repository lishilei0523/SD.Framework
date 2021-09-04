using System;

namespace SD.Infrastructure.Attributes
{
    /// <summary>
    /// 承载程序集特性
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Assembly)]
    public class HostAssemblyAttribute : Attribute
    {
        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="assemblyName">程序集名称</param>
        public HostAssemblyAttribute(string assemblyName)
        {
            this.AssemblyName = assemblyName;
        }

        /// <summary>
        /// 程序集名称
        /// </summary>
        public string AssemblyName { get; private set; }
    }
}
