using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using ShSoft.Framework2016.Infrastructure.IEntity;
using ShSoft.Framework2016.Infrastructure.IRepository;
using ShSoft.Framework2016.Infrastructure.Repository.EntityFrameworkProvider.Base;

namespace ShSoft.Framework2016.Infrastructure.Repository.EntityFrameworkProvider
{
    /// <summary>
    /// EF仓储Provider
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    public abstract class EFRepositoryProvider<T> : IRepository<T> where T : PlainEntity
    {
        #region # 创建EF（读）上下文对象

        /// <summary>
        /// EF（读）上下文对象
        /// </summary>
        private readonly DbContext _dbContext;

        /// <summary>
        /// 构造器
        /// </summary>
        protected EFRepositoryProvider()
        {
            //EF（读）上下文对象
            this._dbContext = BaseDbSession.QueryInstance;
        }

        /// <summary>
        /// 析构器
        /// </summary>
        ~EFRepositoryProvider()
        {
            this.Dispose();
        }

        #endregion

        #region # Public

        #region # 根据Id获取唯一实体对象（查看时用） —— T SingleOrDefault(Guid id)
        /// <summary>
        /// 根据Id获取唯一实体对象（查看时用），
        /// 无该对象时返回null
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>单个实体对象</returns>
        /// <exception cref="ArgumentNullException">id为空</exception>
        /// <exception cref="NotSupportedException">无法将表达式转换SQL语句</exception>
        /// <exception cref="InvalidOperationException">查询到1个以上的实体对象</exception>
        public T SingleOrDefault(Guid id)
        {
            #region # 验证参数

            if (id == Guid.Empty)
            {
                throw new ArgumentNullException("id", string.Format("{0}的id不可为空！", typeof(T).Name));
            }

            #endregion

            return this.SingleOrDefault(x => x.Id == id); ;
        }
        #endregion

        #region # 根据Id获取唯一子类对象（查看时用） —— TSub SingleOrDefault<TSub>(Guid id)
        /// <summary>
        /// 根据Id获取唯一子类对象（查看时用），
        /// 无该对象时返回null
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>唯一子类对象</returns>
        /// <exception cref="ArgumentNullException">id为空</exception>
        public TSub SingleOrDefault<TSub>(Guid id) where TSub : T
        {
            #region # 验证参数

            if (id == Guid.Empty)
            {
                throw new ArgumentNullException("id", string.Format("{0}的id不可为空！", typeof(TSub).Name));
            }

            #endregion

            return this.SingleOrDefault<TSub>(x => x.Id == id);
        }
        #endregion

        #region # 根据编号获取唯一实体对象（查看时用） —— T SingleOrDefault(string no)
        /// <summary>
        /// 根据编号获取唯一实体对象（查看时用），
        /// 无该对象时返回null
        /// </summary>
        /// <param name="no">编号</param>
        /// <returns>单个实体对象</returns>
        /// <exception cref="ArgumentNullException">编号为空</exception>
        public T SingleOrDefault(string no)
        {
            #region # 验证参数

            if (string.IsNullOrWhiteSpace(no))
            {
                throw new ArgumentNullException("no", string.Format("{0}的编号不可为空！", typeof(T).Name));
            }

            #endregion

            return this.SingleOrDefault(x => x.Number == no); ;
        }
        #endregion

        #region # 根据编号获取唯一子类对象（查看时用） —— TSub SingleOrDefault<TSub>(string no)
        /// <summary>
        /// 根据编号获取唯一子类对象（查看时用），
        /// 无该对象时返回null
        /// </summary>
        /// <param name="no">编号</param>
        /// <returns>唯一子类对象</returns>
        /// <exception cref="ArgumentNullException">编号为空</exception>
        public TSub SingleOrDefault<TSub>(string no) where TSub : T
        {
            #region # 验证参数

            if (string.IsNullOrWhiteSpace(no))
            {
                throw new ArgumentNullException("no", string.Format("{0}的编号不可为空！", typeof(TSub).Name));
            }

            #endregion

            return this.SingleOrDefault<TSub>(x => x.Number == no); ;
        }
        #endregion

        #region # 根据Id获取唯一实体对象（查看时用） —— T Single(Guid id)
        /// <summary>
        /// 根据Id获取唯一实体对象（查看时用），
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>单个实体对象</returns>
        /// <exception cref="ArgumentNullException">id为空</exception>
        /// <exception cref="NullReferenceException">无该对象</exception>
        public T Single(Guid id)
        {
            T current = this.SingleOrDefault(id);

            #region # 非空验证

            if (current == null)
            {
                throw new NullReferenceException(string.Format("Id为\"{0}\"的{1}实体不存在！", id, typeof(T).Name));
            }

            #endregion

            return current;
        }
        #endregion

        #region # 根据Id获取唯一子类对象（查看时用） —— TSub Single<TSub>(Guid id)
        /// <summary>
        /// 根据Id获取唯一子类对象（查看时用），
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>单个子类对象</returns>
        /// <exception cref="ArgumentNullException">id为空</exception>
        /// <exception cref="NullReferenceException">无该对象</exception>
        public TSub Single<TSub>(Guid id) where TSub : T
        {
            TSub current = this.SingleOrDefault<TSub>(id);

            #region # 非空验证

            if (current == null)
            {
                throw new NullReferenceException(string.Format("Id为\"{0}\"的{1}实体不存在！", id, typeof(TSub).Name));
            }

            #endregion

            return current;
        }
        #endregion

        #region # 根据编号获取唯一实体对象（查看时用） —— T Single(string no)
        /// <summary>
        /// 根据编号获取唯一实体对象（查看时用），
        /// </summary>
        /// <param name="no">编号</param>
        /// <returns>单个实体对象</returns>
        /// <exception cref="ArgumentNullException">编号为空</exception>
        /// <exception cref="NullReferenceException">无该对象</exception>
        public T Single(string no)
        {
            T current = this.SingleOrDefault(no);

            #region # 非空验证

            if (current == null)
            {
                throw new NullReferenceException(string.Format("编号为\"{0}\"的{1}实体不存在！", no, typeof(T).Name));
            }

            #endregion

            return current;
        }
        #endregion

        #region # 根据编号获取唯一子类对象（查看时用） —— TSub Single<TSub>(string no)
        /// <summary>
        /// 根据编号获取唯一子类对象（查看时用），
        /// </summary>
        /// <param name="no">编号</param>
        /// <returns>单个子类对象</returns>
        /// <exception cref="ArgumentNullException">编号为空</exception>
        /// <exception cref="NullReferenceException">无该对象</exception>
        public TSub Single<TSub>(string no) where TSub : T
        {
            TSub current = this.SingleOrDefault<TSub>(no);

            #region # 非空验证

            if (current == null)
            {
                throw new NullReferenceException(string.Format("编号为\"{0}\"的{1}实体不存在！", no, typeof(TSub).Name));
            }

            #endregion

            return current;
        }
        #endregion

        #region # 根据名称获取唯一实体对象（查看时用） —— T SingleByName(string name)
        /// <summary>
        /// 根据名称获取唯一实体对象（查看时用），
        /// 无该对象时返回null
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>单个实体对象</returns>
        /// <exception cref="ArgumentNullException">名称为空</exception>
        public T SingleByName(string name)
        {
            #region # 验证参数

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("name", string.Format("{0}的名称不可为空！", typeof(T).Name));
            }

            #endregion

            return this.SingleOrDefault(x => x.Name == name); ;
        }
        #endregion

