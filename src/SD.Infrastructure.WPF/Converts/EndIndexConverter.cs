using System;
using System.Globalization;
using System.Windows.Data;

namespace SD.Infrastructure.WPF.Converts
{
    /// <summary>
    /// 首行索引转换器
    /// </summary>
    public class EndIndexConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            string pageIndexStr = values[0].ToString();
            string pageSizeStr = values[1].ToString();
            string rowCountStr = values[2].ToString();

            int pageIndex = string.IsNullOrWhiteSpace(pageIndexStr) ? 1 : int.Parse(pageIndexStr);
            int pageSize = string.IsNullOrWhiteSpace(pageSizeStr) ? 10 : int.Parse(pageSizeStr);
            int rowCount = string.IsNullOrWhiteSpace(rowCountStr) ? int.MaxValue : int.Parse(rowCountStr);

            int startIndex = (pageIndex * pageSize) - pageSize + 1;
            int endIndex = startIndex + pageSize > rowCount ? rowCount : startIndex - 1 + pageSize;

            return endIndex.ToString();
        }

        public object[] ConvertBack(object values, Type[] targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
