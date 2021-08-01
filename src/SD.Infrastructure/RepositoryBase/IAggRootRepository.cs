using SD.Infrastructure.EntityBase;
using System;
using System.Collections.Generic;

namespace SD.Infrastructure.RepositoryBase
{
    /// <summary>
    /// 聚合根仓储基接口
    /// </summary>
    /// <typeparam name="T">聚合根类型</typeparam>
    public interface IAggRootRepository<T> : IEntityRepository<T> where T : AggregateRootEntity
    {
        //Single部分

        #region # 根据编号获取唯一实体对象（查看时用） —— T SingleOrDefault(string number)
        /// <summary>
        /// 根据编号获取唯一实体对象（查看时用），
        /// </summary>
        /// <param name="number">编号</param>
        /// <returns>实体对象</returns>
        /// <remarks>无该对象时返回null</remarks>
        T SingleOrDefault(string number);
        #endregion

        #region # 根据编号获取唯一子类对象（查看时用） —— TSub SingleOrDefault<TSub>(string number)
        /// <summary>
        /// 根据编号获取唯一子类对象（查看时用），
        /// </summary>
        /// <param name="number">编号</param>
        /// <returns>子类对象</returns>
        /// <remarks>无该对象时返回null</remarks>
        TSub SingleOrDefault<TSub>(string number) where TSub : T;
        #endregion

        #region # 根据编号获取唯一实体对象（查看时用） —— T Single(string number)
        /// <summary>
        /// 根据编号获取唯一实体对象（查看时用），
        /// </summary>
        /// <param name="number">编号</param>
        /// <returns>实体对象</returns>
        T Single(string number);
        #endregion

        #region # 根据编号获取唯一子类对象（查看时用） —— TSub Single<TSub>(string number)
        /// <summary>
        /// 根据编号获取唯一子类对象（查看时用），
        /// </summary>
        /// <param name="number">编号</param>
        /// <returns>子类对象</returns>
        TSub Single<TSub>(string number) where TSub : T;
        #endregion


        //ICollection部分

        #region # 根据关键字获取实体对象列表 —— ICollection<T> Find(string keywords)
        /// <summary>
        /// 根据关键字获取实体对象列表
        /// </summary>
        /// <returns>实体对象列表</returns>
        ICollection<T> Find(string keywords);
        #endregion

        #region # 根据关键字获取子类对象列表 —— ICollection<TSub> Find<TSub>(string keywords)
        /// <summary>
        /// 根据关键字获取子类对象列表
        /// </summary>
        /// <typeparam name="TSub">子类类型</typeparam>
        /// <returns>子类对象列表</returns>
        ICollection<TSub> Find<TSub>(string keywords) where TSub : T;
        #endregion

        #region # 根据关键字分页获取实体对象列表 —— ICollection<T> FindByPage(...
        /// <summary>
        /// 根据关键字分页获取实体对象列表
        /// </summary>
        /// <param name="keywords">关键字</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页容量</param>
        /// <param name="rowCount">总记录条数</param>
        /// <param name="pageCount">总页数</param>
        /// <returns>实体对象列表</returns>
        ICollection<T> FindByPage(string keywords, int pageIndex, int pageSize, out int rowCount, out int pageCount);
        #endregion

        #region # 根据关键字分页获取子类对象列表 —— ICollection<TSub> FindByPage...
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
        ICollection<TSub> FindByPage<TSub>(string keywords, int pageIndex, int pageSize, out int rowCount, out int pageCount) where TSub : T;
        #endregion


        //IDictionary部分

        #region # 根据编号集获取实体对象字典 —— IDictionary<string, T> Find(IEnumerable<string> numbers)
        /// <summary>
        /// 根据编号集获取实体对象字典
        /// </summary>
        /// <returns>实体对象字典</returns>
        /// <remarks>IDictionary[string, T]，[编号, 实体对象]</remarks>
        IDictionary<string, T> Find(IEnumerable<string> numbers);
        #endregion

        #region # 根据编号集获取子类对象字典 —— IDictionary<string, TSub> Find<TSub>(IEnumerable<string>...
        /// <summary>
        /// 根据编号集获取子类对象字典
        /// </summary>
        /// <returns>子类对象字典</returns>
        /// <remarks>IDictionary[string, TSub]，[编号, 子类对象]</remarks>
        IDictionary<string, TSub> Find<TSub>(IEnumerable<string> numbers) where TSub : T;
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

        #region # 是否存在给定编号的子类对象 —— bool ExistsNo<TSub>(string number)
        /// <summary>
        /// 是否存在给定编号的子类对象
        /// </summary>
        /// <param name="number">编号</param>
        /// <returns>是否存在</returns>
        bool ExistsNo<TSub>(string number) where TSub : T;
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

        #region # 是否存在给定编号的子类对象 —— bool ExistsNo<TSub>(Guid? id, string number)
        /// <summary>
        /// 是否存在给定编号的子类对象
        /// </summary>
        /// <param name="id">标识id</param>
        /// <param name="number">编号</param>
        /// <returns>是否存在</returns>
        bool ExistsNo<TSub>(Guid? id, string number) where TSub : T;
        #endregion

        #region # 是否存在给定名称的实体对象 —— bool ExistsName(string name)
        /// <summary>
        /// 是否存在给定名称的实体对象
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>是否存在</returns>
        bool ExistsName(string name);
        #endregion

        #region # 是否存在给定名称的子类对象 —— bool ExistsName<TSub>(string name)
        /// <summary>
        /// 是否存在给定名称的子类对象
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>是否存在</returns>
        bool ExistsName<TSub>(string name) where TSub : T;
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

        #region # 是否存在给定名称的子类对象 —— bool ExistsName<TSub>(Guid? id, string name)
        /// <summary>
        /// 是否存在给定名称的子类对象
        /// </summary>
        /// <param name="id">标识id</param>
        /// <param name="name">名称</param>
        /// <returns>是否存在</returns>
        bool ExistsName<TSub>(Guid? id, string name) where TSub : T;
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

        #region # 是否存在给定名称的子类对象 —— bool ExistsName<TSub>(string number, string name)
        /// <summary>
        /// 是否存在给定名称的子类对象
        /// </summary>
        /// <param name="number">编号</param>
        /// <param name="name">名称</param>
        /// <returns>是否存在</returns>
        bool ExistsName<TSub>(string number, string name) where TSub : T;
        #endregion
    }
}
