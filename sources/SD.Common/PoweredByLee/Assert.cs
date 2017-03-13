using System;
using System.Collections.Generic;

namespace SD.Common.PoweredByLee
{
    /// <summary>
    /// 断言工具类
    /// </summary>
    public static class Assert
    {
        #region # 断言引用相等 —— void AreEqual(object source, object target...
        /// <summary>
        /// 断言引用相等
        /// </summary>
        /// <param name="source">源</param>
        /// <param name="target">目标</param>
        /// <param name="errorMessage">错误消息</param>
        public static void AreEqual(object source, object target, string errorMessage = null)
        {
            if (source != target)
            {
                throw new AssertFailedException(errorMessage);
            }
        }
        #endregion

        #region # 断言值相等 —— void AreEqual(ValueType source, ValueType target...
        /// <summary>
        /// 断言值相等
        /// </summary>
        /// <param name="source">源</param>
        /// <param name="target">目标</param>
        /// <param name="errorMessage">错误消息</param>
        public static void AreEqual(ValueType source, ValueType target, string errorMessage = null)
        {
            if (!Equals(source, target))
            {
                throw new AssertFailedException(errorMessage);
            }
        }
        #endregion

        #region # 断言为真 —— void IsTrue(bool condition, string errorMessage = null)
        /// <summary>
        /// 断言为真
        /// </summary>
        /// <param name="condition">条件</param>
        /// <param name="errorMessage">错误消息</param>
        public static void IsTrue(bool condition, string errorMessage = null)
        {
            if (!condition)
            {
                throw new AssertFailedException(errorMessage);
            }
        }
        #endregion

        #region # 断言为假 —— void IsFalse(bool condition, string errorMessage = null)
        /// <summary>
        /// 断言为假
        /// </summary>
        /// <param name="condition">条件</param>
        /// <param name="errorMessage">错误消息</param>
        public static void IsFalse(bool condition, string errorMessage = null)
        {
            if (condition)
            {
                throw new AssertFailedException(errorMessage);
            }
        }
        #endregion

        #region # 断言实例为空 —— void IsNull(object instance, string errorMessage = null)
        /// <summary>
        /// 断言实例为空
        /// </summary>
        /// <param name="instance">实例</param>
        /// <param name="errorMessage">错误消息</param>
        public static void IsNull(object instance, string errorMessage = null)
        {
            if (instance != null)
            {
                throw new AssertFailedException(errorMessage);
            }
        }
        #endregion

        #region # 断言实例不为空 —— void IsNotNull(object instance, string errorMessage = null)
        /// <summary>
        /// 断言实例不为空
        /// </summary>
        /// <param name="instance">实例</param>
        /// <param name="errorMessage">错误消息</param>
        public static void IsNotNull(object instance, string errorMessage = null)
        {
            if (instance == null)
            {
                throw new AssertFailedException(errorMessage);
            }
        }
        #endregion

        #region # 断言集合不为空 —— void IsNotEmpty<T>(IEnumerable<T> enumerable, string errorMessage = null)
        /// <summary>
        /// 断言集合不为空
        /// </summary>
        /// <param name="enumerable">集合</param>
        /// <param name="errorMessage">错误消息</param>
        public static void IsNotEmpty<T>(IEnumerable<T> enumerable, string errorMessage = null)
        {
            if (enumerable.IsNullOrEmpty())
            {
                throw new AssertFailedException(errorMessage);
            }
        }
        #endregion

        #region # 断言字符串不为空 —— void IsNotNull(string str, string errorMessage = null)
        /// <summary>
        /// 断言字符串不为空
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="errorMessage">错误消息</param>
        public static void IsNotNull(string str, string errorMessage = null)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                throw new AssertFailedException(errorMessage);
            }
        }
        #endregion

        #region # 断言字符串长度短于给定值 —— void IsShorterThan(string str, uint length, string errorMessage = null)
        /// <summary>
        /// 断言字符串长度短于给定值
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="length">给定长度</param>
        /// <param name="errorMessage">错误消息</param>
        public static void IsShorterThan(string str, uint length, string errorMessage = null)
        {
            if (str != null && str.Length > length)
            {
                throw new AssertFailedException(errorMessage);
            }
        }
        #endregion

        #region # 断言字符串长度长于给定值 —— void IsLongerThan(string str, uint length, string errorMessage = null)
        /// <summary>
        /// 断言字符串长度长于给定值
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="length">给定长度</param>
        /// <param name="errorMessage">错误消息</param>
        public static void IsLongerThan(string str, uint length, string errorMessage = null)
        {
            if ((str != null && str.Length < length) || str == null)
            {
                throw new AssertFailedException(errorMessage);
            }
        }
        #endregion

        #region # 断言数值在给定范围 —— void IsInRange(int value, int min, int max, string errorMessage = null)
        /// <summary>
        /// 断言数值在给定范围
        /// </summary>
        /// <param name="value">数值</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <param name="errorMessage">错误消息</param>
        public static void IsInRange(int value, int min, int max, string errorMessage = null)
        {
            if (value < min || value > max)
            {
                throw new AssertFailedException(errorMessage);
            }
        }
        #endregion
    }

    /// <summary>
    /// 断言失败异常
    /// </summary>
    [Serializable]
    public class AssertFailedException : ApplicationException
    {
        /// <summary>
        /// 无参构造器
        /// </summary>
        public AssertFailedException() { }

        /// <summary>
        /// 基础构造器
        /// </summary>
        /// <param name="message"></param>
        public AssertFailedException(string message)
            : base(message)
        {

        }
    }
}
