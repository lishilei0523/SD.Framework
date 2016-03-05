using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace ShSoft.Framework2015.Common.PoweredByLee
{
    /// <summary>
    /// 枚举扩展工具类
    /// </summary>
    public static class EnumExtension
    {
        #region # 获取枚举成员名称 —— static string GetEnumMember(this Enum @enum)
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
        #endregion

        #region # 获取枚举成员名称字典 —— static IDictionary<string, string> GetEnumMembers(...
        /// <summary>
        /// 获取枚举成员名称字典
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <returns>枚举成员名称字典</returns>
        public static IDictionary<string, string> GetEnumMembers(this Type enumType)
        {
            #region # 验证参数

            if (!enumType.IsSubclassOf(typeof(Enum)))
            {
                throw new ArgumentOutOfRangeException(string.Format("类型\"{0}\"不是枚举类型！", enumType.Name));
            }

            #endregion

            FieldInfo[] fields = enumType.GetFields();

            IDictionary<string, string> dictionaries = new Dictionary<string, string>();

            foreach (FieldInfo field in fields.Where(x => x.Name != "value__"))
            {
                DescriptionAttribute enumMember = field.GetCustomAttribute<DescriptionAttribute>();

                dictionaries.Add(field.Name, enumMember == null ? field.Name : string.IsNullOrEmpty(enumMember.Description) ? field.Name : enumMember.Description);
            }

            return dictionaries;
        }
        #endregion
    }
}
