using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace SD.Infrastructure.Avalonia.Converters
{
    /// <summary>
    /// 空对象可见性转换器
    /// </summary>
    public class EmptyObjectToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// 转换
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null;
        }

        /// <summary>
        /// 转换回
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
