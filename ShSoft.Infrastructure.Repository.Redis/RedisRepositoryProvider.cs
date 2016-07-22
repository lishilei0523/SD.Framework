using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using ServiceStack.Redis;
using ServiceStack.Redis.Generic;
using ShSoft.Infrastructure.EntityBase;
using ShSoft.Infrastructure.RepositoryBase;

namespace ShSoft.Infrastructure.Repository.Redis
{
    /// <summary>
    /// Redis简单仓储Provider
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    public class RedisRepositoryProvider<T> : ISimpleRepository<T> where T : AggregateRootEntity
    {
        #region # 字段及构造器

        /// <summary>
        /// Redis服务器地址AppSetting键
        /// </summary>
        private const string RedisServerAppSettingKey = "RedisServer";

        /// <summary>
        /// Redis客户端
        /// </summary>
        private static readonly IRedisClient _RedisClient;

        /// <summary>
        /// 静态构造器
        /// </summary>

        static RedisRepositoryProvider()
        {
            //读取配置文件中的Redis服务端IP地址、端口号
            string ip = ConfigurationManager.AppSettings[RedisServerAppSettingKey];   //127.0.0.1,6379

            //判断是否为空
            if (string.IsNullOrWhiteSpace(ip))
            {
                throw new SystemException("Redis服务端IP地址未配置！");
            }

            string[] redisServer = ip.Split(',');
            //实例化RedisClient
            _RedisClient = new RedisClient(redisServer[0], int.Parse(redisServer[1]));
        }

        /// <summary>
        /// Redis类型客户端
        /// </summary>
        private readonly IRedisTypedClient<T> _redisTypedClient;

        /// <summary>
        /// Redis表
        /// </summary>
        private readonly IRedisList<T> _table;

        /// <summary>
        /// 构造器
        /// </summary>
        public RedisRepositoryProvider()
        {
            this._redisTypedClient = _RedisClient.As<T>();
            this._table = this._redisTypedClient.Lists[typeof(T).Name];
        }

        #endregion

        #region # 命令部分

        #region # 添加单个实体对象 —— void Add(T entity)
        /// <summary>
        /// 添加单个实体对象
        /// </summary>
        /// <typeparam name="T">聚合根类型</typeparam>
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
            if (this.Exists(x => x.Id == entity.Id))
            {
                throw new ArgumentOutOfRangeException("Id", string.Format("Id为\"{0}\"的实体已存在！", entity.Id));
            }

            #endregion

            this._redisTypedClient.AddItemToList(this._table, entity);
        }
        #endregion

        #region # 保存单个实体对象 —— void Save(T entity)
        /// <summary>
        /// 保存单个实体对象
        /// </summary>
        /// <typeparam name="T">聚合根类型</typeparam>
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

            if (this.FindAllInner().All(x => x.Id != entity.Id))
            {
                throw new NullReferenceException(string.Format("不存在Id为{0}的{1}实体对象，请尝试添加操作！", entity.Id, typeof(T).Name));
            }

            #endregion

            T oldEntity = this.Single(entity.Id);

