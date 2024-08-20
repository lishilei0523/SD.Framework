using System.Windows;
using System.Windows.Controls;

namespace SD.Infrastructure.WPF.Attachers
{
    /// <summary>
    /// DataGrid附加器
    /// </summary>
    public static class DataGridAttacher
    {
        #region # 字段及构造器

        /// <summary>
        /// 是否显示行号依赖属性
        /// </summary>
        public static readonly DependencyProperty DisplayRowNumberProperty;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static DataGridAttacher()
        {
            //注册依赖属性
            DisplayRowNumberProperty = DependencyProperty.RegisterAttached("DisplayRowNumber", typeof(bool), typeof(DataGridAttacher), new PropertyMetadata(false, OnDisplayRowNumberChanged));
        }

        #endregion

        #region # 属性

        #region 获取是否显示行号 —— static bool GetDisplayRowNumber(DependencyObject...
        /// <summary>
        /// 获取是否显示行号
        /// </summary>
        public static bool GetDisplayRowNumber(DependencyObject dependencyObject)
        {
            return (bool)dependencyObject.GetValue(DisplayRowNumberProperty);
        }
        #endregion

        #region 设置是否显示行号 —— static void SetDisplayRowNumber(DependencyObject...
        /// <summary>
        /// 设置是否显示行号
        /// </summary>
        [AttachedPropertyBrowsableForType(typeof(DataGrid))]
        public static void SetDisplayRowNumber(DependencyObject dependencyObject, bool value)
        {
            dependencyObject.SetValue(DisplayRowNumberProperty, value);
        }
        #endregion

        #endregion

        #region # 方法

        #region 是否显示行号改变回调方法 —— static void OnDisplayRowNumberChanged(DependencyObject...
        /// <summary>
        /// 是否显示行号改变回调方法
        /// </summary>
        private static void OnDisplayRowNumberChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            DataGrid dataGrid = (DataGrid)dependencyObject;
            bool isDisplayRowNumber = (bool)eventArgs.NewValue;
            if (isDisplayRowNumber)
            {
                dataGrid.LoadingRow += DataGridLoadingRowEventHandler;
            }
            else
            {
                dataGrid.LoadingRow -= DataGridLoadingRowEventHandler;
            }
        }
        #endregion

        #region DataGrid加载行事件处理程序 —— static void DataGridLoadingRowEventHandler(object sender...
        /// <summary>
        /// DataGrid加载行事件处理程序
        /// </summary>
        private static void DataGridLoadingRowEventHandler(object sender, DataGridRowEventArgs eventArgs)
        {
            eventArgs.Row.Header = eventArgs.Row.GetIndex() + 1;
        }
        #endregion

        #endregion
    }
}
