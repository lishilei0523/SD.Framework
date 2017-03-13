using SD.Infrastructure.EntityBase;
using SD.Infrastructure.Repository.EntityFramework.Base;
using SD.Infrastructure.RepositoryBase;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SD.Infrastructure.Repository.EntityFramework
{
    /// <summary>
    /// EF单元事务Provider
    /// </summary>
    public abstract class EFUnitOfWorkProvider : IUnitOfWork
    {
        #region # 创建EF（写）上下文对象

        /// <summary>
        /// 同步锁
        /// </summary>
        private static readonly object _Sync;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static EFUnitOfWorkProvider()
        {
            _Sync = new object();
        }

        /// <summary>
        /// EF（写）上下文对象字段
        /// </summary>
        private readonly DbContext _dbContext;

        /// <summary>
        /// 构造器
        /// </summary>
        protected EFUnitOfWorkProvider()
        {
            //EF（写）上下文对象字段
            this._dbContext = BaseDbSession.CommandInstance;
        }

        /// <summary>
        /// 析构器
        /// </summary>
        ~EFUnitOfWorkProvider()
        {
            this.Dispose();
        }

        #endregion

        //Public

        #region # 注册添加单个实体对象 —— void RegisterAdd<T>(T entity)
        /// <summary>
        /// 注册添加单个实体对象
        /// </summary>
        /// <typeparam name="T">聚合根类型</typeparam>
        /// <param name="entity">新实体对象</param>
        /// <exception cref="ArgumentNullException">新实体对象为空</exception>
        public void RegisterAdd<T>(T entity) where T : AggregateRootEntity
        {
            #region # 验证参数

            if (entity == null)
            {
                throw new ArgumentNullException("entity", string.Format(@"要添加的{0}实体对象不可为空！", typeof(T).Name));
            }

            if (entity.Id == Guid.Empty)
            {
                throw new ArgumentNullException("Id", string.Format(@"要添加的{0}实体对象Id不可为空！", typeof(T).Name));
            }

            #endregion

            this._dbContext.Set<T>().Add(entity);
        }
        #endregion

        #region # 注册添加实体对象集合 —— void RegisterAddRange<T>(IEnumerable<T> entities)
        /// <summary>
        /// 注册添加实体对象集合
        /// </summary>
        /// <typeparam name="T">聚合根类型</typeparam>
        /// <param name="entities">实体对象集合</param>
        /// <exception cref="ArgumentNullException">实体对象集合为null或长度为0</exception>
        public void RegisterAddRange<T>(IEnumerable<T> entities) where T : AggregateRootEntity
        {
            #region # 验证参数

            if (entities == null || !entities.Any())
            {
                throw new ArgumentNullException("entities", string.Format("要添加的{0}实体对象集合不可为空！", typeof(T).Name));
            }

            #endregion

            this._dbContext.Set<T>().AddRange(entities);
        }
        #endregion

        #region # 注册保存单个实体对象 —— void RegisterSave<T>(T entity)
        /// <summary>
        /// 注册保存单个实体对象
        /// </summary>
        /// <typeparam name="T">聚合根类型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <exception cref="ArgumentNullException">实体对象为空</exception>
        /// <exception cref="NullReferenceException">要保存的对象不存在</exception>
        public void RegisterSave<T>(T entity) where T : AggregateRootEntity
        {
            #region # 验证参数

            if (entity == null)
            {
                throw new ArgumentNullException("entity", string.Format("要保存的{0}实体对象不可为空！", typeof(T).Name));
            }

            if (entity.Id == Guid.Empty)
            {
                throw new ArgumentNullException("Id", string.Format(@"要保存的{0}实体对象Id不可为空！", typeof(T).Name));
            }

            if (this._dbContext.Set<T>().Where(x => !x.Deleted).All(x => x.Id != entity.Id))
            {
                throw new NullReferenceException(string.Format("不存在Id为{0}的{1}实体对象，请尝试添加操作！", entity.Id, typeof(T).Name));
            }

            #endregion

            entity.SavedTime = DateTime.Now;
            DbEntityEntry entry = this._dbContext.Entry<T>(entity);
            entry.State = EntityState.Modified;
        }
        #endregion

        #region # 注册保存实体对象集合 —— void RegisterSaveRange<T>(IEnumerable<T> entities)
        /// <summary>
        /// 注册保存实体对象集合
        /// </summary>
        /// <typeparam name="T">聚合根类型</typeparam>
        /// <param name="entities">实体对象集合</param>
        /// <exception cref="ArgumentNullException">实体对象集合</exception>
        /// <exception cref="NullReferenceException">要保存的对象不存在</exception>
        public void RegisterSaveRange<T>(IEnumerable<T> entities) where T : AggregateRootEntity
        {
            #region # 验证参数

            if (entities == null || !entities.Any())
            {
                throw new ArgumentNullException("entities", string.Format("要保存的{0}实体对象集合不可为空！", typeof(T).Name));
            }

            #endregion

            foreach (T entity in entities)
            {
                this.RegisterSave(entity);
            }
        }
        #endregion

        #region # 注册删除单行（物理删除） —— void RegisterPhysicsRemove<T>(Guid id)
        /// <summary>
        /// 注册删除单行（物理删除）
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="id">标识Id</param>
        /// <exception cref="ArgumentNullException">id为空</exception>
        /// <exception cref="NullReferenceException">要删除的对象不存在</exception>
        public void RegisterPhysicsRemove<T>(Guid id) where T : AggregateRootEntity
        {
            #region # 验证参数

            if (id == Guid.Empty)
            {
                throw new ArgumentNullException("id", string.Format("要删除的{0}实体对象id不可为空！", typeof(T).Name));
            }

            if (!this._dbContext.Set<T>().Where(x => !x.Deleted).Any(x => x.Id == id))
            {
                throw new NullReferenceException(string.Format("Id为{0}的{1}实体对象不存在，请重试！", id, typeof(T).Name));
            }

            #endregion

            T entity = this._dbContext.Set<T>().Single(x => x.Id == id);
            this._dbContext.Set<T>().Remove(entity);
        }
        #endregion

        #region # 注册删除单行（物理删除） —— void RegisterPhysicsRemove<T>(string number)
        /// <summary>
        /// 注册删除单行（物理删除）
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="number">编号</param>
        /// <exception cref="ArgumentNullException">编号为空</exception>
        /// <exception cref="NullReferenceException">要删除的对象不存在</exception>
        public void RegisterPhysicsRemove<T>(string number) where T : AggregateRootEntity
        {
            #region # 验证参数

            if (string.IsNullOrWhiteSpace(number))
            {
                throw new ArgumentNullException("number", string.Format("要删除的{0}实体对象编号不可为空！", typeof(T).Name));
            }

            if (this._dbContext.Set<T>().Where(x => !x.Deleted).All(x => x.Number != number))
            {
                throw new NullReferenceException(string.Format("编号为{0}的{1}实体对象不存在，请重试！", number, typeof(T).Name));
            }

            #endregion

            T entity = this._dbContext.Set<T>().Single(x => x.Number == number);
            this._dbContext.Set<T>().Remove(entity);
        }
        #endregion

        #region # 注册删除多行（物理删除） —— void RegisterPhysicsRemoveRange<T>(IEnumerable<Guid> ids)
        /// <summary>
        /// 注册删除多行（物理删除）
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="ids">标识Id集合</param>
        /// <exception cref="ArgumentNullException">ids为null或长度为0</exception>
        /// <exception cref="NullReferenceException">要删除的对象不存在</exception>
        public void RegisterPhysicsRemoveRange<T>(IEnumerable<Guid> ids) where T : AggregateRootEntity
        {
            #region # 验证参数

            if (ids == null || !ids.Any())
            {
                throw new ArgumentNullException("ids", string.Format("要删除的{0}的id集合不可为空！", typeof(T).Name));
            }

            #endregion

            foreach (Guid id in ids)
            {
                this.RegisterPhysicsRemove<T>(id);
            }
        }
        #endregion

        #region # 注册删除全部（物理删除） —— void RegisterPhysicsRemoveAll<T>()
        /// <summary>
        /// 注册删除全部（物理删除）
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <exception cref="NullReferenceException">要删除的对象不存在</exception>
        public void RegisterPhysicsRemoveAll<T>() where T : AggregateRootEntity
        {
            this.RegisterPhysicsRemove<T>(x => true);
        }
        #endregion

        #region # 注册删除单行（逻辑删除） —— void RegisterRemove<T>(Guid id)
        /// <summary>
        /// 注册删除单行（逻辑删除）
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="id">标识Id</param>
        /// <exception cref="ArgumentNullException">id为空</exception>
        /// <exception cref="NullReferenceException">要删除的对象不存在</exception>
        public void RegisterRemove<T>(Guid id) where T : AggregateRootEntity
        {
            #region # 验证参数

            if (id == Guid.Empty)
            {
                throw new ArgumentNullException("id", string.Format("要删除的{0}实体对象id不可为空！", typeof(T).Name));
            }

            if (!this._dbContext.Set<T>().Where(x => !x.Deleted).Any(x => x.Id == id))
            {
                throw new NullReferenceException(string.Format("Id为{0}的{1}实体对象不存在，请重试！", id, typeof(T).Name));
            }

            #endregion

            T entity = this.Single<T>(x => x.Id == id);
            entity.Deleted = true;
            entity.DeletedTime = DateTime.Now;
            DbEntityEntry entry = this._dbContext.Entry<T>(entity);
            entry.State = EntityState.Modified;
        }
        #endregion

        #region # 注册删除单行（逻辑删除） —— void RegisterRemove<T>(string number)
        /// <summary>
        /// 注册删除单行（逻辑删除）
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="number">编号</param>
        /// <exception cref="ArgumentNullException">编号为空</exception>
        /// <exception cref="NullReferenceException">要删除的对象不存在</exception>
        public void RegisterRemove<T>(string number) where T : AggregateRootEntity
        {
            #region # 验证参数

            if (string.IsNullOrWhiteSpace(number))
            {
                throw new ArgumentNullException("number", string.Format(@"要删除的{0}实体对象编号不可为空！", typeof(T).Name));
            }

            if (this._dbContext.Set<T>().Where(x => !x.Deleted).All(x => x.Number != number))
            {
                throw new NullReferenceException(string.Format("编号为{0}的{1}实体对象不存在，请重试！", number, typeof(T).Name));
            }

            #endregion

            T entity = this.Single<T>(x => x.Number == number);
            entity.Deleted = true;
            entity.DeletedTime = DateTime.Now;
            DbEntityEntry entry = this._dbContext.Entry<T>(entity);
            entry.State = EntityState.Modified;
        }
        #endregion

        #region # 注册删除多行（逻辑删除） —— void RegisterRemoveRange<T>(IEnumerable<Guid> ids)
        /// <summary>
        /// 注册删除多行（逻辑删除）
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="ids">标识Id集合</param>
        /// <exception cref="ArgumentNullException">ids为null或长度为0</exception>
        public void RegisterRemoveRange<T>(IEnumerable<Guid> ids) where T : AggregateRootEntity
        {
            #region # 验证参数

            if (ids == null || !ids.Any())
            {
                throw new ArgumentNullException("ids", string.Format("要删除的{0}的id集合不可为空！", typeof(T).Name));
            }

            #endregion

            foreach (Guid id in ids)
            {
                this.RegisterRemove<T>(id);
            }
        }
        #endregion

        #region # 注册删除全部（逻辑删除） —— void RegisterRemoveAll<T>()
        /// <summary>
        /// 注册删除全部（逻辑删除）
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        public void RegisterRemoveAll<T>() where T : AggregateRootEntity
        {
            this.RegisterRemove<T>(x => true);
        }
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
        public T Resolve<T>(Guid id) where T : AggregateRootEntity
        {
            #region # 验证参数

            if (id == Guid.Empty)
            {
                throw new ArgumentNullException("id", string.Format("{0}的id不可为空！", typeof(T).Name));
            }

            #endregion

            return this.Resolve<T>(x => x.Id == id);
        }
        #endregion

        #region # 根据编号获取唯一实体对象（修改时用） —— T Resolve<T>(string number)
        /// <summary>
        /// 根据编号获取唯一实体对象（修改时用）
        /// </summary>
        /// <typeparam name="T">聚合根类型</typeparam>
        /// <param name="number">编号</param>
        /// <returns>单个实体对象</returns>
        /// <exception cref="ArgumentNullException">编号为空</exception>
        public T Resolve<T>(string number) where T : AggregateRootEntity
        {
            #region # 验证参数

            if (string.IsNullOrWhiteSpace(number))
            {
                throw new ArgumentNullException("number", string.Format("{0}的编号不可为空！", typeof(T).Name));
            }

            #endregion

            return this.Resolve<T>(x => x.Number == number);
        }
        #endregion

        #region # 获取全部实体对象（修改时用） —— IEnumerable<T> ResolveAll<T>()
        /// <summary>
        /// 获取全部实体对象（修改时用）
        /// </summary>
        /// <typeparam name="T">聚合根类型</typeparam>
        /// <returns>实体对象集合</returns>
        public IEnumerable<T> ResolveAll<T>() where T : AggregateRootEntity
        {
            return this._dbContext.Set<T>().Where(x => !x.Deleted).AsEnumerable();
        }
        #endregion

        #region # 统一事务处理保存更改 —— void Commit()
        /// <summary>
        /// 统一事务处理保存更改
        /// </summary>
        public void Commit()
        {
            try
            {
                //提交事务
                this._dbContext.SaveChanges();
            }
            catch
            {
                this.RollBack();
                throw;
            }
        }
        #endregion

        #region # 统一事务处理保存更改（异步） —— async Task CommitAsync()
        /// <summary>
        /// 统一事务处理保存更改（异步）
        /// </summary>
        public async Task CommitAsync()
        {
            try
            {
                //提交事务
                await this._dbContext.SaveChangesAsync();
            }
            catch
            {
                this.RollBack();
                throw;
            }
        }
        #endregion

        #region # 统一事务回滚取消更改 —— void RollBack()
        /// <summary>
        /// 统一事务回滚取消更改
        /// </summary>
        public void RollBack()
        {
            foreach (DbEntityEntry entry in this._dbContext.ChangeTracker.Entries())
            {
                entry.State = EntityState.Unchanged;
            }
        }
        #endregion

        #region # 执行SQL命令（无需Commit） —— void ExecuteSqlCommand(string sql...
        /// <summary>
        /// 执行SQL命令（无需Commit）
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">参数</param>
        /// <exception cref="ArgumentNullException">SQL语句为空</exception>
        public void ExecuteSqlCommand(string sql, params object[] parameters)
        {
            #region # 验证参数

            if (string.IsNullOrWhiteSpace(sql))
            {
                throw new ArgumentNullException("sql", @"SQL语句不可为空！");
            }

            #endregion

            this._dbContext.Database.ExecuteSqlCommand(sql, parameters);
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


        //Protected

        #region # 注册条件删除（物理删除） —— void RegisterPhysicsRemove<T>(...
        /// <summary>
        /// 注册条件删除（物理删除）
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="predicate">条件表达式</param>
        /// <exception cref="ArgumentNullException">条件表达式为空</exception>
        /// <exception cref="NullReferenceException">要删除的对象不存在</exception>
        protected void RegisterPhysicsRemove<T>(Expression<Func<T, bool>> predicate) where T : AggregateRootEntity
        {
            #region # 验证参数

            if (predicate == null)
            {
                throw new ArgumentNullException("predicate", @"条件表达式不可为空！");
            }

            #endregion

            IQueryable<T> list = this._dbContext.Set<T>().Where(x => !x.Deleted).Where(predicate);
            foreach (T entity in list)
            {
                this._dbContext.Set<T>().Attach(entity);
                this._dbContext.Set<T>().Remove(entity);
            }
        }
        #endregion

        #region # 注册条件删除（逻辑删除） —— void RegisterRemove<T>(...
        /// <summary>
        /// 注册条件删除（逻辑删除）
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="predicate">条件表达式</param>
        /// <exception cref="ArgumentNullException">条件表达式为空</exception>
        protected void RegisterRemove<T>(Expression<Func<T, bool>> predicate) where T : AggregateRootEntity
        {
            #region # 验证参数

            if (predicate == null)
            {
                throw new ArgumentNullException("predicate", @"条件表达式不可为空！");
            }

            #endregion

            IQueryable<T> list = this._dbContext.Set<T>().Where(x => !x.Deleted).Where(predicate);
            foreach (T entity in list)
            {
                entity.Deleted = true;
                entity.DeletedTime = DateTime.Now;
                DbEntityEntry entry = this._dbContext.Entry<T>(entity);
                entry.State = EntityState.Modified;
            }
        }
        #endregion

        #region # 根据条件获取唯一实体对象（修改时用） —— T Resolve<T>(...
        /// <summary>
        /// 根据条件获取唯一实体对象（修改时用）
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <returns>实体对象</returns>
        /// <exception cref="ArgumentNullException">条件表达式为空</exception>
        /// <exception cref="NullReferenceException">查询不到任何实体对象</exception>
        /// <exception cref="NotSupportedException">无法将表达式转换SQL语句</exception>
        /// <exception cref="InvalidOperationException">查询到1个以上的实体对象</exception>
        protected T Resolve<T>(Expression<Func<T, bool>> predicate) where T : AggregateRootEntity
        {
            return this.Single<T>(predicate);
        }
        #endregion

        #region # 根据条件获取实体对象集合（修改时用） —— IQueryable<T> ResolveRange<T>(...
        /// <summary>
        /// 根据条件获取实体对象集合（修改时用）
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <returns>实体对象集合</returns>
        /// <exception cref="ArgumentNullException">条件表达式为空</exception>
        /// <exception cref="NotSupportedException">无法将表达式转换SQL语句</exception>
        /// <exception cref="InvalidOperationException">查询到1个以上的实体对象</exception>
        protected IQueryable<T> ResolveRange<T>(Expression<Func<T, bool>> predicate) where T : AggregateRootEntity
        {
            #region # 验证参数

            if (predicate == null)
            {
                throw new ArgumentNullException("predicate", @"条件表达式不可为空！");
            }

            #endregion

            lock (_Sync)
            {
                return this._dbContext.Set<T>().Where(x => !x.Deleted).Where(predicate);
            }
        }
        #endregion

        #region # 根据条件获取唯一实体对象（修改时用） —— T Single<T>(...
        /// <summary>
        /// 根据条件获取唯一实体对象（修改时用）
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <returns>实体对象集合</returns>
        /// <exception cref="ArgumentNullException">条件表达式为空</exception>
        /// <exception cref="NullReferenceException">查询不到任何实体对象</exception>
        /// <exception cref="NotSupportedException">无法将表达式转换SQL语句</exception>
        /// <exception cref="InvalidOperationException">查询到1个以上的实体对象</exception>
        private T Single<T>(Expression<Func<T, bool>> predicate) where T : AggregateRootEntity
        {
            #region # 验证参数

            if (predicate == null)
            {
                throw new ArgumentNullException("predicate", @"条件表达式不可为空！");
            }

            if (!this._dbContext.Set<T>().Where(x => !x.Deleted).Any(predicate))
            {
                throw new NullReferenceException(string.Format("给定的条件\"{0}\"中查询不到任何{1}实体对象！", predicate, typeof(T).Name));
            }

            if (this._dbContext.Set<T>().Where(x => !x.Deleted).Count(predicate) > 1)
            {
                throw new InvalidOperationException(string.Format("给定的条件\"{0}\"中查询到1个以上的{1}实体对象！", predicate, typeof(T).Name));
            }

            #endregion

            lock (_Sync)
            {
                return this._dbContext.Set<T>().Where(x => !x.Deleted).Single(predicate);
            }
        }
        #endregion
    }
}
