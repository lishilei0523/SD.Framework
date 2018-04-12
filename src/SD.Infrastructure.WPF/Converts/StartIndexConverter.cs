using System;
using System.Globalization;
using System.Windows.Data;

namespace SD.Infrastructure.WPF.Converts
{
    /// <summary>
    /// 末行索引转换器
    /// </summary>
    public class StartIndexConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            string pageIndexStr = values[0].ToString();
            string pageSizeStr = values[1].ToString();

            int pageIndex = string.IsNullOrWhiteSpace(pageIndexStr) ? 1 : Int32.Parse(pageIndexStr);
            int pageSize = string.IsNullOrWhiteSpace(pageSizeStr) ? 10 : Int32.Parse(pageSizeStr);

            int startIndex = (pageIndex * pageSize) - pageSize + 1;
            return startIndex.ToString();
        }


        public object[] ConvertBack(object values, Type[] targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
