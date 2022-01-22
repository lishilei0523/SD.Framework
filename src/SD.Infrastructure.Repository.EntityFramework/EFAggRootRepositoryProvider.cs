using SD.Infrastructure.EntityBase;
using SD.Infrastructure.RepositoryBase;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SD.Infrastructure.Repository.EntityFramework
{
    /// <summary>
    /// EF聚合根仓储Provider
    /// </summary>
    /// <typeparam name="T">聚合根类型</typeparam>
    public abstract class EFAggRootRepositoryProvider<T> : EFEntityRepositoryProvider<T>, IAggRootRepository<T> where T : AggregateRootEntity
    {
        #region # 析构器

        /// <summary>
        /// 析构器
        /// </summary>
        ~EFAggRootRepositoryProvider()
        {
            base.Dispose();
        }

        #endregion

        #region # Public

        //Single部分

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

            return base.SingleOrDefault(x => x.Number == number);
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

            return await base.SingleOrDefaultAsync(x => x.Number == number);
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

            return base.SingleOrDefault<TSub>(x => x.Number == number);
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

            return await base.SingleOrDefaultAsync<TSub>(x => x.Number == number);
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


        //ICollection部分

        #region # 根据关键字获取实体对象列表 —— ICollection<T> Find(string keywords)
        /// <summary>
        /// 根据关键字获取实体对象列表
        /// </summary>
        /// <returns>实体对象列表</returns>
        public ICollection<T> Find(string keywords)
        {
            IQueryable<T> entities = base.FindAllInner();
            if (!string.IsNullOrWhiteSpace(keywords))
            {
                entities = base.Find(x => x.Keywords.Contains(keywords));
            }

            return entities.ToList();
        }
        #endregion

        #region # 根据关键字获取实体对象列表 —— Task<ICollection<T>> FindAsync(string keywords)
        /// <summary>
        /// 根据关键字获取实体对象列表
        /// </summary>
        /// <returns>实体对象列表</returns>
        public async Task<ICollection<T>> FindAsync(string keywords)
        {
            IQueryable<T> entities = base.FindAllInner();
            if (!string.IsNullOrWhiteSpace(keywords))
            {
                entities = base.Find(x => x.Keywords.Contains(keywords));
            }

            return await entities.ToListAsync();
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
            IQueryable<TSub> entities = base.FindAllInner<TSub>();
            if (!string.IsNullOrWhiteSpace(keywords))
            {
                entities = base.Find<TSub>(x => x.Keywords.Contains(keywords));
            }

            return entities.ToList();
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
            IQueryable<TSub> entities = base.FindAllInner<TSub>();
            if (!string.IsNullOrWhiteSpace(keywords))
            {
                entities = base.Find<TSub>(x => x.Keywords.Contains(keywords));
            }

            return await entities.ToListAsync();
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
            Expression<Func<T, bool>> condition;
            if (!string.IsNullOrWhiteSpace(keywords))
            {
                condition = x => x.Keywords.Contains(keywords);
            }
            else
            {
                condition = x => true;
            }

            return base.FindByPage(condition, pageIndex, pageSize, out rowCount, out pageCount).ToList();
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
            Expression<Func<T, bool>> condition;
            if (!string.IsNullOrWhiteSpace(keywords))
            {
                condition = x => x.Keywords.Contains(keywords);
            }
            else
            {
                condition = x => true;
            }

            Task<List<T>> task = base.FindByPage(condition, pageIndex, pageSize, out rowCount, out pageCount).ToListAsync();

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
        public ICollection<TSub> FindByPage<TSub>(string keywords, int pageIndex, int pageSize, out int rowCount, out int pageCount) where TSub : T
        {
            Expression<Func<TSub, bool>> condition;
            if (!string.IsNullOrWhiteSpace(keywords))
            {
                condition = x => x.Keywords.Contains(keywords);
            }
            else
            {
                condition = x => true;
            }

            return base.FindByPage(condition, pageIndex, pageSize, out rowCount, out pageCount).ToList();
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
            Expression<Func<TSub, bool>> condition;
            if (!string.IsNullOrWhiteSpace(keywords))
            {
                condition = x => x.Keywords.Contains(keywords);
            }
            else
            {
                condition = x => true;
            }

            Task<List<TSub>> task = base.FindByPage(condition, pageIndex, pageSize, out rowCount, out pageCount).ToListAsync();

            return new Task<ICollection<TSub>>(() => task.Result);
        }
        #endregion


        //IDictionary部分

        #region # 根据编号集获取实体对象字典 —— IDictionary<string, T> Find(IEnumerable<string> numbers)
        /// <summary>
        /// 根据编号集获取实体对象字典
        /// </summary>
        /// <returns>实体对象字典</returns>
        /// <remarks>IDictionary[string, T]，[编号, 实体对象]</remarks>
        public IDictionary<string, T> Find(IEnumerable<string> numbers)
        {
            #region # 验证

            string[] numbers_ = numbers?.Distinct().ToArray() ?? new string[0];
            if (!numbers_.Any())
            {
                return new Dictionary<string, T>();
            }

            #endregion

            var entities = from entity in this._dbContext.Set<T>()
                           where numbers_.Contains(entity.Number)
                           select new { entity.Number, entity };

            return entities.ToDictionary(x => x.Number, x => x.entity);
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

            string[] numbers_ = numbers?.Distinct().ToArray() ?? new string[0];
            if (!numbers_.Any())
            {
                return new Dictionary<string, T>();
            }

            #endregion

            var entities = from entity in this._dbContext.Set<T>()
                           where numbers_.Contains(entity.Number)
                           select new { entity.Number, entity };

            return await entities.ToDictionaryAsync(x => x.Number, x => x.entity);
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

            string[] numbers_ = numbers?.Distinct().ToArray() ?? new string[0];
            if (!numbers_.Any())
            {
                return new Dictionary<string, TSub>();
            }

            #endregion

            var entities = from entity in this._dbContext.Set<TSub>()
                           where numbers_.Contains(entity.Number)
                           select new { entity.Number, entity };

            return entities.ToDictionary(x => x.Number, x => x.entity);
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

            string[] numbers_ = numbers?.Distinct().ToArray() ?? new string[0];
            if (!numbers_.Any())
            {
                return new Dictionary<string, TSub>();
            }

            #endregion

            var entities = from entity in this._dbContext.Set<TSub>()
                           where numbers_.Contains(entity.Number)
                           select new { entity.Number, entity };

            return await entities.ToDictionaryAsync(x => x.Number, x => x.entity);
        }
        #endregion


        //Exists部分

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

            return base.Exists(x => x.Number == number);
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

            return await base.ExistsAsync(x => x.Number == number);
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

            return base.Exists<TSub>(x => x.Number == number);
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

            return await base.ExistsAsync<TSub>(x => x.Number == number);
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

                return this.ExistsNo(number);
            }

            return this.ExistsNo(number);
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

                return await this.ExistsNoAsync(number);
            }

            return await this.ExistsNoAsync(number);
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

            return base.Exists(x => x.Name == name);
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

            return await base.ExistsAsync(x => x.Name == name);
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

            return base.Exists<TSub>(x => x.Name == name);
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

            return await base.ExistsAsync<TSub>(x => x.Name == name);
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

                return this.ExistsName(name);
            }

            return this.ExistsName(name);
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

                return await this.ExistsNameAsync(name);
            }

            return await this.ExistsNameAsync(name);
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

                return this.ExistsName(name);
            }

            return this.ExistsName(name);
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

                return await this.ExistsNameAsync(name);
            }

            return await this.ExistsNameAsync(name);
        }
        #endregion

        #endregion

        #region # Protected

        //IQueryable部分

        #region # 根据条件获取实体对象编号列表 —— IQueryable<string> FindNos(Expression<Func<T, bool>> condition)
        /// <summary>
        /// 根据条件获取实体对象编号列表
        /// </summary>
        /// <param name="condition">条件表达式</param>
        /// <returns>实体对象编号列表</returns>
        protected IQueryable<string> FindNos(Expression<Func<T, bool>> condition)
        {
            return base.Find(condition).Select(x => x.Number);
        }
        #endregion

        #region # 根据条件获取子类对象编号列表 —— IQueryable<string> FindNos<TSub>(Expression<Func<TSub...
        /// <summary>
        /// 根据条件获取子类对象编号列表
        /// </summary>
        /// <param name="condition">条件表达式</param>
        /// <returns>子类对象编号列表</returns>
        protected IQueryable<string> FindNos<TSub>(Expression<Func<TSub, bool>> condition) where TSub : T
        {
            return base.Find<TSub>(condition).Select(x => x.Number);
        }
        #endregion

        #endregion
    }
}
