using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using System;

namespace SD.Infrastructure.Avalonia.UserControls
{
    /// <summary>
    /// 分页控件
    /// </summary>
    public partial class Paginator : UserControl
    {
        #region # 构造器

        /// <summary>
        /// 最小页码
        /// </summary>
        private const int MinPageIndex = 1;

        /// <summary>
        /// 默认页容量
        /// </summary>
        private const int DefaultPageSize = 15;

        /// <summary>
        /// 页容量集合
        /// </summary>
        private static readonly int[] _PageSizes;

        /// <summary>
        /// 页码依赖属性
        /// </summary>
        public static readonly StyledProperty<int> PageIndexProperty;

        /// <summary>
        /// 页容量依赖属性
        /// </summary>
        public static readonly StyledProperty<int> PageSizeProperty;

        /// <summary>
        /// 总记录数依赖属性
        /// </summary>
        public static readonly StyledProperty<int> RowCountProperty;

        /// <summary>
        /// 总页数依赖属性
        /// </summary>
        public static readonly StyledProperty<int> PageCountProperty;

        /// <summary>
        /// 起始行依赖属性
        /// </summary>
        public static readonly StyledProperty<int> StartRowIndexProperty;

        /// <summary>
        /// 终止行依赖属性
        /// </summary>
        public static readonly StyledProperty<int> EndRowIndexProperty;

        /// <summary>
        /// 刷新数据路由事件
        /// </summary>
        public static readonly RoutedEvent RefreshEvent;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static Paginator()
        {
            //初始化页容量集合
            _PageSizes = new[] { 5, 10, DefaultPageSize, 20, 30, 50 };

            //注册依赖属性
            PageIndexProperty = AvaloniaProperty.Register<Paginator, int>(nameof(PageIndex), MinPageIndex);
            PageSizeProperty = AvaloniaProperty.Register<Paginator, int>(nameof(PageSize), DefaultPageSize);
            RowCountProperty = AvaloniaProperty.Register<Paginator, int>(nameof(RowCount), 0);
            PageCountProperty = AvaloniaProperty.Register<Paginator, int>(nameof(PageCount), 0);
            StartRowIndexProperty = AvaloniaProperty.Register<Paginator, int>(nameof(StartRowIndex), 1);
            EndRowIndexProperty = AvaloniaProperty.Register<Paginator, int>(nameof(EndRowIndex));
            PageIndexProperty.Changed.AddClassHandler<Paginator>(OnPageIndexChanged);
            RowCountProperty.Changed.AddClassHandler<Paginator>(OnRowCountChanged);
            PageCountProperty.Changed.AddClassHandler<Paginator>(OnPageCountChanged);
            StartRowIndexProperty.Changed.AddClassHandler<Paginator>(OnStartRowIndexChanged);

            //注册路由事件
            RefreshEvent = RoutedEvent.Register<Paginator, RoutedEventArgs>(nameof(Refresh), RoutingStrategies.Direct);
        }

        /// <summary>
        /// 实例构造器
        /// </summary>
        public Paginator()
        {
            this.InitializeComponent();
        }

        #endregion

        #region # 依赖属性 & 路由事件

        #region 页码 —— int PageIndex
        /// <summary>
        /// 页码
        /// </summary>
        public int PageIndex
        {
            get => base.GetValue(PageIndexProperty);
            set => base.SetValue(PageIndexProperty, value);
        }
        #endregion

        #region 页容量 —— int PageSize
        /// <summary>
        /// 页容量
        /// </summary>
        public int PageSize
        {
            get => base.GetValue(PageSizeProperty);
            set => base.SetValue(PageSizeProperty, value);
        }
        #endregion

        #region 总记录数 —— int RowCount
        /// <summary>
        /// 总记录数
        /// </summary>
        public int RowCount
        {
            get => base.GetValue(RowCountProperty);
            set => base.SetValue(RowCountProperty, value);
        }
        #endregion

        #region 总页数 —— int PageCount
        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount
        {
            get => base.GetValue(PageCountProperty);
            set => base.SetValue(PageCountProperty, value);
        }
        #endregion

