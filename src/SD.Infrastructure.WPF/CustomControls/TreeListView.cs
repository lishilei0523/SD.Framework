using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;

namespace SD.Infrastructure.WPF.CustomControls
{
    /// <summary>
    /// �����б���ͼ
    /// </summary>
    public class TreeListView : TreeView
    {
        /// <summary>
        /// Ĭ�Ϲ�����
        /// </summary>
        public TreeListView()
        {
            //���Tag���������������к�
            this.Loaded += (sender, _) => ((TreeListView)sender).Tag = null;
        }

        /// <summary>
        /// ���������¼�
        /// </summary>
        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs eventArgs)
        {
            //���Tag���������������к�
            this.Tag = null;
            base.OnItemsChanged(eventArgs);
        }

        /// <summary>
        /// ��ȡ�����и���Ԫ��
        /// </summary>
        protected override DependencyObject GetContainerForItemOverride()
        {
            TreeListViewItem treeListViewItem = new TreeListViewItem();

            return treeListViewItem;
        }

        /// <summary>
        /// �Ƿ��Ѹ�������Ĭ��Ԫ��
        /// </summary>
        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is TreeListViewItem;
        }
    }
}