        #region # 获取默认或第一个实体对象 —— T FirstOrDefault()
        /// <summary>
        /// 获取默认或第一个实体对象，
        /// 无该对象时返回null
        /// </summary>
        public virtual T FirstOrDefault()
        {
            return this.FindAllInner().FirstOrDefault();
        }
        #endregion

        #region # 获取默认或第一个子类对象 —— TSub FirstOrDefault<TSub>()
        /// <summary>
        /// 获取默认或第一个子类对象，
        /// 无该对象时返回null
        /// </summary>
        public virtual TSub FirstOrDefault<TSub>() where TSub : T
        {
            return this.FindAllInner<TSub>().FirstOrDefault();
        }
        #endregion

        #region # 获取实体对象集合 —— IEnumerable<T> FindAll()
        /// <summary>
        /// 获取实体对象集合
        /// </summary>
        /// <returns>实体对象集合</returns>
        public IEnumerable<T> FindAll()
        {
            return this.FindAllInner().AsEnumerable();
        }
        #endregion

        #region # 获取给定类型子类对象集合 —— IEnumerable<TSub> FindAll<TSub>()
        /// <summary>
        /// 获取给定类型子类对象集合
        /// </summary>
        /// <typeparam name="TSub">子类类型</typeparam>
        /// <returns>子类对象集合</returns>
        public IEnumerable<TSub> FindAll<TSub>() where TSub : T
        {
            return this.FindAllInner<TSub>().AsEnumerable();
        }
        #endregion

