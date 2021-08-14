using SD.Infrastructure.EntityBase;
using SD.Infrastructure.Repository.EntityFramework.Base;
using SD.Infrastructure.RepositoryBase;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace SD.Infrastructure.Repository.EntityFramework
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
            this._dbContext = BaseDbSession.QueryInstance;
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

        #region # 根据Id获取唯一实体对象（查看时用） —— T SingleOrDefault(Guid id)
        /// <summary>
        /// 根据Id获取唯一实体对象（查看时用），
        /// </summary>
        /// <param name="id">标识Id</param>
        /// <returns>实体对象</returns>
        /// <remarks>无该对象时返回null</remarks>
        public T SingleOrDefault(Guid id)
        {
            return this.SingleOrDefault(x => x.Id == id);
        }
        #endregion

        #region # 根据Id获取唯一子类对象（查看时用） —— TSub SingleOrDefault<TSub>(Guid id)
        /// <summary>
        /// 根据Id获取唯一子类对象（查看时用），
        /// </summary>
        /// <param name="id">标识Id</param>
        /// <returns>子类对象</returns>
        /// <remarks>无该对象时返回null</remarks>
        public TSub SingleOrDefault<TSub>(Guid id) where TSub : T
        {
            return this.SingleOrDefault<TSub>(x => x.Id == id);
        }
        #endregion

        #region # 根据Id获取唯一实体对象（查看时用） —— T Single(Guid id)
        /// <summary>
        /// 根据Id获取唯一实体对象（查看时用），
        /// </summary>
        /// <param name="id">标识Id</param>
        /// <returns>实体对象</returns>
        public T Single(Guid id)
        {
            T current = this.SingleOrDefault(id);

            #region # 非空验证

            if (current == null)
            {
                throw new NullReferenceException($"Id为\"{id}\"的{typeof(T).Name}实体不存在！");
            }

            #endregion

            return current;
        }
        #endregion

        #region # 根据Id获取唯一子类对象（查看时用） —— TSub Single<TSub>(Guid id)
        /// <summary>
        /// 根据Id获取唯一子类对象（查看时用），
        /// </summary>
        /// <param name="id">标识Id</param>
        /// <returns>子类对象</returns>
        public TSub Single<TSub>(Guid id) where TSub : T
        {
            TSub current = this.SingleOrDefault<TSub>(id);

            #region # 非空验证

            if (current == null)
            {
                throw new NullReferenceException($"Id为\"{id}\"的{typeof(TSub).Name}实体不存在！");
            }

            #endregion

            return current;
        }
        #endregion

        #region # 获取默认或第一个实体对象 —— T FirstOrDefault()
        /// <summary>
        /// 获取默认或第一个实体对象，
        /// </summary>
        /// <remarks>无该对象时返回null</remarks>
        public virtual T FirstOrDefault()
        {
            return this.FindAllBySort().FirstOrDefault();
        }
        #endregion

        #region # 获取默认或第一个子类对象 —— TSub FirstOrDefault<TSub>()
        /// <summary>
        /// 获取默认或第一个子类对象，
        /// </summary>
        /// <remarks>无该对象时返回null</remarks>
        public virtual TSub FirstOrDefault<TSub>() where TSub : T
        {
            return this.FindAllInner<TSub>().FirstOrDefault();
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

        #region # 获取给定类型子类对象列表 —— ICollection<TSub> FindAll<TSub>()
        /// <summary>
        /// 获取给定类型子类对象列表
        /// </summary>
        /// <typeparam name="TSub">子类类型</typeparam>
        /// <returns>子类对象列表</returns>
        public ICollection<TSub> FindAll<TSub>() where TSub : T
        {
            return this.FindAllInner<TSub>().ToList();
        }
        #endregion


        //IDictionary部分

        #region # 根据Id集获取实体对象字典 —— IDictionary<Guid, T> Find(IEnumerable<Guid> ids)
        /// <summary>
        /// 根据Id集获取实体对象字典
        /// </summary>
        /// <returns>实体对象字典</returns>
        /// <remarks>IDictionary[Guid, T]，[Id, 实体对象]</remarks>
        public IDictionary<Guid, T> Find(IEnumerable<Guid> ids)
        {
            #region # 验证

            Guid[] ids_ = ids?.Distinct().ToArray() ?? new Guid[0];
            if (!ids_.Any())
            {
                return new Dictionary<Guid, T>();
            }

            #endregion

            var entities = from entity in this._dbContext.Set<T>()
                           where ids_.Contains(entity.Id)
                           select new { entity.Id, entity };

            return entities.ToDictionary(x => x.Id, x => x.entity);
        }
        #endregion

        #region # 根据Id集获取子类对象字典 —— IDictionary<Guid, TSub> Find<TSub>(IEnumerable<Guid> ids)
        /// <summary>
        /// 根据Id集获取子类对象字典
        /// </summary>
        /// <returns>子类对象字典</returns>
        /// <remarks>IDictionary[Guid, TSub]，[Id, 子类对象]</remarks>
        public IDictionary<Guid, TSub> Find<TSub>(IEnumerable<Guid> ids) where TSub : T
        {
            #region # 验证

            Guid[] ids_ = ids?.Distinct().ToArray() ?? new Guid[0];
            if (!ids_.Any())
            {
                return new Dictionary<Guid, TSub>();
            }

            #endregion

            var entities = from entity in this._dbContext.Set<TSub>()
                           where ids_.Contains(entity.Id)
                           select new { entity.Id, entity };

            return entities.ToDictionary(x => x.Id, x => x.entity);
        }
        #endregion


        //Count部分

        #region # 获取总记录条数 —— int Count()
        /// <summary>
        /// 获取总记录条数
        /// </summary>
        /// <returns>总记录条数</returns>
        public int Count()
        {
            return this.Count(x => true);
        }
        #endregion

        #region # 获取子类记录条数 —— int Count<TSub>()
        /// <summary>
        /// 获取子类记录条数
        /// </summary>
        /// <returns>子类记录条数</returns>
        public int Count<TSub>() where TSub : T
        {
            return this.Count<TSub>(x => true);
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
                throw new ArgumentNullException(nameof(sql), @"SQL语句不可为空！");
            }

            #endregion

            return this._dbContext.Database.SqlQuery<TEntity>(sql, parameters).ToList();
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

        #region # 根据条件获取唯一实体对象（查看时用） —— T SingleOrDefault(Expression<Func<T, bool>> predicate)
        /// <summary>
        /// 根据条件获取唯一实体对象（查看时用），
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <returns>实体对象</returns>
        /// <remarks>无该对象时返回null</remarks>
        protected T SingleOrDefault(Expression<Func<T, bool>> predicate)
        {
            #region # 验证

            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate), @"条件表达式不可为空！");
            }

            #endregion

            return this.FindAllInner().SingleOrDefault(predicate);
        }
        #endregion

        #region # 根据条件获取唯一子类对象（查看时用） —— TSub SingleOrDefault<TSub>(Expression<Func<TSub>...
        /// <summary>
        /// 根据条件获取唯一子类对象（查看时用），
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <returns>子类对象</returns>
        /// <remarks>无该对象时返回null</remarks>
        protected TSub SingleOrDefault<TSub>(Expression<Func<TSub, bool>> predicate) where TSub : T
        {
            #region # 验证

            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate), @"条件表达式不可为空！");
            }

            #endregion

            return this.FindAllInner<TSub>().SingleOrDefault(predicate);
        }
        #endregion

        #region # 根据条件获取第一个实体对象（查看时用） —— T FirstOrDefault(Expression<Func<T, bool>> predicate)
        /// <summary>
        /// 根据条件获取第一个实体对象（查看时用），
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <returns>实体对象</returns>
        /// <remarks>无该对象时返回null</remarks>
        protected T FirstOrDefault(Expression<Func<T, bool>> predicate)
        {
            #region # 验证

            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate), @"条件表达式不可为空！");
            }

            #endregion

            return this.Find(predicate).FirstOrDefault();
        }
        #endregion

        #region # 根据条件获取第一个子类对象（查看时用） —— TSub FirstOrDefault<TSub>(Expression<Func<TSub...
        /// <summary>
        /// 根据条件获取第一个子类对象（查看时用），
        /// </summary>
        /// <typeparam name="TSub">子类类型</typeparam>
        /// <param name="predicate">条件</param>
        /// <returns>子类对象</returns>
        /// <remarks>无该对象时返回null</remarks>
        protected TSub FirstOrDefault<TSub>(Expression<Func<TSub, bool>> predicate) where TSub : T
        {
            #region # 验证

            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate), @"条件表达式不可为空！");
            }

            #endregion

            return this.Find(predicate).FirstOrDefault();
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

        #region # 获取给定类型子类对象列表 —— IQueryable<TSub> FindAllInner<TSub>()
        /// <summary>
        /// 获取给定类型子类对象列表
        /// </summary>
        /// <typeparam name="TSub">子类类型</typeparam>
        /// <returns>子类对象列表</returns>
        protected IQueryable<TSub> FindAllInner<TSub>() where TSub : T
        {
            return this.FindAllInner().OfType<TSub>();
        }
        #endregion

        #region # 获取实体对象列表（默认排序） —— IOrderedQueryable<T> FindAllBySort()
        /// <summary>
        /// 获取实体对象列表（默认排序）
        /// </summary>
        /// <returns>实体对象列表</returns>
        protected IOrderedQueryable<T> FindAllBySort()
        {
            return this.FindBySort(x => true);
        }
        #endregion

        #region # 获取给定类型子类对象列表（默认排序） —— IOrderedQueryable<TSub> FindAllBySort<TSub>()
        /// <summary>
        /// 获取给定类型子类对象列表（默认排序）
        /// </summary>
        /// <typeparam name="TSub">子类类型</typeparam>
        /// <returns>子类对象列表</returns>
        protected IOrderedQueryable<TSub> FindAllBySort<TSub>() where TSub : T
        {
            return this.FindBySort<TSub>(x => true);
        }
        #endregion

        #region # 根据条件获取实体对象列表 —— IQueryable<T> Find(Expression<Func<T, bool>> predicate)
        /// <summary>
        /// 根据条件获取实体对象列表
        /// </summary>
        /// <param name="predicate">条件表达式</param>
        /// <returns>实体对象列表</returns>
        protected IQueryable<T> Find(Expression<Func<T, bool>> predicate)
        {
            #region # 验证

            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate), @"条件表达式不可为空！");
            }

            #endregion

            return this.FindAllInner().Where(predicate);
        }
        #endregion

        #region # 根据条件获取子类对象列表 —— IQueryable<TSub> Find<TSub>(Expression<Func<TSub, bool>>...
        /// <summary>
        /// 根据条件获取子类对象列表
        /// </summary>
        /// <typeparam name="TSub">子类类型</typeparam>
        /// <param name="predicate">条件表达式</param>
        /// <returns>子类对象列表</returns>
        protected IQueryable<TSub> Find<TSub>(Expression<Func<TSub, bool>> predicate) where TSub : T
        {
            #region # 验证

            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate), @"条件表达式不可为空！");
            }

            #endregion

            return this.FindAllInner<TSub>().Where(predicate);
        }
        #endregion

        #region # 根据条件获取实体对象列表（默认排序） —— virtual IOrderedQueryable<T> FindBySort(...
        /// <summary>
        /// 根据条件获取实体对象列表（默认排序）
        /// </summary>
        /// <returns>实体对象列表</returns>
        protected virtual IOrderedQueryable<T> FindBySort(Expression<Func<T, bool>> condition)
        {
            return this.Find(condition).OrderByDescending(x => x.AddedTime);
        }
        #endregion

        #region # 根据条件获取子类对象列表（默认排序） —— virtual IOrderedQueryable<TSub> FindBySort<TSub>(...
        /// <summary>
        /// 根据条件获取子类对象列表（默认排序）
        /// </summary>
        /// <returns>实体对象列表</returns>
        protected virtual IOrderedQueryable<TSub> FindBySort<TSub>(Expression<Func<TSub, bool>> condition) where TSub : T
        {
            return this.Find<TSub>(condition).OrderByDescending(x => x.AddedTime);
        }
        #endregion

        #region # 根据条件获取实体对象Id列表 —— IQueryable<Guid> FindIds(Expression<Func<T, bool>> predicate)
        /// <summary>
        /// 根据条件获取实体对象Id列表
        /// </summary>
        /// <param name="predicate">条件表达式</param>
        /// <returns>实体对象Id列表</returns>
        protected IQueryable<Guid> FindIds(Expression<Func<T, bool>> predicate)
        {
            return this.Find(predicate).Select(x => x.Id);
        }
        #endregion

        #region # 根据条件获取子类对象Id列表 —— IQueryable<Guid> FindIds<TSub>(Expression<Func<...
        /// <summary>
        /// 根据条件获取子类对象Id列表
        /// </summary>
        /// <param name="predicate">条件表达式</param>
        /// <returns>子类对象Id列表</returns>
        protected IQueryable<Guid> FindIds<TSub>(Expression<Func<TSub, bool>> predicate) where TSub : T
        {
            return this.Find(predicate).Select(x => x.Id);
        }
        #endregion

        #region # 根据条件分页获取实体对象列表 —— IQueryable<T> FindByPage(...
        /// <summary>
        /// 根据条件分页获取实体对象列表
        /// </summary>
        /// <param name="predicate">条件表达式</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页容量</param>
        /// <param name="rowCount">总记录条数</param>
        /// <param name="pageCount">总页数</param>
        /// <returns>实体对象列表</returns>
        protected IQueryable<T> FindByPage(Expression<Func<T, bool>> predicate, int pageIndex, int pageSize, out int rowCount, out int pageCount)
        {
            return this.FindBySort(predicate).ToPage(pageIndex, pageSize, out rowCount, out pageCount);
        }
        #endregion

        #region # 根据条件分页获取子类对象列表 —— IQueryable<TSub> FindByPage<TSub>(...
        /// <summary>
        /// 根据条件分页获取子类对象列表
        /// </summary>
        /// <typeparam name="TSub">子类类型</typeparam>
        /// <param name="predicate">条件表达式</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页容量</param>
        /// <param name="rowCount">总记录条数</param>
        /// <param name="pageCount">总页数</param>
        /// <returns>实体对象列表</returns>
        protected IQueryable<TSub> FindByPage<TSub>(Expression<Func<TSub, bool>> predicate, int pageIndex, int pageSize, out int rowCount, out int pageCount) where TSub : T
        {
            return this.FindBySort<TSub>(predicate).ToPage(pageIndex, pageSize, out rowCount, out pageCount);
        }
        #endregion


        //Count部分

        #region # 根据条件获取记录条数 —— int Count(Expression<Func<T, bool>> predicate)
        /// <summary>
        /// 根据条件获取记录条数
        /// </summary>
        /// <param name="predicate">条件表达式</param>
        /// <returns>符合条件的记录条数</returns>
        protected int Count(Expression<Func<T, bool>> predicate)
        {
            #region # 验证

            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate), @"条件表达式不可为空！");
            }

            #endregion

            return this.FindAllInner().Count(predicate);
        }
        #endregion

        #region # 根据条件获取子类记录条数 —— int Count(Expression<Func<T, bool>> predicate)
        /// <summary>
        /// 根据条件获取子类记录条数
        /// </summary>
        /// <param name="predicate">条件表达式</param>
        /// <returns>符合条件的子类记录条数</returns>
        protected int Count<TSub>(Expression<Func<TSub, bool>> predicate) where TSub : T
        {
            #region # 验证

            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate), @"条件表达式不可为空！");
            }

            #endregion

            return this.FindAllInner<TSub>().Count(predicate);
        }
        #endregion


        //Exists部分

        #region # 是否存在给定条件的实体对象 —— bool Exists(Expression<Func<T, bool>> predicate)
        /// <summary>
        /// 是否存在给定条件的实体对象
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <returns>是否存在</returns>
        protected bool Exists(Expression<Func<T, bool>> predicate)
        {
            #region # 验证

            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate), @"条件表达式不可为空！");
            }

            #endregion

            lock (_Sync)
            {
                return this.FindAllInner().Any(predicate);
            }
        }
        #endregion

        #region # 是否存在给定条件的子类对象 —— bool Exists<TSub>(Expression<Func<TSub, bool>> predicate)
        /// <summary>
        /// 是否存在给定条件的子类对象
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <returns>是否存在</returns>
        protected bool Exists<TSub>(Expression<Func<TSub, bool>> predicate) where TSub : T
        {
            #region # 验证

            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate), @"条件表达式不可为空！");
            }

            #endregion

            lock (_Sync)
            {
                return this.FindAllInner<TSub>().Any(predicate);
            }
        }
        #endregion

        #endregion
    }
}
