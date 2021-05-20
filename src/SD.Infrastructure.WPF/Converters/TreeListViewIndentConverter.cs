using SD.Infrastructure.WPF.CustomControls;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SD.Infrastructure.WPF.Converters
{
    /// <summary>
    /// 树形列表视图缩进尺寸转换器
    /// </summary>
    public class TreeListViewIndentConverter : IValueConverter
    {
        /// <summary>
        /// 缩进单位尺寸
        /// </summary>
        private const double IndentUnitSize = 14.0;

        /// <summary>
        /// 转换树形列表视图缩进尺寸
        /// </summary>
        public object Convert(object value, Type type, object parameter, CultureInfo culture)
        {
            TreeListViewItem treeListViewItem = (TreeListViewItem)value;
            int level/*树节点级别*/ = treeListViewItem.Level;
            double indentSize/*缩进尺寸*/ = level * IndentUnitSize;
            Thickness margin = new Thickness(indentSize, 0, 0, 0);

            return margin;
        }

        public object ConvertBack(object value, Type type, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
