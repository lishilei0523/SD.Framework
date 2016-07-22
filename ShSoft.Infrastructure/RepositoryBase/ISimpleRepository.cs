using System;
using ShSoft.Infrastructure.EntityBase;

namespace ShSoft.Infrastructure.RepositoryBase
{
    /// <summary>
    /// 简单仓储层基接口
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    public interface ISimpleRepository<T> : IRepository<T> where T : AggregateRootEntity
    {
        #region # 添加单个实体对象 —— void Add(T entity)
        /// <summary>
        /// 添加单个实体对象
        /// </summary>
        /// <param name="entity">新实体对象</param>
        /// <exception cref="ArgumentNullException">新实体对象为空</exception>
        void Add(T entity);
        #endregion

        #region # 保存单个实体对象 —— void Save(T entity)
        /// <summary>
        /// 保存单个实体对象
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <exception cref="ArgumentNullException">实体对象为空</exception>
        /// <exception cref="NullReferenceException">要保存的对象不存在</exception>
        void Save(T entity);
        #endregion

        #region # 删除单行 —— void Remove(Guid id)
        /// <summary>
        /// 删除单行
        /// </summary>
        /// <param name="id">标识Id</param>
        /// <exception cref="ArgumentNullException">id为空</exception>
        /// <exception cref="NullReferenceException">要删除的对象不存在</exception>
        void Remove(Guid id);
        #endregion

        #region # 删除单行 —— void Remove(string number)
        /// <summary>
        /// 删除单行
        /// </summary>
        /// <param name="number">编号</param>
        /// <exception cref="ArgumentNullException">编号为空</exception>
        /// <exception cref="NullReferenceException">要删除的对象不存在</exception>
        void Remove(string number);
        #endregion

        #region # 删除全部 —— void RemoveAll()
        /// <summary>
        /// 删除全部
        /// </summary>
        void RemoveAll();
        #endregion
    }
}
