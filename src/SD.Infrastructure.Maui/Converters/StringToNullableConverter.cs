using Microsoft.Maui.Controls;
using System;
using System.Globalization;

namespace SD.Infrastructure.Maui.Converters
{
    /// <summary>
    /// 字符串转可空类型转换器
    /// </summary>
    public class StringToNullableConverter : IValueConverter
    {
        /// <summary>
        /// 转换可空类型
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string text && string.IsNullOrWhiteSpace(text))
            {
                return null;
            }

            return value;
        }

        /// <summary>
        /// 转回可空类型
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string text && string.IsNullOrWhiteSpace(text))
            {
                return null;
            }

            return value;
        }
    }
}
