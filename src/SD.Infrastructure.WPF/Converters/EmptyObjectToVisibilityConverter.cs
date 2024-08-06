using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SD.Infrastructure.WPF.Converters
{
    /// <summary>
    /// 空对象可见性转换器
    /// </summary>
    public class EmptyObjectToVisibilityConverter : DependencyObject, IValueConverter
    {
        /// <summary>
        /// 标识 EmptyValue 依赖属性。
        /// </summary>
        public static readonly DependencyProperty EmptyValueProperty =
            DependencyProperty.Register(nameof(EmptyValue), typeof(Visibility), typeof(EmptyObjectToVisibilityConverter), new PropertyMetadata(default(Visibility)));

        /// <summary>
        /// 标识 NotEmptyValue 依赖属性。
        /// </summary>
        public static readonly DependencyProperty NotEmptyValueProperty =
            DependencyProperty.Register(nameof(NotEmptyValue), typeof(Visibility), typeof(EmptyObjectToVisibilityConverter), new PropertyMetadata(default(Visibility)));

        /// <summary>
        /// 构造器
        /// </summary>
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
            get => (Visibility)this.GetValue(EmptyValueProperty);
            set => this.SetValue(EmptyValueProperty, value);
        }

        /// <summary>
        /// 获取或设置NotEmptyValue的值
        /// </summary>
        public Visibility NotEmptyValue
        {
            get => (Visibility)this.GetValue(NotEmptyValueProperty);
            set => this.SetValue(NotEmptyValueProperty, value);
        }

        /// <summary>
        /// 转换
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return this.EmptyValue;
            }
            else
            {
                return this.NotEmptyValue;
            }
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
