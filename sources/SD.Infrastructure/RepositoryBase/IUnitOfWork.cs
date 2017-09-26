using SD.Infrastructure.EntityBase;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SD.Infrastructure.RepositoryBase
{
    /// <summary>
    /// 单元事务基接口
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        #region # 注册添加单个实体对象 —— void RegisterAdd<T>(T entity)
        /// <summary>
        /// 注册添加单个实体对象
        /// </summary>
        /// <typeparam name="T">聚合根类型</typeparam>
        /// <param name="entity">新实体对象</param>
        /// <exception cref="ArgumentNullException">新实体对象为空</exception>
        void RegisterAdd<T>(T entity) where T : AggregateRootEntity;
        #endregion

        #region # 注册添加实体对象集合 —— void RegisterAddRange<T>(IEnumerable<T> entities)
        /// <summary>
        /// 注册添加实体对象集合
        /// </summary>
        /// <typeparam name="T">聚合根类型</typeparam>
        /// <param name="entities">实体对象集合</param>
        /// <exception cref="ArgumentNullException">实体对象集合为null或长度为0</exception>
        void RegisterAddRange<T>(IEnumerable<T> entities) where T : AggregateRootEntity;
        #endregion

        #region # 注册保存单个实体对象 —— void RegisterSave<T>(T entity)
        /// <summary>
        /// 注册保存单个实体对象
        /// </summary>
        /// <typeparam name="T">聚合根类型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <exception cref="ArgumentNullException">实体对象为空</exception>
        /// <exception cref="NullReferenceException">要保存的对象不存在</exception>
        void RegisterSave<T>(T entity) where T : AggregateRootEntity;
        #endregion

        #region # 注册保存实体对象集合 —— void RegisterSaveRange<T>(IEnumerable<T> entities)
        /// <summary>
        /// 注册保存实体对象集合
        /// </summary>
        /// <typeparam name="T">聚合根类型</typeparam>
        /// <param name="entities">实体对象集合</param>
        /// <exception cref="ArgumentNullException">实体对象集合</exception>
        /// <exception cref="NullReferenceException">要保存的对象不存在</exception>
        void RegisterSaveRange<T>(IEnumerable<T> entities) where T : AggregateRootEntity;
        #endregion

        #region # 注册删除单个实体对象（物理删除） —— void RegisterPhysicsRemove<T>(Guid id)
        /// <summary>
        /// 注册删除单个实体对象（物理删除）
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="id">标识Id</param>
        /// <exception cref="ArgumentNullException">id为空</exception>
        /// <exception cref="NullReferenceException">要删除的对象不存在</exception>
        void RegisterPhysicsRemove<T>(Guid id) where T : AggregateRootEntity;
        #endregion

        #region # 注册删除单个实体对象（物理删除） —— void RegisterPhysicsRemove<T>(string number)
        /// <summary>
        /// 注册删除单个实体对象（物理删除）
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="number">编号</param>
        /// <exception cref="ArgumentNullException">编号为空</exception>
        /// <exception cref="NullReferenceException">要删除的对象不存在</exception>
        void RegisterPhysicsRemove<T>(string number) where T : AggregateRootEntity;
        #endregion

        #region # 注册删除单个实体对象（物理删除） —— void RegisterPhysicsRemove<T>(T entity)
        /// <summary>
        /// 注册删除单个实体对象（物理删除）
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">实体对象</param>
        void RegisterPhysicsRemove<T>(T entity) where T : AggregateRootEntity;
        #endregion

        #region # 注册删除多个实体对象（物理删除） —— void RegisterPhysicsRemoveRange<T>(IEnumerable<Guid> ids)
        /// <summary>
        /// 注册删除多个实体对象（物理删除）
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="ids">标识Id集合</param>
        /// <exception cref="ArgumentNullException">ids为null或长度为0</exception>
        /// <exception cref="NullReferenceException">要删除的对象不存在</exception>
        void RegisterPhysicsRemoveRange<T>(IEnumerable<Guid> ids) where T : AggregateRootEntity;
        #endregion

        #region # 注册删除全部（物理删除） —— void RegisterPhysicsRemoveAll<T>()
        /// <summary>
        /// 注册删除全部（物理删除）
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <exception cref="NullReferenceException">要删除的对象不存在</exception>
        void RegisterPhysicsRemoveAll<T>() where T : AggregateRootEntity;
        #endregion

        #region # 注册删除单个实体对象（逻辑删除） —— void RegisterRemove<T>(Guid id)
        /// <summary>
        /// 注册删除单个实体对象（逻辑删除）
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="id">标识Id</param>
        /// <exception cref="ArgumentNullException">id为空</exception>
        /// <exception cref="NullReferenceException">要删除的对象不存在</exception>
        void RegisterRemove<T>(Guid id) where T : AggregateRootEntity;
        #endregion

        #region # 注册删除单个实体对象（逻辑删除） —— void RegisterRemove<T>(string number)
        /// <summary>
        /// 注册删除单个实体对象（逻辑删除）
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="number">编号</param>
        /// <exception cref="ArgumentNullException">编号为空</exception>
        /// <exception cref="NullReferenceException">要删除的对象不存在</exception>
        void RegisterRemove<T>(string number) where T : AggregateRootEntity;
        #endregion

        #region # 注册删除单个实体对象（逻辑删除） —— void RegisterRemove<T>(T entity)
        /// <summary>
        /// 注册删除单个实体对象（逻辑删除）
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">实体对象</param>
        void RegisterRemove<T>(T entity) where T : AggregateRootEntity;
        #endregion

        #region # 注册删除多个实体对象（逻辑删除） —— void RegisterRemoveRange<T>(IEnumerable<Guid> ids)
        /// <summary>
        /// 注册删除多个实体对象（逻辑删除）
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="ids">标识Id集合</param>
        /// <exception cref="ArgumentNullException">ids为null或长度为0</exception>
        void RegisterRemoveRange<T>(IEnumerable<Guid> ids) where T : AggregateRootEntity;
        #endregion

        #region # 注册删除全部（逻辑删除） —— void RegisterRemoveAll<T>()
        /// <summary>
        /// 注册删除全部（逻辑删除）
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        void RegisterRemoveAll<T>() where T : AggregateRootEntity;
        #endregion

        #region # 根据Id获取唯一实体对象（修改时用） —— T Resolve<T>(Guid id)
        /// <summary>
        /// 根据Id获取唯一实体对象（修改时用）
        /// </summary>
        /// <typeparam name="T">聚合根类型</typeparam>
        /// <param name="id">Id</param>
        /// <returns>唯一实体对象</returns>
        /// <exception cref="ArgumentNullException">id为空</exception>
        /// <exception cref="NullReferenceException">查询不到任何实体对象</exception>
        /// <exception cref="NotSupportedException">无法将表达式转换SQL语句</exception>
        /// <exception cref="InvalidOperationException">查询到1个以上的实体对象、查询到的实体对象已被删除</exception>
        T Resolve<T>(Guid id) where T : AggregateRootEntity;
        #endregion

        #region # 根据Id集获取实体对象集合（修改时用） —— IEnumerable<T> ResolveRange<T>(...
        /// <summary>
        /// 根据Id集获取实体对象集合（修改时用）
        /// </summary>
        /// <typeparam name="T">聚合根类型</typeparam>
        /// <param name="ids">Id集</param>
        /// <returns>实体对象集合</returns>
        IEnumerable<T> ResolveRange<T>(IEnumerable<Guid> ids) where T : AggregateRootEntity;
        #endregion

        #region # 获取全部实体对象（修改时用） —— IEnumerable<T> ResolveAll<T>()
        /// <summary>
        /// 获取全部实体对象（修改时用）
        /// </summary>
        /// <typeparam name="T">聚合根类型</typeparam>
        /// <returns>实体对象集合</returns>
        IEnumerable<T> ResolveAll<T>() where T : AggregateRootEntity;
        #endregion

        #region # 根据编号获取唯一实体对象（修改时用） —— T Resolve<T>(string number)
        /// <summary>
        /// 根据编号获取唯一实体对象（修改时用）
        /// </summary>
        /// <typeparam name="T">聚合根类型</typeparam>
        /// <param name="number">编号</param>
        /// <returns>单个实体对象</returns>
        /// <exception cref="ArgumentNullException">编号为空</exception>
        T Resolve<T>(string number) where T : AggregateRootEntity;
        #endregion

        #region # 根据编号集获取实体对象集合（修改时用） —— IEnumerable<T> ResolveRange<T>(...
        /// <summary>
        /// 根据编号集获取实体对象集合（修改时用）
        /// </summary>
        /// <typeparam name="T">聚合根类型</typeparam>
        /// <param name="numbers">编号集</param>
        /// <returns>实体对象集合</returns>
        IEnumerable<T> ResolveRange<T>(IEnumerable<string> numbers) where T : AggregateRootEntity;
        #endregion

        #region # 统一事务处理保存更改 —— void Commit()
        /// <summary>
        /// 统一事务处理保存更改
        /// </summary>
        void Commit();
        #endregion

        #region # 统一事务处理保存更改（异步） —— Task CommitAsync()
        /// <summary>
        /// 统一事务处理保存更改（异步）
        /// </summary>
        Task CommitAsync();
        #endregion

        #region # 统一事务回滚取消更改 —— void RollBack()
        /// <summary>
        /// 统一事务回滚取消更改
        /// </summary>
        void RollBack();
        #endregion

        #region # 执行SQL命令（无需Commit） —— void ExecuteSqlCommand(string sql...
        /// <summary>
        /// 执行SQL命令（无需Commit）
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">参数</param>
        /// <exception cref="ArgumentNullException">SQL语句为空</exception>
        void ExecuteSqlCommand(string sql, params object[] parameters);
        #endregion
    }
}