        #region # 根据关键字获取实体对象集合 —— IEnumerable<T> Find(string keywords)
        /// <summary>
        /// 根据关键字获取实体对象集合
        /// </summary>
        /// <returns>实体对象集合</returns>
        public IEnumerable<T> Find(string keywords)
        {
            Expression<Func<T, bool>> condition =
                x => string.IsNullOrEmpty(keywords) || x.Keywords.Contains(keywords);
            return this.Find(condition).AsEnumerable();
        }
        #endregion

        #region # 根据Id集获取实体对象集合 —— IEnumerable<T> Find(IEnumerable<Guid> ids)
        /// <summary>
        /// 根据Id集获取实体对象集合
        /// </summary>
        /// <returns>实体对象集合</returns>
        public IEnumerable<T> Find(IEnumerable<Guid> ids)
        {
            return this.Find(x => ids.Contains(x.Id)).AsEnumerable();
        }
        #endregion

        #region # 根据编号集获取实体对象集合 —— IEnumerable<T> Find(IEnumerable<string> nos)

        /// <summary>
        /// 根据编号集获取实体对象集合
        /// </summary>
        /// <returns>实体对象集合</returns>
        public IEnumerable<T> Find(IEnumerable<string> nos)
        {
            return this.Find(x => nos.Contains(x.Number)).AsEnumerable();
        }
        #endregion

        #region # 根据关键字获取给定类型子类对象集合 —— IEnumerable<TSub> Find<TSub>(string keywords)
        /// <summary>
        /// 根据关键字获取给定类型子类对象集合
        /// </summary>
        /// <typeparam name="TSub">子类类型</typeparam>
        /// <returns>子类对象集合</returns>
        public IEnumerable<TSub> Find<TSub>(string keywords) where TSub : T
        {
            Expression<Func<TSub, bool>> condition =
                x => string.IsNullOrEmpty(keywords) || x.Keywords.Contains(keywords);
            return this.Find<TSub>(condition).AsEnumerable();
        }
        #endregion

        #region # 根据关键字分页获取实体对象集合 + 输出记录条数与页数
        /// <summary>
        /// 根据关键字获取实体对象集合 + 分页 + 输出记录条数与页数
        /// </summary>
        /// <param name="keywords">关键字</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页容量</param>
        /// <param name="rowCount">记录条数</param>
        /// <param name="pageCount">页数</param>
        /// <returns>实体对象集合</returns>
        /// <exception cref="ArgumentNullException">条件表达式为空</exception>
        /// <exception cref="NotSupportedException">无法将表达式转换SQL语句</exception>
        public IEnumerable<T> FindByPage(string keywords, int pageIndex, int pageSize, out int rowCount, out int pageCount)
        {
            Expression<Func<T, bool>> condition =
                x => string.IsNullOrEmpty(keywords) || x.Keywords.Contains(keywords);
            return this.FindByPage(condition, pageIndex, pageSize, out rowCount, out pageCount).AsEnumerable();
        }
        #endregion

