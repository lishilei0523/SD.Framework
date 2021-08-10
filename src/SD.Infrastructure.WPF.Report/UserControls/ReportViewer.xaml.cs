using Microsoft.Reporting.WinForms;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;

namespace SD.Infrastructure.WPF.Report.UserControls
{
    /// <summary>
    /// RDLC报表查看器用户控件
    /// </summary>
    public partial class ReportViewer : UserControl
    {
        #region # 构造器

        /// <summary>
        /// 静态构造器
        /// </summary>
        static ReportViewer()
        {
            //注册依赖属性
            ReportPathProperty = DependencyProperty.Register(nameof(ReportPath), typeof(string), typeof(ReportViewer), new PropertyMetadata(OnReportPathChanged));
            ReportDataSourcesProperty = DependencyProperty.Register(nameof(ReportDataSources), typeof(ObservableCollection<ReportDataSource>), typeof(ReportViewer), new FrameworkPropertyMetadata(OnReportDataSourcesChanged));

        }

        /// <summary>
        /// 实例构造器
        /// </summary>
        public ReportViewer()
        {
            this.InitializeComponent();
        }

        #endregion

        #region # 依赖属性

        #region 报表路径 —— string ReportPath

        /// <summary>
        /// 报表路径依赖属性
        /// </summary>
        public static DependencyProperty ReportPathProperty;

        /// <summary>
        /// 报表路径
        /// </summary>
        public string ReportPath
        {
            get { return base.GetValue(ReportPathProperty)?.ToString(); }
            set { base.SetValue(ReportPathProperty, value); }
        }

        #endregion

        #region 报表数据源列表 —— ObservableCollection<ReportDataSource> ReportDataSources

        /// <summary>
        /// 报表数据源列表依赖属性
        /// </summary>
        public static DependencyProperty ReportDataSourcesProperty;

        /// <summary>
        /// 报表数据源列表
        /// </summary>
        public ObservableCollection<ReportDataSource> ReportDataSources
        {
            get { return (ObservableCollection<ReportDataSource>)base.GetValue(ReportDataSourcesProperty); }
            set { base.SetValue(ReportDataSourcesProperty, value); value.CollectionChanged += OnReportDataSourcesItemChanged; }
        }

        #endregion

        #endregion

        #region # 回调方法

        #region 报表路径改变回调方法 —— static void OnReportPathChanged(...
        /// <summary>
        /// 报表路径改变回调方法
        /// </summary>
        private static void OnReportPathChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            ReportViewer reportViewer = (ReportViewer)dependencyObject;

            string reportPath = eventArgs.NewValue?.ToString();
            reportViewer.ReportContainer.LocalReport.ReportPath = reportPath;

            reportViewer.ReportContainer.RefreshReport();
        }
        #endregion

        #region 报表数据源列表改变回调方法 —— static void OnReportDataSourcesChanged(...
        /// <summary>
        /// 报表数据源列表改变回调方法
        /// </summary>
        private static void OnReportDataSourcesChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            ReportViewer reportViewer = (ReportViewer)dependencyObject;

            ObservableCollection<ReportDataSource> reportDataSources = eventArgs.NewValue == null
                ? new ObservableCollection<ReportDataSource>()
                : (ObservableCollection<ReportDataSource>)eventArgs.NewValue;
            reportViewer.ReportContainer.LocalReport.DataSources.Clear();
            foreach (ReportDataSource reportDataSource in reportDataSources)
            {
                reportViewer.ReportContainer.LocalReport.DataSources.Add(reportDataSource);
            }

            reportViewer.ReportContainer.RefreshReport();
        }
        #endregion

        #region 报表数据源列表元素改变回调方法 —— static void OnReportDataSourcesItemChanged(...
        /// <summary>
        /// 报表数据源列表元素改变回调方法
        /// </summary>
        private static void OnReportDataSourcesItemChanged(object sender, NotifyCollectionChangedEventArgs eventArgs)
        {
            ReportViewer reportViewer = (ReportViewer)sender;

            reportViewer.ReportContainer.LocalReport.DataSources.Clear();
            foreach (ReportDataSource reportDataSource in eventArgs.NewItems)
            {
                reportViewer.ReportContainer.LocalReport.DataSources.Add(reportDataSource);
            }

            reportViewer.ReportContainer.RefreshReport();
        }
        #endregion

        #endregion
    }
}
