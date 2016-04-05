using ShSoft.Framework2016.Infrastructure.IEntity;

namespace ShSoft.Framework2016.Infrastructure.IDomainService
{
    /// <summary>
    /// 领域服务基接口
    /// </summary>
    public interface IDomainService<in T> where T : AggregateRootEntity
    {
        #region # 获取聚合根实体关键字 —— string GetKeywords(T entity)
        /// <summary>
        /// 获取聚合根实体关键字
        /// </summary>
        /// <param name="entity">聚合根实体对象</param>
        /// <returns>关键字</returns>
        string GetKeywords(T entity);
        #endregion
    }
}
