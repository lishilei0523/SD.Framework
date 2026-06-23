using Avalonia;
using Avalonia.Controls;
using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace SD.Infrastructure.Avalonia.Converters
{
    /// <summary>
    /// 字符串-几何转换器
    /// </summary>
    public class StringToGeometryConverter : IValueConverter
    {
        /// <summary>
        /// 转换
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            #region # 验证

            if (value is not string icon)
            {
                return null;
            }

            #endregion

            object resource;

            //从当前控件往上逐级查找
            if (parameter is Control control)
            {
                if (control.TryFindResource(icon, out resource))
                {
                    return resource;
                }
            }

            //回退到Application资源
            if (Application.Current!.Resources.TryGetResource(icon, null, out resource))
            {
                return resource;
            }

            return null;
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
