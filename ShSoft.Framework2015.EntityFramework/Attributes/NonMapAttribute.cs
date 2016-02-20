using System;

namespace ShSoft.Framework2015.EntityFramework.Attributes
{
    /// <summary>
    /// 不映射表特性
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class NonMapAttribute : Attribute
    {

    }
}
