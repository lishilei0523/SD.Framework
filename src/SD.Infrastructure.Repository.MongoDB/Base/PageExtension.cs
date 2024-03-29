﻿using MongoDB.Driver;
using SD.Infrastructure.EntityBase;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace SD.Infrastructure.RepositoryBase
{
    /// <summary>
    /// 分页扩展
    /// </summary>
    public static class PageExtension
    {
        #region # 分页 —— static Page<T> ToPage<T>(this IFindFluent<T, T>...
        /// <summary>
        /// 分页
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="orderedResult">已排序集合对象</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页容量</param>
        /// <returns>实体对象列表</returns>
        public static Page<T> ToPage<T>(this IFindFluent<T, T> orderedResult, int pageIndex, int pageSize)
        {
            int rowCount = (int)orderedResult.CountDocuments();
            int pageCount = (int)Math.Ceiling(rowCount * 1.0 / pageSize);
            IList<T> pagedResult = orderedResult.Skip((pageIndex - 1) * pageSize).Limit(pageSize).ToList();

            return new Page<T>(pagedResult, pageIndex, pageSize, pageCount, rowCount);
        }
        #endregion

        #region # 分页 —— static Task<Page<T>> ToPageAsync<T>(this IFindFluent<T, T>...
        /// <summary>
        /// 分页
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="orderedResult">已排序集合对象</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页容量</param>
        /// <returns>实体对象列表</returns>
        public static async Task<Page<T>> ToPageAsync<T>(this IFindFluent<T, T> orderedResult, int pageIndex, int pageSize)
        {
            int rowCount = (int)await orderedResult.CountDocumentsAsync();
            int pageCount = (int)Math.Ceiling(rowCount * 1.0 / pageSize);
            IList<T> pagedResult = await orderedResult.Skip((pageIndex - 1) * pageSize).Limit(pageSize).ToListAsync();

            return new Page<T>(pagedResult, pageIndex, pageSize, pageCount, rowCount);
        }
        #endregion
    }
}
