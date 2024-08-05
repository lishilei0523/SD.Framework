using System.Windows;
using System.Windows.Controls;

namespace SD.Infrastructure.WPF.CustomControls
{
    /// <summary>
    /// 树形列表视图项
    /// </summary>
    public class TreeListViewItem : TreeViewItem
    {
        #region # 字段及构造器

        /// <summary>
        /// 树节点层级
        /// </summary>
        private int _level;

        /// <summary>
        /// 默认构造器
        /// </summary>
        public TreeListViewItem()
        {
            this._level = -1;
        }

        #endregion

        #region # 属性

        #region 只读属性 - 树节点层级 —— int Level
        /// <summary>
        /// 只读属性 - 树节点层级
        /// </summary>
        public int Level
        {
            get
            {
                if (this._level == -1)
                {
                    TreeListViewItem parent = ItemsControl.ItemsControlFromItemContainer(this) as TreeListViewItem;
                    this._level = parent?.Level + 1 ?? 0;
                }
                return this._level;
            }
        }
        #endregion 

        #endregion

        #region # 方法

        #region 获取容器中覆盖元素 —— override DependencyObject GetContainerForItemOverride()
        /// <summary>
        /// 获取容器中覆盖元素
        /// </summary>
        protected override DependencyObject GetContainerForItemOverride()
        {
            TreeListViewItem treeListViewItem = new TreeListViewItem();

            return treeListViewItem;
        }
        #endregion

        #region 是否已覆盖容器默认元素 —— override bool IsItemItsOwnContainerOverride(object item)
        /// <summary>
        /// 是否已覆盖容器默认元素
        /// </summary>
        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is TreeListViewItem;
        }
        #endregion 

        #endregion
    }
}
