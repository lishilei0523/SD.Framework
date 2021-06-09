using SD.Infrastructure.WPF.CustomControls;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace SD.Infrastructure.WPF.Converters
{
    /// <summary>
    /// 树形列表视图缩进尺寸转换器
    /// </summary>
    public class TreeListViewIndentConverter : IValueConverter
    {
        /// <summary>
        /// 转换树形列表视图缩进尺寸
        /// </summary>
        public object Convert(object value, Type type, object parameter, CultureInfo culture)
        {
            TreeListViewItem treeListViewItem = (TreeListViewItem)value;
            TreeListView treeListView = this.GetTreeListView(treeListViewItem);

            int level/*树节点级别*/ = treeListViewItem.Level;
            double indentSize/*缩进尺寸*/ = level * treeListView.IndentUnitSize;
            Thickness margin = new Thickness(indentSize, 0, 0, 0);

            return margin;
        }

        /// <summary>
        /// 获取树形列表视图
        /// </summary>
        /// <param name="treeListViewItem">树形列表视图项</param>
        /// <returns>树形列表视图</returns>
        private TreeListView GetTreeListView(TreeListViewItem treeListViewItem)
        {
            ItemsControl parentItem = ItemsControl.ItemsControlFromItemContainer(treeListViewItem);
            if (parentItem is TreeListView treeListView)
            {
                return treeListView;
            }
            if (parentItem is TreeListViewItem parentTreeListViewItem)
            {
                return this.GetTreeListView(parentTreeListViewItem);
            }

            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type type, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
