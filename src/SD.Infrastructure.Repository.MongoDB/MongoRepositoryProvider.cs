using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using SD.Infrastructure.Constants;
using SD.Infrastructure.EntityBase;
using SD.Infrastructure.RepositoryBase;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace SD.Infrastructure.Repository.MongoDB
{
    /// <summary>
    /// MongoDB简单仓储Provider
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    public class MongoRepositoryProvider<T> : ISimpleRepository<T> where T : AggregateRootEntity
    {
        #region # 字段及构造器

        /// <summary>
        /// Mongo服务器与数据库名分隔符号
        /// </summary>
        private const string Separator = "::";

        /// <summary>
        /// MongoDB连接字符串
        /// </summary>
        private static readonly string _ConnectionString;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static MongoRepositoryProvider()
        {
            _ConnectionString = ConfigurationManager.ConnectionStrings[CommonConstants.MongoConnectionStringName]?.ConnectionString;
            if (string.IsNullOrWhiteSpace(_ConnectionString))
            {
                throw new ApplicationException($"MongoDB连接字符串未设置，默认连接字符串名称为\"{CommonConstants.MongoConnectionStringName}\"！");
            }

            //注册实体类型
            RegisterTypes();
        }


        /// <summary>
        /// MongoDB实体对象列表
        /// </summary>
        protected readonly IMongoCollection<T> _collection;

        /// <summary>
        /// 构造器
        /// </summary>
        protected MongoRepositoryProvider()
        {
            #region # 验证

            string[] connectionStrings = _ConnectionString.Split(new[] { Separator }, StringSplitOptions.None);
            if (connectionStrings.Length != 2)
            {
                throw new ApplicationException($"连接字符串格式不正确，请使用\"{Separator}\"来分隔服务器地址与数据库名称！");
            }

            #endregion

            MongoClient client = new MongoClient(connectionStrings[0]);
            IMongoDatabase database = client.GetDatabase(connectionStrings[1]);
            this._collection = database.GetCollection<T>(typeof(T).FullName);
        }

        #endregion

        #region # 命令部分

        #region # 添加实体对象 —— void Add(T entity)
        /// <summary>
        /// 添加实体对象
        /// </summary>
        /// <param name="entity">实体对象</param>
        public void Add(T entity)
        {
            #region # 验证

            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), $@"要添加的{typeof(T).Name}实体对象不可为空！");
            }
            if (this.Exists(entity.Id))
            {
                throw new ArgumentOutOfRangeException(nameof(entity), $"Id为\"{entity.Id}\"的实体已存在！");
            }

            #endregion

            this._collection.InsertOneAsync(entity).Wait();
        }
        #endregion

        #region # 添加实体对象列表 —— void AddRange(IEnumerable<T> entities)
        /// <summary>
        /// 添加实体对象列表
        /// </summary>
        /// <typeparam name="T">聚合根类型</typeparam>
        /// <param name="entities">实体对象集</param>
        public void AddRange(IEnumerable<T> entities)
        {
            #region # 验证

            entities = entities?.ToArray() ?? Array.Empty<T>();
            if (!entities.Any())
            {
                throw new ArgumentNullException(nameof(entities), $"要添加的{typeof(T).Name}实体对象列表不可为空！");
            }

            #endregion

            this._collection.InsertMany(entities);
        }
        #endregion

        #region # 保存实体对象 —— void Save(T entity)
        /// <summary>
        /// 保存实体对象
        /// </summary>
        /// <param name="entity">实体对象</param>
        public void Save(T entity)
        {
            #region # 验证

            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), $"要保存的{typeof(T).Name}实体对象不可为空！");
            }
            if (!this.Exists(entity.Id))
            {
                throw new NullReferenceException($"不存在Id为{entity.Id}的{typeof(T).Name}实体对象，请尝试添加操作！");
            }

            #endregion

            entity.SavedTime = DateTime.Now;

            this._collection.FindOneAndReplaceAsync(x => x.Id == entity.Id, entity).Wait();
        }
        #endregion

        #region # 保存实体对象列表 —— void SaveRange(IEnumerable<T> entities)
        /// <summary>
        /// 保存实体对象列表
        /// </summary>
        /// <typeparam name="T">聚合根类型</typeparam>
        /// <param name="entities">实体对象集</param>
        public void SaveRange(IEnumerable<T> entities)
        {
            #region # 验证

            entities = entities?.ToArray() ?? Array.Empty<T>();
            if (!entities.Any())
            {
                throw new ArgumentNullException(nameof(entities), $"要保存的{typeof(T).Name}实体对象列表不可为空！");
            }

            #endregion

            DateTime savedTime = DateTime.Now;

            IList<WriteModel<T>> bulkWrites = new List<WriteModel<T>>();
            foreach (T entity in entities)
            {
                entity.SavedTime = savedTime;

                FilterDefinitionBuilder<T> builder = new FilterDefinitionBuilder<T>();
                FilterDefinition<T> filter = builder.Eq(x => x.Id, entity.Id);
                WriteModel<T> writeModel = new ReplaceOneModel<T>(filter, entity);

                bulkWrites.Add(writeModel);
            }

            this.BulkWrite(bulkWrites);
        }
        #endregion

        #region # 删除实体对象 —— void Remove(Guid id)
        /// <summary>
        /// 删除实体对象
        /// </summary>
        /// <param name="id">标识Id</param>
        public void Remove(Guid id)
        {
            this._collection.FindOneAndDeleteAsync(x => x.Id == id).Wait();
        }
        #endregion

        #region # 删除实体对象 —— void Remove(string number)
        /// <summary>
        /// 删除实体对象
        /// </summary>
        /// <param name="number">编号</param>
        public void Remove(string number)
        {
            this._collection.FindOneAndDeleteAsync(x => x.Number == number).Wait();
        }
        #endregion

        #region # 删除多个实体对象 —— void RemoveRange(IEnumerable<Guid> ids)
        /// <summary>
        /// 删除多个实体对象
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="ids">标识Id集</param>
        public void RemoveRange(IEnumerable<Guid> ids)
        {
            #region # 验证

            Guid[] ids_ = ids?.Distinct().ToArray() ?? Array.Empty<Guid>();
            if (!ids_.Any())
            {
                throw new ArgumentNullException(nameof(ids), $"要删除的{typeof(T).Name}的id集合不可为空！");
            }

            #endregion

            this._collection.DeleteManyAsync(x => ids_.Contains(x.Id)).Wait();
        }
        #endregion

        #region # 删除多个实体对象 —— void RemoveRange(IEnumerable<string> numbers)
        /// <summary>
        /// 删除多个实体对象
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="numbers">编号集</param>
        public void RemoveRange(IEnumerable<string> numbers)
        {
            #region # 验证

            string[] numbers_ = numbers?.Distinct().ToArray() ?? Array.Empty<string>();
            if (!numbers_.Any())
            {
                throw new ArgumentNullException(nameof(numbers), $"要删除的{typeof(T).Name}的编号集合不可为空！");
            }

            #endregion

            this._collection.DeleteManyAsync(x => numbers_.Contains(x.Number)).Wait();
        }
        #endregion

        #region # 删除全部 —— void RemoveAll()
        /// <summary>
        /// 删除全部
        /// </summary>
        public void RemoveAll()
        {
            this._collection.DeleteManyAsync(x => true).Wait();
        }
        #endregion

        #endregion

        #region # 查询部分

        //Single部分

        #region # 根据Id获取唯一实体对象 —— T SingleOrDefault(Guid id)
        /// <summary>
        /// 根据Id获取唯一实体对象
        /// </summary>
        /// <param name="id">标识Id</param>
        /// <returns>实体对象</returns>
        public T SingleOrDefault(Guid id)
        {
            return this.Find(x => x.Id == id).SingleOrDefault();
        }
        #endregion

        #region # 根据Id获取唯一实体对象 —— Task<T> SingleOrDefaultAsync(Guid id)
        /// <summary>
        /// 根据Id获取唯一实体对象
        /// </summary>
        /// <param name="id">标识Id</param>
        /// <returns>实体对象</returns>
        public async Task<T> SingleOrDefaultAsync(Guid id)
        {
            return await this.Find(x => x.Id == id).SingleOrDefaultAsync();
        }
        #endregion

        #region # 根据Id获取唯一子类对象 —— TSub SingleOrDefault<TSub>(Guid id)
        /// <summary>
        /// 根据Id获取唯一子类对象
        /// </summary>
        /// <param name="id">标识Id</param>
        /// <returns>子类对象</returns>
        public TSub SingleOrDefault<TSub>(Guid id) where TSub : T
        {
            return this.Find<TSub>(x => x.Id == id).SingleOrDefault();
        }
        #endregion

        #region # 根据Id获取唯一子类对象 —— Task<TSub> SingleOrDefaultAsync<TSub>(Guid id)
        /// <summary>
        /// 根据Id获取唯一子类对象
        /// </summary>
        /// <param name="id">标识Id</param>
        /// <returns>子类对象</returns>
        public async Task<TSub> SingleOrDefaultAsync<TSub>(Guid id) where TSub : T
        {
            return await this.Find<TSub>(x => x.Id == id).SingleOrDefaultAsync();
        }
        #endregion

        #region # 根据Id获取唯一实体对象 —— T Single(Guid id)
        /// <summary>
        /// 根据Id获取唯一实体对象
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

        #region # 根据Id获取唯一实体对象 —— Task<T> SingleAsync(Guid id)
        /// <summary>
        /// 根据Id获取唯一实体对象
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

        #region # 根据Id获取唯一子类对象 —— TSub Single<TSub>(Guid id)
        /// <summary>
        /// 根据Id获取唯一子类对象
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

        #region # 根据Id获取唯一子类对象 —— Task<TSub> SingleAsync<TSub>(Guid id)
        /// <summary>
        /// 根据Id获取唯一子类对象
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

        #region # 根据编号获取唯一实体对象 —— T SingleOrDefault(string number)
        /// <summary>
        /// 根据编号获取唯一实体对象
        /// </summary>
        /// <param name="number">编号</param>
        /// <returns>实体对象</returns>
        public T SingleOrDefault(string number)
        {
            #region # 验证

            if (string.IsNullOrWhiteSpace(number))
            {
                throw new ArgumentNullException(nameof(number), $"{typeof(T).Name}的编号不可为空！");
            }

            #endregion

            return this.Find(x => x.Number == number).SingleOrDefault();
        }
        #endregion

        #region # 根据编号获取唯一实体对象 —— Task<T> SingleOrDefaultAsync(string number)
        /// <summary>
        /// 根据编号获取唯一实体对象
        /// </summary>
        /// <param name="number">编号</param>
        /// <returns>实体对象</returns>
        public async Task<T> SingleOrDefaultAsync(string number)
        {
            #region # 验证

            if (string.IsNullOrWhiteSpace(number))
            {
                throw new ArgumentNullException(nameof(number), $"{typeof(T).Name}的编号不可为空！");
            }

            #endregion

            return await this.Find(x => x.Number == number).SingleOrDefaultAsync();
        }
        #endregion

        #region # 根据编号获取唯一子类对象 —— TSub SingleOrDefault<TSub>(string number)
        /// <summary>
        /// 根据编号获取唯一子类对象
        /// </summary>
        /// <param name="number">编号</param>
        /// <returns>子类对象</returns>
        public TSub SingleOrDefault<TSub>(string number) where TSub : T
        {
            #region # 验证

            if (string.IsNullOrWhiteSpace(number))
            {
                throw new ArgumentNullException(nameof(number), $"{typeof(TSub).Name}的编号不可为空！");
            }

            #endregion

            return this.Find<TSub>(x => x.Number == number).SingleOrDefault();
        }
        #endregion

        #region # 根据编号获取唯一子类对象 —— Task<TSub> SingleOrDefaultAsync<TSub>(string number)
        /// <summary>
        /// 根据编号获取唯一子类对象
        /// </summary>
        /// <param name="number">编号</param>
        /// <returns>子类对象</returns>
        public async Task<TSub> SingleOrDefaultAsync<TSub>(string number) where TSub : T
        {
            #region # 验证

            if (string.IsNullOrWhiteSpace(number))
            {
                throw new ArgumentNullException(nameof(number), $"{typeof(TSub).Name}的编号不可为空！");
            }

            #endregion

            return await this.Find<TSub>(x => x.Number == number).SingleOrDefaultAsync();
        }
        #endregion

        #region # 根据编号获取唯一实体对象 —— T Single(string number)
        /// <summary>
        /// 根据编号获取唯一实体对象
        /// </summary>
        /// <param name="number">编号</param>
        /// <returns>实体对象</returns>
        public T Single(string number)
        {
            T current = this.SingleOrDefault(number);

            #region # 验证

            if (current == null)
            {
                throw new NullReferenceException($"编号为\"{number}\"的{typeof(T).Name}实体不存在！");
            }

            #endregion

            return current;
        }
        #endregion

        #region # 根据编号获取唯一实体对象 —— Task<T> SingleAsync(string number)
        /// <summary>
        /// 根据编号获取唯一实体对象
        /// </summary>
        /// <param name="number">编号</param>
        /// <returns>实体对象</returns>
        public async Task<T> SingleAsync(string number)
        {
            T current = await this.SingleOrDefaultAsync(number);

            #region # 验证

            if (current == null)
            {
                throw new NullReferenceException($"编号为\"{number}\"的{typeof(T).Name}实体不存在！");
            }

            #endregion

            return current;
        }
        #endregion

        #region # 根据编号获取唯一子类对象 —— TSub Single<TSub>(string number)
        /// <summary>
        /// 根据编号获取唯一子类对象
        /// </summary>
        /// <param name="number">编号</param>
        /// <returns>子类对象</returns>
        public TSub Single<TSub>(string number) where TSub : T
        {
            TSub current = this.SingleOrDefault<TSub>(number);

            #region # 验证

            if (current == null)
            {
                throw new NullReferenceException($"编号为\"{number}\"的{typeof(TSub).Name}实体不存在！");
            }

            #endregion

            return current;
        }
        #endregion

        #region # 根据编号获取唯一子类对象 —— Task<TSub> SingleAsync<TSub>(string number)
        /// <summary>
        /// 根据编号获取唯一子类对象
        /// </summary>
        /// <param name="number">编号</param>
        /// <returns>子类对象</returns>
        public async Task<TSub> SingleAsync<TSub>(string number) where TSub : T
        {
            TSub current = await this.SingleOrDefaultAsync<TSub>(number);

            #region # 验证

            if (current == null)
            {
                throw new NullReferenceException($"编号为\"{number}\"的{typeof(TSub).Name}实体不存在！");
            }

            #endregion

            return current;
        }
        #endregion

        #region # 根据行号获取唯一实体对象 —— TRowable SingleOrDefault(long rowNo)
        /// <summary>
        /// 根据行号获取唯一实体对象
        /// </summary>
        /// <param name="rowNo">行号</param>
        /// <returns>实体对象</returns>
        public TRowable SingleOrDefault<TRowable>(long rowNo) where TRowable : T, IRowable
        {
            return this.Find<TRowable>(x => x.RowNo == rowNo).SingleOrDefault();
        }
        #endregion

        #region # 根据行号获取唯一实体对象 —— Task<TRowable> SingleOrDefaultAsync(long rowNo)
        /// <summary>
        /// 根据行号获取唯一实体对象
        /// </summary>
        /// <param name="rowNo">行号</param>
        /// <returns>实体对象</returns>
        public async Task<TRowable> SingleOrDefaultAsync<TRowable>(long rowNo) where TRowable : T, IRowable
        {
            return await this.Find<TRowable>(x => x.RowNo == rowNo).SingleOrDefaultAsync();
        }
        #endregion

        #region # 根据行号获取唯一实体对象 —— TRowable Single(long rowNo)
        /// <summary>
        /// 根据行号获取唯一实体对象
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

        #region # 根据行号获取唯一实体对象 —— Task<TRowable> SingleAsync(long rowNo)
        /// <summary>
        /// 根据行号获取唯一实体对象
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
            return this.FindAndSort(x => true).FirstOrDefault();
        }
        #endregion

        #region # 获取默认或第一个实体对象 —— Task<T> FirstOrDefaultAsync()
        /// <summary>
        /// 获取默认或第一个实体对象
        /// </summary>
        /// <returns>实体对象</returns>
        public virtual async Task<T> FirstOrDefaultAsync()
        {
            return await this.FindAndSort(x => true).FirstOrDefaultAsync();
        }
        #endregion

        #region # 获取默认或第一个子类对象 —— TSub FirstOrDefault<TSub>()
        /// <summary>
        /// 获取默认或第一个子类对象
        /// </summary>
        /// <returns>子类对象</returns>
        public virtual TSub FirstOrDefault<TSub>() where TSub : T
        {
            return this.FindAndSort<TSub>(x => true).FirstOrDefault();
        }
        #endregion

        #region # 获取默认或第一个子类对象 —— Task<TSub> FirstOrDefaultAsync<TSub>()
        /// <summary>
        /// 获取默认或第一个子类对象
        /// </summary>
        /// <returns>子类对象</returns>
        public async Task<TSub> FirstOrDefaultAsync<TSub>() where TSub : T
        {
            return await this.FindAndSort<TSub>(x => true).FirstOrDefaultAsync();
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
            return this.Find(x => true).ToList();
        }
        #endregion

        #region # 获取实体对象列表 —— Task<ICollection<T>> FindAllAsync()
        /// <summary>
        /// 获取实体对象列表
        /// </summary>
        /// <returns>实体对象列表</returns>
        public async Task<ICollection<T>> FindAllAsync()
        {
            return await this.Find(x => true).ToListAsync();
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
            return this.Find<TSub>(x => true).ToList();
        }
        #endregion

        #region # 获取给定类型子类对象列表 —— Task<ICollection<TSub>> FindAllAsync<TSub>()
        /// <summary>
        /// 获取给定类型子类对象列表
        /// </summary>
        /// <typeparam name="TSub">子类类型</typeparam>
        /// <returns>子类对象列表</returns>
        public async Task<ICollection<TSub>> FindAllAsync<TSub>() where TSub : T
        {
            return await this.Find<TSub>(x => true).ToListAsync();
        }
        #endregion

        #region # 根据关键字获取实体对象列表 —— ICollection<T> Find(string keywords)
        /// <summary>
        /// 根据关键字获取实体对象列表
        /// </summary>
        /// <returns>实体对象列表</returns>
        public ICollection<T> Find(string keywords)
        {
            Expression<Func<T, bool>> condition =
                x =>
                    (string.IsNullOrEmpty(keywords) || x.Keywords.Contains(keywords));

            return this.Find(condition).ToList();
        }
        #endregion

        #region # 根据关键字获取实体对象列表 —— Task<ICollection<T>> FindAsync(string keywords)
        /// <summary>
        /// 根据关键字获取实体对象列表
        /// </summary>
        /// <returns>实体对象列表</returns>
        public async Task<ICollection<T>> FindAsync(string keywords)
        {
            Expression<Func<T, bool>> condition =
                x =>
                    (string.IsNullOrEmpty(keywords) || x.Keywords.Contains(keywords));

            return await this.Find(condition).ToListAsync();
        }
        #endregion

        #region # 根据关键字获取子类对象列表 —— ICollection<TSub> Find<TSub>(string keywords)
        /// <summary>
        /// 根据关键字获取子类对象列表
        /// </summary>
        /// <typeparam name="TSub">子类类型</typeparam>
        /// <returns>子类对象列表</returns>
        public ICollection<TSub> Find<TSub>(string keywords) where TSub : T
        {
            Expression<Func<TSub, bool>> condition =
                x =>
                    (string.IsNullOrEmpty(keywords) || x.Keywords.Contains(keywords));

            return this.Find<TSub>(condition).ToList();
        }
        #endregion

        #region # 根据关键字获取子类对象列表 —— Task<ICollection<TSub>> FindAsync<TSub>(string keywords)
        /// <summary>
        /// 根据关键字获取子类对象列表
        /// </summary>
        /// <typeparam name="TSub">子类类型</typeparam>
        /// <returns>子类对象列表</returns>
        public async Task<ICollection<TSub>> FindAsync<TSub>(string keywords) where TSub : T
        {
            Expression<Func<TSub, bool>> condition =
                x =>
                    (string.IsNullOrEmpty(keywords) || x.Keywords.Contains(keywords));

            return await this.Find<TSub>(condition).ToListAsync();
        }
        #endregion

        #region # 根据关键字分页获取实体对象列表 —— ICollection<T> FindByPage(string keywords...
        /// <summary>
        /// 根据关键字分页获取实体对象列表
        /// </summary>
        /// <param name="keywords">关键字</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页容量</param>
        /// <param name="rowCount">总记录条数</param>
        /// <param name="pageCount">总页数</param>
        /// <returns>实体对象列表</returns>
        public ICollection<T> FindByPage(string keywords, int pageIndex, int pageSize, out int rowCount, out int pageCount)
        {
            Expression<Func<T, bool>> condition =
                x =>
                    (string.IsNullOrEmpty(keywords) || x.Keywords.Contains(keywords));

            return this.FindByPage(condition, pageIndex, pageSize, out rowCount, out pageCount).ToList();
        }
        #endregion

        #region # 根据关键字分页获取实体对象列表 —— Task<ICollection<T>> FindByPageAsync(string keywords...
        /// <summary>
        /// 根据关键字分页获取实体对象列表
        /// </summary>
        /// <param name="keywords">关键字</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页容量</param>
        /// <param name="rowCount">总记录条数</param>
        /// <param name="pageCount">总页数</param>
        /// <returns>实体对象列表</returns>
        public Task<ICollection<T>> FindByPageAsync(string keywords, int pageIndex, int pageSize, out int rowCount, out int pageCount)
        {
            Expression<Func<T, bool>> condition =
                x =>
                    (string.IsNullOrEmpty(keywords) || x.Keywords.Contains(keywords));

            Task<List<T>> task = this.FindByPage(condition, pageIndex, pageSize, out rowCount, out pageCount).ToListAsync();

            return new Task<ICollection<T>>(() => task.Result);
        }
        #endregion

        #region # 根据关键字分页获取子类对象列表 —— ICollection<TSub> FindByPage<TSub>(string keywords...
        /// <summary>
        /// 根据关键字分页获取子类对象列表
        /// </summary>
        /// <typeparam name="TSub">子类类型</typeparam>
        /// <param name="keywords">关键字</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页容量</param>
        /// <param name="rowCount">总记录条数</param>
        /// <param name="pageCount">总页数</param>
        /// <returns>子类对象列表</returns>
        public ICollection<TSub> FindByPage<TSub>(string keywords, int pageIndex, int pageSize, out int rowCount,
            out int pageCount) where TSub : T
        {
            Expression<Func<TSub, bool>> condition =
                x =>
                    (string.IsNullOrEmpty(keywords) || x.Keywords.Contains(keywords));

            return this.FindByPage<TSub>(condition, pageIndex, pageSize, out rowCount, out pageCount).ToList();
        }
        #endregion

        #region # 根据关键字分页获取子类对象列表 —— Task<ICollection<TSub>> FindByPageAsync<TSub>(string keywords...
        /// <summary>
        /// 根据关键字分页获取子类对象列表
        /// </summary>
        /// <typeparam name="TSub">子类类型</typeparam>
        /// <param name="keywords">关键字</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页容量</param>
        /// <param name="rowCount">总记录条数</param>
        /// <param name="pageCount">总页数</param>
        /// <returns>子类对象列表</returns>
        public Task<ICollection<TSub>> FindByPageAsync<TSub>(string keywords, int pageIndex, int pageSize, out int rowCount, out int pageCount) where TSub : T
        {
            Expression<Func<TSub, bool>> condition =
                x =>
                    (string.IsNullOrEmpty(keywords) || x.Keywords.Contains(keywords));

            Task<List<TSub>> task = this.FindByPage<TSub>(condition, pageIndex, pageSize, out rowCount, out pageCount).ToListAsync();

            return new Task<ICollection<TSub>>(() => task.Result);
        }
        #endregion


        //IDictionary部分

        #region # 根据Id集获取实体对象字典 —— IDictionary<Guid, T> Find(IEnumerable<Guid> ids)
        /// <summary>
        /// 根据Id集获取实体对象字典
        /// </summary>
        /// <param name="ids">标识Id集</param>
        /// <returns>实体对象字典</returns>
        /// <remarks>IDictionary[Guid, T]，[Id, 实体对象]</remarks>
        public IDictionary<Guid, T> Find(IEnumerable<Guid> ids)
        {
            #region # 验证

            Guid[] ids_ = ids?.Distinct().ToArray() ?? Array.Empty<Guid>();
            if (!ids_.Any())
            {
                return new Dictionary<Guid, T>();
            }

            #endregion

            IList<T> entities = this.Find(x => ids_.Contains(x.Id)).ToList();

            return entities.ToDictionary(x => x.Id, x => x);
        }
        #endregion

        #region # 根据Id集获取实体对象字典 —— Task<IDictionary<Guid, T>> FindAsync(IEnumerable<Guid> ids)
        /// <summary>
        /// 根据Id集获取实体对象字典
        /// </summary>
        /// <param name="ids">标识Id集</param>
        /// <returns>实体对象字典</returns>
        /// <remarks>IDictionary[Guid, T]，[Id, 实体对象]</remarks>
        public async Task<IDictionary<Guid, T>> FindAsync(IEnumerable<Guid> ids)
        {
            #region # 验证

            Guid[] ids_ = ids?.Distinct().ToArray() ?? Array.Empty<Guid>();
            if (!ids_.Any())
            {
                return new Dictionary<Guid, T>();
            }

            #endregion

            IList<T> entities = await this.Find(x => ids_.Contains(x.Id)).ToListAsync();

            return entities.ToDictionary(x => x.Id, x => x);
        }
        #endregion

        #region # 根据Id集获取子类对象字典 —— IDictionary<Guid, TSub> Find<TSub>(IEnumerable<Guid> ids)
        /// <summary>
        /// 根据Id集获取子类对象字典
        /// </summary>
        /// <param name="ids">标识Id集</param>
        /// <returns>子类对象字典</returns>
        /// <remarks>IDictionary[Guid, TSub]，[Id, 子类对象]</remarks>
        public IDictionary<Guid, TSub> Find<TSub>(IEnumerable<Guid> ids) where TSub : T
        {
            #region # 验证

            Guid[] ids_ = ids?.Distinct().ToArray() ?? Array.Empty<Guid>();
            if (!ids_.Any())
            {
                return new Dictionary<Guid, TSub>();
            }

            #endregion

            IList<TSub> entities = this.Find<TSub>(x => ids_.Contains(x.Id)).ToList();

            return entities.ToDictionary(x => x.Id, x => x);
        }
        #endregion

        #region # 根据Id集获取子类对象字典 —— Task<IDictionary<Guid, TSub>> FindAsync<TSub>(IEnumerable<Guid> ids)
        /// <summary>
        /// 根据Id集获取子类对象字典
        /// </summary>
        /// <param name="ids">标识Id集</param>
        /// <returns>子类对象字典</returns>
        /// <remarks>IDictionary[Guid, TSub]，[Id, 子类对象]</remarks>
        public async Task<IDictionary<Guid, TSub>> FindAsync<TSub>(IEnumerable<Guid> ids) where TSub : T
        {
            #region # 验证

            Guid[] ids_ = ids?.Distinct().ToArray() ?? Array.Empty<Guid>();
            if (!ids_.Any())
            {
                return new Dictionary<Guid, TSub>();
            }

            #endregion

            IList<TSub> entities = await this.Find<TSub>(x => ids_.Contains(x.Id)).ToListAsync();

            return entities.ToDictionary(x => x.Id, x => x);
        }
        #endregion

        #region # 根据编号集获取实体对象字典 —— IDictionary<string, T> Find(IEnumerable<string> numbers)
        /// <summary>
        /// 根据编号集获取实体对象字典
        /// </summary>
        /// <returns>实体对象字典</returns>
        /// <remarks>IDictionary[string, T]，[编号, 实体对象]</remarks>
        public IDictionary<string, T> Find(IEnumerable<string> numbers)
        {
            #region # 验证

            string[] numbers_ = numbers?.Distinct().ToArray() ?? Array.Empty<string>();
            if (!numbers_.Any())
            {
                return new Dictionary<string, T>();
            }

            #endregion

            IList<T> entities = this.Find<T>(x => numbers_.Contains(x.Number)).ToList();

            return entities.ToDictionary(x => x.Number, x => x);
        }
        #endregion

        #region # 根据编号集获取实体对象字典 —— Task<IDictionary<string, T>> FindAsync(IEnumerable<string> numbers)
        /// <summary>
        /// 根据编号集获取实体对象字典
        /// </summary>
        /// <returns>实体对象字典</returns>
        /// <remarks>IDictionary[string, T]，[编号, 实体对象]</remarks>
        public async Task<IDictionary<string, T>> FindAsync(IEnumerable<string> numbers)
        {
            #region # 验证

            string[] numbers_ = numbers?.Distinct().ToArray() ?? Array.Empty<string>();
            if (!numbers_.Any())
            {
                return new Dictionary<string, T>();
            }

            #endregion

            IList<T> entities = await this.Find<T>(x => numbers_.Contains(x.Number)).ToListAsync();

            return entities.ToDictionary(x => x.Number, x => x);
        }
        #endregion

        #region # 根据编号集获取子类对象字典 —— IDictionary<string, TSub> Find<TSub>(IEnumerable<string>...
        /// <summary>
        /// 根据编号集获取子类对象字典
        /// </summary>
        /// <returns>子类对象字典</returns>
        /// <remarks>IDictionary[string, TSub]，[编号, 子类对象]</remarks>
        public IDictionary<string, TSub> Find<TSub>(IEnumerable<string> numbers) where TSub : T
        {
            #region # 验证

            string[] numbers_ = numbers?.Distinct().ToArray() ?? Array.Empty<string>();
            if (!numbers_.Any())
            {
                return new Dictionary<string, TSub>();
            }

            #endregion

            IList<TSub> entities = this.Find<TSub>(x => numbers_.Contains(x.Number)).ToList();

            return entities.ToDictionary(x => x.Number, x => x);
        }
        #endregion

        #region # 根据编号集获取子类对象字典 —— Task<IDictionary<string, TSub>> FindAsync<TSub>(IEnumerable<string...
        /// <summary>
        /// 根据编号集获取子类对象字典
        /// </summary>
        /// <returns>子类对象字典</returns>
        /// <remarks>IDictionary[string, TSub]，[编号, 子类对象]</remarks>
        public async Task<IDictionary<string, TSub>> FindAsync<TSub>(IEnumerable<string> numbers) where TSub : T
        {
            #region # 验证

            string[] numbers_ = numbers?.Distinct().ToArray() ?? Array.Empty<string>();
            if (!numbers_.Any())
            {
                return new Dictionary<string, TSub>();
            }

            #endregion

            IList<TSub> entities = await this.Find<TSub>(x => numbers_.Contains(x.Number)).ToListAsync();

            return entities.ToDictionary(x => x.Number, x => x);
        }
        #endregion

        #region # 根据行号集获取实体对象字典 —— IDictionary<long, TRowable> Find<TRowable>(IEnumerable<long> rowNos)
        /// <summary>
        /// 根据行号集获取实体对象字典
        /// </summary>
        /// <param name="rowNos">行号集</param>
        /// <returns>实体对象字典</returns>
        /// <remarks>IDictionary[long, TRowable]，[行号, 实体对象]</remarks>
        public IDictionary<long, TRowable> Find<TRowable>(IEnumerable<long> rowNos) where TRowable : T, IRowable
        {
            #region # 验证

            long[] rowNos_ = rowNos?.Distinct().ToArray() ?? Array.Empty<long>();
            if (!rowNos_.Any())
            {
                return new Dictionary<long, TRowable>();
            }

            #endregion

            IList<TRowable> entities = this.Find<TRowable>(x => rowNos_.Contains(x.RowNo)).ToList();

            return entities.ToDictionary(x => x.RowNo, x => x);
        }
        #endregion

        #region # 根据行号集获取实体对象字典 —— Task<IDictionary<long, TRowable>> FindAsync<TRowable>(IEnumerable<long> rowNos)
        /// <summary>
        /// 根据行号集获取实体对象字典
        /// </summary>
        /// <param name="rowNos">行号集</param>
        /// <returns>实体对象字典</returns>
        /// <remarks>IDictionary[long, TRowable]，[行号, 实体对象]</remarks>
        public async Task<IDictionary<long, TRowable>> FindAsync<TRowable>(IEnumerable<long> rowNos) where TRowable : T, IRowable
        {
            #region # 验证

            long[] rowNos_ = rowNos?.Distinct().ToArray() ?? Array.Empty<long>();
            if (!rowNos_.Any())
            {
                return new Dictionary<long, TRowable>();
            }

            #endregion

            IList<TRowable> entities = await this.Find<TRowable>(x => rowNos_.Contains(x.RowNo)).ToListAsync();

            return entities.ToDictionary(x => x.RowNo, x => x);
        }
        #endregion


        //Count部分

        #region # 获取总记录条数 —— long Count()
        /// <summary>
        /// 获取总记录条数
        /// </summary>
        /// <returns>总记录条数</returns>
        public long Count()
        {
            long count = this.Find(x => true).CountDocuments();

            return count;
        }
        #endregion

        #region # 获取总记录条数 —— Task<long> CountAsync()
        /// <summary>
        /// 获取总记录条数
        /// </summary>
        /// <returns>总记录条数</returns>
        public async Task<long> CountAsync()
        {
            long count = await this.Find(x => true).CountDocumentsAsync();

            return count;
        }
        #endregion

        #region # 获取子类记录条数 —— long Count<TSub>()
        /// <summary>
        /// 获取子类记录条数
        /// </summary>
        /// <returns>子类记录条数</returns>
        public long Count<TSub>() where TSub : T
        {
            long count = this.Find<TSub>(x => true).CountDocuments();

            return count;
        }
        #endregion

        #region # 获取子类记录条数 —— Task<long> CountAsync<TSub>()
        /// <summary>
        /// 获取子类记录条数
        /// </summary>
        /// <returns>子类记录条数</returns>
        public async Task<long> CountAsync<TSub>() where TSub : T
        {
            long count = await this.Find<TSub>(x => true).CountDocumentsAsync();

            return count;
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
            return this.Find(x => x.Id == id).Any();
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
            return await this.Find(x => x.Id == id).AnyAsync();
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
            return this.Find<TSub>(x => x.Id == id).Any();
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
            return await this.Find<TSub>(x => x.Id == id).AnyAsync();
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
            return this.Find<TRowable>(x => x.RowNo == rowNo).Any();
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
            return await this.Find<TRowable>(x => x.RowNo == rowNo).AnyAsync();
        }
        #endregion

        #region # 是否存在给定编号的实体对象 —— bool ExistsNo(string number)
        /// <summary>
        /// 是否存在给定编号的实体对象
        /// </summary>
        /// <param name="number">编号</param>
        /// <returns>是否存在</returns>
        public bool ExistsNo(string number)
        {
            #region # 验证

            if (string.IsNullOrWhiteSpace(number))
            {
                throw new ArgumentNullException(nameof(number), $"{typeof(T).Name}的编号不可为空！");
            }

            #endregion

            return this.Find(x => x.Number == number).Any();
        }
        #endregion

        #region # 是否存在给定编号的实体对象 —— Task<bool> ExistsNoAsync(string number)
        /// <summary>
        /// 是否存在给定编号的实体对象
        /// </summary>
        /// <param name="number">编号</param>
        /// <returns>是否存在</returns>
        public async Task<bool> ExistsNoAsync(string number)
        {
            #region # 验证

            if (string.IsNullOrWhiteSpace(number))
            {
                throw new ArgumentNullException(nameof(number), $"{typeof(T).Name}的编号不可为空！");
            }

            #endregion

            return await this.Find(x => x.Number == number).AnyAsync();
        }
        #endregion

        #region # 是否存在给定编号的子类对象 —— bool ExistsNo<TSub>(string number)
        /// <summary>
        /// 是否存在给定编号的子类对象
        /// </summary>
        /// <param name="number">编号</param>
        /// <returns>是否存在</returns>
        public bool ExistsNo<TSub>(string number) where TSub : T
        {
            #region # 验证

            if (string.IsNullOrWhiteSpace(number))
            {
                throw new ArgumentNullException(nameof(number), $"{typeof(T).Name}的编号不可为空！");
            }

            #endregion

            return this.Find<TSub>(x => x.Number == number).Any();
        }
        #endregion

        #region # 是否存在给定编号的子类对象 —— Task<bool> ExistsNoAsync<TSub>(string number)
        /// <summary>
        /// 是否存在给定编号的子类对象
        /// </summary>
        /// <param name="number">编号</param>
        /// <returns>是否存在</returns>
        public async Task<bool> ExistsNoAsync<TSub>(string number) where TSub : T
        {
            #region # 验证

            if (string.IsNullOrWhiteSpace(number))
            {
                throw new ArgumentNullException(nameof(number), $"{typeof(T).Name}的编号不可为空！");
            }

            #endregion

            return await this.Find<TSub>(x => x.Number == number).AnyAsync();
        }
        #endregion

        #region # 是否存在给定编号的实体对象 —— bool ExistsNo(Guid? id, string number)
        /// <summary>
        /// 是否存在给定编号的实体对象
        /// </summary>
        /// <param name="id">标识id</param>
        /// <param name="number">编号</param>
        /// <returns>是否存在</returns>
        public bool ExistsNo(Guid? id, string number)
        {
            if (id.HasValue)
            {
                T current = this.SingleOrDefault(id.Value);
                if (current != null && current.Number == number)
                {
                    return false;
                }

                return this.ExistsNo(number);
            }

            return this.ExistsNo(number);
        }
        #endregion

        #region # 是否存在给定编号的实体对象 —— Task<bool> ExistsNoAsync(Guid? id, string number)
        /// <summary>
        /// 是否存在给定编号的实体对象
        /// </summary>
        /// <param name="id">标识id</param>
        /// <param name="number">编号</param>
        /// <returns>是否存在</returns>
        public async Task<bool> ExistsNoAsync(Guid? id, string number)
        {
            if (id.HasValue)
            {
                T current = await this.SingleOrDefaultAsync(id.Value);
                if (current != null && current.Number == number)
                {
                    return false;
                }

                return await this.ExistsNoAsync(number);
            }

            return await this.ExistsNoAsync(number);
        }
        #endregion

        #region # 是否存在给定编号的子类对象 —— bool ExistsNo<TSub>(Guid? id, string number)
        /// <summary>
        /// 是否存在给定编号的子类对象
        /// </summary>
        /// <param name="id">标识id</param>
        /// <param name="number">编号</param>
        /// <returns>是否存在</returns>
        public bool ExistsNo<TSub>(Guid? id, string number) where TSub : T
        {
            if (id.HasValue)
            {
                TSub current = this.SingleOrDefault<TSub>(id.Value);
                if (current != null && current.Number == number)
                {
                    return false;
                }

                return this.ExistsNo<TSub>(number);
            }

            return this.ExistsNo<TSub>(number);
        }
        #endregion

        #region # 是否存在给定编号的子类对象 —— Task<bool> ExistsNoAsync<TSub>(Guid? id, string number)
        /// <summary>
        /// 是否存在给定编号的子类对象
        /// </summary>
        /// <param name="id">标识id</param>
        /// <param name="number">编号</param>
        /// <returns>是否存在</returns>
        public async Task<bool> ExistsNoAsync<TSub>(Guid? id, string number) where TSub : T
        {
            if (id.HasValue)
            {
                TSub current = await this.SingleOrDefaultAsync<TSub>(id.Value);
                if (current != null && current.Number == number)
                {
                    return false;
                }

                return await this.ExistsNoAsync<TSub>(number);
            }

            return await this.ExistsNoAsync<TSub>(number);
        }
        #endregion

        #region # 是否存在给定名称的实体对象 —— bool ExistsName(string name)
        /// <summary>
        /// 是否存在给定名称的实体对象
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>是否存在</returns>
        public bool ExistsName(string name)
        {
            #region # 验证

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name), $"{typeof(T).Name}的名称不可为空！");
            }

            #endregion

            return this.Find(x => x.Name == name).Any();
        }
        #endregion

        #region # 是否存在给定名称的实体对象 —— Task<bool> ExistsNameAsync(string name)
        /// <summary>
        /// 是否存在给定名称的实体对象
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>是否存在</returns>
        public async Task<bool> ExistsNameAsync(string name)
        {
            #region # 验证

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name), $"{typeof(T).Name}的名称不可为空！");
            }

            #endregion

            return await this.Find(x => x.Name == name).AnyAsync();
        }
        #endregion

        #region # 是否存在给定名称的子类对象 —— bool ExistsName<TSub>(string name)
        /// <summary>
        /// 是否存在给定名称的子类对象
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>是否存在</returns>
        public bool ExistsName<TSub>(string name) where TSub : T
        {
            #region # 验证

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name), $"{typeof(T).Name}的名称不可为空！");
            }

            #endregion

            return this.Find<TSub>(x => x.Name == name).Any();
        }
        #endregion

        #region # 是否存在给定名称的子类对象 —— Task<bool> ExistsNameAsync<TSub>(string name)
        /// <summary>
        /// 是否存在给定名称的子类对象
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>是否存在</returns>
        public async Task<bool> ExistsNameAsync<TSub>(string name) where TSub : T
        {
            #region # 验证

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name), $"{typeof(T).Name}的名称不可为空！");
            }

            #endregion

            return await this.Find<TSub>(x => x.Name == name).AnyAsync();
        }
        #endregion

        #region # 是否存在给定名称的实体对象 —— bool ExistsName(Guid? id, string name)
        /// <summary>
        /// 是否存在给定名称的实体对象
        /// </summary>
        /// <param name="id">标识id</param>
        /// <param name="name">名称</param>
        /// <returns>是否存在</returns>
        public bool ExistsName(Guid? id, string name)
        {
            if (id.HasValue)
            {
                T current = this.SingleOrDefault(id.Value);
                if (current != null && current.Name == name)
                {
                    return false;
                }

                return this.ExistsName(name);
            }

            return this.ExistsName(name);
        }
        #endregion

        #region # 是否存在给定名称的实体对象 —— Task<bool> ExistsNameAsync(Guid? id, string name)
        /// <summary>
        /// 是否存在给定名称的实体对象
        /// </summary>
        /// <param name="id">标识id</param>
        /// <param name="name">名称</param>
        /// <returns>是否存在</returns>
        public async Task<bool> ExistsNameAsync(Guid? id, string name)
        {
            if (id.HasValue)
            {
                T current = await this.SingleOrDefaultAsync(id.Value);
                if (current != null && current.Name == name)
                {
                    return false;
                }

                return await this.ExistsNameAsync(name);
            }

            return await this.ExistsNameAsync(name);
        }
        #endregion

        #region # 是否存在给定名称的子类对象 —— bool ExistsName<TSub>(Guid? id, string name)
        /// <summary>
        /// 是否存在给定名称的子类对象
        /// </summary>
        /// <param name="id">标识id</param>
        /// <param name="name">名称</param>
        /// <returns>是否存在</returns>
        public bool ExistsName<TSub>(Guid? id, string name) where TSub : T
        {
            if (id.HasValue)
            {
                TSub current = this.SingleOrDefault<TSub>(id.Value);
                if (current != null && current.Name == name)
                {
                    return false;
                }

                return this.ExistsName<TSub>(name);
            }

            return this.ExistsName<TSub>(name);
        }
        #endregion

        #region # 是否存在给定名称的子类对象 —— Task<bool> ExistsNameAsync<TSub>(Guid? id, string name)
        /// <summary>
        /// 是否存在给定名称的子类对象
        /// </summary>
        /// <param name="id">标识id</param>
        /// <param name="name">名称</param>
        /// <returns>是否存在</returns>
        public async Task<bool> ExistsNameAsync<TSub>(Guid? id, string name) where TSub : T
        {
            if (id.HasValue)
            {
                TSub current = await this.SingleOrDefaultAsync<TSub>(id.Value);
                if (current != null && current.Name == name)
                {
                    return false;
                }

                return await this.ExistsNameAsync<TSub>(name);
            }

            return await this.ExistsNameAsync<TSub>(name);
        }
        #endregion

        #region # 是否存在给定名称的实体对象 —— bool ExistsName(string number, string name)
        /// <summary>
        /// 是否存在给定名称的实体对象
        /// </summary>
        /// <param name="number">编号</param>
        /// <param name="name">名称</param>
        /// <returns>是否存在</returns>
        public bool ExistsName(string number, string name)
        {
            if (!string.IsNullOrWhiteSpace(number))
            {
                T current = this.SingleOrDefault(number);

                if (current != null && current.Name == name)
                {
                    return false;
                }

                return this.ExistsName(name);
            }

            return this.ExistsName(name);
        }
        #endregion

        #region # 是否存在给定名称的实体对象 —— Task<bool> ExistsNameAsync(string number, string name)
        /// <summary>
        /// 是否存在给定名称的实体对象
        /// </summary>
        /// <param name="number">编号</param>
        /// <param name="name">名称</param>
        /// <returns>是否存在</returns>
        public async Task<bool> ExistsNameAsync(string number, string name)
        {
            if (!string.IsNullOrWhiteSpace(number))
            {
                T current = await this.SingleOrDefaultAsync(number);
                if (current != null && current.Name == name)
                {
                    return false;
                }

                return await this.ExistsNameAsync(name);
            }

            return await this.ExistsNameAsync(name);
        }
        #endregion

        #region # 是否存在给定名称的子类对象 —— bool ExistsName<TSub>(string number, string name)
        /// <summary>
        /// 是否存在给定名称的子类对象
        /// </summary>
        /// <param name="number">编号</param>
        /// <param name="name">名称</param>
        /// <returns>是否存在</returns>
        public bool ExistsName<TSub>(string number, string name) where TSub : T
        {
            if (!string.IsNullOrWhiteSpace(number))
            {
                TSub current = this.SingleOrDefault<TSub>(number);

                if (current != null && current.Name == name)
                {
                    return false;
                }

                return this.ExistsName<TSub>(name);
            }

            return this.ExistsName<TSub>(name);
        }
        #endregion

        #region # 是否存在给定名称的子类对象 —— Task<bool> ExistsNameAsync<TSub>(string number, string name)
        /// <summary>
        /// 是否存在给定名称的子类对象
        /// </summary>
        /// <param name="number">编号</param>
        /// <param name="name">名称</param>
        /// <returns>是否存在</returns>
        public async Task<bool> ExistsNameAsync<TSub>(string number, string name) where TSub : T
        {
            if (!string.IsNullOrWhiteSpace(number))
            {
                TSub current = await this.SingleOrDefaultAsync<TSub>(number);
                if (current != null && current.Name == name)
                {
                    return false;
                }

                return await this.ExistsNameAsync<TSub>(name);
            }

            return await this.ExistsNameAsync<TSub>(name);
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
            throw new NotSupportedException("MongoDB不支持SQL");
        }
        #endregion

        #region # 执行SQL查询 —— Task<ICollection<TEntity>> ExecuteSqlQueryAsync<TEntity>(string sql...
        /// <summary>
        /// 执行SQL查询
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">参数集</param>
        /// <returns>实体对象集合</returns>
        public Task<ICollection<TEntity>> ExecuteSqlQueryAsync<TEntity>(string sql, params object[] parameters)
        {
            throw new NotSupportedException("MongoDB不支持SQL");
        }
        #endregion

        #region # 释放资源 —— void Dispose()
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {

        }
        #endregion

        #endregion

        #region # Protected

        #region # 批量操作 —— void BulkWrite(IEnumerable<WriteModel<T>> requests)
        /// <summary>
        /// 批量操作
        /// </summary>
        /// <param name="requests">批量操作请求</param>
        protected void BulkWrite(IEnumerable<WriteModel<T>> requests)
        {
            this._collection.BulkWriteAsync(requests).Wait();
        }
        #endregion

        #region # 获取实体对象列表默认排序 —— IFindFluent<T, T> FindAndSort(...
        /// <summary>
        /// 获取实体对象列表默认排序
        /// </summary>
        /// <param name="condition">条件表达式</param>
        /// <returns>实体对象列表</returns>
        protected IFindFluent<T, T> FindAndSort(Expression<Func<T, bool>> condition)
        {
            return this._collection.Find(condition).SortByDescending(x => x.AddedTime);
        }
        #endregion

        #region # 获取子类对象列表默认排序 —— IFindFluent<TSub, TSub> FindAndSort<TSub>(...
        /// <summary>
        /// 获取子类对象列表默认排序
        /// </summary>
        /// <param name="condition">条件表达式</param>
        /// <returns>子类对象列表</returns>
        protected IFindFluent<TSub, TSub> FindAndSort<TSub>(Expression<Func<TSub, bool>> condition) where TSub : T
        {
            return this._collection.OfType<TSub>().Find<TSub>(condition).SortByDescending(x => x.AddedTime);
        }
        #endregion

        #region # 获取实体对象列表 —— IFindFluent<T, T> Find(...
        /// <summary>
        /// 获取实体对象列表
        /// </summary>
        /// <param name="condition">条件表达式</param>
        /// <returns>实体对象列表</returns>
        protected IFindFluent<T, T> Find(Expression<Func<T, bool>> condition)
        {
            return this._collection.Find(condition);
        }
        #endregion

        #region # 获取子类对象列表 —— IFindFluent<TSub, TSub> Find<TSub>(...
        /// <summary>
        /// 获取子类对象列表
        /// </summary>
        /// <param name="condition">条件表达式</param>
        /// <returns>子类对象列表</returns>
        protected IFindFluent<TSub, TSub> Find<TSub>(Expression<Func<TSub, bool>> condition) where TSub : T
        {
            return this._collection.OfType<TSub>().Find(condition);
        }
        #endregion

        #region # 分页获取实体对象列表 —— IFindFluent<T, T> FindByPage(...
        /// <summary>
        /// 分页获取实体对象列表
        /// </summary>
        /// <param name="condition">条件表达式</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页容量</param>
        /// <param name="rowCount">总记录条数</param>
        /// <param name="pageCount">总页数</param>
        /// <returns>实体对象列表</returns>
        protected IFindFluent<T, T> FindByPage(Expression<Func<T, bool>> condition, int pageIndex, int pageSize, out int rowCount, out int pageCount)
        {
            IFindFluent<T, T> list = this.FindAndSort(condition);
            rowCount = unchecked((int)list.CountDocuments());
            pageCount = (int)Math.Ceiling(rowCount * 1.0 / pageSize);
            return list.Skip((pageIndex - 1) * pageSize).Limit(pageSize);
        }
        #endregion

        #region # 分页获取子类对象列表 —— IFindFluent<TSub, TSub> FindByPage<TSub>(...
        /// <summary>
        /// 分页获取子类对象列表
        /// </summary>
        /// <param name="condition">条件表达式</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页容量</param>
        /// <param name="rowCount">总记录条数</param>
        /// <param name="pageCount">总页数</param>
        /// <returns>子类对象列表</returns>
        protected IFindFluent<TSub, TSub> FindByPage<TSub>(Expression<Func<TSub, bool>> condition, int pageIndex, int pageSize, out int rowCount, out int pageCount) where TSub : T
        {
            IFindFluent<TSub, TSub> list = this.FindAndSort(condition);
            rowCount = unchecked((int)list.CountDocuments());
            pageCount = (int)Math.Ceiling(rowCount * 1.0 / pageSize);
            return list.Skip((pageIndex - 1) * pageSize).Limit(pageSize);
        }
        #endregion

        #endregion

        #region # Private

        #region # 注册实体类型 —— static void RegisterTypes()
        /// <summary>
        /// 注册实体类型
        /// </summary>
        private static void RegisterTypes()
        {
            //加载实体所在程序集
            Assembly entityAssembly = Assembly.Load(FrameworkSection.Setting.EntityAssembly.Value);
            Type[] types = entityAssembly.GetTypes();

            //查询所有实体抽象基类
            Func<Type, bool> baseTypeQuery =
                 type =>
                     type.IsAbstract &&
                     type.BaseType == typeof(AggregateRootEntity);

            IEnumerable<Type> baseTypes = types.Where(baseTypeQuery);

            //注册实体类
            foreach (Type baseType in baseTypes)
            {
                BsonClassMap baseMap = new BsonClassMap(baseType);
                baseMap.AutoMap();
                baseMap.SetIsRootClass(true);
                BsonClassMap.RegisterClassMap(baseMap);

                IEnumerable<Type> subTypes = types.Where(x => x.IsSubclassOf(baseType));
                foreach (Type subType in subTypes)
                {
                    BsonClassMap subMap = new BsonClassMap(subType);
                    subMap.AutoMap();
                    subMap.SetIsRootClass(false);
                    BsonClassMap.RegisterClassMap(subMap);
                }
            }
        }
        #endregion

        #endregion
    }
}
