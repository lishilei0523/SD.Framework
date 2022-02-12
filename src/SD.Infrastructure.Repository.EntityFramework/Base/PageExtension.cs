using SD.Infrastructure.EntityBase;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace SD.Infrastructure.RepositoryBase
{
    /// <summary>
    /// 分页扩展
    /// </summary>
    public static class PageExtension
    {
        #region # 分页 —— static Page<T> ToPage<T>(this IOrderedQueryable<T>...
        /// <summary>
        /// 分页
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="orderedResult">已排序集合对象</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页容量</param>
        /// <returns>实体对象列表</returns>
        public static Page<T> ToPage<T>(this IOrderedQueryable<T> orderedResult, int pageIndex, int pageSize)
        {
            int rowCount = orderedResult.Count();
            int pageCount = (int)Math.Ceiling(rowCount * 1.0 / pageSize);
            IList<T> pagedResult = orderedResult.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            return new Page<T>(pagedResult, pageIndex, pageSize, pageCount, rowCount);
        }
        #endregion

        #region # 分页 —— static Task<Page<T>> ToPageAsync<T>(this IOrderedQueryable<T>...
        /// <summary>
        /// 分页
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="orderedResult">已排序集合对象</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页容量</param>
        /// <returns>实体对象列表</returns>
        public static async Task<Page<T>> ToPageAsync<T>(this IOrderedQueryable<T> orderedResult, int pageIndex, int pageSize)
        {
            int rowCount = await orderedResult.CountAsync();
            int pageCount = (int)Math.Ceiling(rowCount * 1.0 / pageSize);
            IList<T> pagedResult = await orderedResult.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

            return new Page<T>(pagedResult, pageIndex, pageSize, pageCount, rowCount);
        }
        #endregion
    }
}
