using System.Collections.Generic;

namespace ShSoft.Framework2015.LogSite.Model.Format
{
    /// <summary>
    /// 分页模型
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    public class PageModel<T> where T : class, new()
    {
        #region 01.数据
        /// <summary>
        /// 数据
        /// </summary>
        public List<T> Data { get; set; } 
        #endregion

        #region 02.页码
        /// <summary>
        /// 页码
        /// </summary>
        public int PageIndex { get; set; } 
        #endregion

        #region 03.页容量
        /// <summary>
        /// 页容量
        /// </summary>
        public int PageSize { get; set; } 
        #endregion

        #region 04.总页数
        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount { get; set; } 
        #endregion

        #region 05.总记录条数
        /// <summary>
        /// 总记录条数
        /// </summary>
        public int RowCount { get; set; } 
        #endregion
    }
}