        #region # 根据关键字分页获取子类对象集合 + 输出记录条数与页数
        /// <summary>
        /// 根据关键字分页获取子类对象集合 + 分页 + 输出记录条数与页数
        /// </summary>
        /// <typeparam name="TSub">子类类型</typeparam>
        /// <param name="keywords">关键字</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页容量</param>
        /// <param name="rowCount">记录条数</param>
        /// <param name="pageCount">页数</param>
        /// <returns>实体对象集合</returns>
        /// <exception cref="ArgumentNullException">条件表达式为空</exception>
        /// <exception cref="NotSupportedException">无法将表达式转换SQL语句</exception>
        public IEnumerable<TSub> FindByPage<TSub>(string keywords, int pageIndex, int pageSize, out int rowCount,
            out int pageCount) where TSub : T
        {
            Expression<Func<TSub, bool>> condition =
                x => string.IsNullOrEmpty(keywords) || x.Keywords.Contains(keywords);
            return this.FindByPage<TSub>(condition, pageIndex, pageSize, out rowCount, out pageCount).AsEnumerable();
        }
        #endregion

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

        #region # 判断是否存在给定Id的实体对象 —— bool Exists(Guid id)
        /// <summary>
        /// 判断是否存在给定Id的实体对象
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>是否存在</returns>
        /// <exception cref="ArgumentNullException">id为空</exception>
        public bool Exists(Guid id)
        {
            #region # 验证参数

            if (id == Guid.Empty)
            {
                throw new ArgumentNullException("id", string.Format("{0}的id不可为空！", typeof(T).Name));
            }

            #endregion

            return this.Exists(x => x.Id == id);
        }
        #endregion

        #region # 判断是否存在给定Id的子类对象 —— bool Exists<TSub>(Guid id)
        /// <summary>
        /// 判断是否存在给定Id的子类对象
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>是否存在</returns>
        /// <exception cref="ArgumentNullException">id为空</exception>
        public bool Exists<TSub>(Guid id) where TSub : T
        {
            #region # 验证参数

            if (id == Guid.Empty)
            {
                throw new ArgumentNullException("id", string.Format("{0}的id不可为空！", typeof(T).Name));
            }

            #endregion

            return this.Exists<TSub>(x => x.Id == id);
        }
        #endregion

        #region # 判断是否存在给定编号的实体对象 —— bool Exists(string no)
        /// <summary>
        /// 判断是否存在给定编号的实体对象
        /// </summary>
        /// <param name="no">编号</param>
        /// <returns>是否存在</returns>
        /// <exception cref="ArgumentNullException">编号为空</exception>
        public bool Exists(string no)
        {
            #region # 验证参数

            if (string.IsNullOrWhiteSpace(no))
            {
                throw new ArgumentNullException("no", string.Format("{0}的编号不可为空！", typeof(T).Name));
            }

            #endregion

            return this.Exists(x => x.Number == no);
        }
        #endregion

        #region # 判断是否存在给定编号的子类对象 —— bool Exists<TSub>(string no)
        /// <summary>
        /// 判断是否存在给定编号的子类对象
        /// </summary>
        /// <param name="no">编号</param>
        /// <returns>是否存在</returns>
        /// <exception cref="ArgumentNullException">编号为空</exception>
        public bool Exists<TSub>(string no) where TSub : T
        {
            #region # 验证参数

            if (string.IsNullOrWhiteSpace(no))
            {
                throw new ArgumentNullException("no", string.Format("{0}的编号不可为空！", typeof(T).Name));
            }

            #endregion

            return this.Exists<TSub>(x => x.Number == no);
        }
        #endregion

