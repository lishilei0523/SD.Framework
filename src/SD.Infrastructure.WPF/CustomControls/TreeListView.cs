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
        /// 缩进单位尺寸依赖属性
        /// </summary>
        public static readonly DependencyProperty IndentUnitSizeProperty;

        /// <summary>
        /// 列集合依赖属性
        /// </summary>
        public static readonly DependencyProperty ColumnsDefinitionProperty;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static TreeListView()
        {
            //注册依赖属性
            IndentUnitSizeProperty = DependencyProperty.Register(nameof(IndentUnitSize), typeof(double), typeof(TreeListView), new PropertyMetadata(14.0));
            ColumnsDefinitionProperty = DependencyProperty.Register(nameof(ColumnsDefinition), typeof(GridViewColumnCollection), typeof(TreeListView));
        }

        /// <summary>
        /// 缩进单位尺寸
        /// </summary>
        public double IndentUnitSize
        {
            get { return (double)base.GetValue(IndentUnitSizeProperty); }
            set { base.SetValue(IndentUnitSizeProperty, value); }
        }

        /// <summary>
        /// 列定义
        /// </summary>
        public GridViewColumnCollection ColumnsDefinition
        {
            get { return (GridViewColumnCollection)base.GetValue(ColumnsDefinitionProperty); }
            set { base.SetValue(ColumnsDefinitionProperty, value); }
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
