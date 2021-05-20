using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace SD.Infrastructure.WPF.Models
{
    /// <summary>
    /// 树节点
    /// </summary>
    public class Node : DependencyObject
    {
        #region # 构造器

        #region 00.静态构造器
        /// <summary>
        /// 静态构造器
        /// </summary>
        static Node()
        {
            //注册依赖属性
            IsSelectedProperty = DependencyProperty.Register(nameof(IsSelected), typeof(bool?), typeof(Node), new PropertyMetadata(false));
            IsCheckedProperty = DependencyProperty.Register(nameof(IsChecked), typeof(bool?), typeof(Node), new PropertyMetadata(false, OnIsCheckedChanged));
        }
        #endregion

        #region 01.无参构造器
        /// <summary>
        /// 无参构造器
        /// </summary>
        public Node()
        {
            //初始化导航属性
            this.SubNodes = new ObservableCollection<Node>();

            //默认值
            this.IsSelected = false;
            this.IsChecked = false;
        }
        #endregion

        #region 02.创建节点构造器
        /// <summary>
        /// 创建节点构造器
        /// </summary>
        /// <param name="id">标识Id</param>
        /// <param name="name">名称</param>
        /// <param name="isChecked">是否勾选</param>
        /// <param name="parentNode">上级节点</param>
        public Node(Guid id, string name, bool? isChecked, Node parentNode)
            : this()
        {
            this.Id = id;
            this.Name = name;
            this.IsChecked = isChecked;
            this.ParentNode = parentNode;
            parentNode?.SubNodes.Add(this);
        }
        #endregion

        #region 03.创建节点构造器
        /// <summary>
        /// 创建节点构造器
        /// </summary>
        /// <param name="id">标识Id</param>
        /// <param name="name">名称</param>
        /// <param name="isSelected">是否选中</param>
        /// <param name="isChecked">是否勾选</param>
        /// <param name="parentNode">上级节点</param>
        public Node(Guid id, string name, bool? isSelected, bool? isChecked, Node parentNode)
            : this()
        {
            this.Id = id;
            this.Name = name;
            this.IsSelected = isSelected;
            this.IsChecked = isChecked;
            this.ParentNode = parentNode;
            parentNode?.SubNodes.Add(this);
        }
        #endregion

        #endregion

        #region # 属性

        #region 标识Id —— Guid Id
        /// <summary>
        /// 标识Id
        /// </summary>
        public Guid Id { get; set; }
        #endregion

        #region 名称 —— string Name
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        #endregion

        #region 图标 —— string Icon
        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }
        #endregion

        #region 是否选中 —— bool? IsSelected

        /// <summary>
        /// 是否选中依赖属性
        /// </summary>
        public static DependencyProperty IsSelectedProperty;

        /// <summary>
        /// 是否选中
        /// </summary>
        public bool? IsSelected
        {
            get { return base.GetValue(IsSelectedProperty) == null ? (bool?)null : Convert.ToBoolean(base.GetValue(IsSelectedProperty)); }
            set { base.SetValue(IsSelectedProperty, value); }
        }

        #endregion

        #region 是否勾选 —— bool? IsChecked

        /// <summary>
        /// 是否勾选依赖属性
        /// </summary>
        public static DependencyProperty IsCheckedProperty;

        /// <summary>
        /// 是否勾选
        /// </summary>
        public bool? IsChecked
        {
            get { return base.GetValue(IsCheckedProperty) == null ? (bool?)null : Convert.ToBoolean(base.GetValue(IsCheckedProperty)); }
            set { base.SetValue(IsCheckedProperty, value); }
        }

        #endregion

        #region 只读属性 - 是否有三种状态 —— bool IsThreeState
        /// <summary>
        /// 只读属性 - 是否有三种状态
        /// </summary>
        public bool IsThreeState
        {
            get { return !this.IsLeaf; }
        }
        #endregion

        #region 只读属性 - 是否是根级节点 —— bool IsRoot
        /// <summary>
        /// 只读属性 - 是否是根级节点
        /// </summary>
        public bool IsRoot
        {
            get { return this.ParentNode == null; }
        }
        #endregion

        #region 只读属性 - 是否是叶子级节点 —— bool IsLeaf
        /// <summary>
        /// 只读属性 - 是否是叶子级节点
        /// </summary>
        public bool IsLeaf
        {
            get { return !this.SubNodes.Any(); }
        }
        #endregion

        #region 导航属性 - 上级节点 —— Node ParentNode
        /// <summary>
        /// 导航属性 - 上级节点
        /// </summary>
        public Node ParentNode { get; set; }
        #endregion

        #region 导航属性 - 下级节点集 —— ObservableCollection<Node> SubNodes
        /// <summary>
        /// 导航属性 - 下级节点集
        /// </summary>
        public ObservableCollection<Node> SubNodes { get; set; }
        #endregion

        #endregion

        #region # 方法

        #region 是否选中改变回调方法 —— static void OnIsCheckedChanged(DependencyObject dependencyObject...
        /// <summary>
        /// 是否选中改变回调方法
        /// </summary>
        private static void OnIsCheckedChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            Node node = (Node)dependencyObject;
            if (node.IsChecked == true)
            {
                CheckDown(node);
            }
            else if (node.IsChecked == false)
            {
                UncheckDown(node);
            }

            RefreshUp(node);

            node.IsChecked = (bool?)eventArgs.NewValue;
        }
        #endregion

        #region 向上刷新 —— static void RefreshUp(Node node)
        /// <summary>
        /// 向上刷新
        /// </summary>
        /// <param name="node">节点</param>
        private static void RefreshUp(Node node)
        {
            if (node.ParentNode != null)
            {
                if (node.ParentNode.SubNodes.All(x => x.IsChecked == true))
                {
                    node.ParentNode.IsChecked = true;
                }
                else if (node.ParentNode.SubNodes.All(x => x.IsChecked == false))
                {
                    node.ParentNode.IsChecked = false;
                }
                else
                {
                    node.ParentNode.IsChecked = null;
                }

                RefreshUp(node.ParentNode);
            }
        }
        #endregion

        #region 向下选中 —— static void CheckDown(Node node)
        /// <summary>
        /// 向下选中
        /// </summary>
        /// <param name="node">节点</param>
        private static void CheckDown(Node node)
        {
            foreach (Node subNode in node.SubNodes)
            {
                subNode.IsChecked = true;
                CheckDown(subNode);
            }
        }
        #endregion

        #region 向下取消选中 —— static void UncheckDown(Node node)
        /// <summary>
        /// 向下取消选中
        /// </summary>
        /// <param name="node">节点</param>
        private static void UncheckDown(Node node)
        {
            foreach (Node subNode in node.SubNodes)
            {
                subNode.IsChecked = false;
                UncheckDown(subNode);
            }
        }
        #endregion 

        #endregion
    }
}
