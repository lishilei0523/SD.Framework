using SD.Infrastructure.WPF.CustomControls;
using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace SD.Infrastructure.WPF.Converters
{
    /// <summary>
    /// 树形列表视图交替背景转换器
    /// </summary>
    public class TreeListViewBackgroundConverter : IValueConverter
    {
        /// <summary>
        /// 转换树形列表视图背景
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            TreeListViewItem treeListViewItem = (TreeListViewItem)value;
            ItemsControl parentItem = ItemsControl.ItemsControlFromItemContainer(treeListViewItem);

            int parentIndex = 0;
            if (parentItem != null && parentItem is TreeListViewItem parentTreeListViewItem)
            {
                parentIndex = this.GetRelativeIndex(parentTreeListViewItem);
            }

            int level = treeListViewItem.Level;
            int index = this.GetRelativeIndex(treeListViewItem);
            int levelBit = level % 2;
            int indexBit = (index + parentIndex) % 2;

            if (levelBit == 0 && indexBit == 0)
            {
                return new SolidColorBrush(Colors.White);
            }
            if (levelBit == 0 && indexBit != 0)
            {
                return new SolidColorBrush(Colors.WhiteSmoke);
            }
            if (levelBit != 0 && indexBit == 0)
            {
                return new SolidColorBrush(Colors.WhiteSmoke);
            }
            if (levelBit != 0 && indexBit != 0)
            {
                return new SolidColorBrush(Colors.White);
            }

            throw new NotSupportedException();
        }

        /// <summary>
        /// 获取相对索引
        /// </summary>
        private int GetRelativeIndex(ItemsControl item)
        {
            int index = 0;
            ItemsControl parentItem = ItemsControl.ItemsControlFromItemContainer(item);
            if (parentItem != null)
            {
                index = parentItem.ItemContainerGenerator.IndexFromContainer(item);
            }

            return index;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
