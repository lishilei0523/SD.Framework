using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;

namespace SD.Infrastructure.WPF.CustomControls
{
    /// <summary>
    /// 树形列表视图
    /// </summary>
    public class TreeListView : TreeView
    {
        /// <summary>
        /// 默认构造器
        /// </summary>
        public TreeListView()
        {
            //清空Tag，用于重新生成行号
            this.Loaded += (sender, _) => ((TreeListView)sender).Tag = null;
        }

        /// <summary>
        /// 数据项变更事件
        /// </summary>
        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs eventArgs)
        {
            //清空Tag，用于重新生成行号
            this.Tag = null;
            base.OnItemsChanged(eventArgs);
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
