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
        /// MongoDB连接字符串键
        /// </summary>
        private const string MongoConnectionStringKey = "MongoConnection";

        /// <summary>
        /// MongoDB连接字符串
        /// </summary>
        private static readonly string _ConnectionString;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static MongoRepositoryProvider()
        {
            string connStr = ConfigurationManager.ConnectionStrings[MongoConnectionStringKey].ConnectionString;

            if (string.IsNullOrWhiteSpace(connStr))
            {
                throw new ApplicationException(string.Format("MongoDB连接字符串未设置，默认连接字符串键为\"{0}\"！", MongoConnectionStringKey));
            }
            _ConnectionString = connStr;

            //注册实体类型
            RegisterTypes();
        }


        /// <summary>
        /// MongoDB实体对象集合
        /// </summary>
        private readonly IMongoCollection<T> _collection;

        /// <summary>
        /// 构造器
        /// </summary>
        protected MongoRepositoryProvider()
        {
            string[] connStr = _ConnectionString.Split(new[] { Separator }, StringSplitOptions.None);

            if (connStr.Length != 2)
            {
                throw new ApplicationException(string.Format("连接字符串格式不正确，请使用\"{0}\"来分隔服务器地址与数据库名称！", Separator));
            }

            MongoClient client = new MongoClient(connStr[0]);
            IMongoDatabase database = client.GetDatabase(connStr[1]);
            this._collection = database.GetCollection<T>(typeof(T).FullName);
        }

        #endregion

        #region # 命令部分

        #region # 添加单个实体对象 —— void Add(T entity)
        /// <summary>
        /// 添加单个实体对象
        /// </summary>
        /// <param name="entity">新实体对象</param>
        /// <exception cref="ArgumentNullException">新实体对象为空</exception>
        public void Add(T entity)
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
            if (this.Exists(entity.Id))
            {
                throw new ArgumentOutOfRangeException("Id", string.Format("Id为\"{0}\"的实体已存在！", entity.Id));
            }

            #endregion

            this._collection.InsertOneAsync(entity).Wait();
        }
        #endregion

        #region # 添加实体对象集合 —— void AddRange(IEnumerable<T> entities)
        /// <summary>
        /// 添加实体对象集合
        /// </summary>
        /// <typeparam name="T">聚合根类型</typeparam>
        /// <param name="entities">实体对象集合</param>
        /// <exception cref="ArgumentNullException">实体对象集合为null或长度为0</exception>
        public void AddRange(IEnumerable<T> entities)
        {
            #region # 验证参数

            if (entities == null || !entities.Any())
            {
                throw new ArgumentNullException("entities", string.Format("要保存的{0}实体对象集合不可为空！", typeof(T).Name));
            }

            #endregion

            this._collection.InsertMany(entities);
        }
        #endregion

        #region # 保存单个实体对象 —— void Save(T entity)
        /// <summary>
        /// 保存单个实体对象
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <exception cref="ArgumentNullException">实体对象为空</exception>
        /// <exception cref="NullReferenceException">要保存的对象不存在</exception>
        public void Save(T entity)
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
            if (!this.Exists(entity.Id))
            {
                throw new NullReferenceException(string.Format("不存在Id为{0}的{1}实体对象，请尝试添加操作！", entity.Id, typeof(T).Name));
            }

            #endregion

            entity.SavedTime = DateTime.Now;

            this._collection.FindOneAndReplaceAsync(x => x.Id == entity.Id, entity).Wait();
        }
        #endregion

        #region # 保存实体对象集合 —— void SaveRange(IEnumerable<T> entities)
        /// <summary>
        /// 保存实体对象集合
        /// </summary>
        /// <typeparam name="T">聚合根类型</typeparam>
        /// <param name="entities">实体对象集合</param>
        /// <exception cref="ArgumentNullException">实体对象集合</exception>
        /// <exception cref="NullReferenceException">要保存的对象不存在</exception>
        public void SaveRange(IEnumerable<T> entities)
        {
            #region # 验证参数

            entities = entities == null ? new T[0] : entities.ToArray();

            if (!entities.Any())
            {
                throw new ArgumentNullException("entities", string.Format("要保存的{0}实体对象集合不可为空！", typeof(T).Name));
            }

            #endregion

            IList<WriteModel<T>> bulkWrites = new List<WriteModel<T>>();

            foreach (T entity in entities)
            {
                entity.SavedTime = DateTime.Now;

                FilterDefinitionBuilder<T> builder = new FilterDefinitionBuilder<T>();
                FilterDefinition<T> filter = builder.Eq(x => x.Id, entity.Id);
                WriteModel<T> writeModel = new ReplaceOneModel<T>(filter, entity);

                bulkWrites.Add(writeModel);
            }

            this.BulkWrite(bulkWrites);
        }
        #endregion

        #region # 删除单行 —— void Remove(Guid id)
        /// <summary>
        /// 删除单行
        /// </summary>
        /// <param name="id">标识Id</param>
        /// <exception cref="ArgumentNullException">id为空</exception>
        /// <exception cref="NullReferenceException">要删除的对象不存在</exception>
        public void Remove(Guid id)
        {
            this._collection.FindOneAndDeleteAsync(x => x.Id == id).Wait();
        }
        #endregion

        #region # 删除单行 —— void Remove(string number)
        /// <summary>
        /// 删除单行
        /// </summary>
        /// <param name="number">编号</param>
        /// <exception cref="ArgumentNullException">编号为空</exception>
        /// <exception cref="NullReferenceException">要删除的对象不存在</exception>
        public void Remove(string number)
        {
            this._collection.FindOneAndDeleteAsync(x => x.Number == number).Wait();
        }
        #endregion

        #region # 删除多行 —— void RemoveRange(IEnumerable<Guid> ids)
        /// <summary>
        /// 删除多行
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="ids">标识Id集合</param>
        /// <exception cref="ArgumentNullException">ids为null或长度为0</exception>
        public void RemoveRange(IEnumerable<Guid> ids)
        {
            #region # 验证参数

            if (ids == null || !ids.Any())
            {
                throw new ArgumentNullException("ids", string.Format("要删除的{0}的id集合不可为空！", typeof(T).Name));
            }

            #endregion

            this._collection.DeleteManyAsync(x => ids.Contains(x.Id)).Wait();
        }
        #endregion

        #region # 删除多行 —— void RemoveRange(IEnumerable<string> numbers)
        /// <summary>
        /// 删除多行
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="numbers">编号集合</param>
        /// <exception cref="ArgumentNullException">numbers为null或长度为0</exception>
        public void RemoveRange(IEnumerable<string> numbers)
        {
            #region # 验证参数

            if (numbers == null || !numbers.Any())
            {
                throw new ArgumentNullException("ids", string.Format("要删除的{0}的编号集合不可为空！", typeof(T).Name));
            }

            #endregion

            this._collection.DeleteManyAsync(x => numbers.Contains(x.Number)).Wait();
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

            return this.Find(x => x.Id == id).SingleOrDefault();
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

            return this.Find<TSub>(x => x.Id == id).SingleOrDefault();
        }
        #endregion

        #region # 根据编号获取唯一实体对象（查看时用） —— T SingleOrDefault(string number)
        /// <summary>
        /// 根据编号获取唯一实体对象（查看时用），
        /// 无该对象时返回null
        /// </summary>
        /// <param name="number">编号</param>
        /// <returns>单个实体对象</returns>
        /// <exception cref="ArgumentNullException">编号为空</exception>
        public T SingleOrDefault(string number)
        {
            #region # 验证参数

            if (string.IsNullOrWhiteSpace(number))
            {
                throw new ArgumentNullException("number", string.Format("{0}的编号不可为空！", typeof(T).Name));
            }

            #endregion

            return this.Find(x => x.Number == number).SingleOrDefault();
        }
        #endregion

        #region # 根据编号获取唯一子类对象（查看时用） —— TSub SingleOrDefault<TSub>(string number)
        /// <summary>
        /// 根据编号获取唯一子类对象（查看时用），
        /// 无该对象时返回null
        /// </summary>
        /// <param name="number">编号</param>
        /// <returns>唯一子类对象</returns>
        /// <exception cref="ArgumentNullException">编号为空</exception>
        public TSub SingleOrDefault<TSub>(string number) where TSub : T
        {
            #region # 验证参数

            if (string.IsNullOrWhiteSpace(number))
            {
                throw new ArgumentNullException("number", string.Format("{0}的编号不可为空！", typeof(TSub).Name));
            }

            #endregion

            return this.Find<TSub>(x => x.Number == number).SingleOrDefault();
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

        #region # 根据编号获取唯一实体对象（查看时用） —— T Single(string number)
        /// <summary>
        /// 根据编号获取唯一实体对象（查看时用），
        /// </summary>
        /// <param name="number">编号</param>
        /// <returns>单个实体对象</returns>
        /// <exception cref="ArgumentNullException">编号为空</exception>
        /// <exception cref="NullReferenceException">无该对象</exception>
        public T Single(string number)
        {
            T current = this.SingleOrDefault(number);

            #region # 非空验证

            if (current == null)
            {
                throw new NullReferenceException(string.Format("编号为\"{0}\"的{1}实体不存在！", number, typeof(T).Name));
            }

            #endregion

            return current;
        }
        #endregion

        #region # 根据编号获取唯一子类对象（查看时用） —— TSub Single<TSub>(string number)
        /// <summary>
        /// 根据编号获取唯一子类对象（查看时用），
        /// </summary>
        /// <param name="number">编号</param>
        /// <returns>单个子类对象</returns>
        /// <exception cref="ArgumentNullException">编号为空</exception>
        /// <exception cref="NullReferenceException">无该对象</exception>
        public TSub Single<TSub>(string number) where TSub : T
        {
            TSub current = this.SingleOrDefault<TSub>(number);

            #region # 非空验证

            if (current == null)
            {
                throw new NullReferenceException(string.Format("编号为\"{0}\"的{1}实体不存在！", number, typeof(TSub).Name));
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

            return this.Find(x => x.Name == name).SingleOrDefault();
        }
        #endregion

        #region # 根据Id获取唯一实体对象Name —— string GetName(Guid id)
        /// <summary>
        /// 根据Id获取唯一实体对象Name
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>实体对象Name</returns>
        /// <exception cref="ArgumentNullException">id为空</exception>
        /// <exception cref="NullReferenceException">无该对象</exception>
        public string GetName(Guid id)
        {
            return this.Single(id).Name;
        }
        #endregion

        #region # 根据编号获取唯一实体对象Name —— string GetName(string number)
        /// <summary>
        /// 根据编号获取唯一实体对象Name
        /// </summary>
        /// <param name="number">编号</param>
        /// <returns>实体对象Name</returns>
        /// <exception cref="ArgumentNullException">编号为空</exception>
        /// <exception cref="NullReferenceException">无该对象</exception>
        public string GetName(string number)
        {
            return this.Single(number).Name;
        }
        #endregion

        #region # 根据Id获取唯一实体对象Number —— string GetNumber(Guid id)
        /// <summary>
        /// 根据Id获取唯一实体对象Number
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>实体对象Number</returns>
        /// <exception cref="ArgumentNullException">id为空</exception>
        /// <exception cref="NullReferenceException">无该对象</exception>
        public string GetNumber(Guid id)
        {
            return this.Single(id).Number;
        }
        #endregion

        #region # 获取默认或第一个实体对象 —— T FirstOrDefault()
        /// <summary>
        /// 获取默认或第一个实体对象，
        /// 无该对象时返回null
        /// </summary>
        public virtual T FirstOrDefault()
        {
            return this.FindAndSort(x => true).FirstOrDefault();
        }
        #endregion

        #region # 获取默认或第一个子类对象 —— TSub FirstOrDefault<TSub>()
        /// <summary>
        /// 获取默认或第一个子类对象，
        /// 无该对象时返回null
        /// </summary>
        public virtual TSub FirstOrDefault<TSub>() where TSub : T
        {
            return this.FindAndSort<TSub>(x => true).FirstOrDefault();
        }
        #endregion


        //IEnumerable部分

        #region # 获取实体对象集合 —— IEnumerable<T> FindAll()
        /// <summary>
        /// 获取实体对象集合
        /// </summary>
        /// <returns>实体对象集合</returns>
        public IEnumerable<T> FindAll()
        {
            return this.FindAndSort(x => true).ToEnumerable();
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
            return this.FindAndSort<TSub>(x => true).ToEnumerable();
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

            return this.FindAndSort(condition).ToEnumerable();
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
                x =>
                    string.IsNullOrEmpty(keywords) || x.Keywords.Contains(keywords);

            return this.FindAndSort<TSub>(condition).ToEnumerable();
        }
        #endregion

        #region # 根据关键字分页获取实体对象集合 + 输出记录条数与页数 —— IEnumerable<T> FindByPage(...
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

            return this.FindByPage(condition, pageIndex, pageSize, out rowCount, out pageCount).ToEnumerable();
        }
        #endregion

        #region # 根据关键字分页获取子类对象集合 + 输出记录条数与页数 —— IEnumerable<TSub> FindByPage...
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
                x =>
                    string.IsNullOrEmpty(keywords) || x.Keywords.Contains(keywords);

            return this.FindByPage<TSub>(condition, pageIndex, pageSize, out rowCount, out pageCount).ToEnumerable();
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

            if (ids == null)
            {
                throw new ArgumentNullException("ids", "Id集合不可为null！");
            }

            #endregion

            ids = ids.Distinct();

            IEnumerable<T> entities = this.FindAndSort(x => ids.Contains(x.Id)).ToList();

            IDictionary<Guid, T> dictionary = new Dictionary<Guid, T>();

            foreach (Guid id in ids)
            {
                T entity = entities.Single(x => x.Id == id);
                dictionary.Add(id, entity);
            }


            return dictionary;
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

            if (ids == null)
            {
                throw new ArgumentNullException("ids", "Id集合不可为null！");
            }

            #endregion

            ids = ids.Distinct();

            IEnumerable<TSub> entities = this.FindAndSort(x => x is TSub && ids.Contains(x.Id)).ToEnumerable().OfType<TSub>().ToArray();

            IDictionary<Guid, TSub> dictionary = new Dictionary<Guid, TSub>();

            foreach (Guid id in ids)
            {
                TSub entity = entities.Single(x => x.Id == id);
                dictionary.Add(id, entity);
            }


            return dictionary;
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

            if (numbers == null)
            {
                throw new ArgumentNullException("numbers", "编号集合不可为null！");
            }

            #endregion

            numbers = numbers.Distinct();

            IEnumerable<T> entities = this.FindAndSort(x => numbers.Contains(x.Number)).ToList();

            IDictionary<string, T> dictionary = new Dictionary<string, T>();

            foreach (string number in numbers)
            {
                T entity = entities.Single(x => x.Number == number);
                dictionary.Add(number, entity);
            }

            return dictionary;
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

            if (numbers == null)
            {
                throw new ArgumentNullException("numbers", "编号集合不可为null！");
            }

            #endregion

            numbers = numbers.Distinct();

            IEnumerable<TSub> entities = this.FindAndSort(x => x is TSub && numbers.Contains(x.Number)).ToEnumerable().OfType<TSub>().ToArray();

            IDictionary<string, TSub> dictionary = new Dictionary<string, TSub>();

            foreach (string number in numbers)
            {
                TSub entity = entities.Single(x => x.Number == number);
                dictionary.Add(number, entity);
            }

            return dictionary;
        }
        #endregion

        #region # 获取Id与Name字典 —— IDictionary<Guid, string> FindIdNames()
        /// <summary>
        /// 获取Id与Name字典
        /// </summary>
        /// <returns>Id与Name字典</returns>
        /// <remarks>
        /// IDictionary[Guid, string]，键：Id，值：Name
        /// </remarks>
        public IDictionary<Guid, string> FindIdNames()
        {
            IDictionary<Guid, string> dictionary = new Dictionary<Guid, string>();

            foreach (T entity in this.FindAll())
            {
                dictionary.Add(entity.Id, entity.Name);
            }

            return dictionary;
        }
        #endregion

        #region # 获取Id与Name字典 —— IDictionary<Guid, string> FindIdNames<TSub>()
        /// <summary>
        /// 获取Id与Name字典
        /// </summary>
        /// <returns>Id与Name字典</returns>
        /// <remarks>
        /// IDictionary[Guid, string]，键：Id，值：Name
        /// </remarks>
        public IDictionary<Guid, string> FindIdNames<TSub>() where TSub : T
        {
            IDictionary<Guid, string> dictionary = new Dictionary<Guid, string>();

            foreach (TSub entity in this.FindAll<TSub>())
            {
                dictionary.Add(entity.Id, entity.Name);
            }

            return dictionary;
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
            long count = this.Find(x => true).Count();

            return unchecked((int)count);
        }
        #endregion

        #region # 获取子类记录条数 —— int Count<TSub>()
        /// <summary>
        /// 获取子类记录条数
        /// </summary>
        /// <returns>子类记录条数</returns>
        public int Count<TSub>() where TSub : T
        {
            long count = this.Find<TSub>(x => true).Count();

            return unchecked((int)count);
        }
        #endregion


        //Exists部分

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

            return this.Find(x => x.Id == id).Any();
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

            return this.Find<TSub>(x => x.Id == id).Any();
        }
        #endregion

        #region # 判断是否存在给定编号的实体对象 —— bool Exists(string number)
        /// <summary>
        /// 判断是否存在给定编号的实体对象
        /// </summary>
        /// <param name="number">编号</param>
        /// <returns>是否存在</returns>
        /// <exception cref="ArgumentNullException">编号为空</exception>
        public bool Exists(string number)
        {
            #region # 验证参数

            if (string.IsNullOrWhiteSpace(number))
            {
                throw new ArgumentNullException("number", string.Format("{0}的编号不可为空！", typeof(T).Name));
            }

            #endregion

            return this.Find(x => x.Number == number).Any();
        }
        #endregion

        #region # 判断是否存在给定编号的子类对象 —— bool Exists<TSub>(string number)
        /// <summary>
        /// 判断是否存在给定编号的子类对象
        /// </summary>
        /// <param name="number">编号</param>
        /// <returns>是否存在</returns>
        /// <exception cref="ArgumentNullException">编号为空</exception>
        public bool Exists<TSub>(string number) where TSub : T
        {
            #region # 验证参数

            if (string.IsNullOrWhiteSpace(number))
            {
                throw new ArgumentNullException("number", string.Format("{0}的编号不可为空！", typeof(T).Name));
            }

            #endregion

            return this.Find<TSub>(x => x.Number == number).Any();
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

            return this.Find(x => x.Name == name).Any();
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

            return this.Find<TSub>(x => x.Name == name).Any();
        }
        #endregion

        #region # 判断是否存在给定名称的实体对象 —— bool ExistsName(Guid? id, string name)
        /// <summary>
        /// 判断是否存在给定名称的实体对象
        /// </summary>
        /// <param name="id">标识id</param>
        /// <param name="name">名称</param>
        /// <returns>是否已存在</returns>
        public bool ExistsName(Guid? id, string name)
        {
            id = id ?? Guid.Empty;

            T current = this.SingleOrDefault(id.Value);

            if (current != null && current.Name == name)
            {
                return false;
            }

            return this.ExistsName(name);
        }
        #endregion

        #region # 判断是否存在给定名称的子类对象 —— bool ExistsName<TSub>(Guid? id, string name)
        /// <summary>
        /// 判断是否存在给定名称的子类对象
        /// </summary>
        /// <param name="id">标识id</param>
        /// <param name="name">名称</param>
        /// <returns>是否已存在</returns>
        public bool ExistsName<TSub>(Guid? id, string name) where TSub : T
        {
            id = id ?? Guid.Empty;

            TSub current = this.SingleOrDefault<TSub>(id.Value);

            if (current != null && current.Name == name)
            {
                return false;
            }

            return this.ExistsName(name);
        }
        #endregion

        #region # 判断是否存在给定名称的实体对象 —— bool ExistsName(string number, string name)
        /// <summary>
        /// 判断是否存在给定名称的实体对象
        /// </summary>
        /// <param name="number">编号</param>
        /// <param name="name">名称</param>
        /// <returns>是否已存在</returns>
        public bool ExistsName(string number, string name)
        {
            number = string.IsNullOrWhiteSpace(number) ? Guid.NewGuid().ToString().Substring(0, 10) : number;

            T current = this.SingleOrDefault(number);

            if (current != null && current.Name == name)
            {
                return false;
            }

            return this.ExistsName(name);
        }
        #endregion

        #region # 判断是否存在给定名称的子类对象 —— bool ExistsName<TSub>(string number, string name)
        /// <summary>
        /// 判断是否存在给定名称的子类对象
        /// </summary>
        /// <param name="number">编号</param>
        /// <param name="name">名称</param>
        /// <returns>是否已存在</returns>
        public bool ExistsName<TSub>(string number, string name) where TSub : T
        {
            number = string.IsNullOrWhiteSpace(number) ? Guid.NewGuid().ToString().Substring(0, 10) : number;

            TSub current = this.SingleOrDefault<TSub>(number);

            if (current != null && current.Name == name)
            {
                return false;
            }

            return this.ExistsName(name);
        }
        #endregion


        //其他

        #region # 执行SQL查询 —— IEnumerable<TT> ExecuteSqlQuery<TT>(string sql...
        /// <summary>
        /// 执行SQL查询
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>实体对象集合</returns>
        /// <exception cref="ArgumentNullException">SQL语句为空</exception>
        public IEnumerable<TT> ExecuteSqlQuery<TT>(string sql, params object[] parameters)
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
            rowCount = unchecked((int)list.Count());
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
            rowCount = unchecked((int)list.Count());
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
            Assembly entityAssembly = Assembly.Load(WebConfigSetting.EntityAssembly);
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
