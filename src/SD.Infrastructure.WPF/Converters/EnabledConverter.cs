using System;
using System.Windows.Data;

namespace SD.Infrastructure.WPF.Converters
{
    /// <summary>
    /// 停/启用转换器
    /// </summary>
    public class EnabledConverter : IValueConverter
    {
        /// <summary>
        /// 转换停/启用状态
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            #region # 验证

            if (value == null)
            {
                return null;
            }

            #endregion

            bool enabled = (bool)value;
            if (enabled)
            {
                return "已启用";
            }

            return "已停用";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