        #region 起始行 —— int StartRowIndex
        /// <summary>
        /// 起始行
        /// </summary>
        public int StartRowIndex
        {
            get => base.GetValue(StartRowIndexProperty);
            set => base.SetValue(StartRowIndexProperty, value);
        }
        #endregion

        #region 终止行 —— int EndRowIndex
        /// <summary>
        /// 终止行
        /// </summary>
        public int EndRowIndex
        {
            get => base.GetValue(EndRowIndexProperty);
            set => base.SetValue(EndRowIndexProperty, value);
        }
        #endregion

        #region 刷新数据路由事件 —— event EventHandler<RoutedEventArgs> Refresh
        /// <summary>
        /// 刷新数据路由事件
        /// </summary>
        public event EventHandler<RoutedEventArgs> Refresh
        {
            add => base.AddHandler(RefreshEvent, value);
            remove => base.RemoveHandler(RefreshEvent, value);
        }
        #endregion

        #region 只读属性 - 页容量列表 —— int[] PageSizes
        /// <summary>
        /// 只读属性 - 页容量列表
        /// </summary>
        public int[] PageSizes
        {
            get => _PageSizes;
        }
        #endregion

        #endregion

        #region # 回调方法

        #region 页码改变回调方法 —— static void OnPageIndexChanged(...
        /// <summary>
        /// 页码改变回调方法
        /// </summary>
        private static void OnPageIndexChanged(Paginator paginator, AvaloniaPropertyChangedEventArgs eventArgs)
        {
            int pageIndex = Convert.ToInt32(eventArgs.NewValue);
            if (pageIndex <= MinPageIndex)
            {
                pageIndex = MinPageIndex;
            }
            if (paginator.PageCount != 0 && pageIndex > paginator.PageCount)
            {
                pageIndex = paginator.PageCount;
            }

            paginator.PageIndex = pageIndex;
            paginator.StartRowIndex = (paginator.PageIndex * paginator.PageSize) - paginator.PageSize + 1;
            paginator.EndRowIndex = (paginator.StartRowIndex + paginator.PageSize) > paginator.RowCount
                ? paginator.RowCount
                : paginator.StartRowIndex - 1 + paginator.PageSize;
        }
        #endregion

        #region 总记录数改变回调方法 —— static void OnRowCountChanged(...
        /// <summary>
        /// 总记录数改变回调方法
        /// </summary>
        private static void OnRowCountChanged(Paginator paginator, AvaloniaPropertyChangedEventArgs eventArgs)
        {
            paginator.EndRowIndex = paginator.StartRowIndex + paginator.PageSize > paginator.RowCount
                ? paginator.RowCount
                : paginator.StartRowIndex - 1 + paginator.PageSize;
        }
        #endregion

        #region 总页数改变回调方法 —— static void OnPageCountChanged(...
        /// <summary>
        /// 总页数改变回调方法
        /// </summary>
        private static void OnPageCountChanged(Paginator paginator, AvaloniaPropertyChangedEventArgs eventArgs)
        {
            if (paginator.PageIndex > paginator.PageCount)
            {
                paginator.PageIndex = paginator.PageCount;
            }
        }
        #endregion

        #region 起始行改变回调方法 —— static void OnStartRowIndexChanged(...
        /// <summary>
        /// 起始行改变回调方法
        /// </summary>
        private static void OnStartRowIndexChanged(Paginator paginator, AvaloniaPropertyChangedEventArgs eventArgs)
        {
            paginator.EndRowIndex = paginator.StartRowIndex + paginator.PageSize > paginator.RowCount
                ? paginator.RowCount
                : paginator.StartRowIndex - 1 + paginator.PageSize;
        }
        #endregion

        #endregion

        #region # 事件处理程序

        #region 页容量改变事件 —— void CbxSelectionChanged(...
        /// <summary>
        /// 页容量改变事件
        /// </summary>
        private void CbxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.PageCount = (int)Math.Ceiling(this.RowCount * 1.0 / this.PageSize);
            this.StartRowIndex = (this.PageIndex * this.PageSize) - this.PageSize + 1;
            this.EndRowIndex = this.StartRowIndex + this.PageSize > this.RowCount
                ? this.RowCount
                : this.StartRowIndex - 1 + this.PageSize;
            if (this.RowCount == 0)
            {
                return;
            }

