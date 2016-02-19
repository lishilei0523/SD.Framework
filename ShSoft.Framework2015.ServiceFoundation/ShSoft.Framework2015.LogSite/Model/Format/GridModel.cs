namespace ShSoft.Framework2015.LogSite.Model.Format
{
    /// <summary>
    /// EasyUI DataGridModel模型
    /// </summary>
    public class GridModel
    {
        public GridModel(int total, object rows)
        {
            this.total = total;
            this.rows = rows;
        }

        #region 01.总记录条数
        /// <summary>
        /// 总记录条数
        /// </summary>
        public int total { get; set; }
        #endregion

        #region 02.数据
        /// <summary>
        /// 数据
        /// </summary>
        public object rows { get; set; }
        #endregion
    }
}
