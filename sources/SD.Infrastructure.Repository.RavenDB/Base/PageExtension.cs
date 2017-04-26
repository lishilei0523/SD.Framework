using Raven.Client.Linq;
using SD.Infrastructure.EntityBase;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace SD.Infrastructure.Repository.RavenDB.Base
{
    /// <summary>
    /// 分页扩展工具类
    /// </summary>
    public static class PageExtension
    {
        #region # 分页扩展方法 —— static IRavenQueryable<T> ToPage<T>(...
        /// <summary>
        /// 分页扩展方法
        /// </summary>
        /// <typeparam name="T">领域实体</typeparam>
        /// <param name="queryable">集合对象</param>
        /// <param name="predicate">查询条件</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页容量</param>
        /// <param name="rowCount">总记录条数</param>
        /// <param name="pageCount">总页数</param>
        /// <returns>对象集合</returns>
        public static IRavenQueryable<T> ToPage<T>(this IRavenQueryable<T> queryable, Expression<Func<T, bool>> predicate, int pageIndex, int pageSize, out int rowCount, out int pageCount)
            where T : PlainEntity
        {
            IRavenQueryable<T> list = queryable.Where(predicate);
            rowCount = list.Count();
            pageCount = (int)Math.Ceiling(rowCount * 1.0 / pageSize);
            return list.OrderByDescending(x => x.AddedTime).Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }
        #endregion
    }
}
