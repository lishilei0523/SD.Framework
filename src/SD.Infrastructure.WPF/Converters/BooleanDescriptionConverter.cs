using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;

namespace SD.Infrastructure.WPF.Converters
{
    /// <summary>
    /// 布尔值描述转换器
    /// </summary>
    [ContentProperty("ValueDescriptions")]
    public class BooleanDescriptionConverter : IValueConverter
    {
        /// <summary>
        /// 布尔值描述列表字段
        /// </summary>
        private readonly IList<TextBlock> _valueDescriptions;

        /// <summary>
        /// 布尔值描述列表
        /// </summary>
        public IList ValueDescriptions
        {
            get { return (IList)this._valueDescriptions; }
        }

        /// <summary>
        /// 默认构造器
        /// </summary>
        public BooleanDescriptionConverter()
        {
            this._valueDescriptions = new List<TextBlock>();
        }

        /// <summary>
        /// 转换布尔值描述
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            #region # 验证

            if (value == null)
            {
                return null;
            }
            if (this._valueDescriptions.Count != 2)
            {
                return null;
            }

            #endregion

            bool boolean = (bool)value;
            if (boolean)
            {
                return this._valueDescriptions[1].Text;
            }

            return this._valueDescriptions[0].Text;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
