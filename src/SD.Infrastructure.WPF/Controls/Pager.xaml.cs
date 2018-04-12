using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SD.Infrastructure.WPF.Controls
{
    /// <summary>
    /// 分页控件
    /// </summary>
    public partial class Pager
    {
        #region # 构造器

        /// <summary>
        /// 最小页码
        /// </summary>
        private const int MinPageIndex = 1;

        /// <summary>
        /// 默认页容量
        /// </summary>
        private const int DefaultPageSize = 10;

        /// <summary>
        /// 页容量集合
        /// </summary>
        private static readonly IReadOnlyCollection<int> _PageSizes;

        /// <summary>
        /// 无参构造器
        /// </summary>
        public Pager()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 静态构造器
        /// </summary>
        static Pager()
        {
            //初始化页容量集合
            _PageSizes = new List<int> { 5, DefaultPageSize, 15, 20, 30, 50 };

            //注册路由事件
            RefreshEvent = EventManager.RegisterRoutedEvent("Refresh", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(Pager));

            //注册依赖属性
            PageIndexProperty = DependencyProperty.Register("PageIndex", typeof(string), typeof(Pager), new PropertyMetadata(string.Empty, OnPageIndexChanged));
            PageSizeProperty = DependencyProperty.Register("PageSize", typeof(int), typeof(Pager), new PropertyMetadata(DefaultPageSize, OnPageSizeChanged));
            RowCountProperty = DependencyProperty.Register("RowCount", typeof(int), typeof(Pager), new PropertyMetadata(OnRowCountChanged));
        }

        #endregion

        #region # 路由事件

        #region 刷新事件 —— RoutedEvent RefreshEvent
        /// <summary>
        /// 刷新事件
        /// </summary>
        public static RoutedEvent RefreshEvent;

        /// <summary>
        /// 刷新事件
        /// </summary>
        public event RoutedEventHandler Refresh
        {
            add { this.AddHandler(RefreshEvent, value); }
            remove { this.RemoveHandler(RefreshEvent, value); }
        }
        #endregion

        #endregion

        #region # 依赖属性

        #region 页码 —— int PageIndex
        /// <summary>
        /// 页码
        /// </summary>
        public static readonly DependencyProperty PageIndexProperty;

        /// <summary>
        /// 页码
        /// </summary>
        public int PageIndex
        {
            get { return Convert.ToInt32(this.GetValue(PageIndexProperty)); }
            set { this.SetValue(PageIndexProperty, value.ToString()); }
        }

        /// <summary>
        /// 页码改变事件
        /// </summary>
        private static void OnPageIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Pager pager = (Pager)d;

            int pageIndex = Convert.ToInt32(e.NewValue);
            if (pageIndex <= MinPageIndex)
            {
                pageIndex = MinPageIndex;
            }

            pager.Txt_PageIndex.Text = pageIndex.ToString();
        }
        #endregion

        #region 页容量 —— int PageSize
        /// <summary>
        /// 页容量
        /// </summary>
        public static readonly DependencyProperty PageSizeProperty;

        /// <summary>
        /// 页容量
        /// </summary>
        public int PageSize
        {
            get { return Convert.ToInt32(this.GetValue(PageSizeProperty)); }
            set { this.SetValue(PageSizeProperty, value); }
        }

        /// <summary>
        /// 页容量改变事件
        /// </summary>
        private static void OnPageSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Pager pager = (Pager)d;

            int pageSize = Convert.ToInt32(e.NewValue);
            if (!_PageSizes.Contains(pageSize))
            {
                pageSize = DefaultPageSize;
            }

            pager.Cbx_PageSize.Text = pageSize.ToString();
        }
        #endregion

        #region 总记录数 —— int RowCount
        /// <summary>
        /// 总记录数
        /// </summary>
        public static readonly DependencyProperty RowCountProperty;

        /// <summary>
        /// 总记录数
        /// </summary>
        public int RowCount
        {
            get { return Convert.ToInt32(this.GetValue(RowCountProperty)); }
            set { this.SetValue(RowCountProperty, value); }
        }

        /// <summary>
        /// 总记录数改变事件
        /// </summary>
        public static void OnRowCountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Pager pager = (Pager)d;
            pager.Txt_RowCount.Text = e.NewValue.ToString();
        }
        #endregion

        #region 只读属性 - 总页数 —— int PageCount
        /// <summary>
        /// 只读属性 - 总页数
        /// </summary>
        public int PageCount
        {
            get { return (int)Math.Ceiling(this.RowCount * 1.0 / this.PageSize); }
        }
        #endregion

        #endregion

        #region # 事件

        #region 页容量下拉框选中事件 —— void Cbx_PageSize_OnDropDownClosed(object sender, EventArgs e)
        /// <summary>
        /// 页容量下拉框选中事件
        /// </summary>
        private void Cbx_PageSize_OnDropDownClosed(object sender, EventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            string text = comboBox.Text;

            if (!string.IsNullOrWhiteSpace(text))
            {
                this.PageSize = int.Parse(text);
            }
            if (this.PageIndex <= MinPageIndex)
            {
                this.PageIndex = MinPageIndex;
            }
            if (this.PageIndex > this.PageCount)
            {
                this.PageIndex = this.PageCount;
            }

            this.Txt_PageIndex.Text = this.PageIndex.ToString();

            this.RaiseEvent(new RoutedEventArgs(RefreshEvent, this));
        }
        #endregion

        #region 页码文本框输入事件 —— void Txt_PageIndex_PreviewTextInput(object sender, TextCompositionEventArgs e)
        /// <summary>
        /// 页码文本框输入事件
        /// </summary>
        private void Txt_PageIndex_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            int value;
            if (int.TryParse(e.Text, out value))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }
        #endregion

        #region 页码文本框失去焦点事件 —— void Txt_PageIndex_OnLostFocus(object sender, RoutedEventArgs e)
        /// <summary>
        /// 页码文本框失去焦点事件
        /// </summary>
        private void Txt_PageIndex_OnLostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            int pageIndex = int.Parse(textBox.Text);

            if (pageIndex <= MinPageIndex)
            {
                pageIndex = MinPageIndex;
            }
            if (pageIndex > this.PageCount)
            {
                pageIndex = this.PageCount;
            }

            textBox.Text = pageIndex.ToString();
            this.PageIndex = pageIndex;

            this.RaiseEvent(new RoutedEventArgs(RefreshEvent, this));
        }
        #endregion

        #region 页码文本框按回车键事件 —— void Txt_PageIndex_OnKeyDown(object sender, KeyEventArgs e)
        /// <summary>
        /// 页码文本框按回车键事件
        /// </summary>
        private void Txt_PageIndex_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyStates == Keyboard.GetKeyStates(Key.Return))
            {
                TextBox textBox = (TextBox)sender;

                int pageIndex = int.Parse(textBox.Text);

                if (pageIndex <= MinPageIndex)
                {
                    pageIndex = MinPageIndex;
                }
                if (pageIndex > this.PageCount)
                {
                    pageIndex = this.PageCount;
                }

                textBox.Text = pageIndex.ToString();
                this.PageIndex = pageIndex;

                this.RaiseEvent(new RoutedEventArgs(RefreshEvent, this));
            }
        }
        #endregion

        #region 第一页按钮点击事件 —— void Btn_FirstPage_Click(object sender, RoutedEventArgs e)
        /// <summary>
        /// 第一页按钮点击事件
        /// </summary>
        private void Btn_FirstPage_Click(object sender, RoutedEventArgs e)
        {
            this.PageIndex = MinPageIndex;

            this.RaiseEvent(new RoutedEventArgs(RefreshEvent, this));
        }
        #endregion

        #region 上一页按钮点击事件 —— void Btn_PrevPage_Click(object sender, RoutedEventArgs e)
        /// <summary>
        /// 上一页按钮点击事件
        /// </summary>
        private void Btn_PrevPage_Click(object sender, RoutedEventArgs e)
        {
            if (this.PageIndex <= MinPageIndex)
            {
                this.PageIndex = MinPageIndex;
                return;
            }

            this.PageIndex--;
            this.RaiseEvent(new RoutedEventArgs(RefreshEvent, this));
        }
        #endregion

        #region 下一页按钮点击事件 —— void Btn_NextPage_Click(object sender, RoutedEventArgs e)
        /// <summary>
        /// 下一页按钮点击事件
        /// </summary>
        private void Btn_NextPage_Click(object sender, RoutedEventArgs e)
        {
            if (this.PageIndex >= this.PageCount)
            {
                this.PageIndex = this.PageCount;
                return;
            }

            this.PageIndex++;
            this.RaiseEvent(new RoutedEventArgs(RefreshEvent, this));
        }
        #endregion

        #region 最后一页按钮点击事件 —— void Btn_LastPage_Click(object sender, RoutedEventArgs e)
        /// <summary>
        /// 最后一页按钮点击事件
        /// </summary>
        private void Btn_LastPage_Click(object sender, RoutedEventArgs e)
        {
            this.PageIndex = this.PageCount;

            this.RaiseEvent(new RoutedEventArgs(RefreshEvent, this));
        }
        #endregion

        #region 刷新按钮点击事件 —— void Btn_Refresh_OnClick(object sender, RoutedEventArgs e)
        /// <summary>
        /// 刷新按钮点击事件
        /// </summary>
        private void Btn_Refresh_OnClick(object sender, RoutedEventArgs e)
        {
            this.RaiseEvent(new RoutedEventArgs(RefreshEvent, this));
        }
        #endregion

        #endregion
    }
}
