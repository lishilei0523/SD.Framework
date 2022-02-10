using SD.Infrastructure.Constants;
using SD.Infrastructure.EntityBase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace SD.Infrastructure.RepositoryBase
{
    /// <summary>
    /// 单元事务基接口
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        //Transaction部分

        #region # 开启事务 —— DbTransaction BeginTransaction(IsolationLevel isolationLevel)
        /// <summary>
        /// 开启事务
        /// </summary>
        /// <returns>事务</returns>
        DbTransaction BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
        #endregion

        #region # 使用事务 —— void UseTransaction(DbTransaction dbTransaction)
        /// <summary>
        /// 使用事务
        /// </summary>
        /// <param name="dbTransaction">事务</param>
        void UseTransaction(DbTransaction dbTransaction);
        #endregion

        #region # 获取当前事务 —— DbTransaction GetCurrentTransaction()
        /// <summary>
        /// 获取当前事务
        /// </summary>
        /// <returns>事务</returns>
        DbTransaction GetCurrentTransaction();
        #endregion

        #region # 获取实体历史列表 —— ICollection<IEntityHistory> GetEntityHistories<T>()
        /// <summary>
        /// 获取实体历史列表
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="actionType">动作类型</param>
        /// <returns>实体历史列表</returns>
        ICollection<IEntityHistory> GetEntityHistories<T>(ActionType? actionType = null) where T : PlainEntity;
        #endregion


        //Register部分

        #region # 注册添加实体对象 —— void RegisterAdd<T>(T entity)
        /// <summary>
        /// 注册添加实体对象
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">实体对象</param>
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

        #region # 注册保存实体对象 —— void RegisterSave<T>(T entity)
        /// <summary>
        /// 注册保存实体对象
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

        #region # 注册删除实体对象 —— void RegisterPhysicsRemove<T>(Guid id)
        /// <summary>
        /// 注册删除实体对象
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="id">标识Id</param>
        /// <remarks>物理删除</remarks>
        void RegisterPhysicsRemove<T>(Guid id) where T : AggregateRootEntity;
        #endregion

        #region # 注册删除实体对象 —— void RegisterPhysicsRemove<T>(string number)
        /// <summary>
        /// 注册删除实体对象
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="number">编号</param>
        /// <remarks>物理删除</remarks>
        void RegisterPhysicsRemove<T>(string number) where T : AggregateRootEntity;
        #endregion

        #region # 注册删除实体对象 —— void RegisterPhysicsRemove<T>(long rowNo)
        /// <summary>
        /// 注册删除实体对象
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="rowNo">行号</param>
        /// <remarks>物理删除</remarks>
        void RegisterPhysicsRemove<T>(long rowNo) where T : AggregateRootEntity, IRowable;
        #endregion

        #region # 注册删除实体对象 —— void RegisterPhysicsRemove<T>(T entity)
        /// <summary>
        /// 注册删除实体对象
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <remarks>物理删除</remarks>
        void RegisterPhysicsRemove<T>(T entity) where T : AggregateRootEntity;
        #endregion

        #region # 注册删除多个实体对象 —— void RegisterPhysicsRemoveRange<T>(IEnumerable<Guid> ids)
        /// <summary>
        /// 注册删除多个实体对象
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="ids">标识Id集</param>
        /// <remarks>物理删除</remarks>
        void RegisterPhysicsRemoveRange<T>(IEnumerable<Guid> ids) where T : AggregateRootEntity;
        #endregion

        #region # 注册删除多个实体对象 —— void RegisterPhysicsRemoveRange<T>(IEnumerable<string> numbers)
        /// <summary>
        /// 注册删除多个实体对象
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="numbers">编号集</param>
        /// <remarks>物理删除</remarks>
        void RegisterPhysicsRemoveRange<T>(IEnumerable<string> numbers) where T : AggregateRootEntity;
        #endregion

        #region # 注册删除多个实体对象 —— void RegisterPhysicsRemoveRange<T>(IEnumerable<long> rowNos)
        /// <summary>
        /// 注册删除多个实体对象
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="rowNos">行号集</param>
        /// <remarks>物理删除</remarks>
        void RegisterPhysicsRemoveRange<T>(IEnumerable<long> rowNos) where T : AggregateRootEntity, IRowable;
        #endregion

        #region # 注册删除多个实体对象 —— void RegisterPhysicsRemoveRange<T>(IEnumerable<T> entities)
        /// <summary>
        /// 注册删除多个实体对象
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entities">实体对象集</param>
        /// <remarks>物理删除</remarks>
        void RegisterPhysicsRemoveRange<T>(IEnumerable<T> entities) where T : AggregateRootEntity;
        #endregion

        #region # 注册删除实体对象 —— void RegisterRemove<T>(Guid id)
        /// <summary>
        /// 注册删除实体对象
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="id">标识Id</param>
        /// <remarks>逻辑删除</remarks>
        void RegisterRemove<T>(Guid id) where T : AggregateRootEntity;
        #endregion

        #region # 注册删除实体对象 —— void RegisterRemove<T>(string number)
        /// <summary>
        /// 注册删除实体对象
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="number">编号</param>
        /// <remarks>逻辑删除</remarks>
        void RegisterRemove<T>(string number) where T : AggregateRootEntity;
        #endregion

        #region # 注册删除实体对象 —— void RegisterRemove<T>(long rowNo)
        /// <summary>
        /// 注册删除实体对象
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="rowNo">行号</param>
        /// <remarks>逻辑删除</remarks>
        void RegisterRemove<T>(long rowNo) where T : AggregateRootEntity, IRowable;
        #endregion

        #region # 注册删除实体对象 —— void RegisterRemove<T>(T entity)
        /// <summary>
        /// 注册删除实体对象
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <remarks>逻辑删除</remarks>
        void RegisterRemove<T>(T entity) where T : AggregateRootEntity;
        #endregion

        #region # 注册删除多个实体对象 —— void RegisterRemoveRange<T>(IEnumerable<Guid> ids)
        /// <summary>
        /// 注册删除多个实体对象
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="ids">标识Id集</param>
        /// <remarks>逻辑删除</remarks>
        void RegisterRemoveRange<T>(IEnumerable<Guid> ids) where T : AggregateRootEntity;
        #endregion

        #region # 注册删除多个实体对象 —— void RegisterRemoveRange<T>(IEnumerable<string> numbers)
        /// <summary>
        /// 注册删除多个实体对象
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="numbers">编号集</param>
        /// <remarks>逻辑删除</remarks>
        void RegisterRemoveRange<T>(IEnumerable<string> numbers) where T : AggregateRootEntity;
        #endregion

        #region # 注册删除多个实体对象 —— void RegisterRemoveRange<T>(IEnumerable<long> rowNos)
        /// <summary>
        /// 注册删除多个实体对象
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="rowNos">行号集</param>
        /// <remarks>逻辑删除</remarks>
        void RegisterRemoveRange<T>(IEnumerable<long> rowNos) where T : AggregateRootEntity, IRowable;
        #endregion

        #region # 注册删除多个实体对象 —— void RegisterRemoveRange<T>(IEnumerable<T> entities)
        /// <summary>
        /// 注册删除多个实体对象
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entities">实体对象集</param>
        /// <remarks>逻辑删除</remarks>
        void RegisterRemoveRange<T>(IEnumerable<T> entities) where T : AggregateRootEntity;
        #endregion


        //Resolve部分

        #region # 获取唯一实体对象 —— T Resolve<T>(Guid id)
        /// <summary>
        /// 获取唯一实体对象
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="id">标识Id</param>
        /// <returns>实体对象</returns>
        T Resolve<T>(Guid id) where T : AggregateRootEntity;
        #endregion

        #region # 获取唯一实体对象 —— Task<T> ResolveAsync<T>(Guid id)
        /// <summary>
        /// 获取唯一实体对象
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="id">标识Id</param>
        /// <returns>实体对象</returns>
        Task<T> ResolveAsync<T>(Guid id) where T : AggregateRootEntity;
        #endregion

        #region # 获取唯一实体对象 —— T Resolve<T>(string number)
        /// <summary>
        /// 获取唯一实体对象
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="number">编号</param>
        /// <returns>实体对象</returns>
        T Resolve<T>(string number) where T : AggregateRootEntity;
        #endregion

        #region # 获取唯一实体对象 —— Task<T> ResolveAsync<T>(string number)
        /// <summary>
        /// 获取唯一实体对象
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="number">编号</param>
        /// <returns>实体对象</returns>
        Task<T> ResolveAsync<T>(string number) where T : AggregateRootEntity;
        #endregion

        #region # 获取唯一实体对象 —— T Resolve<T>(long rowNo)
        /// <summary>
        /// 获取唯一实体对象
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="rowNo">行号</param>
        /// <returns>实体对象</returns>
        T Resolve<T>(long rowNo) where T : AggregateRootEntity, IRowable;
        #endregion

        #region # 获取唯一实体对象 —— Task<T> ResolveAsync<T>(long rowNo)
        /// <summary>
        /// 获取唯一实体对象
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="rowNo">行号</param>
        /// <returns>实体对象</returns>
        Task<T> ResolveAsync<T>(long rowNo) where T : AggregateRootEntity, IRowable;
        #endregion

        #region # 获取实体对象列表 —— ICollection<T> ResolveRange<T>(...
        /// <summary>
        /// 获取实体对象列表
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="ids">标识Id集</param>
        /// <returns>实体对象列表</returns>
        ICollection<T> ResolveRange<T>(IEnumerable<Guid> ids) where T : AggregateRootEntity;
        #endregion

        #region # 获取实体对象列表 —— Task<ICollection<T>> ResolveRangeAsync<T>(...
        /// <summary>
        /// 获取实体对象列表
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="ids">标识Id集</param>
        /// <returns>实体对象列表</returns>
        Task<ICollection<T>> ResolveRangeAsync<T>(IEnumerable<Guid> ids) where T : AggregateRootEntity;
        #endregion

        #region # 获取实体对象列表 —— ICollection<T> ResolveRange<T>(...
        /// <summary>
        /// 获取实体对象列表
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="numbers">编号集</param>
        /// <returns>实体对象列表</returns>
        ICollection<T> ResolveRange<T>(IEnumerable<string> numbers) where T : AggregateRootEntity;
        #endregion

        #region # 获取实体对象列表 —— Task<ICollection<T>> ResolveRangeAsync<T>(...
        /// <summary>
        /// 获取实体对象列表
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="numbers">编号集</param>
        /// <returns>实体对象列表</returns>
        Task<ICollection<T>> ResolveRangeAsync<T>(IEnumerable<string> numbers) where T : AggregateRootEntity;
        #endregion

        #region # 获取实体对象列表 —— ICollection<T> ResolveRange<T>(...
        /// <summary>
        /// 获取实体对象列表
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="rowNos">行号集</param>
        /// <returns>实体对象列表</returns>
        ICollection<T> ResolveRange<T>(IEnumerable<long> rowNos) where T : AggregateRootEntity, IRowable;
        #endregion

        #region # 获取实体对象列表 —— Task<ICollection<T>> ResolveRangeAsync<T>(...
        /// <summary>
        /// 获取实体对象列表
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="rowNos">行号集</param>
        /// <returns>实体对象列表</returns>
        Task<ICollection<T>> ResolveRangeAsync<T>(IEnumerable<long> rowNos) where T : AggregateRootEntity, IRowable;
        #endregion


        //Commit部分

        #region # 统一事务处理保存更改 —— void Commit()
        /// <summary>
        /// 统一事务处理保存更改
        /// </summary>
        void Commit();
        #endregion

        #region # 统一事务处理保存更改 —— Task CommitAsync()
        /// <summary>
        /// 统一事务处理保存更改
        /// </summary>
        Task CommitAsync();
        #endregion

        #region # 统一事务回滚取消更改 —— void RollBack()
        /// <summary>
        /// 统一事务回滚取消更改
        /// </summary>
        void RollBack();
        #endregion

        #region # 执行SQL命令 —— void ExecuteSqlCommand(string sql...
        /// <summary>
        /// 执行SQL命令
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">参数</param>
        /// <remarks>无需Commit</remarks>
        void ExecuteSqlCommand(string sql, params object[] parameters);
        #endregion

        #region # 执行SQL命令 —— Task ExecuteSqlCommandAsync(string sql...
        /// <summary>
        /// 执行SQL命令
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">参数</param>
        /// <remarks>无需Commit</remarks>
        Task ExecuteSqlCommandAsync(string sql, params object[] parameters);
        #endregion
    }
}
