using SD.Infrastructure.WPF.CustomControls;
using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace SD.Infrastructure.WPF.Converters
{
    /// <summary>
    /// 树形列表视图行号转换器
    /// </summary>
    public class TreeListViewRowIndexConverter : IValueConverter
    {
        /// <summary>
        /// 树形列表视图元素
        /// </summary>
        private TreeListView _treeListView;

        /// <summary>
        /// 转换树形列表视图项行号
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            TreeListViewItem treeListViewItem = (TreeListViewItem)value;
            this.GetTreeListView(treeListViewItem);

            int index = this.GetRelativeIndex(treeListViewItem);
            this.GetAbsoluteIndex(treeListViewItem, ref index);

            if (this._treeListView.Tag != null)
            {
                int seed = (int)this._treeListView.Tag;
                index = seed + 1;
            }
            this._treeListView.Tag = index;

            return index;
        }

        /// <summary>
        /// 获取树形列表视图
        /// </summary>
        /// <param name="treeListViewItem">树形列表视图项</param>
        private void GetTreeListView(TreeListViewItem treeListViewItem)
        {
            ItemsControl parentItem = ItemsControl.ItemsControlFromItemContainer(treeListViewItem);
            if (parentItem == null)
            {
                return;
            }
            if (parentItem is TreeListView treeListView)
            {
                this._treeListView = treeListView;
                return;
            }
            if (parentItem is TreeListViewItem parentTreeListViewItem)
            {
                this.GetTreeListView(parentTreeListViewItem);
            }
        }

        /// <summary>
        /// 获取绝对行号
        /// </summary>
        private void GetAbsoluteIndex(ItemsControl item, ref int relativeIndex)
        {
            ItemsControl parentItem = ItemsControl.ItemsControlFromItemContainer(item);
            if (parentItem != null)
            {
                if (parentItem is TreeListViewItem)
                {
                    int parentRelativeIndex = this.GetRelativeIndex(parentItem);
                    relativeIndex += parentRelativeIndex;
                    this.GetAbsoluteIndex(parentItem, ref relativeIndex);
                }
            }
        }

        /// <summary>
        /// 获取相对行号
        /// </summary>
        private int GetRelativeIndex(ItemsControl item)
        {
            int index = 1;

            ItemsControl parentItem = ItemsControl.ItemsControlFromItemContainer(item);
            if (parentItem != null)
            {
                index = parentItem.ItemContainerGenerator.IndexFromContainer(item) + 1;
            }

            return index;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
