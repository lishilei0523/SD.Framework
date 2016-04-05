namespace ShSoft.Framework2016.Infrastructure.IEntity
{
    /// <summary>
    /// 值对象接口
    /// </summary>
    public interface IValueObject
    {
        /// <summary>
        /// 只读属性 - 是否合法
        /// </summary>
        bool Illegal { get; }
    }
}
