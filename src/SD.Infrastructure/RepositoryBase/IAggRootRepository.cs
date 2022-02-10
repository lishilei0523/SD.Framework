using SD.Infrastructure.EntityBase;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SD.Infrastructure.RepositoryBase
{
    /// <summary>
    /// 聚合根仓储基接口
    /// </summary>
    /// <typeparam name="T">聚合根类型</typeparam>
    public interface IAggRootRepository<T> : IEntityRepository<T> where T : AggregateRootEntity
    {
        //Single部分

        #region # 获取唯一实体对象 —— T SingleOrDefault(string number)
        /// <summary>
        /// 获取唯一实体对象
        /// </summary>
        /// <param name="number">编号</param>
        /// <returns>实体对象</returns>
        T SingleOrDefault(string number);
        #endregion

        #region # 获取唯一实体对象 —— Task<T> SingleOrDefaultAsync(string number)
        /// <summary>
        /// 获取唯一实体对象
        /// </summary>
        /// <param name="number">编号</param>
        /// <returns>实体对象</returns>
        Task<T> SingleOrDefaultAsync(string number);
        #endregion

        #region # 获取唯一子类对象 —— TSub SingleOrDefault<TSub>(string number)
        /// <summary>
        /// 获取唯一子类对象
        /// </summary>
        /// <param name="number">编号</param>
        /// <returns>子类对象</returns>
        TSub SingleOrDefault<TSub>(string number) where TSub : T;
        #endregion

        #region # 获取唯一子类对象 —— Task<TSub> SingleOrDefaultAsync<TSub>(string number)
        /// <summary>
        /// 获取唯一子类对象
        /// </summary>
        /// <param name="number">编号</param>
        /// <returns>子类对象</returns>
        Task<TSub> SingleOrDefaultAsync<TSub>(string number) where TSub : T;
        #endregion

        #region # 获取唯一实体对象 —— T Single(string number)
        /// <summary>
        /// 获取唯一实体对象
        /// </summary>
        /// <param name="number">编号</param>
        /// <returns>实体对象</returns>
        T Single(string number);
        #endregion

        #region # 获取唯一实体对象 —— Task<T> SingleAsync(string number)
        /// <summary>
        /// 获取唯一实体对象
        /// </summary>
        /// <param name="number">编号</param>
        /// <returns>实体对象</returns>
        Task<T> SingleAsync(string number);
        #endregion

        #region # 获取唯一子类对象 —— TSub Single<TSub>(string number)
        /// <summary>
        /// 获取唯一子类对象
        /// </summary>
        /// <param name="number">编号</param>
        /// <returns>子类对象</returns>
        TSub Single<TSub>(string number) where TSub : T;
        #endregion

        #region # 获取唯一子类对象 —— Task<TSub> SingleAsync<TSub>(string number)
        /// <summary>
        /// 获取唯一子类对象
        /// </summary>
        /// <param name="number">编号</param>
        /// <returns>子类对象</returns>
        Task<TSub> SingleAsync<TSub>(string number) where TSub : T;
        #endregion


        //ICollection部分

        #region # 获取实体对象列表 —— ICollection<T> Find(string keywords)
        /// <summary>
        /// 获取实体对象列表
        /// </summary>
        /// <param name="keywords">关键字</param>
        /// <returns>实体对象列表</returns>
        ICollection<T> Find(string keywords);
        #endregion

        #region # 获取实体对象列表 —— Task<ICollection<T>> FindAsync(string keywords)
        /// <summary>
        /// 获取实体对象列表
        /// </summary>
        /// <param name="keywords">关键字</param>
        /// <returns>实体对象列表</returns>
        Task<ICollection<T>> FindAsync(string keywords);
        #endregion

        #region # 获取子类对象列表 —— ICollection<TSub> Find<TSub>(string keywords)
        /// <summary>
        /// 获取子类对象列表
        /// </summary>
        /// <typeparam name="TSub">子类类型</typeparam>
        /// <param name="keywords">关键字</param>
        /// <returns>子类对象列表</returns>
        ICollection<TSub> Find<TSub>(string keywords) where TSub : T;
        #endregion

        #region # 获取子类对象列表 —— Task<ICollection<TSub>> FindAsync<TSub>(string keywords)
        /// <summary>
        /// 获取子类对象列表
        /// </summary>
        /// <typeparam name="TSub">子类类型</typeparam>
        /// <param name="keywords">关键字</param>
        /// <returns>子类对象列表</returns>
        Task<ICollection<TSub>> FindAsync<TSub>(string keywords) where TSub : T;
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
        ICollection<T> FindByPage(string keywords, int pageIndex, int pageSize, out int rowCount, out int pageCount);
        #endregion

        #region # 分页获取实体对象列表 —— Task<Page<T>> FindByPageAsync(string keywords...
        /// <summary>
        /// 分页获取实体对象列表
        /// </summary>
        /// <param name="keywords">关键字</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页容量</param>
        /// <returns>实体对象列表</returns>
        Task<Page<T>> FindByPageAsync(string keywords, int pageIndex, int pageSize);
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
        ICollection<TSub> FindByPage<TSub>(string keywords, int pageIndex, int pageSize, out int rowCount, out int pageCount) where TSub : T;
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
        Task<Page<TSub>> FindByPageAsync<TSub>(string keywords, int pageIndex, int pageSize) where TSub : T;
        #endregion


        //IDictionary部分

        #region # 获取实体对象字典 —— IDictionary<string, T> Find(IEnumerable<string> numbers)
        /// <summary>
        /// 获取实体对象字典
        /// </summary>
        /// <param name="numbers">编号集</param>
        /// <returns>实体对象字典</returns>
        /// <remarks>IDictionary[string, T]，[编号, 实体对象]</remarks>
        IDictionary<string, T> Find(IEnumerable<string> numbers);
        #endregion

        #region # 获取实体对象字典 —— Task<IDictionary<string, T>> FindAsync(IEnumerable<string> numbers)
        /// <summary>
        /// 获取实体对象字典
        /// </summary>
        /// <param name="numbers">编号集</param>
        /// <returns>实体对象字典</returns>
        /// <remarks>IDictionary[string, T]，[编号, 实体对象]</remarks>
        Task<IDictionary<string, T>> FindAsync(IEnumerable<string> numbers);
        #endregion

        #region # 获取子类对象字典 —— IDictionary<string, TSub> Find<TSub>(IEnumerable<string>...
        /// <summary>
        /// 获取子类对象字典
        /// </summary>
        /// <param name="numbers">编号集</param>
        /// <returns>子类对象字典</returns>
        /// <remarks>IDictionary[string, TSub]，[编号, 子类对象]</remarks>
        IDictionary<string, TSub> Find<TSub>(IEnumerable<string> numbers) where TSub : T;
        #endregion

        #region # 获取子类对象字典 —— Task<IDictionary<string, TSub>> FindAsync<TSub>(IEnumerable<string...
        /// <summary>
        /// 获取子类对象字典
        /// </summary>
        /// <param name="numbers">编号集</param>
        /// <returns>子类对象字典</returns>
        /// <remarks>IDictionary[string, TSub]，[编号, 子类对象]</remarks>
        Task<IDictionary<string, TSub>> FindAsync<TSub>(IEnumerable<string> numbers) where TSub : T;
        #endregion


        //Exists部分

        #region # 是否存在给定编号的实体对象 —— bool ExistsNo(string number)
        /// <summary>
        /// 是否存在给定编号的实体对象
        /// </summary>
        /// <param name="number">编号</param>
        /// <returns>是否存在</returns>
        bool ExistsNo(string number);
        #endregion

        #region # 是否存在给定编号的实体对象 —— Task<bool> ExistsNoAsync(string number)
        /// <summary>
        /// 是否存在给定编号的实体对象
        /// </summary>
        /// <param name="number">编号</param>
        /// <returns>是否存在</returns>
        Task<bool> ExistsNoAsync(string number);
        #endregion

        #region # 是否存在给定编号的子类对象 —— bool ExistsNo<TSub>(string number)
        /// <summary>
        /// 是否存在给定编号的子类对象
        /// </summary>
        /// <param name="number">编号</param>
        /// <returns>是否存在</returns>
        bool ExistsNo<TSub>(string number) where TSub : T;
        #endregion

        #region # 是否存在给定编号的子类对象 —— Task<bool> ExistsNoAsync<TSub>(string number)
        /// <summary>
        /// 是否存在给定编号的子类对象
        /// </summary>
        /// <param name="number">编号</param>
        /// <returns>是否存在</returns>
        Task<bool> ExistsNoAsync<TSub>(string number) where TSub : T;
        #endregion

        #region # 是否存在给定编号的实体对象 —— bool ExistsNo(Guid? id, string number)
        /// <summary>
        /// 是否存在给定编号的实体对象
        /// </summary>
        /// <param name="id">标识id</param>
        /// <param name="number">编号</param>
        /// <returns>是否存在</returns>
        bool ExistsNo(Guid? id, string number);
        #endregion

        #region # 是否存在给定编号的实体对象 —— Task<bool> ExistsNoAsync(Guid? id, string number)
        /// <summary>
        /// 是否存在给定编号的实体对象
        /// </summary>
        /// <param name="id">标识id</param>
        /// <param name="number">编号</param>
        /// <returns>是否存在</returns>
        Task<bool> ExistsNoAsync(Guid? id, string number);
        #endregion

        #region # 是否存在给定编号的子类对象 —— bool ExistsNo<TSub>(Guid? id, string number)
        /// <summary>
        /// 是否存在给定编号的子类对象
        /// </summary>
        /// <param name="id">标识id</param>
        /// <param name="number">编号</param>
        /// <returns>是否存在</returns>
        bool ExistsNo<TSub>(Guid? id, string number) where TSub : T;
        #endregion

        #region # 是否存在给定编号的子类对象 —— Task<bool> ExistsNoAsync<TSub>(Guid? id, string number)
        /// <summary>
        /// 是否存在给定编号的子类对象
        /// </summary>
        /// <param name="id">标识id</param>
        /// <param name="number">编号</param>
        /// <returns>是否存在</returns>
        Task<bool> ExistsNoAsync<TSub>(Guid? id, string number) where TSub : T;
        #endregion

        #region # 是否存在给定名称的实体对象 —— bool ExistsName(string name)
        /// <summary>
        /// 是否存在给定名称的实体对象
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>是否存在</returns>
        bool ExistsName(string name);
        #endregion

        #region # 是否存在给定名称的实体对象 —— Task<bool> ExistsNameAsync(string name)
        /// <summary>
        /// 是否存在给定名称的实体对象
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>是否存在</returns>
        Task<bool> ExistsNameAsync(string name);
        #endregion

        #region # 是否存在给定名称的子类对象 —— bool ExistsName<TSub>(string name)
        /// <summary>
        /// 是否存在给定名称的子类对象
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>是否存在</returns>
        bool ExistsName<TSub>(string name) where TSub : T;
        #endregion

        #region # 是否存在给定名称的子类对象 —— Task<bool> ExistsNameAsync<TSub>(string name)
        /// <summary>
        /// 是否存在给定名称的子类对象
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>是否存在</returns>
        Task<bool> ExistsNameAsync<TSub>(string name) where TSub : T;
        #endregion

        #region # 是否存在给定名称的实体对象 —— bool ExistsName(Guid? id, string name)
        /// <summary>
        /// 是否存在给定名称的实体对象
        /// </summary>
        /// <param name="id">标识id</param>
        /// <param name="name">名称</param>
        /// <returns>是否存在</returns>
        bool ExistsName(Guid? id, string name);
        #endregion

        #region # 是否存在给定名称的实体对象 —— Task<bool> ExistsNameAsync(Guid? id, string name)
        /// <summary>
        /// 是否存在给定名称的实体对象
        /// </summary>
        /// <param name="id">标识id</param>
        /// <param name="name">名称</param>
        /// <returns>是否存在</returns>
        Task<bool> ExistsNameAsync(Guid? id, string name);
        #endregion

        #region # 是否存在给定名称的子类对象 —— bool ExistsName<TSub>(Guid? id, string name)
        /// <summary>
        /// 是否存在给定名称的子类对象
        /// </summary>
        /// <param name="id">标识id</param>
        /// <param name="name">名称</param>
        /// <returns>是否存在</returns>
        bool ExistsName<TSub>(Guid? id, string name) where TSub : T;
        #endregion

        #region # 是否存在给定名称的子类对象 —— Task<bool> ExistsNameAsync<TSub>(Guid? id, string name)
        /// <summary>
        /// 是否存在给定名称的子类对象
        /// </summary>
        /// <param name="id">标识id</param>
        /// <param name="name">名称</param>
        /// <returns>是否存在</returns>
        Task<bool> ExistsNameAsync<TSub>(Guid? id, string name) where TSub : T;
        #endregion

        #region # 是否存在给定名称的实体对象 —— bool ExistsName(string number, string name)
        /// <summary>
        /// 是否存在给定名称的实体对象
        /// </summary>
        /// <param name="number">编号</param>
        /// <param name="name">名称</param>
        /// <returns>是否存在</returns>
        bool ExistsName(string number, string name);
        #endregion

        #region # 是否存在给定名称的实体对象 —— Task<bool> ExistsNameAsync(string number, string name)
        /// <summary>
        /// 是否存在给定名称的实体对象
        /// </summary>
        /// <param name="number">编号</param>
        /// <param name="name">名称</param>
        /// <returns>是否存在</returns>
        Task<bool> ExistsNameAsync(string number, string name);
        #endregion

        #region # 是否存在给定名称的子类对象 —— bool ExistsName<TSub>(string number, string name)
        /// <summary>
        /// 是否存在给定名称的子类对象
        /// </summary>
        /// <param name="number">编号</param>
        /// <param name="name">名称</param>
        /// <returns>是否存在</returns>
        bool ExistsName<TSub>(string number, string name) where TSub : T;
        #endregion

        #region # 是否存在给定名称的子类对象 —— Task<bool> ExistsNameAsync<TSub>(string number, string name)
        /// <summary>
        /// 是否存在给定名称的子类对象
        /// </summary>
        /// <param name="number">编号</param>
        /// <param name="name">名称</param>
        /// <returns>是否存在</returns>
        Task<bool> ExistsNameAsync<TSub>(string number, string name) where TSub : T;
        #endregion
    }
}
