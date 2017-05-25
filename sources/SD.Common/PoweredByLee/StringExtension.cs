using Newtonsoft.Json;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace SD.Common.PoweredByLee
{
    /// <summary>
    /// 字符串扩展方法
    /// </summary>
    public static class StringExtension
    {
        #region # 静态字段及静态构造器

        /// <summary>
        /// 二进制序列化器
        /// </summary>
        private static readonly BinaryFormatter _BinaryFormatter;

        /// <summary>
        /// 静态构造函数
        /// </summary>
        static StringExtension()
        {
            _BinaryFormatter = new BinaryFormatter();
        }

        #endregion

        #region # 计算字符串MD5值扩展方法 —— static string ToMD5(this string str)
        /// <summary>
        /// 计算字符串MD5值扩展方法
        /// </summary>
        /// <param name="str">待转换的字符串</param>
        /// <returns>MD5值</returns>
        public static string ToMD5(this string str)
        {
            byte[] buffer = Encoding.Default.GetBytes(str);
            using (MD5 md5 = MD5.Create())
            {
                buffer = md5.ComputeHash(buffer);
                StringBuilder md5Builder = new StringBuilder();
                foreach (byte @byte in buffer)
                {
                    md5Builder.Append(@byte.ToString("x2"));
                }
                return md5Builder.ToString();
            }
        }
        #endregion

        #region # 计算16位MD5值 —— static string ToHash16(this string str)
        /// <summary>
        /// 计算16位MD5值
        /// </summary>
        /// <param name="str">待转换的字符串</param>
        /// <returns>16位MD5值</returns>
        public static string ToHash16(this string str)
        {
            MD5CryptoServiceProvider md5Crypto = new MD5CryptoServiceProvider();
            byte[] buffer = md5Crypto.ComputeHash(Encoding.Default.GetBytes(str));
            string hash = BitConverter.ToString(buffer, 4, 8);
            hash = hash.Replace("-", "");

            return hash;
        }
        #endregion

        #region # 忽略大小写比较字符串是否相等扩展方法 —— static bool EqualsTo(this string source...
        /// <summary>
        /// 忽略大小写比较字符串是否相等扩展方法
        /// </summary>
        /// <param name="source">当前字符串</param>
        /// <param name="target">要比较的字符串</param>
        /// <returns>是否相同</returns>
        public static bool EqualsTo(this string source, string target)
        {
            if (string.IsNullOrWhiteSpace(source) && string.IsNullOrWhiteSpace(target))
            {
                return true;
            }
            return string.Equals(source, target, StringComparison.CurrentCultureIgnoreCase);
        }
        #endregion

        #region # 二进制字符串反序列化为对象扩展方法 —— static T BinaryToObject<T>(this string binaryStr)
        /// <summary>
        /// 二进制字符串反序列化为对象扩展方法
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="binaryStr">二进制字符串</param>
        /// <returns>给定类型对象</returns>
        /// <exception cref="ArgumentNullException">二进制字符串为空</exception>
        /// <exception cref="SerializationException">对象类型未标记"Serializable"特性</exception>
        /// <exception cref="InvalidCastException">反序列化为给定类型失败</exception>
        public static T BinaryToObject<T>(this string binaryStr)
        {
            #region # 验证参数

            if (string.IsNullOrWhiteSpace(binaryStr))
            {
                throw new ArgumentNullException("binaryStr", @"二进制字符串不可为空！");
            }

            #endregion

            using (MemoryStream stream = new MemoryStream(Convert.FromBase64String(binaryStr)))
            {
                try
                {
                    return (T)_BinaryFormatter.Deserialize(stream);
                }
                catch (SerializationException)
                {
                    throw new SerializationException(string.Format("给定对象类型\"{0}\"未标记\"Serializable\"特性！",
                        typeof(T).Name));
                }
                catch (InvalidCastException)
                {
                    throw new InvalidCastException(string.Format("无法将源二进制字符串反序列化为给定类型\"{0}\"，请检查类型后重试！", typeof(T).Name));
                }
            }
        }
        #endregion

        #region # XML字符串反序列化为对象扩展方法 —— static T XmlToObject<T>(this string xml)
        /// <summary>
        /// XML字符串反序列化为对象扩展方法
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="xml">Xml字符串</param>
        /// <returns>给定类型对象</returns>
        /// <exception cref="ArgumentNullException">Xml字符串为空</exception>
        public static T XmlToObject<T>(this string xml)
        {
            #region # 验证参数

            if (string.IsNullOrWhiteSpace(xml))
            {
                throw new ArgumentNullException("xml", @"Xml字符串不可为空！");
            }

            #endregion

            using (StringReader stringReader = new StringReader(xml))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                try
                {
                    return (T)xmlSerializer.Deserialize(stringReader);
                }
                catch (InvalidCastException)
                {
                    throw new InvalidCastException(string.Format("无法将源Xml字符串反序列化为给定类型\"{0}\"，请检查类型后重试！", typeof(T).Name));
                }
            }
        }
        #endregion

        #region # JSON字符串反序列化为对象扩展方法 —— static T JsonToObject<T>(this string json)
        /// <summary>
        /// JSON字符串反序列化为对象扩展方法
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="json">JSON字符串</param>
        /// <returns>给定类型对象</returns>
        /// <exception cref="ArgumentNullException">JSON字符串为空</exception>
        /// <exception cref="InvalidOperationException">反序列化为给定类型失败</exception>
        public static T JsonToObject<T>(this string json)
        {
            #region # 验证参数

            if (string.IsNullOrWhiteSpace(json))
            {
                throw new ArgumentNullException("json", @"JSON字符串不可为空！");
            }

            #endregion

            try
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (InvalidOperationException)
            {
                throw new InvalidOperationException(string.Format("无法将源JSON反序列化为给定类型\"{0}\"，请检查类型后重试！", typeof(T).Name));
            }
        }
        #endregion

        #region # 字符串过滤Html标签扩展方法 —— static string FilterHtml(this string str)
        /// <summary>
        /// 字符串过滤Html标签扩展方法
        /// </summary>
        /// <param name="str">待过虑的字符串</param>
        /// <returns>过滤后的字符串</returns>
        public static string FilterHtml(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return string.Empty;
            }
            str = Regex.Replace(str, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, @"<style[^>]*?>", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, @"</style>", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, @"<p[^>]*?>", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, @"<div[^>]*?>", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, @"</p>", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, @"</div>", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, @"-->", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, @"<!--.*", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "<[^>]*>", "", RegexOptions.Compiled);
            str = Regex.Replace(str, @"([\r\n])[\s]+", " ", RegexOptions.Compiled);
            return str.Replace("&nbsp;", " ");
        }
        #endregion

        #region # 字符串过滤SQL语句关键字扩展方法 —— static string FilterSql(this string sql)
        /// <summary>
        /// 字符串过滤SQL语句关键字扩展方法
        /// </summary>
        /// <param name="sql">SQL字符串</param>
        /// <returns>过滤后的SQL字符串</returns>
        public static string FilterSql(this string sql)
        {
            return sql.Replace("'", string.Empty);
        }
        #endregion
    }
}