            //挂起路由事件
            this.RaiseEvent(new RoutedEventArgs(RefreshEvent, this));
        }
        #endregion

        #region 页码文本框输入事件 —— void TxtPageIndexTextInput(...
        /// <summary>
        /// 页码文本框输入事件
        /// </summary>
        private void TxtPageIndexTextInput(object sender, TextInputEventArgs eventArgs)
        {
            eventArgs.Handled = !int.TryParse(eventArgs.Text, out int _);
        }
        #endregion

        #region 页码文本框失去焦点事件 —— void TxtPageIndexLostFocus(...
        /// <summary>
        /// 页码文本框失去焦点事件
        /// </summary>
        private void TxtPageIndexLostFocus(object sender, RoutedEventArgs eventArgs)
        {
            TextBox textBox = (TextBox)sender;
            this.PageIndex = int.Parse(textBox.Text);

            //挂起路由事件
            this.RaiseEvent(new RoutedEventArgs(RefreshEvent, this));
        }
        #endregion

        #region 页码文本框按回车键事件 —— void TxtPageIndexKeyDown(...
        /// <summary>
        /// 页码文本框按回车键事件
        /// </summary>
        private void TxtPageIndexKeyDown(object sender, KeyEventArgs eventArgs)
        {
            if (eventArgs.Key == Key.Return)
            {
                TextBox textBox = (TextBox)sender;
                this.PageIndex = int.Parse(textBox.Text!);

                //挂起路由事件
                this.RaiseEvent(new RoutedEventArgs(RefreshEvent, this));
            }
        }
        #endregion

        #region 第一页按钮点击事件 —— void BtnFirstPageClick(...
        /// <summary>
        /// 第一页按钮点击事件
        /// </summary>
        private void BtnFirstPageClick(object sender, RoutedEventArgs eventArgs)
        {
            if (this.PageCount == 0)
            {
                return;
            }
            if (this.PageIndex == MinPageIndex)
            {
                return;
            }

            this.PageIndex = MinPageIndex;

            //挂起路由事件
            this.RaiseEvent(new RoutedEventArgs(RefreshEvent, this));
        }
        #endregion

        #region 上一页按钮点击事件 —— void BtnPrevPageClick(...
        /// <summary>
        /// 上一页按钮点击事件
        /// </summary>
        private void BtnPrevPageClick(object sender, RoutedEventArgs eventArgs)
        {
            if (this.PageCount == 0)
            {
                return;
            }
            if (this.PageIndex <= MinPageIndex)
            {
                this.PageIndex = MinPageIndex;
                return;
            }

            this.PageIndex--;

            //挂起路由事件
            this.RaiseEvent(new RoutedEventArgs(RefreshEvent, this));
        }
        #endregion

        #region 下一页按钮点击事件 —— void BtnNextPageClick(...
        /// <summary>
        /// 下一页按钮点击事件
        /// </summary>
        private void BtnNextPageClick(object sender, RoutedEventArgs eventArgs)
        {
            if (this.PageCount == 0)
            {
                return;
            }
            if (this.PageIndex >= this.PageCount)
            {
                this.PageIndex = this.PageCount;
                return;
            }

            this.PageIndex++;

            //挂起路由事件
            this.RaiseEvent(new RoutedEventArgs(RefreshEvent, this));
        }
        #endregion

        #region 最后一页按钮点击事件 —— void BtnLastPageClick(...
        /// <summary>
        /// 最后一页按钮点击事件
        /// </summary>
        private void BtnLastPageClick(object sender, RoutedEventArgs eventArgs)
        {
            if (this.PageCount == 0)
            {
                return;
            }
            if (this.PageIndex == this.PageCount)
            {
                return;
            }

            this.PageIndex = this.PageCount;

            //挂起路由事件
            this.RaiseEvent(new RoutedEventArgs(RefreshEvent, this));
        }
        #endregion

        #region 刷新按钮点击事件 —— void BtnRefreshClick(...
        /// <summary>
        /// 刷新按钮点击事件
        /// </summary>
        private void BtnRefreshClick(object sender, RoutedEventArgs eventArgs)
        {
            this.RaiseEvent(new RoutedEventArgs(RefreshEvent, this));
        }
        #endregion

        #endregion
    }
}
