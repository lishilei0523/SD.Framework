using SD.Infrastructure.WPF.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

namespace SD.Infrastructure.WPF.Extensions
{
    /// <summary>
    /// 数据项扩展
    /// </summary>
    public static class ItemExtension
    {
        /// <summary>
        /// 分组数据项列表
        /// </summary>
        /// <param name="items">数据项列表</param>
        public static void Group(this ObservableCollection<Item> items)
        {
            PropertyGroupDescription groupKeyDescription = new PropertyGroupDescription(nameof(Item.GroupKey));
            ICollectionView collectionView = CollectionViewSource.GetDefaultView(items);
            collectionView?.GroupDescriptions.Add(groupKeyDescription);
        }
    }
}
