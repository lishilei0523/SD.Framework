using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;
using AutoMapper;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.PanGu;
using Newtonsoft.Json;

namespace ShSoft.Framework2016.Common.PoweredByLee
{
    /// <summary>
    /// 常用扩展方法
    /// </summary>
    public static class CommonExtension
    {
        #region # 静态字段及静态构造器

        /// <summary>
        /// 二进制序列化器
        /// </summary>
        private static readonly BinaryFormatter _BinaryFormatter;

        /// <summary>
        /// 静态构造函数
        /// </summary>
        static CommonExtension()
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
                StringBuilder sb = new StringBuilder();
                foreach (byte byt in buffer)
                {
                    sb.Append(byt.ToString("x2"));
                }
                return sb.ToString();
            }
        }
        #endregion

        #region # 计算流的MD5值扩展方法 —— static string ToMD5(this Stream stream)
        /// <summary>
        /// 计算流的MD5值扩展方法
        /// </summary>
        /// <param name="stream">流</param>
        /// <returns>MD5值</returns>
        public static string ToMD5(this Stream stream)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] buffer = md5.ComputeHash(stream);
                StringBuilder sb = new StringBuilder();
                foreach (byte byt in buffer)
                {
                    sb.Append(byt.ToString("x2"));
                }
                return sb.ToString();
            }
        }
        #endregion

        #region # 忽略大小写比较字符串是否相等扩展方法 —— static bool EqualsTo(this string source, string target)
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

        #region # object序列化JSON字符串扩展方法 —— static string ToJson(this object obj)
        /// <summary>
        /// object序列化JSON字符串扩展方法
        /// </summary>
        /// <param name="obj">object及其子类对象</param>
        /// <returns>JSON字符串</returns>
        public static string ToJson(this object obj)
        {
            #region # 验证参数

            if (obj == null)
            {
                return null;
            }

            #endregion

            try
            {
                JsonSerializerSettings settting = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
                return JsonConvert.SerializeObject(obj, Formatting.None, settting);
            }
            catch (InvalidOperationException)
            {
                return null;
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

        #region # object原型深拷贝扩展方法 —— static T Clone<T>(this object obj)
        /// <summary>
        /// object原型深拷贝扩展方法
        /// 异常：ArgumentNullException，NullReferenceException，SerializationException
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="obj">object及其子类对象</param>
        /// <returns>给定类型对象</returns>
        /// <exception cref="ArgumentNullException">源对象为空</exception>
        /// <exception cref="InvalidCastException">反序列化为给定类型失败</exception>
        /// <exception cref="SerializationException">对象类型未标记"Serializable"特性</exception>
        public static T Clone<T>(this object obj) where T : class
        {
            #region # 验证参数

            if (obj == null)
            {
                throw new ArgumentNullException("obj", @"源对象不可为空！");
            }

            #endregion

            using (Stream stream = new MemoryStream())
            {
                try
                {
                    _BinaryFormatter.Serialize(stream, obj);
                    stream.Position = 0;
                    T ectype = _BinaryFormatter.Deserialize(stream) as T;

                    #region # 非空验证

                    if (ectype == null)
                    {
                        throw new InvalidCastException(string.Format("无法将源类型\"{0}\"反序列化为给定类型\"{1}\"，请检查类型后重试！", obj.GetType().Name, typeof(T).Name));
                    }

                    #endregion

                    return ectype;
                }
                catch (SerializationException)
                {
                    throw new SerializationException(string.Format("给定对象类型\"{0}\"未标记\"Serializable\"特性！", obj.GetType().Name));
                }
            }
        }
        #endregion

        #region # object序列化为二进制字符串扩展方法 —— static string ToBinaryString(this object obj)
        /// <summary>
        /// object序列化为二进制字符串扩展方法
        /// </summary>
        /// <param name="obj">object及其子类</param>
        /// <returns>二进制字符串</returns>
        /// <exception cref="ArgumentNullException">源对象为空</exception>
        /// <exception cref="SerializationException">对象类型未标记"Serializable"特性</exception>
        public static string ToBinaryString(this object obj)
        {
            #region # 验证参数

            if (obj == null)
            {
                throw new ArgumentNullException("obj", @"源对象不可为空！");
            }

            #endregion

            using (MemoryStream stream = new MemoryStream())
            {
                try
                {
                    _BinaryFormatter.Serialize(stream, obj);
                    return Convert.ToBase64String(stream.ToArray());
                }
                catch (SerializationException)
                {
                    throw new SerializationException(string.Format("给定对象类型\"{0}\"未标记\"Serializable\"特性！", obj.GetType().Name));
                }
            }
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

        #region # object序列化Xml字符串扩展方法 —— static string ToXml(this object obj)
        /// <summary>
        /// object序列化Xml字符串扩展方法
        /// </summary>
        /// <param name="obj">object及其子类对象</param>
        /// <returns>Xml字符串</returns>
        /// <exception cref="ArgumentNullException">源对象为空</exception>
        public static string ToXml(this object obj)
        {
            #region # 验证参数

            if (obj == null)
            {
                throw new ArgumentNullException("obj", @"源对象不可为空！");
            }

            #endregion

            using (TextWriter stringWriter = new StringWriter())
            {
                XmlSerializer xmlSerializer = new XmlSerializer(obj.GetType());
                xmlSerializer.Serialize(stringWriter, obj);
                return stringWriter.ToString();
            }
        }
        #endregion

        #region # Xml字符串反序列化为对象扩展方法 —— static T XmlToObject<T>(this string xml)
        /// <summary>
        /// Xml字符串反序列化为对象扩展方法
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

        #region # 字符串分词扩展方法 —— static string[] SplitWord(this string str)
        /// <summary>
        /// 字符串分词扩展方法
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>分词后的字符串数组</returns>
        public static string[] SplitWord(this string str)
        {
            StringReader strReader = new StringReader(str);
            Analyzer analyzer = new PanGuAnalyzer();
            TokenStream tokenStream = analyzer.TokenStream(string.Empty, strReader);
            Token token;
            List<string> strs = new List<string>();
            while ((token = tokenStream.Next()) != null)
            {
                strs.Add(token.Term());
            }
            return strs.ToArray();
        }
        #endregion

        #region # 模型对象映射通用扩展方法 —— static T Map<T>(this object instance)
        /// <summary>
        /// 模型对象映射通用扩展方法
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="instance">源实例</param>
        /// <returns>目标类型对象</returns>
        public static T Map<T>(this object instance)
        {
            return instance.Map<object, T>();
        }
        #endregion

        #region # C#值转数据库值空值处理 —— static object ToDbValue(this object value)
        /// <summary>
        /// C#值转数据库值空值处理
        /// </summary>
        /// <param name="value">C#值</param>
        /// <returns>处理后的数据库值</returns>
        public static object ToDbValue(this object value)
        {
            return value ?? DBNull.Value;
        }
        #endregion

        #region # 获取本机IP地址 —— static string GetLocalIPAddress()
        /// <summary>
        /// 获取本机IP地址
        /// </summary>
        /// <returns>本机IP</returns>
        public static string GetLocalIPAddress()
        {
            StringBuilder buid = new StringBuilder();

            string hostName = Dns.GetHostName();//本机名   
            buid.Append(hostName + ";");
            IPAddress[] addressList = Dns.GetHostAddresses(hostName);//会返回所有地址，包括IPv4和IPv6   

            foreach (IPAddress ip in addressList)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    buid.Append(ip + ";");
                }
            }
            return buid.ToString();
        }
        #endregion
    }
}
