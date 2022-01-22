using SD.Infrastructure.Constants;
using SD.Infrastructure.EntityBase;
using SD.Infrastructure.Membership;
using SD.Infrastructure.Repository.EntityFramework.Base;
using SD.Infrastructure.RepositoryBase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
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

        //Transaction部分

        #region # 开启事务 —— DbTransaction BeginTransaction(IsolationLevel isolationLevel)
        /// <summary>
        /// 开启事务
        /// </summary>
        /// <returns>事务</returns>
        public DbTransaction BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            DbContextTransaction dbContextTransaction = this._dbContext.Database.BeginTransaction(isolationLevel);
            DbTransaction dbTransaction = dbContextTransaction.UnderlyingTransaction;

            return dbTransaction;
        }
        #endregion

        #region # 使用事务 —— void UseTransaction(DbTransaction dbTransaction)
        /// <summary>
        /// 使用事务
        /// </summary>
        /// <param name="dbTransaction">事务</param>
        public void UseTransaction(DbTransaction dbTransaction)
        {
            this._dbContext.Database.UseTransaction(dbTransaction);
        }
        #endregion

        #region # 获取当前事务 —— DbTransaction GetCurrentTransaction()
        /// <summary>
        /// 获取当前事务
        /// </summary>
        /// <returns>事务</returns>
        public DbTransaction GetCurrentTransaction()
        {
            DbContextTransaction dbContextTransaction = this._dbContext.Database.CurrentTransaction;
            DbTransaction dbTransaction = dbContextTransaction?.UnderlyingTransaction;

            return dbTransaction;
        }
        #endregion

        #region # 获取实体历史列表 —— ICollection<IEntityHistory> GetEntityHistories<T>()
        /// <summary>
        /// 获取实体历史列表
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="actionType">动作类型</param>
        /// <returns>实体历史列表</returns>
        public ICollection<IEntityHistory> GetEntityHistories<T>(ActionType? actionType = null) where T : PlainEntity
        {
            LoginInfo loginInfo = GetLoginInfo?.Invoke();
            ICollection<IEntityHistory> entityHistories = new HashSet<IEntityHistory>();
            IEnumerable<DbEntityEntry<T>> entries =
                from entry in this._dbContext.ChangeTracker.Entries<T>()
                where actionType == null || entry.State == (EntityState)actionType.Value
                select entry;
            foreach (DbEntityEntry<T> entry in entries)
            {
                ActionType actualActionType;
                IDictionary<string, object> beforeSnapshot = new Dictionary<string, object>();
                IDictionary<string, object> afterSnapshot = new Dictionary<string, object>();
                if (entry.State == EntityState.Added)
                {
                    actualActionType = ActionType.Create;
                    foreach (string propertyName in entry.CurrentValues.PropertyNames)
                    {
                        DbPropertyEntry propertyEntry = entry.Property(propertyName);
                        afterSnapshot[propertyName] = propertyEntry.CurrentValue;
                    }
                }
                else if (entry.State == EntityState.Modified)
                {
                    actualActionType = ActionType.Update;
                    foreach (string propertyName in entry.OriginalValues.PropertyNames)
                    {
                        DbPropertyEntry propertyEntry = entry.Property(propertyName);
                        if (propertyEntry.OriginalValue?.ToString() != propertyEntry.CurrentValue?.ToString())
                        {
                            beforeSnapshot[propertyName] = propertyEntry.OriginalValue;
                            afterSnapshot[propertyName] = propertyEntry.CurrentValue;
                        }
                    }
                }
                else if (entry.State == EntityState.Deleted)
                {
                    actualActionType = ActionType.Delete;
                    foreach (string propertyName in entry.OriginalValues.PropertyNames)
                    {
                        DbPropertyEntry propertyEntry = entry.Property(propertyName);
                        beforeSnapshot[propertyName] = propertyEntry.OriginalValue;
                    }
                }
                else
                {
                    continue;
                }

                EntityHistory entityHistory = new EntityHistory(actualActionType, entry.Entity.GetType(), entry.Entity.Id, beforeSnapshot, afterSnapshot, loginInfo?.LoginId, loginInfo?.RealName);
                entityHistories.Add(entityHistory);
            }

            return entityHistories;
        }
        #endregion


        //Register部分

        #region # 注册添加实体对象 —— void RegisterAdd<T>(T entity)
        /// <summary>
        /// 注册添加实体对象
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

        #region # 注册保存实体对象 —— void RegisterSave<T>(T entity)
        /// <summary>
        /// 注册保存实体对象
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
            DbEntityEntry entry = this._dbContext.Entry<T>(entity);
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
            DateTime savedTime = DateTime.Now;

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
                entity.SavedTime = savedTime;
                DbEntityEntry entry = this._dbContext.Entry<T>(entity);
                entry.State = EntityState.Modified;
            }
        }
        #endregion

        #region # 注册删除实体对象 —— void RegisterPhysicsRemove<T>(Guid id)
        /// <summary>
        /// 注册删除实体对象
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="id">标识Id</param>
        /// <remarks>物理删除</remarks>
        public void RegisterPhysicsRemove<T>(Guid id) where T : AggregateRootEntity
        {
            T entity = this._dbContext.Set<T>().Single(x => x.Id == id);
            this._dbContext.Set<T>().Remove(entity);
        }
        #endregion

        #region # 注册删除实体对象 —— void RegisterPhysicsRemove<T>(string number)
        /// <summary>
        /// 注册删除实体对象
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="number">编号</param>
        /// <remarks>物理删除</remarks>
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

        #region # 注册删除实体对象 —— void RegisterPhysicsRemove<T>(long rowNo)
        /// <summary>
        /// 注册删除实体对象
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="rowNo">行号</param>
        /// <remarks>物理删除</remarks>
        public void RegisterPhysicsRemove<T>(long rowNo) where T : AggregateRootEntity, IRowable
        {
            T entity = this._dbContext.Set<T>().Single(x => x.RowNo == rowNo);
            this._dbContext.Set<T>().Remove(entity);
        }
        #endregion

        #region # 注册删除实体对象 —— void RegisterPhysicsRemove<T>(T entity)
        /// <summary>
        /// 注册删除实体对象
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <remarks>物理删除</remarks>
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

        #region # 注册删除多个实体对象 —— void RegisterPhysicsRemoveRange<T>(IEnumerable<Guid> ids)
        /// <summary>
        /// 注册删除多个实体对象
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="ids">标识Id集</param>
        /// <remarks>物理删除</remarks>
        public void RegisterPhysicsRemoveRange<T>(IEnumerable<Guid> ids) where T : AggregateRootEntity
        {
            #region # 验证

            ids = ids?.ToArray() ?? new Guid[0];
            if (!ids.Any())
            {
                throw new ArgumentNullException(nameof(ids), $"要删除的{typeof(T).Name}的Id集不可为空！");
            }

            #endregion

            ICollection<T> entities = this.ResolveRange<T>(ids);
            this._dbContext.Set<T>().RemoveRange(entities);
        }
        #endregion

        #region # 注册删除多个实体对象 —— void RegisterPhysicsRemoveRange<T>(IEnumerable<string> numbers)
        /// <summary>
        /// 注册删除多个实体对象
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="numbers">编号集</param>
        /// <remarks>物理删除</remarks>
        public void RegisterPhysicsRemoveRange<T>(IEnumerable<string> numbers) where T : AggregateRootEntity
        {
            #region # 验证

            numbers = numbers?.ToArray() ?? new string[0];
            if (!numbers.Any())
            {
                throw new ArgumentNullException(nameof(numbers), $"要删除的{typeof(T).Name}的编号集不可为空！");
            }

            #endregion

            ICollection<T> entities = this.ResolveRange<T>(numbers);
            this._dbContext.Set<T>().RemoveRange(entities);
        }
        #endregion

        #region # 注册删除多个实体对象 —— void RegisterPhysicsRemoveRange<T>(IEnumerable<long> rowNos)
        /// <summary>
        /// 注册删除多个实体对象
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="rowNos">行号集</param>
        /// <remarks>物理删除</remarks>
        public void RegisterPhysicsRemoveRange<T>(IEnumerable<long> rowNos) where T : AggregateRootEntity, IRowable
        {
            #region # 验证

            rowNos = rowNos?.ToArray() ?? new long[0];
            if (!rowNos.Any())
            {
                throw new ArgumentNullException(nameof(rowNos), $"要删除的{typeof(T).Name}的行号集不可为空！");
            }

            #endregion

            ICollection<T> entities = this.ResolveRange<T>(rowNos);
            this._dbContext.Set<T>().RemoveRange(entities);
        }
        #endregion

        #region # 注册删除多个实体对象 —— void RegisterPhysicsRemoveRange<T>(IEnumerable<T> entities)
        /// <summary>
        /// 注册删除多个实体对象
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entities">实体对象集</param>
        /// <remarks>物理删除</remarks>
        public void RegisterPhysicsRemoveRange<T>(IEnumerable<T> entities) where T : AggregateRootEntity
        {
            #region # 验证

            entities = entities?.ToArray() ?? new T[0];
            if (!entities.Any())
            {
                throw new ArgumentNullException(nameof(entities), $"要删除的{typeof(T).Name}的实体对象集不可为空！");
            }

            #endregion

            this._dbContext.Set<T>().RemoveRange(entities);
        }
        #endregion

        #region # 注册删除实体对象 —— void RegisterRemove<T>(Guid id)
        /// <summary>
        /// 注册删除实体对象
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="id">标识Id</param>
        /// <remarks>逻辑删除</remarks>
        public void RegisterRemove<T>(Guid id) where T : AggregateRootEntity
        {
            T entity = this.ResolveOptional<T>(x => x.Id == id);

            #region # 验证

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
            DbEntityEntry entry = this._dbContext.Entry<T>(entity);
            entry.State = EntityState.Modified;
        }
        #endregion

        #region # 注册删除实体对象 —— void RegisterRemove<T>(string number)
        /// <summary>
        /// 注册删除实体对象
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="number">编号</param>
        /// <remarks>逻辑删除</remarks>
        public void RegisterRemove<T>(string number) where T : AggregateRootEntity
        {
            #region # 验证

            if (string.IsNullOrWhiteSpace(number))
            {
                throw new ArgumentNullException(nameof(number), $"要删除的{typeof(T).Name}实体对象编号不可为空！");
            }

            #endregion

            T entity = this.ResolveOptional<T>(x => x.Number == number);

            #region # 验证

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
            DbEntityEntry entry = this._dbContext.Entry<T>(entity);
            entry.State = EntityState.Modified;
        }
        #endregion

        #region # 注册删除实体对象 —— void RegisterRemove<T>(long rowNo)
        /// <summary>
        /// 注册删除实体对象
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="rowNo">行号</param>
        /// <remarks>逻辑删除</remarks>
        public void RegisterRemove<T>(long rowNo) where T : AggregateRootEntity, IRowable
        {
            T entity = this.ResolveOptional<T>(x => x.RowNo == rowNo);

            #region # 验证

            if (entity == null)
            {
                throw new NullReferenceException($"行号为\"{rowNo}\"的{typeof(T).Name}实体不存在！");
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
            DbEntityEntry entry = this._dbContext.Entry<T>(entity);
            entry.State = EntityState.Modified;
        }
        #endregion

        #region # 注册删除实体对象 —— void RegisterRemove<T>(T entity)
        /// <summary>
        /// 注册删除实体对象
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <remarks>逻辑删除</remarks>
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
            DbEntityEntry entry = this._dbContext.Entry<T>(entity);
            entry.State = EntityState.Modified;
        }
        #endregion

        #region # 注册删除多个实体对象 —— void RegisterRemoveRange<T>(IEnumerable<Guid> ids)
        /// <summary>
        /// 注册删除多个实体对象
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="ids">标识Id集</param>
        /// <remarks>逻辑删除</remarks>
        public void RegisterRemoveRange<T>(IEnumerable<Guid> ids) where T : AggregateRootEntity
        {
            #region # 验证

            ids = ids?.ToArray() ?? new Guid[0];
            if (!ids.Any())
            {
                throw new ArgumentNullException(nameof(ids), $"要删除的{typeof(T).Name}的Id集不可为空！");
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

            ICollection<T> entities = this.ResolveRange<T>(ids);
            foreach (T entity in entities)
            {
                entity.OperatorAccount = loginInfo?.LoginId;
                entity.OperatorName = loginInfo?.RealName;
                entity.Deleted = true;
                entity.DeletedTime = deletedTime;
                DbEntityEntry entry = this._dbContext.Entry<T>(entity);
                entry.State = EntityState.Modified;
            }
        }
        #endregion

        #region # 注册删除多个实体对象 —— void RegisterRemoveRange<T>(IEnumerable<string> numbers)
        /// <summary>
        /// 注册删除多个实体对象
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="numbers">编号集</param>
        /// <remarks>逻辑删除</remarks>
        public void RegisterRemoveRange<T>(IEnumerable<string> numbers) where T : AggregateRootEntity
        {
            #region # 验证

            numbers = numbers?.ToArray() ?? new string[0];
            if (!numbers.Any())
            {
                throw new ArgumentNullException(nameof(numbers), $"要删除的{typeof(T).Name}的编号集不可为空！");
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

            ICollection<T> entities = this.ResolveRange<T>(numbers);
            foreach (T entity in entities)
            {
                entity.OperatorAccount = loginInfo?.LoginId;
                entity.OperatorName = loginInfo?.RealName;
                entity.Deleted = true;
                entity.DeletedTime = deletedTime;
                DbEntityEntry entry = this._dbContext.Entry<T>(entity);
                entry.State = EntityState.Modified;
            }
        }
        #endregion

        #region # 注册删除多个实体对象 —— void RegisterRemoveRange<T>(IEnumerable<long> rowNos)
        /// <summary>
        /// 注册删除多个实体对象
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="rowNos">行号集</param>
        /// <remarks>逻辑删除</remarks>
        public void RegisterRemoveRange<T>(IEnumerable<long> rowNos) where T : AggregateRootEntity, IRowable
        {
            #region # 验证

            rowNos = rowNos?.ToArray() ?? new long[0];
            if (!rowNos.Any())
            {
                throw new ArgumentNullException(nameof(rowNos), $"要删除的{typeof(T).Name}的行号集不可为空！");
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

            ICollection<T> entities = this.ResolveRange<T>(rowNos);
            foreach (T entity in entities)
            {
                entity.OperatorAccount = loginInfo?.LoginId;
                entity.OperatorName = loginInfo?.RealName;
                entity.Deleted = true;
                entity.DeletedTime = deletedTime;
                DbEntityEntry entry = this._dbContext.Entry<T>(entity);
                entry.State = EntityState.Modified;
            }
        }
        #endregion

        #region # 注册删除多个实体对象 —— void RegisterRemoveRange<T>(IEnumerable<T> entities)
        /// <summary>
        /// 注册删除多个实体对象
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entities">实体对象集</param>
        /// <remarks>逻辑删除</remarks>
        public void RegisterRemoveRange<T>(IEnumerable<T> entities) where T : AggregateRootEntity
        {
            #region # 验证

            entities = entities?.ToArray() ?? new T[0];
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
                DbEntityEntry entry = this._dbContext.Entry<T>(entity);
                entry.State = EntityState.Modified;
            }
        }
        #endregion


        //Resolve部分

        #region # 根据Id获取实体对象 —— T Resolve<T>(Guid id)
        /// <summary>
        /// 根据Id获取实体对象
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="id">标识Id</param>
        /// <returns>实体对象</returns>
        public T Resolve<T>(Guid id) where T : AggregateRootEntity
        {
            T entity = this.ResolveOptional<T>(x => x.Id == id);

            #region # 验证

            if (entity == null)
            {
                throw new NullReferenceException($"Id为\"{id}\"的{typeof(T).Name}实体不存在！");
            }

            #endregion

            return entity;
        }
        #endregion

        #region # 根据Id获取实体对象 —— Task<T> ResolveAsync<T>(Guid id)
        /// <summary>
        /// 根据Id获取实体对象
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="id">标识Id</param>
        /// <returns>实体对象</returns>
        public async Task<T> ResolveAsync<T>(Guid id) where T : AggregateRootEntity
        {
            T entity = await this.ResolveOptionalAsync<T>(x => x.Id == id);

            #region # 验证

            if (entity == null)
            {
                throw new NullReferenceException($"Id为\"{id}\"的{typeof(T).Name}实体不存在！");
            }

            #endregion

            return entity;
        }
        #endregion

        #region # 根据Id集获取实体对象列表 —— ICollection<T> ResolveRange<T>(...
        /// <summary>
        /// 根据Id集获取实体对象列表
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="ids">标识Id集</param>
        /// <returns>实体对象列表</returns>
        public ICollection<T> ResolveRange<T>(IEnumerable<Guid> ids) where T : AggregateRootEntity
        {
            #region # 验证

            ids = ids?.Distinct().ToArray() ?? new Guid[0];
            if (!ids.Any())
            {
                return new List<T>();
            }

            #endregion

            return this.ResolveRange<T>(x => ids.Contains(x.Id)).ToList();
        }
        #endregion

        #region # 根据Id集获取实体对象列表 —— Task<ICollection<T>> ResolveRangeAsync<T>(...
        /// <summary>
        /// 根据Id集获取实体对象列表
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="ids">标识Id集</param>
        /// <returns>实体对象列表</returns>
        public async Task<ICollection<T>> ResolveRangeAsync<T>(IEnumerable<Guid> ids) where T : AggregateRootEntity
        {
            #region # 验证

            ids = ids?.Distinct().ToArray() ?? new Guid[0];
            if (!ids.Any())
            {
                return new List<T>();
            }

            #endregion

            return await this.ResolveRange<T>(x => ids.Contains(x.Id)).ToListAsync();
        }
        #endregion

        #region # 根据编号获取实体对象 —— T Resolve<T>(string number)
        /// <summary>
        /// 根据编号获取实体对象
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="number">编号</param>
        /// <returns>实体对象</returns>
        public T Resolve<T>(string number) where T : AggregateRootEntity
        {
            #region # 验证

            if (string.IsNullOrWhiteSpace(number))
            {
                throw new ArgumentNullException(nameof(number), $"{typeof(T).Name}的编号不可为空！");
            }

            #endregion

            T entity = this.ResolveOptional<T>(x => x.Number == number);

            #region # 验证

            if (entity == null)
            {
                throw new NullReferenceException($"编号为\"{number}\"的{typeof(T).Name}实体不存在！");
            }

            #endregion

            return entity;
        }
        #endregion

        #region # 根据编号获取实体对象 —— Task<T> ResolveAsync<T>(string number)
        /// <summary>
        /// 根据编号获取实体对象
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="number">编号</param>
        /// <returns>实体对象</returns>
        public async Task<T> ResolveAsync<T>(string number) where T : AggregateRootEntity
        {
            #region # 验证

            if (string.IsNullOrWhiteSpace(number))
            {
                throw new ArgumentNullException(nameof(number), $"{typeof(T).Name}的编号不可为空！");
            }

            #endregion

            T entity = await this.ResolveOptionalAsync<T>(x => x.Number == number);

            #region # 验证

            if (entity == null)
            {
                throw new NullReferenceException($"编号为\"{number}\"的{typeof(T).Name}实体不存在！");
            }

            #endregion

            return entity;
        }
        #endregion

        #region # 根据编号集获取实体对象列表 —— ICollection<T> ResolveRange<T>(...
        /// <summary>
        /// 根据编号集获取实体对象列表
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="numbers">编号集</param>
        /// <returns>实体对象列表</returns>
        public ICollection<T> ResolveRange<T>(IEnumerable<string> numbers) where T : AggregateRootEntity
        {
            #region # 验证

            numbers = numbers?.Distinct().ToArray() ?? new string[0];
            if (!numbers.Any())
            {
                return new List<T>();
            }

            #endregion

            return this.ResolveRange<T>(x => numbers.Contains(x.Number)).ToList();
        }
        #endregion

        #region # 根据编号集获取实体对象列表 —— Task<ICollection<T>> ResolveRangeAsync<T>(...
        /// <summary>
        /// 根据编号集获取实体对象列表
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="numbers">编号集</param>
        /// <returns>实体对象列表</returns>
        public async Task<ICollection<T>> ResolveRangeAsync<T>(IEnumerable<string> numbers) where T : AggregateRootEntity
        {
            #region # 验证

            numbers = numbers?.Distinct().ToArray() ?? new string[0];
            if (!numbers.Any())
            {
                return new List<T>();
            }

            #endregion

            return await this.ResolveRange<T>(x => numbers.Contains(x.Number)).ToListAsync();
        }
        #endregion

        #region # 根据行号获取实体对象 —— T Resolve<T>(long rowNo)
        /// <summary>
        /// 根据行号获取实体对象
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="rowNo">行号</param>
        /// <returns>实体对象</returns>
        public T Resolve<T>(long rowNo) where T : AggregateRootEntity, IRowable
        {
            T entity = this.ResolveOptional<T>(x => x.RowNo == rowNo);

            #region # 验证

            if (entity == null)
            {
                throw new NullReferenceException($"行号为\"{rowNo}\"的{typeof(T).Name}实体不存在！");
            }

            #endregion

            return entity;
        }
        #endregion

        #region # 根据行号获取实体对象 —— Task<T> ResolveAsync<T>(long rowNo)
        /// <summary>
        /// 根据行号获取实体对象
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="rowNo">行号</param>
        /// <returns>实体对象</returns>
        public async Task<T> ResolveAsync<T>(long rowNo) where T : AggregateRootEntity, IRowable
        {
            T entity = await this.ResolveOptionalAsync<T>(x => x.RowNo == rowNo);

            #region # 验证

            if (entity == null)
            {
                throw new NullReferenceException($"行号为\"{rowNo}\"的{typeof(T).Name}实体不存在！");
            }

            #endregion

            return entity;
        }
        #endregion

        #region # 根据行号集获取实体对象列表 —— ICollection<T> ResolveRange<T>(...
        /// <summary>
        /// 根据行号集获取实体对象列表
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="rowNos">行号集</param>
        /// <returns>实体对象列表</returns>
        public ICollection<T> ResolveRange<T>(IEnumerable<long> rowNos) where T : AggregateRootEntity, IRowable
        {
            #region # 验证

            rowNos = rowNos?.Distinct().ToArray() ?? new long[0];
            if (!rowNos.Any())
            {
                return new List<T>();
            }

            #endregion

            return this.ResolveRange<T>(x => rowNos.Contains(x.RowNo)).ToList();
        }
        #endregion

        #region # 根据行号集获取实体对象列表 —— Task<ICollection<T>> ResolveRangeAsync<T>(...
        /// <summary>
        /// 根据行号集获取实体对象列表
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="rowNos">行号集</param>
        /// <returns>实体对象列表</returns>
        public async Task<ICollection<T>> ResolveRangeAsync<T>(IEnumerable<long> rowNos) where T : AggregateRootEntity, IRowable
        {
            #region # 验证

            rowNos = rowNos?.Distinct().ToArray() ?? new long[0];
            if (!rowNos.Any())
            {
                return new List<T>();
            }

            #endregion

            return await this.ResolveRange<T>(x => rowNos.Contains(x.RowNo)).ToListAsync();
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
                    string sqlCommands = this._sqlCommandBuilder.ToString();
                    if (this._dbContext.Database.CurrentTransaction != null)
                    {
                        //保存修改
                        this._dbContext.SaveChanges();

                        //执行SQL命令
                        this._dbContext.Database.ExecuteSqlCommand(sqlCommands);
                    }
                    else
                    {
                        using (DbContextTransaction transaction = this._dbContext.Database.BeginTransaction())
                        {
                            //保存修改
                            this._dbContext.SaveChanges();

                            //执行SQL命令
                            this._dbContext.Database.ExecuteSqlCommand(sqlCommands);

                            //提交事务
                            transaction.Commit();
                        }
                    }

                    //清空SQL命令
                    this._sqlCommandBuilder.Clear();
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

        #region # 统一事务处理保存更改 —— Task CommitAsync()
        /// <summary>
        /// 统一事务处理保存更改
        /// </summary>
        public async Task CommitAsync()
        {
            try
            {
                if (this._sqlCommandBuilder.Length > 0)
                {
                    string sqlCommands = this._sqlCommandBuilder.ToString();
                    if (this._dbContext.Database.CurrentTransaction != null)
                    {
                        //保存修改
                        await this._dbContext.SaveChangesAsync();

                        //执行SQL命令
                        await this._dbContext.Database.ExecuteSqlCommandAsync(sqlCommands);
                    }
                    else
                    {
                        using (DbContextTransaction transaction = this._dbContext.Database.BeginTransaction())
                        {
                            //保存修改
                            await this._dbContext.SaveChangesAsync();

                            //执行SQL命令
                            await this._dbContext.Database.ExecuteSqlCommandAsync(sqlCommands);

                            //提交事务
                            transaction.Commit();
                        }
                    }

                    //清空SQL命令
                    this._sqlCommandBuilder.Clear();
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
            foreach (DbEntityEntry entry in this._dbContext.ChangeTracker.Entries())
            {
                entry.State = EntityState.Unchanged;
            }

            //清空SQL命令
            this._sqlCommandBuilder.Clear();
        }
        #endregion

        #region # 执行SQL命令 —— void ExecuteSqlCommand(string sql...
        /// <summary>
        /// 执行SQL命令
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">参数列表</param>
        /// <remarks>无需Commit</remarks>
        public void ExecuteSqlCommand(string sql, params object[] parameters)
        {
            #region # 验证

            if (string.IsNullOrWhiteSpace(sql))
            {
                throw new ArgumentNullException(nameof(sql), "SQL语句不可为空！");
            }

            #endregion

            this._dbContext.Database.ExecuteSqlCommand(sql, parameters);
        }
        #endregion

        #region # 执行SQL命令 —— Task ExecuteSqlCommandAsync(string sql...
        /// <summary>
        /// 执行SQL命令
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">参数</param>
        /// <remarks>无需Commit</remarks>
        public async Task ExecuteSqlCommandAsync(string sql, params object[] parameters)
        {
            #region # 验证

            if (string.IsNullOrWhiteSpace(sql))
            {
                throw new ArgumentNullException(nameof(sql), "SQL语句不可为空！");
            }

            #endregion

            await this._dbContext.Database.ExecuteSqlCommandAsync(sql, parameters);
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

        #region # 注册条件删除 —— void RegisterPhysicsRemove<T>(...
        /// <summary>
        /// 注册条件删除
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="condition">条件表达式</param>
        /// <remarks>物理删除</remarks>
        protected void RegisterPhysicsRemove<T>(Expression<Func<T, bool>> condition) where T : AggregateRootEntity
        {
            #region # 验证

            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition), "条件表达式不可为空！");
            }

            #endregion

            IQueryable<T> queryable = this._dbContext.Set<T>().Where(condition);

            foreach (T entity in queryable)
            {
                this._dbContext.Set<T>().Attach(entity);
                this._dbContext.Set<T>().Remove(entity);
            }
        }
        #endregion

        #region # 注册条件删除 —— void RegisterRemove<T>(...
        /// <summary>
        /// 注册条件删除
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="condition">条件表达式</param>
        /// <remarks>逻辑删除</remarks>
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

            IQueryable<T> queryable = this._dbContext.Set<T>().Where(condition);
            DateTime deletedTime = DateTime.Now;

            foreach (T entity in queryable)
            {
                entity.OperatorAccount = loginInfo?.LoginId;
                entity.OperatorName = loginInfo?.RealName;
                entity.Deleted = true;
                entity.DeletedTime = deletedTime;
                DbEntityEntry entry = this._dbContext.Entry<T>(entity);
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

        #region # 根据条件获取实体对象 —— T ResolveOptional<T>(...
        /// <summary>
        /// 根据条件获取实体对象
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

            return this._dbContext.Set<T>().SingleOrDefault(condition);
        }
        #endregion

        #region # 根据条件获取实体对象 —— Task<T> ResolveOptionalAsync<T>(...
        /// <summary>
        /// 根据条件获取实体对象
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

            return await this._dbContext.Set<T>().SingleOrDefaultAsync(condition);
        }
        #endregion

        #region # 根据条件获取实体对象列表 —— IQueryable<T> ResolveRange<T>(...
        /// <summary>
        /// 根据条件获取实体对象列表
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

            return this._dbContext.Set<T>().Where(condition);
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
                return this._dbContext.Set<T>().Any(condition);
            }
        }
        #endregion

        #region # 是否存在给定条件的实体对象 —— Task<bool> ExistsAsync<T>(Expression<Func<T, bool>> condition)
        /// <summary>
        /// 是否存在给定条件的实体对象
        /// </summary>
        /// <param name="condition">条件</param>
        /// <returns>是否存在</returns>
        protected async Task<bool> ExistsAsync<T>(Expression<Func<T, bool>> condition) where T : AggregateRootEntity
        {
            #region # 验证

            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition), "条件表达式不可为空！");
            }

            #endregion

            return await this._dbContext.Set<T>().AnyAsync(condition);
        }
        #endregion
    }
}
