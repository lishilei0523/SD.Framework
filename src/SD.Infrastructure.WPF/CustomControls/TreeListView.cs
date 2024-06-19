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
        /// ������λ�ߴ���������
        /// </summary>
        public static readonly DependencyProperty IndentUnitSizeProperty;

        /// <summary>
        /// �м�����������
        /// </summary>
        public static readonly DependencyProperty ColumnsDefinitionProperty;

        /// <summary>
        /// ��̬������
        /// </summary>
        static TreeListView()
        {
            //ע����������
            IndentUnitSizeProperty = DependencyProperty.Register(nameof(IndentUnitSize), typeof(double), typeof(TreeListView), new PropertyMetadata(14.0));
            ColumnsDefinitionProperty = DependencyProperty.Register(nameof(ColumnsDefinition), typeof(GridViewColumnCollection), typeof(TreeListView));
        }

        /// <summary>
        /// ������λ�ߴ�
        /// </summary>
        public double IndentUnitSize
        {
            get { return (double)base.GetValue(IndentUnitSizeProperty); }
            set { base.SetValue(IndentUnitSizeProperty, value); }
        }

        /// <summary>
        /// �ж���
        /// </summary>
        public GridViewColumnCollection ColumnsDefinition
        {
            get { return (GridViewColumnCollection)base.GetValue(ColumnsDefinitionProperty); }
            set { base.SetValue(ColumnsDefinitionProperty, value); }
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
