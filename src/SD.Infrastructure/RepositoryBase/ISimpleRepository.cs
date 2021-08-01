using SD.Infrastructure.EntityBase;
using System;
using System.Collections.Generic;

namespace SD.Infrastructure.RepositoryBase
{
    /// <summary>
    /// 简单仓储层基接口
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    public interface ISimpleRepository<T> : IAggRootRepository<T> where T : AggregateRootEntity
    {
        #region # 添加单个实体对象 —— void Add(T entity)
        /// <summary>
        /// 添加单个实体对象
        /// </summary>
        /// <param name="entity">实体对象</param>
        void Add(T entity);
        #endregion

        #region # 添加实体对象列表 —— void AddRange(IEnumerable<T> entities)
        /// <summary>
        /// 添加实体对象列表
        /// </summary>
        /// <typeparam name="T">聚合根类型</typeparam>
        /// <param name="entities">实体对象集</param>
        void AddRange(IEnumerable<T> entities);
        #endregion

        #region # 保存单个实体对象 —— void Save(T entity)
        /// <summary>
        /// 保存单个实体对象
        /// </summary>
        /// <param name="entity">实体对象</param>
        void Save(T entity);
        #endregion

        #region # 保存实体对象列表 —— void SaveRange(IEnumerable<T> entities)
        /// <summary>
        /// 保存实体对象列表
        /// </summary>
        /// <typeparam name="T">聚合根类型</typeparam>
        /// <param name="entities">实体对象集</param>
        void SaveRange(IEnumerable<T> entities);
        #endregion

        #region # 删除单行 —— void Remove(Guid id)
        /// <summary>
        /// 删除单行
        /// </summary>
        /// <param name="id">标识Id</param>
        void Remove(Guid id);
        #endregion

        #region # 删除单行 —— void Remove(string number)
        /// <summary>
        /// 删除单行
        /// </summary>
        /// <param name="number">编号</param>
        void Remove(string number);
        #endregion

        #region # 删除多行 —— void RemoveRange(IEnumerable<Guid> ids)
        /// <summary>
        /// 删除多行
        /// </summary>
        /// <param name="ids">标识Id集</param>
        void RemoveRange(IEnumerable<Guid> ids);
        #endregion

        #region # 删除多行 —— void RemoveRange(IEnumerable<string> numbers)
        /// <summary>
        /// 删除多行
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="numbers">编号集</param>
        void RemoveRange(IEnumerable<string> numbers);
        #endregion

        #region # 删除全部 —— void RemoveAll()
        /// <summary>
        /// 删除全部
        /// </summary>
        void RemoveAll();
        #endregion
    }
}
