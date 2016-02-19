namespace ShSoft.Framework2015.Infrastructure.IEntity
{
    /// <summary>
    /// 版本控制接口
    /// </summary>
    /// <typeparam name="T">版本号类型</typeparam>
    public interface IVersionControl<out T>
    {
        /// <summary>
        /// 版本号
        /// </summary>
        T VersionNo { get; }
    }
}
