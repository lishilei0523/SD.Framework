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
        //Register部分

        #region # 注册添加单个实体对象 —— void RegisterAdd<T>(T entity)
        /// <summary>
        /// 注册添加单个实体对象
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">新实体对象</param>
        void RegisterAdd<T>(T entity) where T : AggregateRootEntity;
        #endregion

        #region # 注册添加实体对象列表 —— void RegisterAddRange<T>(IEnumerable<T> entities)
        /// <summary>
        /// 注册添加实体对象列表
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entities">实体对象列表</param>
        void RegisterAddRange<T>(IEnumerable<T> entities) where T : AggregateRootEntity;
        #endregion

        #region # 注册保存单个实体对象 —— void RegisterSave<T>(T entity)
        /// <summary>
        /// 注册保存单个实体对象
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">实体对象</param>
        void RegisterSave<T>(T entity) where T : AggregateRootEntity;
        #endregion

        #region # 注册保存实体对象列表 —— void RegisterSaveRange<T>(IEnumerable<T> entities)
        /// <summary>
        /// 注册保存实体对象列表
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entities">实体对象列表</param>
        void RegisterSaveRange<T>(IEnumerable<T> entities) where T : AggregateRootEntity;
        #endregion

        #region # 注册删除单个实体对象（物理删除） —— void RegisterPhysicsRemove<T>(Guid id)
        /// <summary>
        /// 注册删除单个实体对象（物理删除）
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="id">标识Id</param>
        void RegisterPhysicsRemove<T>(Guid id) where T : AggregateRootEntity;
        #endregion

        #region # 注册删除单个实体对象（物理删除） —— void RegisterPhysicsRemove<T>(string number)
        /// <summary>
        /// 注册删除单个实体对象（物理删除）
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="number">编号</param>
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
        /// <param name="ids">标识Id集</param>
        void RegisterPhysicsRemoveRange<T>(IEnumerable<Guid> ids) where T : AggregateRootEntity;
        #endregion

        #region # 注册删除单个实体对象（逻辑删除） —— void RegisterRemove<T>(Guid id)
        /// <summary>
        /// 注册删除单个实体对象（逻辑删除）
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="id">标识Id</param>
        void RegisterRemove<T>(Guid id) where T : AggregateRootEntity;
        #endregion

        #region # 注册删除单个实体对象（逻辑删除） —— void RegisterRemove<T>(string number)
        /// <summary>
        /// 注册删除单个实体对象（逻辑删除）
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="number">编号</param>
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
        /// <param name="ids">标识Id集</param>
        void RegisterRemoveRange<T>(IEnumerable<Guid> ids) where T : AggregateRootEntity;
        #endregion


        //Resolve部分

        #region # 根据Id获取唯一实体对象（修改时用） —— T Resolve<T>(Guid id)
        /// <summary>
        /// 根据Id获取唯一实体对象（修改时用）
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="id">Id</param>
        /// <returns>唯一实体对象</returns>
        T Resolve<T>(Guid id) where T : AggregateRootEntity;
        #endregion

        #region # 根据Id集获取实体对象列表（修改时用） —— ICollection<T> ResolveRange<T>(...
        /// <summary>
        /// 根据Id集获取实体对象列表（修改时用）
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="ids">Id集</param>
        /// <returns>实体对象列表</returns>
        ICollection<T> ResolveRange<T>(IEnumerable<Guid> ids) where T : AggregateRootEntity;
        #endregion

        #region # 根据编号获取唯一实体对象（修改时用） —— T Resolve<T>(string number)
        /// <summary>
        /// 根据编号获取唯一实体对象（修改时用）
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="number">编号</param>
        /// <returns>单个实体对象</returns>
        T Resolve<T>(string number) where T : AggregateRootEntity;
        #endregion

        #region # 根据编号集获取实体对象列表（修改时用） —— ICollection<T> ResolveRange<T>(...
        /// <summary>
        /// 根据编号集获取实体对象列表（修改时用）
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="numbers">编号集</param>
        /// <returns>实体对象列表</returns>
        ICollection<T> ResolveRange<T>(IEnumerable<string> numbers) where T : AggregateRootEntity;
        #endregion

        #region # 异步根据Id获取唯一实体对象（修改时用） —— Task<T> ResolveAsync<T>(Guid id)
        /// <summary>
        /// 异步根据Id获取唯一实体对象（修改时用）
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="id">Id</param>
        /// <returns>唯一实体对象</returns>
        Task<T> ResolveAsync<T>(Guid id) where T : AggregateRootEntity;
        #endregion

        #region # 异步根据Id集获取实体对象列表（修改时用） —— Task<ICollection<T>> ResolveRangeAsync<T>(...
        /// <summary>
        /// 异步根据Id集获取实体对象列表（修改时用）
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="ids">Id集</param>
        /// <returns>实体对象列表</returns>
        Task<ICollection<T>> ResolveRangeAsync<T>(IEnumerable<Guid> ids) where T : AggregateRootEntity;
        #endregion

        #region # 异步根据编号获取唯一实体对象（修改时用） —— Task<T> ResolveAsync<T>(string number)
        /// <summary>
        /// 异步根据编号获取唯一实体对象（修改时用）
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="number">编号</param>
        /// <returns>单个实体对象</returns>
        Task<T> ResolveAsync<T>(string number) where T : AggregateRootEntity;
        #endregion

        #region # 异步根据编号集获取实体对象列表（修改时用） —— Task<ICollection<T>> ResolveRangeAsync<T>(...
        /// <summary>
        /// 异步根据编号集获取实体对象列表（修改时用）
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="numbers">编号集</param>
        /// <returns>实体对象列表</returns>
        Task<ICollection<T>> ResolveRangeAsync<T>(IEnumerable<string> numbers) where T : AggregateRootEntity;
        #endregion


        //Commit部分

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
        void ExecuteSqlCommand(string sql, params object[] parameters);
        #endregion

        #region # 异步执行SQL命令（无需Commit） —— Task ExecuteSqlCommandAsync(string sql...
        /// <summary>
        /// 异步执行SQL命令（无需Commit）
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">参数</param>
        Task ExecuteSqlCommandAsync(string sql, params object[] parameters);
        #endregion
    }
}
