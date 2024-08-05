using System.Windows;
using System.Windows.Controls;

namespace SD.Infrastructure.WPF.CustomControls
{
    /// <summary>
    /// 树形列表视图
    /// </summary>
    public class TreeListView : TreeView
    {
        #region # 字段及构造器

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

        #endregion

        #region # 属性

        #region 依赖属性 - 缩进单位尺寸 —— double IndentUnitSize
        /// <summary>
        /// 依赖属性 - 缩进单位尺寸
        /// </summary>
        public double IndentUnitSize
        {
            get => (double)base.GetValue(IndentUnitSizeProperty);
            set => base.SetValue(IndentUnitSizeProperty, value);
        }
        #endregion

        #region 依赖属性 - 列定义 —— GridViewColumnCollection ColumnsDefinition
        /// <summary>
        /// 依赖属性 - 列定义
        /// </summary>
        public GridViewColumnCollection ColumnsDefinition
        {
            get => (GridViewColumnCollection)base.GetValue(ColumnsDefinitionProperty);
            set => base.SetValue(ColumnsDefinitionProperty, value);
        }
        #endregion

        #endregion

        #region # 方法

        #region 获取容器中覆盖元素 —— override DependencyObject GetContainerForItemOverride()
        /// <summary>
        /// 获取容器中覆盖元素
        /// </summary>
        protected override DependencyObject GetContainerForItemOverride()
        {
            TreeListViewItem treeListViewItem = new TreeListViewItem();

            return treeListViewItem;
        }
        #endregion

        #region 是否已覆盖容器默认元素 —— override bool IsItemItsOwnContainerOverride(object item)
        /// <summary>
        /// 是否已覆盖容器默认元素
        /// </summary>
        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is TreeListViewItem;
        }
        #endregion 

        #endregion
    }
}
