using System;
using System.ComponentModel;
using System.Reflection;

namespace ShSoft.Framework2015.Common.PoweredByLee
{
    /// <summary>
    /// 枚举扩展工具类
    /// </summary>
    public static class EnumExtension
    {
        /// <summary>
        /// 获取枚举成员名称
        /// </summary>
        /// <param name="enum">枚举值</param>
        /// <returns>枚举成员名称</returns>
        public static string GetEnumMember(this Enum @enum)
        {
            Type type = @enum.GetType();
            FieldInfo field = type.GetField(@enum.ToString());
            DescriptionAttribute enumMember = field.GetCustomAttribute<DescriptionAttribute>();
            return enumMember == null ? @enum.ToString()
                : string.IsNullOrEmpty(enumMember.Description) ? @enum.ToString() :
                enumMember.Description;
        }
    }
}