            this._redisTypedClient.RemoveItemFromList(this._table, oldEntity);
            this._redisTypedClient.AddItemToList(this._table, entity);
        }
        #endregion

        #region # 删除单行 —— void Remove(Guid id)
        /// <summary>
        /// 删除单行
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="id">标识Id</param>
        /// <exception cref="ArgumentNullException">id为空</exception>
        /// <exception cref="NullReferenceException">要删除的对象不存在</exception>
        public void Remove(Guid id)
        {
            T entity = this.Single(id);

            this._redisTypedClient.RemoveItemFromList(this._table, entity);
        }
        #endregion

        #region # 删除单行 —— void Remove(string number)
        /// <summary>
        /// 删除单行
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="number">编号</param>
        /// <exception cref="ArgumentNullException">编号为空</exception>
        /// <exception cref="NullReferenceException">要删除的对象不存在</exception>
        public void Remove(string number)
        {
            T entity = this.Single(number);

            this._redisTypedClient.RemoveItemFromList(this._table, entity);
        }
        #endregion

        #region # 删除全部 —— void RemoveAll()
        /// <summary>
        /// 删除全部
        /// </summary>
        public void RemoveAll()
        {
            foreach (T entity in this.FindAll().ToArray())
            {
                this.Remove(entity.Id);
            }
        }
        #endregion

        #endregion

        #region # 查询部分

        #region # Public

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

            return this.SingleOrDefault(x => x.Id == id);
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

            return this.SingleOrDefault(x => x.Number == number);
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

            return this.SingleOrDefault<TSub>(x => x.Number == number);
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

            return this.SingleOrDefault(x => x.Name == name);
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


        //IEnumerable部分

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

        #region # 根据Id集获取子类对象集合 —— IEnumerable<TSub> Find<TSub>(IEnumerable<Guid> ids)
        /// <summary>
        /// 根据Id集获取子类对象集合
        /// </summary>
        /// <returns>子类对象集合</returns>
        public IEnumerable<TSub> Find<TSub>(IEnumerable<Guid> ids) where TSub : T
        {
            return this.Find<TSub>(x => ids.Contains(x.Id)).AsEnumerable();
        }
        #endregion

        #region # 根据编号集获取实体对象集合 —— IEnumerable<T> Find(IEnumerable<string> numbers)
        /// <summary>
        /// 根据编号集获取实体对象集合
        /// </summary>
        /// <returns>实体对象集合</returns>
        public IEnumerable<T> Find(IEnumerable<string> numbers)
        {
            return this.Find(x => numbers.Contains(x.Number)).AsEnumerable();
        }
        #endregion

        #region # 根据编号集获取子类对象集合 —— IEnumerable<TSub> Find<TSub>(IEnumerable<string> numbers)
        /// <summary>
        /// 根据编号集获取子类对象集合
        /// </summary>
        /// <returns>子类对象集合</returns>
        public IEnumerable<TSub> Find<TSub>(IEnumerable<string> numbers) where TSub : T
        {
            return this.Find<TSub>(x => numbers.Contains(x.Number)).AsEnumerable();
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
            return this.Find(condition).AsEnumerable();
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
            return this.FindByPage(condition, pageIndex, pageSize, out rowCount, out pageCount).AsEnumerable();
        }
        #endregion

        #region # 根据关键字分页获取子类对象集合 + 输出记录条数与页数 —— IEnumerable<TSub> FindByPage<TSub>(...
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
            return this.FindByPage(condition, pageIndex, pageSize, out rowCount, out pageCount).AsEnumerable();
        }
        #endregion

        #region # 获取Id与Name字典 —— IDictionary<Guid, string> FindDictionary()
        /// <summary>
        /// 获取Id与Name字典
        /// </summary>
        /// <returns>Id与Name字典</returns>
        /// <remarks>
        /// IDictionary[Guid, string]，键：Id，值：Name
        /// </remarks>
        public IDictionary<Guid, string> FindDictionary()
        {
            IDictionary<Guid, string> dictionary = new Dictionary<Guid, string>();

            foreach (T entity in this.FindAllInner())
            {
                dictionary.Add(entity.Id, entity.Name);
            }

            return dictionary;
        }
        #endregion

        #region # 获取Id与Name字典 —— IDictionary<Guid, string> FindDictionary<TSub>()
        /// <summary>
        /// 获取Id与Name字典
        /// </summary>
        /// <returns>Id与Name字典</returns>
        /// <remarks>
        /// IDictionary[Guid, string]，键：Id，值：Name
        /// </remarks>
        public IDictionary<Guid, string> FindDictionary<TSub>() where TSub : T
        {
            IDictionary<Guid, string> dictionary = new Dictionary<Guid, string>();

            foreach (TSub entity in this.FindAllInner<TSub>())
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

            return this.Exists(x => x.Number == number);
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

            return this.Exists<TSub>(x => x.Number == number);
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
            throw new NotSupportedException("Redis不支持SQL");
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

        //Single部分

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

            return this.Find(predicate).FirstOrDefault();
        }
        #endregion


        //IQueryable部分

        #region # 获取实体对象集合 —— virtual IQueryable<T> FindAllInner()
        /// <summary>
        /// 获取实体对象集合
        /// </summary>
        /// <returns>实体对象集合</returns>
        protected virtual IQueryable<T> FindAllInner()
        {
            return this._table.GetAll().Where(x => !x.Deleted).OrderByDescending(x => x.Sort).ThenByDescending(x => x.AddedTime).AsQueryable();
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
            return this.Find(predicate).Select(x => x.Id);
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
            return this.Find(predicate).Select(x => x.Number);
        }
        #endregion

        #region # 根据条件分页获取实体对象集合 + 输出记录条数与页数 —— IQueryable<T> FindByPage(...
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

        #region # 根据条件分页获取子类对象集合 + 输出记录条数与页数 —— IQueryable<TSub> FindByPage<TSub>(...
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

        #region # 获取给定条件的Id与Name字典 —— IDictionary<Guid, string> FindDictionary(Expression...
        /// <summary>
        /// 获取给定条件的Id与Name字典
        /// </summary>
        /// <param name="predicate">条件表达式</param>
        /// <returns>Id与Name字典</returns>
        /// <remarks>
        /// IDictionary[Guid, string]，键：Id，值：Name
        /// </remarks>
        protected IDictionary<Guid, string> FindDictionary(Expression<Func<T, bool>> predicate)
        {
            IDictionary<Guid, string> dictionary = new Dictionary<Guid, string>();

            foreach (T entity in this.Find(predicate))
            {
                dictionary.Add(entity.Id, entity.Name);
            }

            return dictionary;
        }
        #endregion

        #region # 获取Id与Name字典 —— IDictionary<Guid, string> FindDictionary<TSub>(Expression...
        /// <summary>
        /// 获取给定条件的Id与Name字典
        /// </summary>
        /// <param name="predicate">条件表达式</param>
        /// <returns>Id与Name字典</returns>
        /// <remarks>
        /// IDictionary[Guid, string]，键：Id，值：Name
        /// </remarks>
        protected IDictionary<Guid, string> FindDictionary<TSub>(Expression<Func<TSub, bool>> predicate) where TSub : T
        {
            IDictionary<Guid, string> dictionary = new Dictionary<Guid, string>();

            foreach (TSub entity in this.Find(predicate))
            {
                dictionary.Add(entity.Id, entity.Name);
            }

            return dictionary;
        }
        #endregion


        //Count部分

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


        //Exists部分

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

        #endregion
    }
}
