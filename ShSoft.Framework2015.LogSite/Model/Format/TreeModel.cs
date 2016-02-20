using System.Collections.Generic;

namespace ShSoft.Framework2015.LogSite.Model.Format
{
    /// <summary>
    /// 为EasyUI Tree封装的Model
    /// </summary>
    public class TreeModel
    {
        #region 01.节点Id
        /// <summary>
        /// 节点Id
        /// </summary>
        public int id { get; set; }
        #endregion

        #region 02.节点文本
        /// <summary>
        /// 节点文本
        /// </summary>
        public string text { get; set; }
        #endregion

        #region 03.节点状态
        /// <summary>
        /// 节点状态，“open”或“closed”
        /// </summary>
        public string state { get; set; }
        #endregion

        #region 04.是否选中
        /// <summary>
        /// 是否选中
        /// </summary>
        public bool @checked { get; set; }
        #endregion

        #region 05.自定义特性
        /// <summary>
        /// 自定义特性
        /// </summary>
        public object attributes { get; set; }
        #endregion

        #region 06.子节点集合
        /// <summary>
        /// 子节点集合
        /// </summary>
        public List<TreeModel> children { get; set; }
        #endregion
    }
}
