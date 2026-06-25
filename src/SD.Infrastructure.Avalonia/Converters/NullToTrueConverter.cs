using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace SD.Infrastructure.Avalonia.Converters
{
    /// <summary>
    /// Null-True转换器
    /// </summary>
    public class NullToTrueConverter : IValueConverter
    {
        /// <summary>
        /// 转换
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null;
        }

        /// <summary>
        /// 转换回
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
