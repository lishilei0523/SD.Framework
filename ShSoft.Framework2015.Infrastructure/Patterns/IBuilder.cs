using ShSoft.Framework2015.Infrastructure.IEntity;

namespace ShSoft.Framework2015.Infrastructure.Patterns
{
    /// <summary>
    /// 建造者基接口
    /// </summary>
    /// <typeparam name="T">聚合根类型</typeparam>
    public interface IBuilder<out T> where T : AggregateRootEntity
    {
        /// <summary>
        /// 建造完毕
        /// </summary>
        /// <returns>聚合根实例</returns>
        T Build();
    }
}