        #region # 判断是否存在给定名称的实体对象 —— bool ExistsName(string name)
        /// <summary>
        /// 判断是否存在给定名称的实体对象
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>是否已存在</returns>
        public bool ExistsName(string name)
        {
            #region # 验证参数

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("name", string.Format("{0}的名称不可为空！", typeof(T).Name));
            }

            #endregion

            return this.Exists(x => x.Name == name);
        }
        #endregion

        #region # 判断是否存在给定名称的子类对象 —— bool ExistsName<TSub>(string name)
        /// <summary>
        /// 判断是否存在给定名称的子类对象
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>是否已存在</returns>
        public bool ExistsName<TSub>(string name) where TSub : T
        {
            #region # 验证参数

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("name", string.Format("{0}的名称不可为空！", typeof(T).Name));
            }

            #endregion

            return this.Exists<TSub>(x => x.Name == name);
        }
        #endregion

        #region # 执行SQL查询 —— IEnumerable<T> ExecuteSqlQuery(string sql, params object[] parameters)
        /// <summary>
        /// 执行SQL查询
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>实体对象集合</returns>
        /// <exception cref="ArgumentNullException">SQL语句为空</exception>
        public IEnumerable<T> ExecuteSqlQuery(string sql, params object[] parameters)
        {
            #region # 验证参数

            if (string.IsNullOrWhiteSpace(sql))
            {
                throw new ArgumentNullException("sql", @"SQL语句不可为空！");
            }

            #endregion

            return this._dbContext.Database.SqlQuery<T>(sql, parameters).AsEnumerable();
        }
        #endregion

        #region # 释放资源 —— void Dispose()
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (this._dbContext != null)
            {
                this._dbContext.Dispose();
            }
        }
        #endregion

        #endregion

        #region # Protected

        #region # 获取实体对象集合 —— virtual IQueryable<T> FindAllInner()
        /// <summary>
        /// 获取实体对象集合
        /// </summary>
        /// <returns>实体对象集合</returns>
        protected virtual IQueryable<T> FindAllInner()
        {
            return this._dbContext.Set<T>().Where(x => !x.Deleted).OrderByDescending(x => x.Sort).ThenByDescending(x => x.AddedTime);
        }
        #endregion

        #region # 获取给定类型子类对象集合 —— IQueryable<TSub> FindAllInner<TSub>()
        /// <summary>
        /// 获取给定类型子类对象集合
        /// </summary>
        /// <typeparam name="TSub">子类类型</typeparam>
        /// <returns>子类对象集合</returns>
        protected IQueryable<TSub> FindAllInner<TSub>() where TSub : T
        {
            return this.FindAllInner().OfType<TSub>();
        }
        #endregion

        #region # 根据条件获取唯一实体对象（查看时用） —— T SingleOrDefault(Expression<Func<T, bool>> predicate)
        /// <summary>
        /// 根据条件获取唯一实体对象（查看时用），
        /// 无该对象时返回null
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <returns>唯一实体对象</returns>
        /// <exception cref="ArgumentNullException">条件表达式为空</exception>
        /// <exception cref="NullReferenceException">查询不到任何实体对象</exception>
        /// <exception cref="NotSupportedException">无法将表达式转换SQL语句</exception>
        /// <exception cref="InvalidOperationException">查询到1个以上的实体对象</exception>
        protected T SingleOrDefault(Expression<Func<T, bool>> predicate)
        {
            #region # 验证参数

            if (predicate == null)
            {
                throw new ArgumentNullException("predicate", @"条件表达式不可为空！");
            }

            if (this.Count(predicate) > 1)
            {
                throw new InvalidOperationException(string.Format("给定的条件\"{0}\"中查询到1个以上的{1}实体对象！", predicate, typeof(T).Name));
            }

            #endregion

            return this.FindAllInner().SingleOrDefault(predicate);
        }
        #endregion

        #region # 根据条件获取唯一子类对象（查看时用） —— TSub SingleOrDefault<TSub>(Expression<Func<TSub, bool>> predicate)
        /// <summary>
        /// 根据条件获取唯一子类对象（查看时用），
        /// 无该对象时返回null
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <returns>唯一子类对象</returns>
        /// <exception cref="ArgumentNullException">条件表达式为空</exception>
        /// <exception cref="NullReferenceException">查询不到任何子类对象</exception>
        /// <exception cref="NotSupportedException">无法将表达式转换SQL语句</exception>
        /// <exception cref="InvalidOperationException">查询到1个以上的子类对象</exception>
        protected TSub SingleOrDefault<TSub>(Expression<Func<TSub, bool>> predicate) where TSub : T
        {
            #region # 验证参数

            if (predicate == null)
            {
                throw new ArgumentNullException("predicate", @"条件表达式不可为空！");
            }

            if (this.Count(predicate) > 1)
            {
                throw new InvalidOperationException(string.Format("给定的条件\"{0}\"中查询到1个以上的{1}实体对象！", predicate, typeof(T).Name));
            }

            #endregion

            return this.FindAllInner<TSub>().SingleOrDefault(predicate);
        }
        #endregion

        #region # 根据条件获取第一个实体对象（查看时用） —— T FirstOrDefault(Expression<Func<T, bool>> predicate)
        /// <summary>
        /// 根据条件获取第一个实体对象（查看时用），
        /// 无该对象时返回null
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <returns>实体对象</returns>
        protected T FirstOrDefault(Expression<Func<T, bool>> predicate)
        {
            #region # 验证参数

            if (predicate == null)
            {
                throw new ArgumentNullException("predicate", @"条件表达式不可为空！");
            }

            #endregion

            return this.Find(predicate).FirstOrDefault();
        }
        #endregion

        #region # 根据条件获取第一个子类对象（查看时用） —— TSub FirstOrDefault<TSub>(Expression<Func<TSub...
        /// <summary>
        /// 根据条件获取第一个子类对象（查看时用），
        /// 无该对象时返回null
        /// </summary>
        /// <typeparam name="TSub">子类类型</typeparam>
        /// <param name="predicate">条件</param>
        /// <returns>子类对象</returns>
        protected TSub FirstOrDefault<TSub>(Expression<Func<TSub, bool>> predicate) where TSub : T
        {
            #region # 验证参数

            if (predicate == null)
            {
                throw new ArgumentNullException("predicate", @"条件表达式不可为空！");
            }

            #endregion

            return this.Find<TSub>(predicate).FirstOrDefault();
        }
        #endregion

        #region # 根据条件获取实体对象集合 —— IQueryable<T> Find(Expression<Func<T, bool>> predicate)
        /// <summary>
        /// 根据条件获取实体对象集合
        /// </summary>
        /// <param name="predicate">条件表达式</param>
        /// <returns>实体对象集合</returns>
        /// <exception cref="ArgumentNullException">条件表达式为空</exception>
        /// <exception cref="NotSupportedException">无法将表达式转换SQL语句</exception>
        protected IQueryable<T> Find(Expression<Func<T, bool>> predicate)
        {
            #region # 验证参数

            if (predicate == null)
            {
                throw new ArgumentNullException("predicate", @"条件表达式不可为空！");
            }

            #endregion

            return this.FindAllInner().Where(predicate);
        }
        #endregion

        #region # 根据条件获取子类对象集合 —— IQueryable<TSub> Find<TSub>(Expression<Func<TSub, bool>>...
        /// <summary>
        /// 根据条件获取子类对象集合
        /// </summary>
        /// <typeparam name="TSub">子类类型</typeparam>
        /// <param name="predicate">条件表达式</param>
        /// <returns>子类对象集合</returns>
        /// <exception cref="ArgumentNullException">条件表达式为空</exception>
        /// <exception cref="NotSupportedException">无法将表达式转换SQL语句</exception>
        protected IQueryable<TSub> Find<TSub>(Expression<Func<TSub, bool>> predicate) where TSub : T
        {
            #region # 验证参数

            if (predicate == null)
            {
                throw new ArgumentNullException("predicate", @"条件表达式不可为空！");
            }

            #endregion

            return this.FindAllInner<TSub>().Where(predicate);
        }
        #endregion

        #region # 根据条件获取实体对象Id集合 —— IQueryable<Guid> FindIds(Expression<Func<T, bool>> predicate)
        /// <summary>
        /// 根据条件获取实体对象Id集合
        /// </summary>
        /// <param name="predicate">条件表达式</param>
        /// <returns>实体对象Id集合</returns>
        /// <exception cref="ArgumentNullException">条件表达式为空</exception>
        /// <exception cref="NotSupportedException">无法将表达式转换SQL语句</exception>
        protected IQueryable<Guid> FindIds(Expression<Func<T, bool>> predicate)
        {
            return this.Find(predicate).Select(x => x.Id);
        }
        #endregion

        #region # 根据条件获取子类对象Id集合 —— IQueryable<Guid> FindIds<TSub>(Expression<Func<TSub, bool>> predicate)
        /// <summary>
        /// 根据条件获取子类对象Id集合
        /// </summary>
        /// <param name="predicate">条件表达式</param>
        /// <returns>子类对象Id集合</returns>
        /// <exception cref="ArgumentNullException">条件表达式为空</exception>
        /// <exception cref="NotSupportedException">无法将表达式转换SQL语句</exception>
        protected IQueryable<Guid> FindIds<TSub>(Expression<Func<TSub, bool>> predicate) where TSub : T
        {
            return this.Find<TSub>(predicate).Select(x => x.Id);
        }
        #endregion

        #region # 根据条件获取实体对象编号集合 —— IQueryable<string> FindNos(Expression<Func<T, bool>> predicate)
        /// <summary>
        /// 根据条件获取实体对象编号集合
        /// </summary>
        /// <param name="predicate">条件表达式</param>
        /// <returns>实体对象编号集合</returns>
        /// <exception cref="ArgumentNullException">条件表达式为空</exception>
        /// <exception cref="NotSupportedException">无法将表达式转换SQL语句</exception>
        protected IQueryable<string> FindNos(Expression<Func<T, bool>> predicate)
        {
            return this.Find(predicate).Select(x => x.Number);
        }
        #endregion

        #region # 根据条件获取子类对象编号集合 —— IQueryable<string> FindNos<TSub>(Expression<Func<TSub, bool>> predicate)
        /// <summary>
        /// 根据条件获取子类对象编号集合
        /// </summary>
        /// <param name="predicate">条件表达式</param>
        /// <returns>子类对象编号集合</returns>
        /// <exception cref="ArgumentNullException">条件表达式为空</exception>
        /// <exception cref="NotSupportedException">无法将表达式转换SQL语句</exception>
        protected IQueryable<string> FindNos<TSub>(Expression<Func<TSub, bool>> predicate) where TSub : T
        {
            return this.Find<TSub>(predicate).Select(x => x.Number);
        }
        #endregion

        #region # 根据条件分页获取实体对象集合 + 输出记录条数与页数
        /// <summary>
        /// 根据条件获取实体对象集合 + 分页 + 输出记录条数与页数
        /// </summary>
        /// <param name="predicate">条件表达式</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页容量</param>
        /// <param name="rowCount">记录条数</param>
        /// <param name="pageCount">页数</param>
        /// <returns>实体对象集合</returns>
        /// <exception cref="ArgumentNullException">条件表达式为空</exception>
        /// <exception cref="NotSupportedException">无法将表达式转换SQL语句</exception>
        protected IQueryable<T> FindByPage(Expression<Func<T, bool>> predicate, int pageIndex, int pageSize, out int rowCount, out int pageCount)
        {
            return this.FindAllInner().ToPage(predicate, pageIndex, pageSize, out rowCount, out pageCount);
        }
        #endregion

        #region # 根据条件分页获取子类对象集合 + 输出记录条数与页数
        /// <summary>
        /// 根据条件分页获取子类对象集合 + 分页 + 输出记录条数与页数
        /// </summary>
        /// <typeparam name="TSub">子类类型</typeparam>
        /// <param name="predicate">条件表达式</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页容量</param>
        /// <param name="rowCount">记录条数</param>
        /// <param name="pageCount">页数</param>
        /// <returns>实体对象集合</returns>
        /// <exception cref="ArgumentNullException">条件表达式为空</exception>
        /// <exception cref="NotSupportedException">无法将表达式转换SQL语句</exception>
        protected IQueryable<TSub> FindByPage<TSub>(Expression<Func<TSub, bool>> predicate, int pageIndex, int pageSize, out int rowCount, out int pageCount) where TSub : T
        {
            return this.FindAllInner<TSub>().ToPage(predicate, pageIndex, pageSize, out rowCount, out pageCount);
        }
        #endregion

        #region # 根据条件获取记录条数 —— int Count(Expression<Func<T, bool>> predicate)
        /// <summary>
        /// 根据条件获取记录条数
        /// </summary>
        /// <param name="predicate">条件表达式</param>
        /// <returns>符合条件的记录条数</returns>
        /// <exception cref="ArgumentNullException">条件表达式为空</exception>
        /// <exception cref="NotSupportedException">无法将表达式转换SQL语句</exception>
        protected int Count(Expression<Func<T, bool>> predicate)
        {
            #region # 验证参数

            if (predicate == null)
            {
                throw new ArgumentNullException("predicate", @"条件表达式不可为空！");
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
        /// <exception cref="ArgumentNullException">条件表达式为空</exception>
        /// <exception cref="NotSupportedException">无法将表达式转换SQL语句</exception>
        protected int Count<TSub>(Expression<Func<TSub, bool>> predicate) where TSub : T
        {
            #region # 验证参数

            if (predicate == null)
            {
                throw new ArgumentNullException("predicate", @"条件表达式不可为空！");
            }

            #endregion

            return this.FindAllInner<TSub>().Count(predicate);
        }
        #endregion

        #region # 判断是否存在给定条件的实体对象 —— bool Exists(Expression<Func<T, bool>> predicate)
        /// <summary>
        /// 判断是否存在给定条件的实体对象
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <returns>是否存在</returns>
        /// <exception cref="ArgumentNullException">条件表达式为空</exception>
        /// <exception cref="NotSupportedException">无法将表达式转换SQL语句</exception>
        protected bool Exists(Expression<Func<T, bool>> predicate)
        {
            #region # 验证参数

            if (predicate == null)
            {
                throw new ArgumentNullException("predicate", @"条件表达式不可为空！");
            }

            #endregion

            return this.FindAllInner().Any(predicate);
        }
        #endregion

        #region # 判断是否存在给定条件的子类对象 —— bool Exists<TSub>(Expression<Func<TSub, bool>> predicate)
        /// <summary>
        /// 判断是否存在给定条件的子类对象
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <returns>是否存在</returns>
        /// <exception cref="ArgumentNullException">条件表达式为空</exception>
        /// <exception cref="NotSupportedException">无法将表达式转换SQL语句</exception>
        protected bool Exists<TSub>(Expression<Func<TSub, bool>> predicate) where TSub : T
        {
            #region # 验证参数

            if (predicate == null)
            {
                throw new ArgumentNullException("predicate", @"条件表达式不可为空！");
            }

            #endregion

            return this.FindAllInner<TSub>().Any(predicate);
        }
        #endregion

        #endregion
    }
}
