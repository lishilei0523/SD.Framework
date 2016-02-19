using System;

namespace ShSoft.Framework2015.AOP.Models.ValueObjects
{
    /// <summary>
    /// 方法参数
    /// </summary>
    [Serializable]
    internal struct MethodArg
    {
        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="type">参数类型</param>
        /// <param name="val">参数值</param>
        public MethodArg(string name, string type, object val)
        {
            this.Name = name;
            this.Type = type;
            this.Value = val;
        }

        /// <summary>
        /// 参数名称
        /// </summary>
        public string Name;

        /// <summary>
        /// 参数类型
        /// </summary>
        public string Type;

        /// <summary>
        /// 参数值
        /// </summary>
        public object Value;
    }
}
