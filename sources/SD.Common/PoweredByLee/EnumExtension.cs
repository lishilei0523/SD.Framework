using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace SD.Common.PoweredByLee
{
    /// <summary>
    /// 枚举扩展工具类
    /// </summary>
    public static class EnumExtension
    {
        #region # 常量

        /// <summary>
        /// 枚举值字段
        /// </summary>
        private const string EnumValueField = "value__";

        #endregion

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
        /// IDictionary[string, string]，[枚举名，枚举描述]
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

            foreach (FieldInfo field in fields.Where(x => x.Name != EnumValueField))
            {
                DescriptionAttribute enumMember = field.GetCustomAttribute<DescriptionAttribute>();

                dictionaries.Add(field.Name, enumMember == null ? field.Name : string.IsNullOrEmpty(enumMember.Description) ? field.Name : enumMember.Description);
            }

            return dictionaries;
        }
        #endregion

        #region # 将字符串转换成可空的枚举 —— static T? GetEnum<T>(this string enumStr)
        /// <summary>
        /// 将字符串转换成可空的枚举
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="enumStr">枚举的字符串值</param>
        /// <returns>可空的枚举值</returns>
        public static T? GetEnum<T>(this string enumStr) where T : struct
        {
            #region # 验证参数

            if (string.IsNullOrWhiteSpace(enumStr))
            {
                return null;
            }
            if (typeof(T).IsSubclassOf(typeof(Enum)))
            {
                throw new ArgumentOutOfRangeException(string.Format("类型\"{0}\"不是枚举类型！", typeof(T).Name));
            }

            #endregion

            T @enum;

            if (!Enum.TryParse(enumStr, out @enum))
            {
                throw new InvalidCastException(string.Format("无法将给定字符串\"{0}\"转换为枚举\"{1}\"！", enumStr, typeof(T).Name));
            }

            return @enum;
        }
        #endregion

        #region # 获取枚举类型完整信息 —— static IEnumerable<Tuple<int, string, string>> GetEnumMemberInfos(...
        /// <summary>
        /// 获取枚举类型完整信息
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <returns>完整信息</returns>
        /// <remarks>
        /// Tuple[int, string, string]，[枚举int值，枚举名，枚举描述]
        /// </remarks>
        public static IEnumerable<Tuple<int, string, string>> GetEnumMemberInfos(this Type enumType)
        {
            #region # 验证参数

            if (!enumType.IsSubclassOf(typeof(Enum)))
            {
                throw new ArgumentOutOfRangeException(string.Format("类型\"{0}\"不是枚举类型！", enumType.Name));
            }

            #endregion

            FieldInfo[] fields = enumType.GetFields();

            ICollection<Tuple<int, string, string>> enumInfos = new HashSet<Tuple<int, string, string>>();

            foreach (FieldInfo field in fields.Where(x => x.Name != EnumValueField))
            {
                DescriptionAttribute enumMember = field.GetCustomAttribute<DescriptionAttribute>();
                int value = Convert.ToInt32(field.GetValue(Activator.CreateInstance(enumType)));

                enumInfos.Add(new Tuple<int, string, string>(value, field.Name, enumMember == null ? field.Name : string.IsNullOrEmpty(enumMember.Description) ? field.Name : enumMember.Description));
            }

            return enumInfos;
        }
        #endregion

        #region # 获取枚举值、描述字典 —— static IDictionary<int, string> GetEnumDictionary(...
        /// <summary>
        /// 获取枚举值、描述字典
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <returns>枚举值、描述字典</returns>
        /// IDictionary[int, string]，[枚举int值，枚举描述]
        public static IDictionary<int, string> GetEnumDictionary(this Type enumType)
        {
            IEnumerable<Tuple<int, string, string>> tuples = GetEnumMemberInfos(enumType);

            IDictionary<int, string> dictionary = new Dictionary<int, string>();
            foreach (Tuple<int, string, string> tuple in tuples)
            {
                dictionary.Add(tuple.Item1, tuple.Item3);
            }

            return dictionary;
        }
        #endregion
    }
}
