using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SD.Infrastructure.DTOBase
{
    /// <summary>
    /// 分页数据模型
    /// </summary>
    /// <typeparam name="T">类型</typeparam>
    [Serializable]
    [DataContract]
    public class PageModel<T>
    {
        #region # 构造器

        /// <summary>
        /// 无参构造器
        /// </summary>
        public PageModel() { }

        /// <summary>
        /// 基础构造器
        /// </summary>
        /// <param name="datas">数据集</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页容量</param>
        /// <param name="pageCount">总页数</param>
        /// <param name="rowCount">总记录条数</param>
        public PageModel(IEnumerable<T> datas, int pageIndex, int pageSize, int pageCount, int rowCount)
            : this()
        {
            this.Datas = datas;
            this.PageIndex = pageIndex;
            this.PageSize = pageSize;
            this.PageCount = pageCount;
            this.RowCount = rowCount;
        }

        #endregion

        #region # 数据集 —— IEnumerable<T> Datas
        /// <summary>
        /// 数据集
        /// </summary>
        [DataMember]
        public IEnumerable<T> Datas { get; set; }
        #endregion

        #region # 页码 —— int PageIndex
        /// <summary>
        /// 页码
        /// </summary>
        [DataMember]
        public int PageIndex { get; set; }
        #endregion

        #region # 页容量 —— int PageSize
        /// <summary>
        /// 页容量
        /// </summary>
        [DataMember]
        public int PageSize { get; set; }
        #endregion

        #region # 总页数 —— int PageCount
        /// <summary>
        /// 总页数
        /// </summary>
        [DataMember]
        public int PageCount { get; set; }
        #endregion

        #region # 总记录条数 —— int RowCount
        /// <summary>
        /// 总记录条数
        /// </summary>
        [DataMember]
        public int RowCount { get; set; }
        #endregion
    }
}
