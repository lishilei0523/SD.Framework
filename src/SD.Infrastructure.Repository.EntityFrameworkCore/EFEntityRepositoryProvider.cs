using Microsoft.EntityFrameworkCore;
using SD.Infrastructure.EntityBase;
using SD.Infrastructure.Repository.EntityFrameworkCore.Base;
using SD.Infrastructure.RepositoryBase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace SD.Infrastructure.Repository.EntityFrameworkCore
{
    /// <summary>
    /// EF普通实体仓储Provider
    /// </summary>
    /// <typeparam name="T">普通实体类型</typeparam>
    public abstract class EFEntityRepositoryProvider<T> : IEntityRepository<T> where T : PlainEntity
    {
        #region # 创建EF（读）上下文对象

        /// <summary>
        /// 同步锁
        /// </summary>
        private static readonly object _Sync;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static EFEntityRepositoryProvider()
        {
            _Sync = new object();
        }

        /// <summary>
        /// EF（读）上下文对象
        /// </summary>
        protected readonly DbContext _dbContext;

        /// <summary>
        /// 构造器
        /// </summary>
        protected EFEntityRepositoryProvider()
        {
            //EF（读）上下文对象
            this._dbContext = DbSessionBase.QueryInstance;
        }

        /// <summary>
        /// 析构器
        /// </summary>
        ~EFEntityRepositoryProvider()
        {
            this.Dispose();
        }

        #endregion

        #region # Public

        //Single部分

        #region # 获取唯一实体对象 —— T SingleOrDefault(Guid id)
        /// <summary>
        /// 获取唯一实体对象
        /// </summary>
        /// <param name="id">标识Id</param>
        /// <returns>实体对象</returns>
        public T SingleOrDefault(Guid id)
        {
            return this.SingleOrDefault(x => x.Id == id);
        }
        #endregion

        #region # 获取唯一实体对象 —— Task<T> SingleOrDefaultAsync(Guid id)
        /// <summary>
        /// 获取唯一实体对象
        /// </summary>
        /// <param name="id">标识Id</param>
        /// <returns>实体对象</returns>
        public async Task<T> SingleOrDefaultAsync(Guid id)
        {
            return await this.SingleOrDefaultAsync(x => x.Id == id);
        }
        #endregion

        #region # 获取唯一子类对象 —— TSub SingleOrDefault<TSub>(Guid id)
        /// <summary>
        /// 获取唯一子类对象
        /// </summary>
        /// <param name="id">标识Id</param>
        /// <returns>子类对象</returns>
        public TSub SingleOrDefault<TSub>(Guid id) where TSub : T
        {
            return this.SingleOrDefault<TSub>(x => x.Id == id);
        }
        #endregion

        #region # 获取唯一子类对象 —— Task<TSub> SingleOrDefaultAsync<TSub>(Guid id)
        /// <summary>
        /// 获取唯一子类对象
        /// </summary>
        /// <param name="id">标识Id</param>
        /// <returns>子类对象</returns>
        public async Task<TSub> SingleOrDefaultAsync<TSub>(Guid id) where TSub : T
        {
            return await this.SingleOrDefaultAsync<TSub>(x => x.Id == id);
        }
        #endregion

        #region # 获取唯一实体对象 —— T Single(Guid id)
        /// <summary>
        /// 获取唯一实体对象
        /// </summary>
        /// <param name="id">标识Id</param>
        /// <returns>实体对象</returns>
        public T Single(Guid id)
        {
            T current = this.SingleOrDefault(id);

            #region # 验证

            if (current == null)
            {
                throw new NullReferenceException($"Id为\"{id}\"的{typeof(T).Name}实体不存在！");
            }

            #endregion

            return current;
        }
        #endregion

        #region # 获取唯一实体对象 —— Task<T> SingleAsync(Guid id)
        /// <summary>
        /// 获取唯一实体对象
        /// </summary>
        /// <param name="id">标识Id</param>
        /// <returns>实体对象</returns>
        public async Task<T> SingleAsync(Guid id)
        {
            T current = await this.SingleOrDefaultAsync(id);

            #region # 验证

            if (current == null)
            {
                throw new NullReferenceException($"Id为\"{id}\"的{typeof(T).Name}实体不存在！");
            }

            #endregion

            return current;
        }
        #endregion

        #region # 获取唯一子类对象 —— TSub Single<TSub>(Guid id)
        /// <summary>
        /// 获取唯一子类对象
        /// </summary>
        /// <param name="id">标识Id</param>
        /// <returns>子类对象</returns>
        public TSub Single<TSub>(Guid id) where TSub : T
        {
            TSub current = this.SingleOrDefault<TSub>(id);

            #region # 验证

            if (current == null)
            {
                throw new NullReferenceException($"Id为\"{id}\"的{typeof(TSub).Name}实体不存在！");
            }

            #endregion

            return current;
        }
        #endregion

        #region # 获取唯一子类对象 —— Task<TSub> SingleAsync<TSub>(Guid id)
        /// <summary>
        /// 获取唯一子类对象
        /// </summary>
        /// <param name="id">标识Id</param>
        /// <returns>子类对象</returns>
        public async Task<TSub> SingleAsync<TSub>(Guid id) where TSub : T
        {
            TSub current = await this.SingleOrDefaultAsync<TSub>(id);

            #region # 验证

            if (current == null)
            {
                throw new NullReferenceException($"Id为\"{id}\"的{typeof(TSub).Name}实体不存在！");
            }

            #endregion

            return current;
        }
        #endregion

        #region # 获取唯一实体对象 —— TRowable SingleOrDefault(long rowNo)
        /// <summary>
        /// 获取唯一实体对象
        /// </summary>
        /// <param name="rowNo">行号</param>
        /// <returns>实体对象</returns>
        public TRowable SingleOrDefault<TRowable>(long rowNo) where TRowable : T, IRowable
        {
            return this.SingleOrDefault<TRowable>(x => x.RowNo == rowNo);
        }
        #endregion

        #region # 获取唯一实体对象 —— Task<TRowable> SingleOrDefaultAsync(long rowNo)
        /// <summary>
        /// 获取唯一实体对象
        /// </summary>
        /// <param name="rowNo">行号</param>
        /// <returns>实体对象</returns>
        public async Task<TRowable> SingleOrDefaultAsync<TRowable>(long rowNo) where TRowable : T, IRowable
        {
            return await this.SingleOrDefaultAsync<TRowable>(x => x.RowNo == rowNo);
        }
        #endregion

        #region # 获取唯一实体对象 —— TRowable Single(long rowNo)
        /// <summary>
        /// 获取唯一实体对象
        /// </summary>
        /// <param name="rowNo">行号</param>
        /// <returns>实体对象</returns>
        public TRowable Single<TRowable>(long rowNo) where TRowable : T, IRowable
        {
            TRowable current = this.SingleOrDefault<TRowable>(rowNo);

            #region # 验证

            if (current == null)
            {
                throw new NullReferenceException($"行号为\"{rowNo}\"的{typeof(TRowable).Name}实体不存在！");
            }

            #endregion

            return current;
        }
        #endregion

        #region # 获取唯一实体对象 —— Task<TRowable> SingleAsync(long rowNo)
        /// <summary>
        /// 获取唯一实体对象
        /// </summary>
        /// <param name="rowNo">行号</param>
        /// <returns>实体对象</returns>
        public async Task<TRowable> SingleAsync<TRowable>(long rowNo) where TRowable : T, IRowable
        {
            TRowable current = await this.SingleOrDefaultAsync<TRowable>(rowNo);

            #region # 验证

            if (current == null)
            {
                throw new NullReferenceException($"行号为\"{rowNo}\"的{typeof(TRowable).Name}实体不存在！");
            }

            #endregion

            return current;
        }
        #endregion

        #region # 获取默认或第一个实体对象 —— T FirstOrDefault()
        /// <summary>
        /// 获取默认或第一个实体对象
        /// </summary>
        /// <returns>实体对象</returns>
        public virtual T FirstOrDefault()
        {
            return this.FindAllAndSort().FirstOrDefault();
        }
        #endregion

        #region # 获取默认或第一个实体对象 —— Task<T> FirstOrDefaultAsync()
        /// <summary>
        /// 获取默认或第一个实体对象
        /// </summary>
        /// <returns>实体对象</returns>
        public virtual async Task<T> FirstOrDefaultAsync()
        {
            return await this.FindAllAndSort().FirstOrDefaultAsync();
        }
        #endregion

        #region # 获取默认或第一个子类对象 —— TSub FirstOrDefault<TSub>()
        /// <summary>
        /// 获取默认或第一个子类对象
        /// </summary>
        /// <returns>子类对象</returns>
        public virtual TSub FirstOrDefault<TSub>() where TSub : T
        {
            return this.FindAllInner<TSub>().FirstOrDefault();
        }
        #endregion

        #region # 获取默认或第一个子类对象 —— Task<TSub> FirstOrDefaultAsync<TSub>()
        /// <summary>
        /// 获取默认或第一个子类对象
        /// </summary>
        /// <returns>子类对象</returns>
        public async Task<TSub> FirstOrDefaultAsync<TSub>() where TSub : T
        {
            return await this.FindAllInner<TSub>().FirstOrDefaultAsync();
        }
        #endregion


        //ICollection部分

        #region # 获取实体对象列表 —— ICollection<T> FindAll()
        /// <summary>
        /// 获取实体对象列表
        /// </summary>
        /// <returns>实体对象列表</returns>
        public ICollection<T> FindAll()
        {
            return this.FindAllInner().ToList();
        }
        #endregion

        #region # 获取实体对象列表 —— Task<ICollection<T>> FindAllAsync()
        /// <summary>
        /// 获取实体对象列表
        /// </summary>
        /// <returns>实体对象列表</returns>
        public async Task<ICollection<T>> FindAllAsync()
        {
            return await this.FindAllInner().ToListAsync();
        }
        #endregion

        #region # 获取子类对象列表 —— ICollection<TSub> FindAll<TSub>()
        /// <summary>
        /// 获取子类对象列表
        /// </summary>
        /// <typeparam name="TSub">子类类型</typeparam>
        /// <returns>子类对象列表</returns>
        public ICollection<TSub> FindAll<TSub>() where TSub : T
        {
            return this.FindAllInner<TSub>().ToList();
        }
        #endregion

        #region # 获取子类对象列表 —— Task<ICollection<TSub>> FindAllAsync<TSub>()
        /// <summary>
        /// 获取子类对象列表
        /// </summary>
        /// <typeparam name="TSub">子类类型</typeparam>
        /// <returns>子类对象列表</returns>
        public async Task<ICollection<TSub>> FindAllAsync<TSub>() where TSub : T
        {
            return await this.FindAllInner<TSub>().ToListAsync();
        }
        #endregion


        //IDictionary部分

        #region # 获取实体对象字典 —— IDictionary<Guid, T> Find(IEnumerable<Guid> ids)
        /// <summary>
        /// 获取实体对象字典
        /// </summary>
        /// <param name="ids">标识Id集</param>
        /// <returns>实体对象字典</returns>
        /// <remarks>IDictionary[Guid, T]，[Id, 实体对象]</remarks>
        public IDictionary<Guid, T> Find(IEnumerable<Guid> ids)
        {
            #region # 验证

            ids = ids?.Distinct().ToArray() ?? Array.Empty<Guid>();
            if (!ids.Any())
            {
                return new Dictionary<Guid, T>();
            }

            #endregion

            var entities = from entity in this._dbContext.Set<T>()
                           where ids.Contains(entity.Id)
                           select new { entity.Id, entity };

            return entities.ToDictionary(x => x.Id, x => x.entity);
        }
        #endregion

        #region # 获取实体对象字典 —— Task<IDictionary<Guid, T>> FindAsync(IEnumerable<Guid> ids)
        /// <summary>
        /// 获取实体对象字典
        /// </summary>
        /// <param name="ids">标识Id集</param>
        /// <returns>实体对象字典</returns>
        /// <remarks>IDictionary[Guid, T]，[Id, 实体对象]</remarks>
        public async Task<IDictionary<Guid, T>> FindAsync(IEnumerable<Guid> ids)
        {
            #region # 验证

            ids = ids?.Distinct().ToArray() ?? Array.Empty<Guid>();
            if (!ids.Any())
            {
                return new Dictionary<Guid, T>();
            }

            #endregion

            var entities = from entity in this._dbContext.Set<T>()
                           where ids.Contains(entity.Id)
                           select new { entity.Id, entity };

            return await entities.ToDictionaryAsync(x => x.Id, x => x.entity);
        }
        #endregion

        #region # 获取子类对象字典 —— IDictionary<Guid, TSub> Find<TSub>(IEnumerable<Guid> ids)
        /// <summary>
        /// 获取子类对象字典
        /// </summary>
        /// <param name="ids">标识Id集</param>
        /// <returns>子类对象字典</returns>
        /// <remarks>IDictionary[Guid, TSub]，[Id, 子类对象]</remarks>
        public IDictionary<Guid, TSub> Find<TSub>(IEnumerable<Guid> ids) where TSub : T
        {
            #region # 验证

            ids = ids?.Distinct().ToArray() ?? Array.Empty<Guid>();
            if (!ids.Any())
            {
                return new Dictionary<Guid, TSub>();
            }

            #endregion

            var entities = from entity in this._dbContext.Set<TSub>()
                           where ids.Contains(entity.Id)
                           select new { entity.Id, entity };

            return entities.ToDictionary(x => x.Id, x => x.entity);
        }
        #endregion

        #region # 获取子类对象字典 —— Task<IDictionary<Guid, TSub>> FindAsync<TSub>(IEnumerable<Guid> ids)
        /// <summary>
        /// 获取子类对象字典
        /// </summary>
        /// <param name="ids">标识Id集</param>
        /// <returns>子类对象字典</returns>
        /// <remarks>IDictionary[Guid, TSub]，[Id, 子类对象]</remarks>
        public async Task<IDictionary<Guid, TSub>> FindAsync<TSub>(IEnumerable<Guid> ids) where TSub : T
        {
            #region # 验证

            ids = ids?.Distinct().ToArray() ?? Array.Empty<Guid>();
            if (!ids.Any())
            {
                return new Dictionary<Guid, TSub>();
            }

            #endregion

            var entities = from entity in this._dbContext.Set<TSub>()
                           where ids.Contains(entity.Id)
                           select new { entity.Id, entity };

            return await entities.ToDictionaryAsync(x => x.Id, x => x.entity);
        }
        #endregion

        #region # 获取实体对象字典 —— IDictionary<long, TRowable> Find<TRowable>(IEnumerable<long> rowNos)
        /// <summary>
        /// 获取实体对象字典
        /// </summary>
        /// <param name="rowNos">行号集</param>
        /// <returns>实体对象字典</returns>
        /// <remarks>IDictionary[long, TRowable]，[行号, 实体对象]</remarks>
        public IDictionary<long, TRowable> Find<TRowable>(IEnumerable<long> rowNos) where TRowable : T, IRowable
        {
            #region # 验证

            rowNos = rowNos?.Distinct().ToArray() ?? Array.Empty<long>();
            if (!rowNos.Any())
            {
                return new Dictionary<long, TRowable>();
            }

            #endregion

            var entities = from entity in this._dbContext.Set<TRowable>()
                           where rowNos.Contains(entity.RowNo)
                           select new { entity.RowNo, entity };

            return entities.ToDictionary(x => x.RowNo, x => x.entity);
        }
        #endregion

        #region # 获取实体对象字典 —— Task<IDictionary<long, TRowable>> FindAsync<TRowable>(IEnumerable<long> rowNos)
        /// <summary>
        /// 获取实体对象字典
        /// </summary>
        /// <param name="rowNos">行号集</param>
        /// <returns>实体对象字典</returns>
        /// <remarks>IDictionary[long, TRowable]，[行号, 实体对象]</remarks>
        public async Task<IDictionary<long, TRowable>> FindAsync<TRowable>(IEnumerable<long> rowNos) where TRowable : T, IRowable
        {
            #region # 验证

            rowNos = rowNos?.Distinct().ToArray() ?? Array.Empty<long>();
            if (!rowNos.Any())
            {
                return new Dictionary<long, TRowable>();
            }

            #endregion

            var entities = from entity in this._dbContext.Set<TRowable>()
                           where rowNos.Contains(entity.RowNo)
                           select new { entity.RowNo, entity };

            return await entities.ToDictionaryAsync(x => x.RowNo, x => x.entity);
        }
        #endregion


        //Count部分

        #region # 获取总记录数 —— long Count()
        /// <summary>
        /// 获取总记录数
        /// </summary>
        /// <returns>总记录数</returns>
        public long Count()
        {
            return this.Count(x => true);
        }
        #endregion

        #region # 获取总记录数 —— Task<long> CountAsync()
        /// <summary>
        /// 获取总记录数
        /// </summary>
        /// <returns>总记录数</returns>
        public async Task<long> CountAsync()
        {
            return await this.CountAsync(x => true);
        }
        #endregion

        #region # 获取子类记录数 —— long Count<TSub>()
        /// <summary>
        /// 获取子类记录数
        /// </summary>
        /// <returns>子类记录条数</returns>
        public long Count<TSub>() where TSub : T
        {
            return this.Count<TSub>(x => true);
        }
        #endregion

        #region # 获取子类记录数 —— Task<long> CountAsync<TSub>()
        /// <summary>
        /// 获取子类记录数
        /// </summary>
        /// <returns>子类记录条数</returns>
        public async Task<long> CountAsync<TSub>() where TSub : T
        {
            return await this.CountAsync<TSub>(x => true);
        }
        #endregion


        //Exists部分

        #region # 是否存在给定Id的实体对象 —— bool Exists(Guid id)
        /// <summary>
        /// 是否存在给定Id的实体对象
        /// </summary>
        /// <param name="id">标识Id</param>
        /// <returns>是否存在</returns>
        public bool Exists(Guid id)
        {
            return this.Exists(x => x.Id == id);
        }
        #endregion

        #region # 是否存在给定Id的实体对象 —— Task<bool> ExistsAsync(Guid id)
        /// <summary>
        /// 是否存在给定Id的实体对象
        /// </summary>
        /// <param name="id">标识Id</param>
        /// <returns>是否存在</returns>
        public async Task<bool> ExistsAsync(Guid id)
        {
            return await this.ExistsAsync(x => x.Id == id);
        }
        #endregion

        #region # 是否存在给定Id的子类对象 —— bool Exists<TSub>(Guid id)
        /// <summary>
        /// 是否存在给定Id的子类对象
        /// </summary>
        /// <param name="id">标识Id</param>
        /// <returns>是否存在</returns>
        public bool Exists<TSub>(Guid id) where TSub : T
        {
            return this.Exists<TSub>(x => x.Id == id);
        }
        #endregion

        #region # 是否存在给定Id的子类对象 —— Task<bool> ExistsAsync<TSub>(Guid id)
        /// <summary>
        /// 是否存在给定Id的子类对象
        /// </summary>
        /// <param name="id">标识Id</param>
        /// <returns>是否存在</returns>
        public async Task<bool> ExistsAsync<TSub>(Guid id) where TSub : T
        {
            return await this.ExistsAsync<TSub>(x => x.Id == id);
        }
        #endregion

        #region # 是否存在给定行号的实体对象 —— bool Exists<TRowable>(long rowNo)
        /// <summary>
        /// 是否存在给定行号的实体对象
        /// </summary>
        /// <param name="rowNo">行号</param>
        /// <returns>是否存在</returns>
        public bool Exists<TRowable>(long rowNo) where TRowable : T, IRowable
        {
            return this.Exists<TRowable>(x => x.RowNo == rowNo);
        }
        #endregion

        #region # 是否存在给定行号的实体对象 —— Task<bool> ExistsAsync<TRowable>(long rowNo)
        /// <summary>
        /// 是否存在给定行号的实体对象
        /// </summary>
        /// <param name="rowNo">行号</param>
        /// <returns>是否存在</returns>
        public async Task<bool> ExistsAsync<TRowable>(long rowNo) where TRowable : T, IRowable
        {
            return await this.ExistsAsync<TRowable>(x => x.RowNo == rowNo);
        }
        #endregion


        //其他

        #region # 执行SQL查询 —— ICollection<TEntity> ExecuteSqlQuery<TEntity>(string sql...
        /// <summary>
        /// 执行SQL查询
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">参数集</param>
        /// <returns>实体对象列表</returns>
        public ICollection<TEntity> ExecuteSqlQuery<TEntity>(string sql, params object[] parameters)
        {
            #region # 验证

            if (string.IsNullOrWhiteSpace(sql))
            {
                throw new ArgumentNullException(nameof(sql), "SQL语句不可为空！");
            }

            #endregion

            DbConnection dbConnection = this._dbContext.Database.GetDbConnection();
            if (dbConnection.State != ConnectionState.Open)
            {
                dbConnection.Open();
            }
            DbCommand dbCommand = dbConnection.CreateCommand();
            dbCommand.CommandText = sql;
            if (parameters != null && parameters.Any())
            {
                dbCommand.Parameters.AddRange(parameters);
            }

            DbDataReader dataReader = dbCommand.ExecuteReader();
            DataTable dataTable = new DataTable();
            dataTable.Load(dataReader);
            dataReader.Close();

            //获取类型与属性列表
            Type type = typeof(TEntity);
            PropertyInfo[] propertyInfos = type.GetProperties();

            //获取无参构造函数
            ConstructorInfo[] constructors = type.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            ConstructorInfo noParamConstructor = constructors.Single(ctor => ctor.GetParameters().Length == 0);

            IList<TEntity> entities = new List<TEntity>();
            foreach (DataRow dataRow in dataTable.Rows)
            {
                TEntity entity = (TEntity)noParamConstructor.Invoke(null);
                foreach (PropertyInfo property in propertyInfos)
                {
                    if (dataTable.Columns.Contains(property.Name))
                    {
                        MethodInfo propertySetter = property.GetSetMethod(true);
                        if (propertySetter != null)
                        {
                            object value = dataRow[property.Name] == DBNull.Value
                                ? null
                                : dataRow[property.Name];
                            propertySetter.Invoke(entity, new[] { value });
                        }
                    }
                }
                entities.Add(entity);
            }

            return entities;
        }
        #endregion

        #region # 执行SQL查询 —— Task<ICollection<TEntity>> ExecuteSqlQueryAsync<TEntity>(string sql...
        /// <summary>
        /// 执行SQL查询
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">参数集</param>
        /// <returns>实体对象集合</returns>
        public async Task<ICollection<TEntity>> ExecuteSqlQueryAsync<TEntity>(string sql, params object[] parameters)
        {
            #region # 验证

            if (string.IsNullOrWhiteSpace(sql))
            {
                throw new ArgumentNullException(nameof(sql), "SQL语句不可为空！");
            }

            #endregion

            DbConnection dbConnection = this._dbContext.Database.GetDbConnection();
            if (dbConnection.State != ConnectionState.Open)
            {
                await dbConnection.OpenAsync();
            }
            DbCommand dbCommand = dbConnection.CreateCommand();
            dbCommand.CommandText = sql;
            if (parameters != null && parameters.Any())
            {
                dbCommand.Parameters.AddRange(parameters);
            }

            DbDataReader dataReader = await dbCommand.ExecuteReaderAsync();
            DataTable dataTable = new DataTable();
            dataTable.Load(dataReader);
#if NETSTANDARD2_0
            dataReader.Close();
#endif
#if NETSTANDARD2_1
            await dataReader.CloseAsync();
#endif
            //获取类型与属性列表
            Type type = typeof(TEntity);
            PropertyInfo[] propertyInfos = type.GetProperties();

            //获取无参构造函数
            ConstructorInfo[] constructors = type.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            ConstructorInfo noParamConstructor = constructors.Single(ctor => ctor.GetParameters().Length == 0);

            IList<TEntity> entities = new List<TEntity>();
            foreach (DataRow dataRow in dataTable.Rows)
            {
                TEntity entity = (TEntity)noParamConstructor.Invoke(null);
                foreach (PropertyInfo property in propertyInfos)
                {
                    if (dataTable.Columns.Contains(property.Name))
                    {
                        MethodInfo propertySetter = property.GetSetMethod(true);
                        if (propertySetter != null)
                        {
                            object value = dataRow[property.Name] == DBNull.Value
                                ? null
                                : dataRow[property.Name];
                            propertySetter.Invoke(entity, new[] { value });
                        }
                    }
                }
                entities.Add(entity);
            }

            return entities;
        }
        #endregion

        #region # 释放资源 —— void Dispose()
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            this._dbContext?.Dispose();
        }
        #endregion

        #endregion

        #region # Protected

        //Single部分

        #region # 获取唯一实体对象 —— T SingleOrDefault(Expression<Func<T, bool>> condition)
        /// <summary>
        /// 获取唯一实体对象
        /// </summary>
        /// <param name="condition">条件表达式</param>
        /// <returns>实体对象</returns>
        protected T SingleOrDefault(Expression<Func<T, bool>> condition)
        {
            #region # 验证

            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition), "条件表达式不可为空！");
            }

            #endregion

            return this.FindAllInner().SingleOrDefault(condition);
        }
        #endregion

        #region # 获取唯一实体对象 —— Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>>...
        /// <summary>
        /// 获取唯一实体对象
        /// </summary>
        /// <param name="condition">条件表达式</param>
        /// <returns>实体对象</returns>
        protected async Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> condition)
        {
            #region # 验证

            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition), "条件表达式不可为空！");
            }

            #endregion

            return await this.FindAllInner().SingleOrDefaultAsync(condition);
        }
        #endregion

        #region # 获取唯一子类对象 —— TSub SingleOrDefault<TSub>(Expression<Func<TSub>...
        /// <summary>
        /// 获取唯一子类对象
        /// </summary>
        /// <param name="condition">条件表达式</param>
        /// <returns>子类对象</returns>
        protected TSub SingleOrDefault<TSub>(Expression<Func<TSub, bool>> condition) where TSub : T
        {
            #region # 验证

            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition), "条件表达式不可为空！");
            }

            #endregion

            return this.FindAllInner<TSub>().SingleOrDefault(condition);
        }
        #endregion

        #region # 获取唯一子类对象 —— Task<TSub> SingleOrDefaultAsync<TSub>(Expression...
        /// <summary>
        /// 获取唯一子类对象
        /// </summary>
        /// <param name="condition">条件表达式</param>
        /// <returns>子类对象</returns>
        protected async Task<TSub> SingleOrDefaultAsync<TSub>(Expression<Func<TSub, bool>> condition) where TSub : T
        {
            #region # 验证

            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition), "条件表达式不可为空！");
            }

            #endregion

            return await this.FindAllInner<TSub>().SingleOrDefaultAsync(condition);
        }
        #endregion

        #region # 获取默认或第一个实体对象 —— T FirstOrDefault(Expression<Func<T, bool>> condition)
        /// <summary>
        /// 获取默认或第一个实体对象
        /// </summary>
        /// <param name="condition">条件表达式</param>
        /// <returns>实体对象</returns>
        protected T FirstOrDefault(Expression<Func<T, bool>> condition)
        {
            #region # 验证

            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition), "条件表达式不可为空！");
            }

            #endregion

            return this.Find(condition).FirstOrDefault();
        }
        #endregion

        #region # 获取默认或第一个实体对象 —— Task<T> FirstOrDefaultAsync(Expression<Func<T...
        /// <summary>
        /// 获取默认或第一个实体对象
        /// </summary>
        /// <param name="condition">条件表达式</param>
        /// <returns>实体对象</returns>
        protected async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> condition)
        {
            #region # 验证

            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition), "条件表达式不可为空！");
            }

            #endregion

            return await this.Find(condition).FirstOrDefaultAsync();
        }
        #endregion

        #region # 获取默认或第一个子类对象 —— TSub FirstOrDefault<TSub>(Expression<Func<TSub...
        /// <summary>
        /// 获取默认或第一个子类对象
        /// </summary>
        /// <typeparam name="TSub">子类类型</typeparam>
        /// <param name="condition">条件表达式</param>
        /// <returns>子类对象</returns>
        protected TSub FirstOrDefault<TSub>(Expression<Func<TSub, bool>> condition) where TSub : T
        {
            #region # 验证

            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition), "条件表达式不可为空！");
            }

            #endregion

            return this.Find(condition).FirstOrDefault();
        }
        #endregion

        #region # 获取默认或第一个子类对象 —— Task<TSub> FirstOrDefaultAsync<TSub>(Expression...
        /// <summary>
        /// 获取默认或第一个子类对象
        /// </summary>
        /// <typeparam name="TSub">子类类型</typeparam>
        /// <param name="condition">条件表达式</param>
        /// <returns>子类对象</returns>
        /// <remarks>无该对象时返回null</remarks>
        protected async Task<TSub> FirstOrDefaultAsync<TSub>(Expression<Func<TSub, bool>> condition) where TSub : T
        {
            #region # 验证

            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition), "条件表达式不可为空！");
            }

            #endregion

            return await this.Find(condition).FirstOrDefaultAsync();
        }
        #endregion


        //IQueryable部分

        #region # 获取实体对象列表 —— virtual IQueryable<T> FindAllInner()
        /// <summary>
        /// 获取实体对象列表
        /// </summary>
        /// <returns>实体对象列表</returns>
        protected virtual IQueryable<T> FindAllInner()
        {
            Expression<Func<T, bool>> condition = RepositoryExtension.BuildFilterExpression<T>();
            return this._dbContext.Set<T>().Where(condition);
        }
        #endregion

        #region # 获取子类对象列表 —— IQueryable<TSub> FindAllInner<TSub>()
        /// <summary>
        /// 获取子类对象列表
        /// </summary>
        /// <typeparam name="TSub">子类类型</typeparam>
        /// <returns>子类对象列表</returns>
        protected IQueryable<TSub> FindAllInner<TSub>() where TSub : T
        {
            return this.FindAllInner().OfType<TSub>();
        }
        #endregion

        #region # 获取实体对象列表（默认排序） —— IOrderedQueryable<T> FindAllAndSort()
        /// <summary>
        /// 获取实体对象列表（默认排序）
        /// </summary>
        /// <returns>实体对象列表</returns>
        protected IOrderedQueryable<T> FindAllAndSort()
        {
            return this.FindAndSort(x => true);
        }
        #endregion

        #region # 获取子类对象列表（默认排序） —— IOrderedQueryable<TSub> FindAllAndSort<TSub>()
        /// <summary>
        /// 获取子类对象列表（默认排序）
        /// </summary>
        /// <typeparam name="TSub">子类类型</typeparam>
        /// <returns>子类对象列表</returns>
        protected IOrderedQueryable<TSub> FindAllAndSort<TSub>() where TSub : T
        {
            return this.FindAndSort<TSub>(x => true);
        }
        #endregion

        #region # 获取实体对象列表 —— IQueryable<T> Find(Expression<Func<T, bool>> condition)
        /// <summary>
        /// 获取实体对象列表
        /// </summary>
        /// <param name="condition">条件表达式</param>
        /// <returns>实体对象列表</returns>
        protected IQueryable<T> Find(Expression<Func<T, bool>> condition)
        {
            #region # 验证

            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition), "条件表达式不可为空！");
            }

            #endregion

            return this.FindAllInner().Where(condition);
        }
        #endregion

        #region # 获取子类对象列表 —— IQueryable<TSub> Find<TSub>(Expression<Func<TSub, bool>>...
        /// <summary>
        /// 获取子类对象列表
        /// </summary>
        /// <typeparam name="TSub">子类类型</typeparam>
        /// <param name="condition">条件表达式</param>
        /// <returns>子类对象列表</returns>
        protected IQueryable<TSub> Find<TSub>(Expression<Func<TSub, bool>> condition) where TSub : T
        {
            #region # 验证

            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition), "条件表达式不可为空！");
            }

            #endregion

            return this.FindAllInner<TSub>().Where(condition);
        }
        #endregion

        #region # 获取实体对象列表（默认排序） —— virtual IOrderedQueryable<T> FindAndSort(...
        /// <summary>
        /// 获取实体对象列表（默认排序）
        /// </summary>
        /// <returns>实体对象列表</returns>
        protected virtual IOrderedQueryable<T> FindAndSort(Expression<Func<T, bool>> condition)
        {
            return this.Find(condition).OrderByDescending(x => x.AddedTime);
        }
        #endregion

        #region # 获取子类对象列表（默认排序） —— virtual IOrderedQueryable<TSub> FindAndSort<TSub>(...
        /// <summary>
        /// 获取子类对象列表（默认排序）
        /// </summary>
        /// <returns>实体对象列表</returns>
        protected virtual IOrderedQueryable<TSub> FindAndSort<TSub>(Expression<Func<TSub, bool>> condition) where TSub : T
        {
            return this.Find<TSub>(condition).OrderByDescending(x => x.AddedTime);
        }
        #endregion

        #region # 获取实体对象Id列表 —— IQueryable<Guid> FindIds(Expression<Func<T, bool>> condition)
        /// <summary>
        /// 获取实体对象Id列表
        /// </summary>
        /// <param name="condition">条件表达式</param>
        /// <returns>实体对象Id列表</returns>
        protected IQueryable<Guid> FindIds(Expression<Func<T, bool>> condition)
        {
            return this.Find(condition).Select(x => x.Id);
        }
        #endregion

        #region # 获取子类对象Id列表 —— IQueryable<Guid> FindIds<TSub>(Expression<Func<...
        /// <summary>
        /// 获取子类对象Id列表
        /// </summary>
        /// <param name="condition">条件表达式</param>
        /// <returns>子类对象Id列表</returns>
        protected IQueryable<Guid> FindIds<TSub>(Expression<Func<TSub, bool>> condition) where TSub : T
        {
            return this.Find(condition).Select(x => x.Id);
        }
        #endregion

        #region # 分页获取实体对象列表 —— IQueryable<T> FindByPage(...
        /// <summary>
        /// 分页获取实体对象列表
        /// </summary>
        /// <param name="condition">条件表达式</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页容量</param>
        /// <param name="rowCount">总记录数</param>
        /// <param name="pageCount">总页数</param>
        /// <returns>实体对象列表</returns>
        protected IQueryable<T> FindByPage(Expression<Func<T, bool>> condition, int pageIndex, int pageSize, out int rowCount, out int pageCount)
        {
            return this.FindAndSort(condition).ToPage(pageIndex, pageSize, out rowCount, out pageCount);
        }
        #endregion

        #region # 分页获取实体对象列表 —— Task<Page<T>> FindByPageAsync(...
        /// <summary>
        /// 分页获取实体对象列表
        /// </summary>
        /// <param name="condition">条件表达式</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页容量</param>
        /// <returns>实体对象列表</returns>
        protected async Task<Page<T>> FindByPageAsync(Expression<Func<T, bool>> condition, int pageIndex, int pageSize)
        {
            return await this.FindAndSort(condition).ToPageAsync(pageIndex, pageSize);
        }
        #endregion

        #region # 分页获取子类对象列表 —— IQueryable<TSub> FindByPage<TSub>(...
        /// <summary>
        /// 分页获取子类对象列表
        /// </summary>
        /// <typeparam name="TSub">子类类型</typeparam>
        /// <param name="condition">条件表达式</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页容量</param>
        /// <param name="rowCount">总记录数</param>
        /// <param name="pageCount">总页数</param>
        /// <returns>子类对象列表</returns>
        protected IQueryable<TSub> FindByPage<TSub>(Expression<Func<TSub, bool>> condition, int pageIndex, int pageSize, out int rowCount, out int pageCount) where TSub : T
        {
            return this.FindAndSort<TSub>(condition).ToPage(pageIndex, pageSize, out rowCount, out pageCount);
        }
        #endregion

        #region # 分页获取子类对象列表 —— Task<Page<TSub>> FindByPageAsync<TSub>(...
        /// <summary>
        /// 分页获取子类对象列表
        /// </summary>
        /// <typeparam name="TSub">子类类型</typeparam>
        /// <param name="condition">条件表达式</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页容量</param>
        /// <returns>子类对象列表</returns>
        protected async Task<Page<TSub>> FindByPageAsync<TSub>(Expression<Func<TSub, bool>> condition, int pageIndex, int pageSize) where TSub : T
        {
            return await this.FindAndSort<TSub>(condition).ToPageAsync(pageIndex, pageSize);
        }
        #endregion


        //Count部分

        #region # 获取记录数 —— long Count(Expression<Func<T, bool>> condition)
        /// <summary>
        /// 获取记录数
        /// </summary>
        /// <param name="condition">条件表达式</param>
        /// <returns>符合条件的记录数</returns>
        protected long Count(Expression<Func<T, bool>> condition)
        {
            #region # 验证

            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition), "条件表达式不可为空！");
            }

            #endregion

            return this.FindAllInner().LongCount(condition);
        }
        #endregion

        #region # 获取记录数 —— Task<long> CountAsync(Expression<Func<T, bool>> condition)
        /// <summary>
        /// 获取记录数
        /// </summary>
        /// <param name="condition">条件表达式</param>
        /// <returns>符合条件的记录数</returns>
        protected async Task<long> CountAsync(Expression<Func<T, bool>> condition)
        {
            #region # 验证

            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition), "条件表达式不可为空！");
            }

            #endregion

            return await this.FindAllInner().LongCountAsync(condition);
        }
        #endregion

        #region # 获取子类记录数 —— long Count(Expression<Func<T, bool>> condition)
        /// <summary>
        /// 获取子类记录数
        /// </summary>
        /// <param name="condition">条件表达式</param>
        /// <returns>符合条件的子类记录数</returns>
        protected long Count<TSub>(Expression<Func<TSub, bool>> condition) where TSub : T
        {
            #region # 验证

            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition), "条件表达式不可为空！");
            }

            #endregion

            return this.FindAllInner<TSub>().LongCount(condition);
        }
        #endregion

        #region # 获取子类记录数 —— Task<long> CountAsync<TSub>(Expression<Func<TSub, bool>> condition)
        /// <summary>
        /// 获取子类记录数
        /// </summary>
        /// <param name="condition">条件表达式</param>
        /// <returns>符合条件的子类记录数</returns>
        protected async Task<long> CountAsync<TSub>(Expression<Func<TSub, bool>> condition) where TSub : T
        {
            #region # 验证

            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition), "条件表达式不可为空！");
            }

            #endregion

            return await this.FindAllInner<TSub>().LongCountAsync(condition);
        }
        #endregion


        //Exists部分

        #region # 是否存在给定条件的实体对象 —— bool Exists(Expression<Func<T, bool>> condition)
        /// <summary>
        /// 是否存在给定条件的实体对象
        /// </summary>
        /// <param name="condition">条件表达式</param>
        /// <returns>是否存在</returns>
        protected bool Exists(Expression<Func<T, bool>> condition)
        {
            #region # 验证

            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition), "条件表达式不可为空！");
            }

            #endregion

            lock (_Sync)
            {
                return this.FindAllInner().Any(condition);
            }
        }
        #endregion

        #region # 是否存在给定条件的实体对象 —— Task<bool> ExistsAsync(Expression<Func<T, bool>> condition)
        /// <summary>
        /// 是否存在给定条件的实体对象
        /// </summary>
        /// <param name="condition">条件表达式</param>
        /// <returns>是否存在</returns>
        protected async Task<bool> ExistsAsync(Expression<Func<T, bool>> condition)
        {
            #region # 验证

            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition), "条件表达式不可为空！");
            }

            #endregion

            return await this.FindAllInner().AnyAsync(condition);
        }
        #endregion

        #region # 是否存在给定条件的子类对象 —— bool Exists<TSub>(Expression<Func<TSub, bool>> condition)
        /// <summary>
        /// 是否存在给定条件的子类对象
        /// </summary>
        /// <param name="condition">条件表达式</param>
        /// <returns>是否存在</returns>
        protected bool Exists<TSub>(Expression<Func<TSub, bool>> condition) where TSub : T
        {
            #region # 验证

            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition), "条件表达式不可为空！");
            }

            #endregion

            lock (_Sync)
            {
                return this.FindAllInner<TSub>().Any(condition);
            }
        }
        #endregion

        #region # 是否存在给定条件的子类对象 —— Task<bool> ExistsAsync<TSub>(Expression<Func<TSub, bool>> condition)
        /// <summary>
        /// 是否存在给定条件的子类对象
        /// </summary>
        /// <param name="condition">条件表达式</param>
        /// <returns>是否存在</returns>
        protected async Task<bool> ExistsAsync<TSub>(Expression<Func<TSub, bool>> condition) where TSub : T
        {
            #region # 验证

            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition), "条件表达式不可为空！");
            }

            #endregion

            return await this.FindAllInner<TSub>().AnyAsync(condition);
        }
        #endregion

        #endregion
    }
}
