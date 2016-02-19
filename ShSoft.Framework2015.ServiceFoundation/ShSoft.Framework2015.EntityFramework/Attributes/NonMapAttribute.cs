using System;

namespace ShSoft.Framework2015.EntityFramework.Attributes
{
    /// <summary>
    /// 无需映射数据库表类特性
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class NonMapAttribute : Attribute
    {

    }
}
