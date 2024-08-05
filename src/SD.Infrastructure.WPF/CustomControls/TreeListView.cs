using System.Windows;
using System.Windows.Controls;

namespace SD.Infrastructure.WPF.CustomControls
{
    /// <summary>
    /// �����б���ͼ
    /// </summary>
    public class TreeListView : TreeView
    {
        #region # �ֶμ�������

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

        #endregion

        #region # ����

        #region �������� - ������λ�ߴ� ���� double IndentUnitSize
        /// <summary>
        /// �������� - ������λ�ߴ�
        /// </summary>
        public double IndentUnitSize
        {
            get => (double)base.GetValue(IndentUnitSizeProperty);
            set => base.SetValue(IndentUnitSizeProperty, value);
        }
        #endregion

        #region �������� - �ж��� ���� GridViewColumnCollection ColumnsDefinition
        /// <summary>
        /// �������� - �ж���
        /// </summary>
        public GridViewColumnCollection ColumnsDefinition
        {
            get => (GridViewColumnCollection)base.GetValue(ColumnsDefinitionProperty);
            set => base.SetValue(ColumnsDefinitionProperty, value);
        }
        #endregion

        #endregion

        #region # ����

        #region ��ȡ�����и���Ԫ�� ���� override DependencyObject GetContainerForItemOverride()
        /// <summary>
        /// ��ȡ�����и���Ԫ��
        /// </summary>
        protected override DependencyObject GetContainerForItemOverride()
        {
            TreeListViewItem treeListViewItem = new TreeListViewItem();

            return treeListViewItem;
        }
        #endregion

        #region �Ƿ��Ѹ�������Ĭ��Ԫ�� ���� override bool IsItemItsOwnContainerOverride(object item)
        /// <summary>
        /// �Ƿ��Ѹ�������Ĭ��Ԫ��
        /// </summary>
        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is TreeListViewItem;
        }
        #endregion 

        #endregion
    }
}
