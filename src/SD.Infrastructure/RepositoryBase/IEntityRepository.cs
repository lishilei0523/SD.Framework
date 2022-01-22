using SD.Infrastructure.EntityBase;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SD.Infrastructure.RepositoryBase
{
    /// <summary>
    /// 普通实体仓储基接口
    /// </summary>
    /// <typeparam name="T">普通实体类型</typeparam>
    public interface IEntityRepository<T> : IDisposable where T : PlainEntity
    {
        //Single部分

        #region # 根据Id获取唯一实体对象 —— T SingleOrDefault(Guid id)
        /// <summary>
        /// 根据Id获取唯一实体对象
        /// </summary>
        /// <param name="id">标识Id</param>
        /// <returns>实体对象</returns>
        T SingleOrDefault(Guid id);
        #endregion

        #region # 根据Id获取唯一实体对象 —— Task<T> SingleOrDefaultAsync(Guid id)
        /// <summary>
        /// 根据Id获取唯一实体对象
        /// </summary>
        /// <param name="id">标识Id</param>
        /// <returns>实体对象</returns>
        Task<T> SingleOrDefaultAsync(Guid id);
        #endregion

        #region # 根据Id获取唯一子类对象 —— TSub SingleOrDefault<TSub>(Guid id)
        /// <summary>
        /// 根据Id获取唯一子类对象
        /// </summary>
        /// <param name="id">标识Id</param>
        /// <returns>子类对象</returns>
        /// <remarks>无该对象时返回null</remarks>
        TSub SingleOrDefault<TSub>(Guid id) where TSub : T;
        #endregion

        #region # 根据Id获取唯一子类对象 —— Task<TSub> SingleOrDefaultAsync<TSub>(Guid id)
        /// <summary>
        /// 根据Id获取唯一子类对象
        /// </summary>
        /// <param name="id">标识Id</param>
        /// <returns>子类对象</returns>
        Task<TSub> SingleOrDefaultAsync<TSub>(Guid id) where TSub : T;
        #endregion

        #region # 根据Id获取唯一实体对象 —— T Single(Guid id)
        /// <summary>
        /// 根据Id获取唯一实体对象
        /// </summary>
        /// <param name="id">标识Id</param>
        /// <returns>实体对象</returns>
        T Single(Guid id);
        #endregion

        #region # 根据Id获取唯一实体对象 —— Task<T> SingleAsync(Guid id)
        /// <summary>
        /// 根据Id获取唯一实体对象
        /// </summary>
        /// <param name="id">标识Id</param>
        /// <returns>实体对象</returns>
        Task<T> SingleAsync(Guid id);
        #endregion

        #region # 根据Id获取唯一子类对象 —— TSub Single<TSub>(Guid id)
        /// <summary>
        /// 根据Id获取唯一子类对象
        /// </summary>
        /// <param name="id">标识Id</param>
        /// <returns>子类对象</returns>
        TSub Single<TSub>(Guid id) where TSub : T;
        #endregion

        #region # 根据Id获取唯一子类对象 —— Task<TSub> SingleAsync<TSub>(Guid id)
        /// <summary>
        /// 根据Id获取唯一子类对象，
        /// </summary>
        /// <param name="id">标识Id</param>
        /// <returns>子类对象</returns>
        Task<TSub> SingleAsync<TSub>(Guid id) where TSub : T;
        #endregion

        #region # 根据行号获取唯一实体对象 —— TRowable SingleOrDefault(long rowNo)
        /// <summary>
        /// 根据行号获取唯一实体对象
        /// </summary>
        /// <param name="rowNo">行号</param>
        /// <returns>实体对象</returns>
        TRowable SingleOrDefault<TRowable>(long rowNo) where TRowable : T, IRowable;
        #endregion

        #region # 根据行号获取唯一实体对象 —— Task<TRowable> SingleOrDefaultAsync(long rowNo)
        /// <summary>
        /// 根据行号获取唯一实体对象
        /// </summary>
        /// <param name="rowNo">行号</param>
        /// <returns>实体对象</returns>
        Task<TRowable> SingleOrDefaultAsync<TRowable>(long rowNo) where TRowable : T, IRowable;
        #endregion

        #region # 根据行号获取唯一实体对象 —— TRowable Single(long rowNo)
        /// <summary>
        /// 根据行号获取唯一实体对象
        /// </summary>
        /// <param name="rowNo">行号</param>
        /// <returns>实体对象</returns>
        TRowable Single<TRowable>(long rowNo) where TRowable : T, IRowable;
        #endregion

        #region # 根据行号获取唯一实体对象 —— Task<TRowable> SingleAsync(long rowNo)
        /// <summary>
        /// 根据行号获取唯一实体对象
        /// </summary>
        /// <param name="rowNo">行号</param>
        /// <returns>实体对象</returns>
        Task<TRowable> SingleAsync<TRowable>(long rowNo) where TRowable : T, IRowable;
        #endregion

        #region # 获取默认或第一个实体对象 —— T FirstOrDefault()
        /// <summary>
        /// 获取默认或第一个实体对象
        /// </summary>
        /// <returns>实体对象</returns>
        T FirstOrDefault();
        #endregion

        #region # 获取默认或第一个实体对象 —— Task<T> FirstOrDefaultAsync()
        /// <summary>
        /// 获取默认或第一个实体对象
        /// </summary>
        /// <returns>实体对象</returns>
        Task<T> FirstOrDefaultAsync();
        #endregion

        #region # 获取默认或第一个子类对象 —— TSub FirstOrDefault<TSub>()
        /// <summary>
        /// 获取默认或第一个子类对象
        /// </summary>
        /// <returns>子类对象</returns>
        TSub FirstOrDefault<TSub>() where TSub : T;
        #endregion

        #region # 获取默认或第一个子类对象 —— Task<TSub> FirstOrDefaultAsync<TSub>()
        /// <summary>
        /// 获取默认或第一个子类对象
        /// </summary>
        /// <returns>子类对象</returns>
        Task<TSub> FirstOrDefaultAsync<TSub>() where TSub : T;
        #endregion


        //ICollection部分

        #region # 获取实体对象列表 —— ICollection<T> FindAll()
        /// <summary>
        /// 获取实体对象列表
        /// </summary>
        /// <returns>实体对象列表</returns>
        ICollection<T> FindAll();
        #endregion

        #region # 获取实体对象列表 —— Task<ICollection<T>> FindAllAsync()
        /// <summary>
        /// 获取实体对象列表
        /// </summary>
        /// <returns>实体对象列表</returns>
        Task<ICollection<T>> FindAllAsync();
        #endregion

        #region # 获取给定类型子类对象列表 —— ICollection<TSub> FindAll<TSub>()
        /// <summary>
        /// 获取给定类型子类对象列表
        /// </summary>
        /// <typeparam name="TSub">子类类型</typeparam>
        /// <returns>子类对象列表</returns>
        ICollection<TSub> FindAll<TSub>() where TSub : T;
        #endregion

        #region # 获取给定类型子类对象列表 —— Task<ICollection<TSub>> FindAllAsync<TSub>()
        /// <summary>
        /// 获取给定类型子类对象列表
        /// </summary>
        /// <typeparam name="TSub">子类类型</typeparam>
        /// <returns>子类对象列表</returns>
        Task<ICollection<TSub>> FindAllAsync<TSub>() where TSub : T;
        #endregion


        //IDictionary部分

        #region # 根据Id集获取实体对象字典 —— IDictionary<Guid, T> Find(IEnumerable<Guid> ids)
        /// <summary>
        /// 根据Id集获取实体对象字典
        /// </summary>
        /// <param name="ids">标识Id集</param>
        /// <returns>实体对象字典</returns>
        /// <remarks>IDictionary[Guid, T]，[Id, 实体对象]</remarks>
        IDictionary<Guid, T> Find(IEnumerable<Guid> ids);
        #endregion

        #region # 根据Id集获取实体对象字典 —— Task<IDictionary<Guid, T>> FindAsync(IEnumerable<Guid> ids)
        /// <summary>
        /// 根据Id集获取实体对象字典
        /// </summary>
        /// <param name="ids">标识Id集</param>
        /// <returns>实体对象字典</returns>
        /// <remarks>IDictionary[Guid, T]，[Id, 实体对象]</remarks>
        Task<IDictionary<Guid, T>> FindAsync(IEnumerable<Guid> ids);
        #endregion

        #region # 根据Id集获取子类对象字典 —— IDictionary<Guid, TSub> Find<TSub>(IEnumerable<Guid> ids)
        /// <summary>
        /// 根据Id集获取子类对象字典
        /// </summary>
        /// <param name="ids">标识Id集</param>
        /// <returns>子类对象字典</returns>
        /// <remarks>IDictionary[Guid, TSub]，[Id, 子类对象]</remarks>
        IDictionary<Guid, TSub> Find<TSub>(IEnumerable<Guid> ids) where TSub : T;
        #endregion

        #region # 根据Id集获取子类对象字典 —— Task<IDictionary<Guid, TSub>> FindAsync<TSub>(IEnumerable<Guid> ids)
        /// <summary>
        /// 根据Id集获取子类对象字典
        /// </summary>
        /// <param name="ids">标识Id集</param>
        /// <returns>子类对象字典</returns>
        /// <remarks>IDictionary[Guid, TSub]，[Id, 子类对象]</remarks>
        Task<IDictionary<Guid, TSub>> FindAsync<TSub>(IEnumerable<Guid> ids) where TSub : T;
        #endregion

        #region # 根据行号集获取实体对象字典 —— IDictionary<long, TRowable> Find<TRowable>(IEnumerable<long> rowNos)
        /// <summary>
        /// 根据行号集获取实体对象字典
        /// </summary>
        /// <param name="rowNos">行号集</param>
        /// <returns>实体对象字典</returns>
        /// <remarks>IDictionary[long, TRowable]，[行号, 实体对象]</remarks>
        IDictionary<long, TRowable> Find<TRowable>(IEnumerable<long> rowNos) where TRowable : T, IRowable;
        #endregion

        #region # 根据行号集获取实体对象字典 —— Task<IDictionary<long, TRowable>> FindAsync<TRowable>(IEnumerable<long> rowNos)
        /// <summary>
        /// 根据行号集获取实体对象字典
        /// </summary>
        /// <param name="rowNos">行号集</param>
        /// <returns>实体对象字典</returns>
        /// <remarks>IDictionary[long, TRowable]，[行号, 实体对象]</remarks>
        Task<IDictionary<long, TRowable>> FindAsync<TRowable>(IEnumerable<long> rowNos) where TRowable : T, IRowable;
        #endregion


        //Count部分

        #region # 获取总记录条数 —— long Count()
        /// <summary>
        /// 获取总记录条数
        /// </summary>
        /// <returns>总记录条数</returns>
        long Count();
        #endregion

        #region # 获取总记录条数 —— Task<long> CountAsync()
        /// <summary>
        /// 获取总记录条数
        /// </summary>
        /// <returns>总记录条数</returns>
        Task<long> CountAsync();
        #endregion

        #region # 获取子类记录条数 —— long Count<TSub>()
        /// <summary>
        /// 获取子类记录条数
        /// </summary>
        /// <returns>总记录条数</returns>
        long Count<TSub>() where TSub : T;
        #endregion

        #region # 获取子类记录条数 —— Task<long> CountAsync<TSub>()
        /// <summary>
        /// 获取子类记录条数
        /// </summary>
        /// <returns>总记录条数</returns>
        Task<long> CountAsync<TSub>() where TSub : T;
        #endregion


        //Exists部分

        #region # 是否存在给定Id的实体对象 —— bool Exists(Guid id)
        /// <summary>
        /// 是否存在给定Id的实体对象
        /// </summary>
        /// <param name="id">标识Id</param>
        /// <returns>是否存在</returns>
        bool Exists(Guid id);
        #endregion

        #region # 是否存在给定Id的实体对象 —— Task<bool> ExistsAsync(Guid id)
        /// <summary>
        /// 是否存在给定Id的实体对象
        /// </summary>
        /// <param name="id">标识Id</param>
        /// <returns>是否存在</returns>
        Task<bool> ExistsAsync(Guid id);
        #endregion

        #region # 是否存在给定Id的子类对象 —— bool Exists<TSub>(Guid id)
        /// <summary>
        /// 是否存在给定Id的子类对象
        /// </summary>
        /// <param name="id">标识Id</param>
        /// <returns>是否存在</returns>
        bool Exists<TSub>(Guid id) where TSub : T;
        #endregion

        #region # 是否存在给定Id的子类对象 —— Task<bool> ExistsAsync<TSub>(Guid id)
        /// <summary>
        /// 是否存在给定Id的子类对象
        /// </summary>
        /// <param name="id">标识Id</param>
        /// <returns>是否存在</returns>
        Task<bool> ExistsAsync<TSub>(Guid id) where TSub : T;
        #endregion

        #region # 是否存在给定行号的实体对象 —— bool Exists<TRowable>(long rowNo)
        /// <summary>
        /// 是否存在给定行号的实体对象
        /// </summary>
        /// <param name="rowNo">行号</param>
        /// <returns>是否存在</returns>
        bool Exists<TRowable>(long rowNo) where TRowable : T, IRowable;
        #endregion

        #region # 是否存在给定行号的实体对象 —— Task<bool> ExistsAsync<TRowable>(long rowNo)
        /// <summary>
        /// 是否存在给定行号的实体对象
        /// </summary>
        /// <param name="rowNo">行号</param>
        /// <returns>是否存在</returns>
        Task<bool> ExistsAsync<TRowable>(long rowNo) where TRowable : T, IRowable;
        #endregion


        //其他

        #region # 执行SQL查询 —— ICollection<TEntity> ExecuteSqlQuery<TEntity>(string sql...
        /// <summary>
        /// 执行SQL查询
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">参数集</param>
        /// <returns>实体对象集合</returns>
        ICollection<TEntity> ExecuteSqlQuery<TEntity>(string sql, params object[] parameters);
        #endregion

        #region # 执行SQL查询 —— Task<ICollection<TEntity>> ExecuteSqlQueryAsync<TEntity>(string sql...
        /// <summary>
        /// 执行SQL查询
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">参数集</param>
        /// <returns>实体对象集合</returns>
        Task<ICollection<TEntity>> ExecuteSqlQueryAsync<TEntity>(string sql, params object[] parameters);
        #endregion
    }
}
