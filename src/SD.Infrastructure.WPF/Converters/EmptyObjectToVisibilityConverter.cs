using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SD.Infrastructure.WPF.Converters
{
    public class EmptyObjectToVisibilityConverter : DependencyObject, IValueConverter
    {
        /// <summary>
        /// 标识 EmptyValue 依赖属性。
        /// </summary>
        public static readonly DependencyProperty EmptyValueProperty =
            DependencyProperty.Register(nameof(EmptyObjectToVisibilityConverter.EmptyValue), typeof(Visibility), typeof(EmptyObjectToVisibilityConverter), new PropertyMetadata(default(Visibility)));

        /// <summary>
        /// 标识 NotEmptyValue 依赖属性。
        /// </summary>
        public static readonly DependencyProperty NotEmptyValueProperty =
            DependencyProperty.Register(nameof(EmptyObjectToVisibilityConverter.NotEmptyValue), typeof(Visibility), typeof(EmptyObjectToVisibilityConverter), new PropertyMetadata(default(Visibility)));

        public EmptyObjectToVisibilityConverter()
        {
            this.EmptyValue = Visibility.Collapsed;
            this.NotEmptyValue = Visibility.Visible;
        }

        /// <summary>
        /// 获取或设置EmptyValue的值
        /// </summary>
        public Visibility EmptyValue
        {
            get => (Visibility)this.GetValue(EmptyObjectToVisibilityConverter.EmptyValueProperty);
            set => this.SetValue(EmptyObjectToVisibilityConverter.EmptyValueProperty, value);
        }

        /// <summary>
        /// 获取或设置NotEmptyValue的值
        /// </summary>
        public Visibility NotEmptyValue
        {
            get => (Visibility)this.GetValue(EmptyObjectToVisibilityConverter.NotEmptyValueProperty);
            set => this.SetValue(EmptyObjectToVisibilityConverter.NotEmptyValueProperty, value);
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return this.EmptyValue;
            else
                return this.NotEmptyValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
