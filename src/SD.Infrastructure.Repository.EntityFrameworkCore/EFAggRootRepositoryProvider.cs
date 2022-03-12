using Microsoft.EntityFrameworkCore;
using SD.Infrastructure.EntityBase;
using SD.Infrastructure.RepositoryBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SD.Infrastructure.Repository.EntityFrameworkCore
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
            this.Dispose();
        }

        #endregion

        #region # Public

        //Single部分

        #region # 获取唯一实体对象 —— T SingleOrDefault(string number)
        /// <summary>
        /// 获取唯一实体对象
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

            return this.SingleOrDefault(x => x.Number == number);
        }
        #endregion

        #region # 获取唯一实体对象 —— Task<T> SingleOrDefaultAsync(string number)
        /// <summary>
        /// 获取唯一实体对象
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

            return await this.SingleOrDefaultAsync(x => x.Number == number);
        }
        #endregion

        #region # 获取唯一子类对象 —— TSub SingleOrDefault<TSub>(string number)
        /// <summary>
        /// 获取唯一子类对象
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

            return this.SingleOrDefault<TSub>(x => x.Number == number);
        }
        #endregion

        #region # 获取唯一子类对象 —— Task<TSub> SingleOrDefaultAsync<TSub>(string number)
        /// <summary>
        /// 获取唯一子类对象
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

            return await this.SingleOrDefaultAsync<TSub>(x => x.Number == number);
        }
        #endregion

        #region # 获取唯一实体对象 —— T Single(string number)
        /// <summary>
        /// 获取唯一实体对象
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

        #region # 获取唯一实体对象 —— Task<T> SingleAsync(string number)
        /// <summary>
        /// 获取唯一实体对象
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

        #region # 获取唯一子类对象 —— TSub Single<TSub>(string number)
        /// <summary>
        /// 获取唯一子类对象
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

        #region # 获取唯一子类对象 —— Task<TSub> SingleAsync<TSub>(string number)
        /// <summary>
        /// 获取唯一子类对象
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

        #region # 获取实体对象列表 —— ICollection<T> Find(string keywords)
        /// <summary>
        /// 获取实体对象列表
        /// </summary>
        /// <param name="keywords">关键字</param>
        /// <returns>实体对象列表</returns>
        public ICollection<T> Find(string keywords)
        {
            IQueryable<T> entities = this.FindAllInner();
            if (!string.IsNullOrWhiteSpace(keywords))
            {
                entities = this.Find(x => x.Keywords.Contains(keywords));
            }

            return entities.ToList();
        }
        #endregion

        #region # 获取实体对象列表 —— Task<ICollection<T>> FindAsync(string keywords)
        /// <summary>
        /// 获取实体对象列表
        /// </summary>
        /// <param name="keywords">关键字</param>
        /// <returns>实体对象列表</returns>
        public async Task<ICollection<T>> FindAsync(string keywords)
        {
            IQueryable<T> entities = this.FindAllInner();
            if (!string.IsNullOrWhiteSpace(keywords))
            {
                entities = this.Find(x => x.Keywords.Contains(keywords));
            }

            return await entities.ToListAsync();
        }
        #endregion

        #region # 获取子类对象列表 —— ICollection<TSub> Find<TSub>(string keywords)
        /// <summary>
        /// 获取子类对象列表
        /// </summary>
        /// <typeparam name="TSub">子类类型</typeparam>
        /// <param name="keywords">关键字</param>
        /// <returns>子类对象列表</returns>
        public ICollection<TSub> Find<TSub>(string keywords) where TSub : T
        {
            IQueryable<TSub> entities = this.FindAllInner<TSub>();
            if (!string.IsNullOrWhiteSpace(keywords))
            {
                entities = this.Find<TSub>(x => x.Keywords.Contains(keywords));
            }

            return entities.ToList();
        }
        #endregion

        #region # 获取子类对象列表 —— Task<ICollection<TSub>> FindAsync<TSub>(string keywords)
        /// <summary>
        /// 获取子类对象列表
        /// </summary>
        /// <typeparam name="TSub">子类类型</typeparam>
        /// <param name="keywords">关键字</param>
        /// <returns>子类对象列表</returns>
        public async Task<ICollection<TSub>> FindAsync<TSub>(string keywords) where TSub : T
        {
            IQueryable<TSub> entities = this.FindAllInner<TSub>();
            if (!string.IsNullOrWhiteSpace(keywords))
            {
                entities = this.Find<TSub>(x => x.Keywords.Contains(keywords));
            }

            return await entities.ToListAsync();
        }
        #endregion

        #region # 分页获取实体对象列表 —— ICollection<T> FindByPage(string keywords...
        /// <summary>
        /// 分页获取实体对象列表
        /// </summary>
        /// <param name="keywords">关键字</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页容量</param>
        /// <param name="rowCount">总记录数</param>
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

            return this.FindByPage(condition, pageIndex, pageSize, out rowCount, out pageCount).ToList();
        }
        #endregion

        #region # 分页获取实体对象列表 —— Task<Page<T>> FindByPageAsync(string keywords...
        /// <summary>
        /// 分页获取实体对象列表
        /// </summary>
        /// <param name="keywords">关键字</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页容量</param>
        /// <returns>实体对象列表</returns>
        public async Task<Page<T>> FindByPageAsync(string keywords, int pageIndex, int pageSize)
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

            IOrderedQueryable<T> orderedResult = this.FindAndSort(condition);
            return await orderedResult.ToPageAsync(pageIndex, pageSize);
        }
        #endregion

        #region # 分页获取子类对象列表 —— ICollection<TSub> FindByPage<TSub>(string keywords...
        /// <summary>
        /// 分页获取子类对象列表
        /// </summary>
        /// <typeparam name="TSub">子类类型</typeparam>
        /// <param name="keywords">关键字</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页容量</param>
        /// <param name="rowCount">总记录数</param>
        /// <param name="pageCount">总页数</param>
        /// <returns>子类对象列表</returns>
        public ICollection<TSub> FindByPage<TSub>(string keywords, int pageIndex, int pageSize, out int rowCount,
            out int pageCount) where TSub : T
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

            return this.FindByPage(condition, pageIndex, pageSize, out rowCount, out pageCount).ToList();
        }
        #endregion

        #region # 分页获取子类对象列表 —— Task<Page<TSub>> FindByPageAsync<TSub>(string keywords...
        /// <summary>
        /// 分页获取子类对象列表
        /// </summary>
        /// <typeparam name="TSub">子类类型</typeparam>
        /// <param name="keywords">关键字</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页容量</param>
        /// <returns>子类对象列表</returns>
        public async Task<Page<TSub>> FindByPageAsync<TSub>(string keywords, int pageIndex, int pageSize) where TSub : T
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

            IOrderedQueryable<TSub> orderedResult = this.FindAndSort<TSub>(condition);
            return await orderedResult.ToPageAsync(pageIndex, pageSize);
        }
        #endregion


        //IDictionary部分

        #region # 获取实体对象字典 —— IDictionary<string, T> Find(IEnumerable<string> numbers)
        /// <summary>
        /// 获取实体对象字典
        /// </summary>
        /// <param name="numbers">编号集</param>
        /// <returns>实体对象字典</returns>
        /// <remarks>IDictionary[string, T]，[编号, 实体对象]</remarks>
        public IDictionary<string, T> Find(IEnumerable<string> numbers)
        {
            #region # 验证

            numbers = numbers?.Distinct().ToArray() ?? Array.Empty<string>();
            if (!numbers.Any())
            {
                return new Dictionary<string, T>();
            }

            #endregion

            var entities = from entity in this._dbContext.Set<T>()
                           where numbers.Contains(entity.Number)
                           select new { entity.Number, entity };

            return entities.ToDictionary(x => x.Number, x => x.entity);
        }
        #endregion

        #region # 获取实体对象字典 —— Task<IDictionary<string, T>> FindAsync(IEnumerable<string> numbers)
        /// <summary>
        /// 获取实体对象字典
        /// </summary>
        /// <param name="numbers">编号集</param>
        /// <returns>实体对象字典</returns>
        /// <remarks>IDictionary[string, T]，[编号, 实体对象]</remarks>
        public async Task<IDictionary<string, T>> FindAsync(IEnumerable<string> numbers)
        {
            #region # 验证

            numbers = numbers?.Distinct().ToArray() ?? Array.Empty<string>();
            if (!numbers.Any())
            {
                return new Dictionary<string, T>();
            }

            #endregion

            var entities = from entity in this._dbContext.Set<T>()
                           where numbers.Contains(entity.Number)
                           select new { entity.Number, entity };

            return await entities.ToDictionaryAsync(x => x.Number, x => x.entity);
        }
        #endregion

        #region # 获取子类对象字典 —— IDictionary<string, TSub> Find<TSub>(IEnumerable<string>...
        /// <summary>
        /// 获取子类对象字典
        /// </summary>
        /// <param name="numbers">编号集</param>
        /// <returns>子类对象字典</returns>
        /// <remarks>IDictionary[string, TSub]，[编号, 子类对象]</remarks>
        public IDictionary<string, TSub> Find<TSub>(IEnumerable<string> numbers) where TSub : T
        {
            #region # 验证

            numbers = numbers?.Distinct().ToArray() ?? Array.Empty<string>();
            if (!numbers.Any())
            {
                return new Dictionary<string, TSub>();
            }

            #endregion

            var entities = from entity in this._dbContext.Set<TSub>()
                           where numbers.Contains(entity.Number)
                           select new { entity.Number, entity };

            return entities.ToDictionary(x => x.Number, x => x.entity);
        }
        #endregion

        #region # 获取子类对象字典 —— Task<IDictionary<string, TSub>> FindAsync<TSub>(IEnumerable<string...
        /// <summary>
        /// 获取子类对象字典
        /// </summary>
        /// <param name="numbers">编号集</param>
        /// <returns>子类对象字典</returns>
        /// <remarks>IDictionary[string, TSub]，[编号, 子类对象]</remarks>
        public async Task<IDictionary<string, TSub>> FindAsync<TSub>(IEnumerable<string> numbers) where TSub : T
        {
            #region # 验证

            numbers = numbers?.Distinct().ToArray() ?? Array.Empty<string>();
            if (!numbers.Any())
            {
                return new Dictionary<string, TSub>();
            }

            #endregion

            var entities = from entity in this._dbContext.Set<TSub>()
                           where numbers.Contains(entity.Number)
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

            return this.Exists(x => x.Number == number);
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

            return await this.ExistsAsync(x => x.Number == number);
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

            return this.Exists<TSub>(x => x.Number == number);
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

            return await this.ExistsAsync<TSub>(x => x.Number == number);
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
                string originalNumber = this.Find(x => x.Id == id.Value).Select(x => x.Number).SingleOrDefault();
                if (!string.IsNullOrWhiteSpace(originalNumber) && originalNumber == number)
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
                string originalNumber = await this.Find(x => x.Id == id.Value).Select(x => x.Number).SingleOrDefaultAsync();
                if (!string.IsNullOrWhiteSpace(originalNumber) && originalNumber == number)
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
                string originalNumber = this.Find<TSub>(x => x.Id == id.Value).Select(x => x.Number).SingleOrDefault();
                if (!string.IsNullOrWhiteSpace(originalNumber) && originalNumber == number)
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
                string originalNumber = await this.Find<TSub>(x => x.Id == id.Value).Select(x => x.Number).SingleOrDefaultAsync();
                if (!string.IsNullOrWhiteSpace(originalNumber) && originalNumber == number)
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

            return this.Exists(x => x.Name == name);
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

            return await this.ExistsAsync(x => x.Name == name);
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

            return this.Exists<TSub>(x => x.Name == name);
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

            return await this.ExistsAsync<TSub>(x => x.Name == name);
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
                string originalName = this.Find(x => x.Id == id.Value).Select(x => x.Name).SingleOrDefault();
                if (!string.IsNullOrWhiteSpace(originalName) && originalName == name)
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
                string originalName = await this.Find(x => x.Id == id.Value).Select(x => x.Name).SingleOrDefaultAsync();
                if (!string.IsNullOrWhiteSpace(originalName) && originalName == name)
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
                string originalName = this.Find<TSub>(x => x.Id == id.Value).Select(x => x.Name).SingleOrDefault();
                if (!string.IsNullOrWhiteSpace(originalName) && originalName == name)
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
                string originalName = await this.Find<TSub>(x => x.Id == id.Value).Select(x => x.Name).SingleOrDefaultAsync();
                if (!string.IsNullOrWhiteSpace(originalName) && originalName == name)
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
                string originalName = this.Find(x => x.Number == number).Select(x => x.Name).SingleOrDefault();
                if (!string.IsNullOrWhiteSpace(originalName) && originalName == name)
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
                string originalName = await this.Find(x => x.Number == number).Select(x => x.Name).SingleOrDefaultAsync();
                if (!string.IsNullOrWhiteSpace(originalName) && originalName == name)
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
                string originalName = this.Find<TSub>(x => x.Number == number).Select(x => x.Name).SingleOrDefault();
                if (!string.IsNullOrWhiteSpace(originalName) && originalName == name)
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
                string originalName = await this.Find<TSub>(x => x.Number == number).Select(x => x.Name).SingleOrDefaultAsync();
                if (!string.IsNullOrWhiteSpace(originalName) && originalName == name)
                {
                    return false;
                }

                return await this.ExistsNameAsync<TSub>(name);
            }

            return await this.ExistsNameAsync<TSub>(name);
        }
        #endregion

        #endregion

        #region # Protected

        //IQueryable部分

        #region # 获取实体对象编号列表 —— IQueryable<string> FindNos(Expression<Func<T, bool>> condition)
        /// <summary>
        /// 获取实体对象编号列表
        /// </summary>
        /// <param name="condition">条件表达式</param>
        /// <returns>实体对象编号列表</returns>
        protected IQueryable<string> FindNos(Expression<Func<T, bool>> condition)
        {
            return this.Find(condition).Select(x => x.Number);
        }
        #endregion

        #region # 获取子类对象编号列表 —— IQueryable<string> FindNos<TSub>(Expression<Func<TSub...
        /// <summary>
        /// 获取子类对象编号列表
        /// </summary>
        /// <param name="condition">条件表达式</param>
        /// <returns>子类对象编号列表</returns>
        protected IQueryable<string> FindNos<TSub>(Expression<Func<TSub, bool>> condition) where TSub : T
        {
            return this.Find<TSub>(condition).Select(x => x.Number);
        }
        #endregion

        #endregion
    }
}
