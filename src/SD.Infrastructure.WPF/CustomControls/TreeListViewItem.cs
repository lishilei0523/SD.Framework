using System.Windows;
using System.Windows.Controls;

namespace SD.Infrastructure.WPF.CustomControls
{
    /// <summary>
    /// 树形列表视图项
    /// </summary>
    public class TreeListViewItem : TreeViewItem
    {
        /// <summary>
        /// 树节点层级字段
        /// </summary>
        private int _level = -1;

        /// <summary>
        /// 树节点层级属性
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

        /// <summary>
        /// 获取容器中覆盖元素
        /// </summary>
        protected override DependencyObject GetContainerForItemOverride()
        {
            TreeListViewItem treeListViewItem = new TreeListViewItem();

            return treeListViewItem;
        }

        /// <summary>
        /// 是否已覆盖容器默认元素
        /// </summary>
        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is TreeListViewItem;
        }
    }
}
