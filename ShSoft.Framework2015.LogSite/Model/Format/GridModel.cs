namespace ShSoft.Framework2015.LogSite.Model.Format
{
    /// <summary>
    /// EasyUI DataGridModel模型
    /// </summary>
    public class GridModel
    {
        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="total">总记录条数</param>
        /// <param name="rows">数据集</param>
        public GridModel(int total, object rows)
        {
            this.total = total;
            this.rows = rows;
        }

        #region 总记录条数
        /// <summary>
        /// 总记录条数
        /// </summary>
        public int total { get; set; }
        #endregion

        #region 数据集
        /// <summary>
        /// 数据集
        /// </summary>
        public object rows { get; set; }
        #endregion
    }
}
