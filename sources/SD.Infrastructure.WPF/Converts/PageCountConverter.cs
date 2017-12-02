using System;
using System.Globalization;
using System.Windows.Data;

namespace SD.Infrastructure.WPF.Converts
{
    /// <summary>
    /// 总页数转换器
    /// </summary>
    public class PageCountConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            string pageSizeStr = values[0].ToString();
            string rowCountStr = values[1].ToString();

            int pageSize = string.IsNullOrWhiteSpace(pageSizeStr) ? 10 : int.Parse(pageSizeStr);
            int rowCount = string.IsNullOrWhiteSpace(rowCountStr) ? int.MaxValue : int.Parse(rowCountStr);

            int pageCount = (int)Math.Ceiling(rowCount * 1.0 / pageSize); ;

            return pageCount.ToString();
        }

        public object[] ConvertBack(object values, Type[] targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
