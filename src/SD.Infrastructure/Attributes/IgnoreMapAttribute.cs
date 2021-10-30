using System;

namespace SD.Infrastructure.Attributes
{
    /// <summary>
    /// 忽略映射特性类
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class IgnoreMapAttribute : Attribute
    {

    }
}
