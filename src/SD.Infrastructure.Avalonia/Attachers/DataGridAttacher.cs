using Avalonia;
using Avalonia.Controls;

namespace SD.Infrastructure.Avalonia.Attachers
{
    /// <summary>
    /// DataGrid附加器
    /// </summary>
    public class DataGridAttacher
    {
        #region # 字段及构造器

        /// <summary>
        /// 是否显示行号依赖属性
        /// </summary>
        public static readonly AttachedProperty<bool> DisplayRowNumberProperty;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static DataGridAttacher()
        {
            //注册依赖属性
            DisplayRowNumberProperty = AvaloniaProperty.RegisterAttached<DataGridAttacher, DataGrid, bool>("DisplayRowNumber");
            DisplayRowNumberProperty.Changed.AddClassHandler<DataGrid>(OnDisplayRowNumberChanged);
        }

        #endregion

        #region # 属性

        #region 获取是否显示行号 —— static bool GetDisplayRowNumber(AvaloniaObject...
        /// <summary>
        /// 获取是否显示行号
        /// </summary>
        public static bool GetDisplayRowNumber(AvaloniaObject dependencyObject)
        {
            return dependencyObject.GetValue(DisplayRowNumberProperty);
        }
        #endregion

        #region 设置是否显示行号 —— static void SetDisplayRowNumber(AvaloniaObject...
        /// <summary>
        /// 设置是否显示行号
        /// </summary>
        public static void SetDisplayRowNumber(AvaloniaObject dependencyObject, bool value)
        {
            dependencyObject.SetValue(DisplayRowNumberProperty, value);
        }
        #endregion

        #endregion

        #region # 方法

        #region 是否显示行号改变事件 —— static void OnDisplayRowNumberChanged(DataGrid dataGrid...
        /// <summary>
        /// 是否显示行号改变事件
        /// </summary>
        private static void OnDisplayRowNumberChanged(DataGrid dataGrid, AvaloniaPropertyChangedEventArgs eventArgs)
        {
            bool isDisplayRowNumber = (bool)eventArgs.NewValue!;
            if (isDisplayRowNumber)
            {
                dataGrid.LoadingRow += OnDataGridLoadingRow;
            }
            else
            {
                dataGrid.LoadingRow -= OnDataGridLoadingRow;
            }
        }
        #endregion

        #region DataGrid行加载事件 —— static void OnDataGridLoadingRow(object sender...
        /// <summary>
        /// DataGrid行加载事件
        /// </summary>
        private static void OnDataGridLoadingRow(object sender, DataGridRowEventArgs eventArgs)
        {
            eventArgs.Row.Header = eventArgs.Row.GetIndex() + 1;
        }
        #endregion 

        #endregion
    }
}
