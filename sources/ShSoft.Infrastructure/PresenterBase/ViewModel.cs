using System;

// ReSharper disable once CheckNamespace
namespace ShSoft.Infrastructure
{
    /// <summary>
    /// 视图模型基类
    /// </summary>
    public abstract class ViewModel
    {
        /// <summary>
        /// 标识
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 编号
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
    }
}
