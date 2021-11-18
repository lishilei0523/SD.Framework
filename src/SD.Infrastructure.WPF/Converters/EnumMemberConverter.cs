using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows.Data;

namespace SD.Infrastructure.WPF.Converters
{
    /// <summary>
    /// 枚举成员转换器
    /// </summary>
    public class EnumMemberConverter : IValueConverter
    {
        /// <summary>
        /// 转换枚举成员
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            #region # 验证

            if (value == null)
            {
                return null;
            }

            #endregion

            Enum @enum = (Enum)value;
            string enumMember = this.GetEnumMember(@enum);

            return enumMember;
        }

        /// <summary>
        /// 获取枚举成员名称
        /// </summary>
        /// <param name="enum">枚举值</param>
        /// <returns>枚举成员名称</returns>
        private string GetEnumMember(Enum @enum)
        {
            Type type = @enum.GetType();
            FieldInfo field = type.GetField(@enum.ToString());

#if NET40
            object[] attributes = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
            DescriptionAttribute enumMember = attributes.Any() ? (DescriptionAttribute)attributes[0] : null;
#else
            DescriptionAttribute enumMember = field.GetCustomAttribute<DescriptionAttribute>();
#endif
            return enumMember == null
                ? @enum.ToString()
                : string.IsNullOrEmpty(enumMember.Description)
                    ? @enum.ToString()
                    : enumMember.Description;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
