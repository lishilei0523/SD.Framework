using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using SD.Infrastructure.EntityBase;
using SD.Infrastructure.MemberShip;
using SD.Infrastructure.Repository.EntityFrameworkCore.Base;
using SD.Infrastructure.RepositoryBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SD.Infrastructure.Repository.EntityFrameworkCore
{
    /// <summary>
    /// EF单元事务Provider
    /// </summary>
    public abstract class EFUnitOfWorkProvider : IUnitOfWork
    {
        #region # 创建EF（写）上下文对象

        /// <summary>
        /// 获取操作人信息
        /// </summary>
        public static event Func<LoginInfo> GetLoginInfo;

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
        /// SQL命令建造者
        /// </summary>
        private readonly StringBuilder _sqlCommandBuilder;

        /// <summary>
        /// EF（写）上下文对象
        /// </summary>
        protected readonly DbContext _dbContext;

        /// <summary>
        /// 构造器
        /// </summary>
        protected EFUnitOfWorkProvider()
        {
            //初始化SQL命令建造者
            this._sqlCommandBuilder = new StringBuilder();

            //初始化EF（写）上下文对象
            this._dbContext = DbSessionBase.CommandInstance;
        }

        /// <summary>
        /// 析构器
        /// </summary>
        ~EFUnitOfWorkProvider()
        {
            this.Dispose();
        }

        #endregion


        /**********Public**********/

        //Register部分

        #region # 注册添加单个实体对象 —— void RegisterAdd<T>(T entity)
        /// <summary>
        /// 注册添加单个实体对象
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">新实体对象</param>
        public void RegisterAdd<T>(T entity) where T : AggregateRootEntity
        {
            #region # 验证

            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), $"要添加的{typeof(T).Name}实体对象不可为空！");
            }

            #endregion

            #region # 设置创建/操作人信息

            if (GetLoginInfo != null)
            {
                LoginInfo loginInfo = GetLoginInfo.Invoke();
                entity.CreatorAccount = loginInfo?.LoginId;
                entity.CreatorName = loginInfo?.RealName;
                entity.OperatorAccount = loginInfo?.LoginId;
                entity.OperatorName = loginInfo?.RealName;
            }

            #endregion

            this._dbContext.Set<T>().Add(entity);
        }
        #endregion

        #region # 注册添加实体对象列表 —— void RegisterAddRange<T>(IEnumerable<T> entities)
        /// <summary>
        /// 注册添加实体对象列表
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entities">实体对象列表</param>
        public void RegisterAddRange<T>(IEnumerable<T> entities) where T : AggregateRootEntity
        {
            #region # 验证

            entities = entities?.ToArray() ?? new T[0];
            if (!entities.Any())
            {
                throw new ArgumentNullException(nameof(entities), $"要添加的{typeof(T).Name}实体对象列表不可为空！");
            }

            #endregion

            #region # 设置创建/操作人信息

            if (GetLoginInfo != null)
            {
                LoginInfo loginInfo = GetLoginInfo.Invoke();

                foreach (T entity in entities)
                {
                    entity.CreatorAccount = loginInfo?.LoginId;
                    entity.CreatorName = loginInfo?.RealName;
                    entity.OperatorAccount = loginInfo?.LoginId;
                    entity.OperatorName = loginInfo?.RealName;
                }
            }

            #endregion

            this._dbContext.Set<T>().AddRange(entities);
        }
        #endregion

        #region # 注册保存单个实体对象 —— void RegisterSave<T>(T entity)
        /// <summary>
        /// 注册保存单个实体对象
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">实体对象</param>
        public void RegisterSave<T>(T entity) where T : AggregateRootEntity
        {
            #region # 验证

            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), $"要保存的{typeof(T).Name}实体对象不可为空！");
            }

            #endregion

            #region # 设置操作人信息

            if (GetLoginInfo != null)
            {
                LoginInfo loginInfo = GetLoginInfo.Invoke();
                entity.OperatorAccount = loginInfo?.LoginId;
                entity.OperatorName = loginInfo?.RealName;
            }

            #endregion

            entity.SavedTime = DateTime.Now;
            EntityEntry entry = this._dbContext.Entry<T>(entity);
            entry.State = EntityState.Modified;
        }
        #endregion

        #region # 注册保存实体对象列表 —— void RegisterSaveRange<T>(IEnumerable<T> entities)
        /// <summary>
        /// 注册保存实体对象列表
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entities">实体对象列表</param>
        public void RegisterSaveRange<T>(IEnumerable<T> entities) where T : AggregateRootEntity
        {
            #region # 验证

            entities = entities?.ToArray() ?? new T[0];
            if (!entities.Any())
            {
                throw new ArgumentNullException(nameof(entities), $"要保存的{typeof(T).Name}实体对象列表不可为空！");
            }

            #endregion

            LoginInfo loginInfo = null;

            #region # 获取操作人信息

            if (GetLoginInfo != null)
            {
                loginInfo = GetLoginInfo.Invoke();
            }

            #endregion

            DateTime savedTime = DateTime.Now;

            foreach (T entity in entities)
            {
                entity.OperatorAccount = loginInfo?.LoginId;
                entity.OperatorName = loginInfo?.RealName;
                entity.SavedTime = savedTime;
                EntityEntry entry = this._dbContext.Entry<T>(entity);
                entry.State = EntityState.Modified;
            }
        }
        #endregion

        #region # 注册删除单个实体对象（物理删除） —— void RegisterPhysicsRemove<T>(Guid id)
        /// <summary>
        /// 注册删除单个实体对象（物理删除）
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="id">标识Id</param>
        public void RegisterPhysicsRemove<T>(Guid id) where T : AggregateRootEntity
        {
            T entity = this._dbContext.Set<T>().Single(x => x.Id == id);
            this._dbContext.Set<T>().Remove(entity);
        }
        #endregion

        #region # 注册删除单个实体对象（物理删除） —— void RegisterPhysicsRemove<T>(string number)
        /// <summary>
        /// 注册删除单个实体对象（物理删除）
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="number">编号</param>
        public void RegisterPhysicsRemove<T>(string number) where T : AggregateRootEntity
        {
            #region # 验证

            if (string.IsNullOrWhiteSpace(number))
            {
                throw new ArgumentNullException(nameof(number), $"要删除的{typeof(T).Name}实体对象编号不可为空！");
            }

            #endregion

            T entity = this._dbContext.Set<T>().Single(x => x.Number == number);
            this._dbContext.Set<T>().Remove(entity);
        }
        #endregion

        #region # 注册删除单个实体对象（物理删除） —— void RegisterPhysicsRemove<T>(T entity)
        /// <summary>
        /// 注册删除单个实体对象（物理删除）
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">实体对象</param>
        public void RegisterPhysicsRemove<T>(T entity) where T : AggregateRootEntity
        {
            #region # 验证

            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), $"要删除的{typeof(T).Name}实体对象不可为空！");
            }

            #endregion

            this._dbContext.Set<T>().Remove(entity);
        }
        #endregion

        #region # 注册删除多个实体对象（物理删除） —— void RegisterPhysicsRemoveRange<T>(IEnumerable<Guid> ids)
        /// <summary>
        /// 注册删除多个实体对象（物理删除）
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="ids">标识Id集</param>
        public void RegisterPhysicsRemoveRange<T>(IEnumerable<Guid> ids) where T : AggregateRootEntity
        {
            #region # 验证

            ids = ids?.ToArray() ?? new Guid[0];

            if (!ids.Any())
            {
                throw new ArgumentNullException(nameof(ids), $"要删除的{typeof(T).Name}的Id集不可为空！");
            }

            #endregion

            IEnumerable<T> entities = this.ResolveRange<T>(ids);

            foreach (T entity in entities)
            {
                this._dbContext.Set<T>().Remove(entity);
            }
        }
        #endregion

        #region # 注册删除多个实体对象（物理删除） —— void RegisterPhysicsRemoveRange<T>(IEnumerable<T> entities)
        /// <summary>
        /// 注册删除多个实体对象（物理删除）
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entities">实体对象集</param>
        public void RegisterPhysicsRemoveRange<T>(IEnumerable<T> entities) where T : AggregateRootEntity
        {
            #region # 验证

            entities = entities?.ToArray() ?? Array.Empty<T>();
            if (!entities.Any())
            {
                throw new ArgumentNullException(nameof(entities), $"要删除的{typeof(T).Name}的实体对象集不可为空！");
            }

            #endregion

            this._dbContext.Set<T>().RemoveRange(entities);
        }
        #endregion

        #region # 注册删除单个实体对象（逻辑删除） —— void RegisterRemove<T>(Guid id)
        /// <summary>
        /// 注册删除单个实体对象（逻辑删除）
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="id">标识Id</param>
        public void RegisterRemove<T>(Guid id) where T : AggregateRootEntity
        {
            T entity = this.ResolveOptional<T>(x => x.Id == id);

            #region # 验证为null

            if (entity == null)
            {
                throw new NullReferenceException($"Id为\"{id}\"的{typeof(T).Name}实体不存在！");
            }

            #endregion

            #region # 设置操作人信息

            if (GetLoginInfo != null)
            {
                LoginInfo loginInfo = GetLoginInfo.Invoke();
                entity.OperatorAccount = loginInfo?.LoginId;
                entity.OperatorName = loginInfo?.RealName;
            }

            #endregion

            entity.Deleted = true;
            entity.DeletedTime = DateTime.Now;
            EntityEntry entry = this._dbContext.Entry<T>(entity);
            entry.State = EntityState.Modified;
        }
        #endregion

        #region # 注册删除单个实体对象（逻辑删除） —— void RegisterRemove<T>(string number)
        /// <summary>
        /// 注册删除单个实体对象（逻辑删除）
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="number">编号</param>
        public void RegisterRemove<T>(string number) where T : AggregateRootEntity
        {
            #region # 验证

            if (string.IsNullOrWhiteSpace(number))
            {
                throw new ArgumentNullException(nameof(number), $"要删除的{typeof(T).Name}实体对象编号不可为空！");
            }

            #endregion

            T entity = this.ResolveOptional<T>(x => x.Number == number);

            #region # 验证为null

            if (entity == null)
            {
                throw new NullReferenceException($"编号为\"{number}\"的{typeof(T).Name}实体不存在！");
            }

            #endregion

            #region # 设置操作人信息

            if (GetLoginInfo != null)
            {
                LoginInfo loginInfo = GetLoginInfo.Invoke();
                entity.OperatorAccount = loginInfo?.LoginId;
                entity.OperatorName = loginInfo?.RealName;
            }

            #endregion

            entity.Deleted = true;
            entity.DeletedTime = DateTime.Now;
            EntityEntry entry = this._dbContext.Entry<T>(entity);
            entry.State = EntityState.Modified;
        }
        #endregion

        #region # 注册删除单个实体对象（逻辑删除） —— void RegisterRemove<T>(T entity)
        /// <summary>
        /// 注册删除单个实体对象（逻辑删除）
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">实体对象</param>
        public void RegisterRemove<T>(T entity) where T : AggregateRootEntity
        {
            #region # 验证

            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), $"要删除的{typeof(T).Name}实体对象不可为空！");
            }

            #endregion

            #region # 设置操作人信息

            if (GetLoginInfo != null)
            {
                LoginInfo loginInfo = GetLoginInfo.Invoke();
                entity.OperatorAccount = loginInfo?.LoginId;
                entity.OperatorName = loginInfo?.RealName;
            }

            #endregion

            entity.Deleted = true;
            entity.DeletedTime = DateTime.Now;
            EntityEntry entry = this._dbContext.Entry<T>(entity);
            entry.State = EntityState.Modified;
        }
        #endregion

        #region # 注册删除多个实体对象（逻辑删除） —— void RegisterRemoveRange<T>(IEnumerable<Guid> ids)
        /// <summary>
        /// 注册删除多个实体对象（逻辑删除）
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="ids">标识Id集</param>
        public void RegisterRemoveRange<T>(IEnumerable<Guid> ids) where T : AggregateRootEntity
        {
            #region # 验证

            Guid[] entityIds = ids?.ToArray() ?? new Guid[0];

            if (!entityIds.Any())
            {
                throw new ArgumentNullException(nameof(ids), $"要删除的{typeof(T).Name}的Id集不可为空！");
            }

            #endregion

            LoginInfo loginInfo = null;

            #region # 获取操作人信息

            if (GetLoginInfo != null)
            {
                loginInfo = GetLoginInfo.Invoke();
            }

            #endregion

            IQueryable<T> entities = this.ResolveRange<T>(x => entityIds.Contains(x.Id));
            DateTime deletedTime = DateTime.Now;

            foreach (T entity in entities)
            {
                entity.OperatorAccount = loginInfo?.LoginId;
                entity.OperatorName = loginInfo?.RealName;
                entity.Deleted = true;
                entity.DeletedTime = deletedTime;
                EntityEntry entry = this._dbContext.Entry<T>(entity);
                entry.State = EntityState.Modified;
            }
        }
        #endregion

        #region # 注册删除多个实体对象（逻辑删除） —— void RegisterRemoveRange<T>(IEnumerable<T> entities)
        /// <summary>
        /// 注册删除多个实体对象（逻辑删除）
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entities">实体对象集</param>
        public void RegisterRemoveRange<T>(IEnumerable<T> entities) where T : AggregateRootEntity
        {
            #region # 验证

            entities = entities?.ToArray() ?? Array.Empty<T>();
            if (!entities.Any())
            {
                throw new ArgumentNullException(nameof(entities), $"要删除的{typeof(T).Name}的实体对象集不可为空！");
            }

            #endregion

            LoginInfo loginInfo = null;
            DateTime deletedTime = DateTime.Now;

            #region # 获取操作人信息

            if (GetLoginInfo != null)
            {
                loginInfo = GetLoginInfo.Invoke();
            }

            #endregion

            foreach (T entity in entities)
            {
                entity.OperatorAccount = loginInfo?.LoginId;
                entity.OperatorName = loginInfo?.RealName;
                entity.Deleted = true;
                entity.DeletedTime = deletedTime;
                EntityEntry entry = this._dbContext.Entry<T>(entity);
                entry.State = EntityState.Modified;
            }
        }
        #endregion


        //Resolve部分

        #region # 根据Id获取唯一实体对象（修改时用） —— T Resolve<T>(Guid id)
        /// <summary>
        /// 根据Id获取唯一实体对象（修改时用）
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="id">标识Id</param>
        /// <returns>唯一实体对象</returns>
        public T Resolve<T>(Guid id) where T : AggregateRootEntity
        {
            T entity = this.ResolveOptional<T>(x => x.Id == id);

            #region # 验证为null

            if (entity == null)
            {
                throw new NullReferenceException($"Id为\"{id}\"的{typeof(T).Name}实体不存在！");
            }

            #endregion

            return entity;
        }
        #endregion

        #region # 根据Id集获取实体对象列表（修改时用） —— ICollection<T> ResolveRange<T>(...
        /// <summary>
        /// 根据Id集获取实体对象列表（修改时用）
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="ids">标识Id集</param>
        /// <returns>实体对象列表</returns>
        public ICollection<T> ResolveRange<T>(IEnumerable<Guid> ids) where T : AggregateRootEntity
        {
            return this.ResolveRange<T>(x => ids.Contains(x.Id)).ToList();
        }
        #endregion

        #region # 根据编号获取唯一实体对象（修改时用） —— T Resolve<T>(string number)
        /// <summary>
        /// 根据编号获取唯一实体对象（修改时用）
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="number">编号</param>
        /// <returns>单个实体对象</returns>
        public T Resolve<T>(string number) where T : AggregateRootEntity
        {
            #region # 验证

            if (string.IsNullOrWhiteSpace(number))
            {
                throw new ArgumentNullException(nameof(number), $"{typeof(T).Name}的编号不可为空！");
            }

            #endregion

            T entity = this.ResolveOptional<T>(x => x.Number == number);

            #region # 验证为null

            if (entity == null)
            {
                throw new NullReferenceException($"编号为\"{number}\"的{typeof(T).Name}实体不存在！");
            }

            #endregion

            return entity;
        }
        #endregion

        #region # 根据编号集获取实体对象列表（修改时用） —— ICollection<T> ResolveRange<T>(...
        /// <summary>
        /// 根据编号集获取实体对象列表（修改时用）
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="numbers">编号集</param>
        /// <returns>实体对象列表</returns>
        public ICollection<T> ResolveRange<T>(IEnumerable<string> numbers) where T : AggregateRootEntity
        {
            return this.ResolveRange<T>(x => numbers.Contains(x.Number)).ToList();
        }
        #endregion

        #region # 异步根据Id获取唯一实体对象（修改时用） —— async Task<T> ResolveAsync<T>(Guid id)
        /// <summary>
        /// 异步根据Id获取唯一实体对象（修改时用）
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="id">标识Id</param>
        /// <returns>唯一实体对象</returns>
        public async Task<T> ResolveAsync<T>(Guid id) where T : AggregateRootEntity
        {
            T entity = await this.ResolveOptionalAsync<T>(x => x.Id == id);

            #region # 验证为null

            if (entity == null)
            {
                throw new NullReferenceException($"Id为\"{id}\"的{typeof(T).Name}实体不存在！");
            }

            #endregion

            return entity;
        }
        #endregion

        #region # 异步根据Id集获取实体对象列表（修改时用） —— async Task<ICollection<T>> ResolveRangeAsync<T>(...
        /// <summary>
        /// 异步根据Id集获取实体对象列表（修改时用）
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="ids">标识Id集</param>
        /// <returns>实体对象列表</returns>
        public async Task<ICollection<T>> ResolveRangeAsync<T>(IEnumerable<Guid> ids) where T : AggregateRootEntity
        {
            return await this.ResolveRange<T>(x => ids.Contains(x.Id)).ToListAsync();
        }
        #endregion

        #region # 异步根据编号获取唯一实体对象（修改时用） —— async Task<T> ResolveAsync<T>(string number)

        /// <summary>
        /// 异步根据编号获取唯一实体对象（修改时用）
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="number">编号</param>
        /// <returns>单个实体对象</returns>
        public async Task<T> ResolveAsync<T>(string number) where T : AggregateRootEntity
        {
            #region # 验证

            if (string.IsNullOrWhiteSpace(number))
            {
                throw new ArgumentNullException(nameof(number), $"{typeof(T).Name}的编号不可为空！");
            }

            #endregion

            T entity = await this.ResolveOptionalAsync<T>(x => x.Number == number);

            #region # 验证为null

            if (entity == null)
            {
                throw new NullReferenceException($"编号为\"{number}\"的{typeof(T).Name}实体不存在！");
            }

            #endregion

            return entity;
        }
        #endregion

        #region # 异步根据编号集获取实体对象列表（修改时用） —— async Task<ICollection<T>> ResolveRangeAsync<T>(...
        /// <summary>
        /// 异步根据编号集获取实体对象列表（修改时用）
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="numbers">编号集</param>
        /// <returns>实体对象列表</returns>
        public async Task<ICollection<T>> ResolveRangeAsync<T>(IEnumerable<string> numbers) where T : AggregateRootEntity
        {
            return await this.ResolveRange<T>(x => numbers.Contains(x.Number)).ToListAsync();
        }
        #endregion


        //Commit部分

        #region # 统一事务处理保存更改 —— void Commit()
        /// <summary>
        /// 统一事务处理保存更改
        /// </summary>
        public void Commit()
        {
            try
            {
                if (this._sqlCommandBuilder.Length > 0)
                {
                    using (IDbContextTransaction transaction = this._dbContext.Database.BeginTransaction())
                    {
                        //保存修改
                        this._dbContext.SaveChanges();

                        //执行SQL命令
                        string sqlCommands = this._sqlCommandBuilder.ToString();
                        this._dbContext.Database.ExecuteSqlRaw(sqlCommands);

                        //提交事务
                        transaction.Commit();

                        //清空SQL命令
                        this._sqlCommandBuilder.Clear();
                    }
                }
                else
                {
                    //保存修改
                    this._dbContext.SaveChanges();
                }
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
                if (this._sqlCommandBuilder.Length > 0)
                {
                    using (IDbContextTransaction transaction = await this._dbContext.Database.BeginTransactionAsync())
                    {
                        //保存修改
                        await this._dbContext.SaveChangesAsync();

                        //执行SQL命令
                        string sqlCommands = this._sqlCommandBuilder.ToString();
                        await this._dbContext.Database.ExecuteSqlRawAsync(sqlCommands);

                        //提交事务
                        await transaction.CommitAsync();

                        //清空SQL命令
                        this._sqlCommandBuilder.Clear();
                    }
                }
                else
                {
                    //保存修改
                    await this._dbContext.SaveChangesAsync();
                }
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
            foreach (EntityEntry entry in this._dbContext.ChangeTracker.Entries())
            {
                entry.State = EntityState.Unchanged;
            }

            //清空SQL命令
            this._sqlCommandBuilder.Clear();
        }
        #endregion

        #region # 执行SQL命令（无需Commit） —— void ExecuteSqlCommand(string sql...
        /// <summary>
        /// 执行SQL命令（无需Commit）
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">参数列表</param>
        public void ExecuteSqlCommand(string sql, params object[] parameters)
        {
            #region # 验证

            if (string.IsNullOrWhiteSpace(sql))
            {
                throw new ArgumentNullException(nameof(sql), "SQL语句不可为空！");
            }

            #endregion

            this._dbContext.Database.ExecuteSqlRaw(sql, parameters);
        }
        #endregion

        #region # 异步执行SQL命令（无需Commit） —— Task ExecuteSqlCommandAsync(string sql...
        /// <summary>
        /// 异步执行SQL命令（无需Commit）
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">参数</param>
        public async Task ExecuteSqlCommandAsync(string sql, params object[] parameters)
        {
            #region # 验证

            if (string.IsNullOrWhiteSpace(sql))
            {
                throw new ArgumentNullException(nameof(sql), "SQL语句不可为空！");
            }

            #endregion

            await this._dbContext.Database.ExecuteSqlRawAsync(sql, parameters);
        }
        #endregion

        #region # 释放资源 —— void Dispose()
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            this._sqlCommandBuilder.Clear();
            this._dbContext?.Dispose();
        }
        #endregion


        /**********Protected**********/

        #region # 注册条件删除（物理删除） —— void RegisterPhysicsRemove<T>(...
        /// <summary>
        /// 注册条件删除（物理删除）
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="condition">条件表达式</param>
        protected void RegisterPhysicsRemove<T>(Expression<Func<T, bool>> condition) where T : AggregateRootEntity
        {
            #region # 验证

            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition), "条件表达式不可为空！");
            }

            #endregion

            IQueryable<T> queryable = this._dbContext.Set<T>().Where(x => !x.Deleted).Where(condition);

            foreach (T entity in queryable)
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
        /// <param name="condition">条件表达式</param>
        protected void RegisterRemove<T>(Expression<Func<T, bool>> condition) where T : AggregateRootEntity
        {
            #region # 验证

            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition), "条件表达式不可为空！");
            }

            #endregion

            LoginInfo loginInfo = null;

            #region # 获取操作人信息

            if (GetLoginInfo != null)
            {
                loginInfo = GetLoginInfo.Invoke();
            }

            #endregion

            IQueryable<T> queryable = this._dbContext.Set<T>().Where(x => !x.Deleted).Where(condition);
            DateTime deletedTime = DateTime.Now;

            foreach (T entity in queryable)
            {
                entity.OperatorAccount = loginInfo?.LoginId;
                entity.OperatorName = loginInfo?.RealName;
                entity.Deleted = true;
                entity.DeletedTime = deletedTime;
                EntityEntry entry = this._dbContext.Entry<T>(entity);
                entry.State = EntityState.Modified;
            }
        }
        #endregion

        #region # 注册SQL命令 —— void RegisterSqlCommand(string sql)
        /// <summary>
        /// 注册SQL命令
        /// </summary>
        /// <param name="sql">SQL脚本</param>
        protected void RegisterSqlCommand(string sql)
        {
            this._sqlCommandBuilder.Append(sql);
            this._sqlCommandBuilder.Append(";");
        }
        #endregion

        #region # 根据条件获取唯一实体对象（修改时用） —— T ResolveOptional<T>(...
        /// <summary>
        /// 根据条件获取唯一实体对象（修改时用）
        /// </summary>
        /// <param name="condition">条件</param>
        /// <returns>实体对象</returns>
        ///<remarks>查询不到将返回null</remarks>
        protected T ResolveOptional<T>(Expression<Func<T, bool>> condition) where T : AggregateRootEntity
        {
            #region # 验证

            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition), "条件表达式不可为空！");
            }

            #endregion

            return this._dbContext.Set<T>().Where(x => !x.Deleted).SingleOrDefault(condition);
        }
        #endregion

        #region # 根据条件获取实体对象列表（修改时用） —— IQueryable<T> ResolveRange<T>(...
        /// <summary>
        /// 根据条件获取实体对象列表（修改时用）
        /// </summary>
        /// <param name="condition">条件</param>
        /// <returns>实体对象列表</returns>
        protected IQueryable<T> ResolveRange<T>(Expression<Func<T, bool>> condition) where T : AggregateRootEntity
        {
            #region # 验证

            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition), "条件表达式不可为空！");
            }

            #endregion

            return this._dbContext.Set<T>().Where(x => !x.Deleted).Where(condition);
        }
        #endregion

        #region # 异步根据条件获取唯一实体对象（修改时用） —— async Task<T> ResolveOptionalAsync<T>(...
        /// <summary>
        /// 异步根据条件获取唯一实体对象（修改时用）
        /// </summary>
        /// <param name="condition">条件</param>
        /// <returns>实体对象</returns>
        ///<remarks>查询不到将返回null</remarks>
        protected async Task<T> ResolveOptionalAsync<T>(Expression<Func<T, bool>> condition) where T : AggregateRootEntity
        {
            #region # 验证

            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition), "条件表达式不可为空！");
            }

            #endregion

            return await this._dbContext.Set<T>().Where(x => !x.Deleted).SingleOrDefaultAsync(condition);
        }
        #endregion

        #region # 是否存在给定条件的实体对象 —— bool Exists(Expression<Func<T, bool>> condition)
        /// <summary>
        /// 是否存在给定条件的实体对象
        /// </summary>
        /// <param name="condition">条件</param>
        /// <returns>是否存在</returns>
        protected bool Exists<T>(Expression<Func<T, bool>> condition) where T : AggregateRootEntity
        {
            #region # 验证

            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition), "条件表达式不可为空！");
            }

            #endregion

            lock (_Sync)
            {
                return this._dbContext.Set<T>().Where(x => !x.Deleted).Any(condition);
            }
        }
        #endregion
    }
}
