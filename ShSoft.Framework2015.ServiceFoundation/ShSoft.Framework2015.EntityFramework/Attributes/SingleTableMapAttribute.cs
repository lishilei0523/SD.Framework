using System;

namespace ShSoft.Framework2015.EntityFramework.Attributes
{
    /// <summary>
    /// 单表继承特性
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class SingleTableMapAttribute : Attribute
    {

    }
}
