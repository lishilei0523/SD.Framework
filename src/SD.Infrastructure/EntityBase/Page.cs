using System;
using System.Collections.Generic;

namespace SD.Infrastructure.EntityBase
{
    /// <summary>
    /// 分页集合
    /// </summary>
    /// <typeparam name="T">类型</typeparam>
    [Serializable]
    public class Page<T> : List<T>
    {
        #region # 构造器

        #region 00.无参构造器
        /// <summary>
        /// 无参构造器
        /// </summary>
        public Page() { }
        #endregion

        #region 01.基础构造器
        /// <summary>
        /// 基础构造器
        /// </summary>
        /// <param name="enumerable">数据集</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页容量</param>
        /// <param name="pageCount">总页数</param>
        /// <param name="rowCount">总记录数</param>
        public Page(IEnumerable<T> enumerable, int pageIndex, int pageSize, int pageCount, int rowCount)
            : base(enumerable)
        {
            this.PageIndex = pageIndex;
            this.PageSize = pageSize;
            this.PageCount = pageCount;
            this.RowCount = rowCount;
        }
        #endregion

        #endregion

        #region # 属性

        #region 页码 —— int PageIndex
        /// <summary>
        /// 页码
        /// </summary>
        public int PageIndex { get; set; }
        #endregion

        #region 页容量 —— int PageSize
        /// <summary>
        /// 页容量
        /// </summary>
        public int PageSize { get; set; }
        #endregion

        #region 总页数 —— int PageCount
        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount { get; set; }
        #endregion

        #region 总记录数 —— int RowCount
        /// <summary>
        /// 总记录数
        /// </summary>
        public int RowCount { get; set; }
        #endregion

        #endregion
    }
}
