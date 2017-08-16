using System;

namespace SD.Common.PoweredByLee
{
    /// <summary>
    /// 数值类型常用扩展方法
    /// </summary>
    public static class NumbericExtension
    {
        #region # short类型是否为0或负数 —— static bool IsZeroOrMinus(this short number)
        /// <summary>
        /// short类型是否为0或负数
        /// </summary>
        /// <param name="number">数值</param>
        /// <returns>是否为0或负数</returns>
        public static bool IsZeroOrMinus(this short number)
        {
            if (number <= 0)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region # int类型是否为0或负数 —— static bool IsZeroOrMinus(this int number)
        /// <summary>
        /// int类型是否为0或负数
        /// </summary>
        /// <param name="number">数值</param>
        /// <returns>是否为0或负数</returns>
        public static bool IsZeroOrMinus(this int number)
        {
            if (number <= 0)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region # long类型是否为0或负数 —— static bool IsZeroOrMinus(this long number)
        /// <summary>
        /// long类型是否为0或负数
        /// </summary>
        /// <param name="number">数值</param>
        /// <returns>是否为0或负数</returns>
        public static bool IsZeroOrMinus(this long number)
        {
            if (number <= 0)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region # float类型是否为0或负数 —— static bool IsZeroOrMinus(this float number)
        /// <summary>
        /// float类型是否为0或负数
        /// </summary>
        /// <param name="number">数值</param>
        /// <returns>是否为0或负数</returns>
        public static bool IsZeroOrMinus(this float number)
        {
            if (number <= 0)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region # double类型是否为0或负数 —— static bool IsZeroOrMinus(this double number)
        /// <summary>
        /// double类型是否为0或负数
        /// </summary>
        /// <param name="number">数值</param>
        /// <returns>是否为0或负数</returns>
        public static bool IsZeroOrMinus(this double number)
        {
            if (number <= 0)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region # decimal类型是否为0或负数 —— static bool IsZeroOrMinus(this decimal number)
        /// <summary>
        /// decimal类型是否为0或负数
        /// </summary>
        /// <param name="number">数值</param>
        /// <returns>是否为0或负数</returns>
        public static bool IsZeroOrMinus(this decimal number)
        {
            if (number <= 0)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region # float类型是否为百分数 —— static bool IsPercentage(this float number)
        /// <summary>
        /// float类型是否为百分数
        /// </summary>
        /// <param name="number">数值</param>
        /// <returns>是否为百分数</returns>
        public static bool IsPercentage(this float number)
        {
            if (number > 0 && number <= 1)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region # double类型是否为百分数 —— static bool IsPercentage(this double number)
        /// <summary>
        /// double类型是否为百分数
        /// </summary>
        /// <param name="number">数值</param>
        /// <returns>是否为百分数</returns>
        public static bool IsPercentage(this double number)
        {
            if (number > 0 && number <= 1)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region # decimal类型是否为百分数 —— static bool IsPercentage(this decimal number)
        /// <summary>
        /// decimal类型是否为百分数
        /// </summary>
        /// <param name="number">数值</param>
        /// <returns>是否为百分数</returns>
        public static bool IsPercentage(this decimal number)
        {
            if (number > 0 && number <= 1)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region # float类型是否为0或百分数 —— static bool IsZeroOrPercentage(this float number)
        /// <summary>
        /// float类型是否为0或百分数
        /// </summary>
        /// <param name="number">数值</param>
        /// <returns>是否为0或百分数</returns>
        public static bool IsZeroOrPercentage(this float number)
        {
            if (number.IsPercentage() || number.Equals(0f))
            {
                return true;
            }
            return false;
        }
        #endregion

        #region # double类型是否为0或百分数 —— static bool IsZeroOrPercentage(this double number)
        /// <summary>
        /// double类型是否为0或百分数
        /// </summary>
        /// <param name="number">数值</param>
        /// <returns>是否为0或百分数</returns>
        public static bool IsZeroOrPercentage(this double number)
        {
            if (number.IsPercentage() || number.Equals(0d))
            {
                return true;
            }
            return false;
        }
        #endregion

        #region # decimal类型是否为0或百分数 —— static bool IsZeroOrPercentage(this decimal number)
        /// <summary>
        /// decimal类型是否为0或百分数
        /// </summary>
        /// <param name="number">数值</param>
        /// <returns>是否为0或百分数</returns>
        public static bool IsZeroOrPercentage(this decimal number)
        {
            if (number.IsPercentage() || number.Equals(0m))
            {
                return true;
            }
            return false;
        }
        #endregion

        #region # float类型保留小数 —— static float KeepDigits(this float number, int digits)
        /// <summary>
        /// float类型保留2位百分数
        /// </summary>
        /// <param name="number">数值</param>
        /// <param name="digits">保留位数</param>
        /// <returns>保留位数后的数值</returns>
        public static float KeepDigits(this float number, int digits)
        {
            decimal dec = unchecked((decimal)number);
            decimal result = Math.Round(dec, digits, MidpointRounding.AwayFromZero);

            return unchecked((float)result);
        }
        #endregion

        #region # double类型保留小数 —— static double KeepDigits(this double number, int digits)
        /// <summary>
        /// double类型保留2位百分数
        /// </summary>
        /// <param name="number">数值</param>
        /// <param name="digits">保留位数</param>
        /// <returns>保留位数后的数值</returns>
        public static double KeepDigits(this double number, int digits)
        {
            decimal dec = unchecked((decimal)number);
            decimal result = Math.Round(dec, digits, MidpointRounding.AwayFromZero);

            return unchecked((double)result);
        }
        #endregion

        #region # decimal类型保留小数 —— static decimal KeepDigits(this decimal number, int digits)
        /// <summary>
        /// decimal类型保留2位百分数
        /// </summary>
        /// <param name="number">数值</param>
        /// <param name="digits">保留位数</param>
        /// <returns>保留位数后的数值</returns>
        public static decimal KeepDigits(this decimal number, int digits)
        {
            return Math.Round(number, digits, MidpointRounding.AwayFromZero);
        }
        #endregion

        #region # byte类型是否在给定闭区间 —— static bool IsIn(this byte number, byte min, byte max)
        /// <summary>
        /// byte类型是否在给定闭区间
        /// </summary>
        /// <param name="number">数值</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <returns>是否为百分数</returns>
        public static bool IsIn(this byte number, byte min, byte max)
        {
            if (min > max)
            {
                throw new ArgumentOutOfRangeException("min", @"最小值不可大于最大值！");
            }
            if (number >= min && number <= max)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region # short类型是否在给定闭区间 —— static bool IsIn(this short number, short min, short max)
        /// <summary>
        /// short类型是否在给定闭区间
        /// </summary>
        /// <param name="number">数值</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <returns>是否为百分数</returns>
        public static bool IsIn(this short number, short min, short max)
        {
            if (min > max)
            {
                throw new ArgumentOutOfRangeException("min", @"最小值不可大于最大值！");
            }
            if (number >= min && number <= max)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region # int类型是否在给定闭区间 —— static bool IsIn(this int number, int min, int max)
        /// <summary>
        /// int类型是否在给定闭区间
        /// </summary>
        /// <param name="number">数值</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <returns>是否为百分数</returns>
        public static bool IsIn(this int number, int min, int max)
        {
            if (min > max)
            {
                throw new ArgumentOutOfRangeException("min", @"最小值不可大于最大值！");
            }
            if (number >= min && number <= max)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region # long类型是否在给定闭区间 —— static bool IsIn(this long number, long min, long max)
        /// <summary>
        /// long类型是否在给定闭区间
        /// </summary>
        /// <param name="number">数值</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <returns>是否为百分数</returns>
        public static bool IsIn(this long number, long min, long max)
        {
            if (min > max)
            {
                throw new ArgumentOutOfRangeException("min", @"最小值不可大于最大值！");
            }
            if (number >= min && number <= max)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region # float类型是否在给定闭区间 —— static bool IsIn(this float number, float min, float max)
        /// <summary>
        /// float类型是否在给定闭区间
        /// </summary>
        /// <param name="number">数值</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <returns>是否为百分数</returns>
        public static bool IsIn(this float number, float min, float max)
        {
            if (min > max)
            {
                throw new ArgumentOutOfRangeException("min", @"最小值不可大于最大值！");
            }
            if (number >= min && number <= max)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region # double类型是否在给定闭区间 —— static bool IsIn(this double number, double min, double max)
        /// <summary>
        /// double类型是否在给定闭区间
        /// </summary>
        /// <param name="number">数值</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <returns>是否为百分数</returns>
        public static bool IsIn(this double number, double min, double max)
        {
            if (min > max)
            {
                throw new ArgumentOutOfRangeException("min", @"最小值不可大于最大值！");
            }
            if (number >= min && number <= max)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region # decimal类型是否在给定闭区间 —— static bool IsIn(this decimal number, decimal min, decimal max)
        /// <summary>
        /// decimal类型是否在给定闭区间
        /// </summary>
        /// <param name="number">数值</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <returns>是否为百分数</returns>
        public static bool IsIn(this decimal number, decimal min, decimal max)
        {
            if (min > max)
            {
                throw new ArgumentOutOfRangeException("min", @"最小值不可大于最大值！");
            }
            if (number >= min && number <= max)
            {
                return true;
            }
            return false;
        }
        #endregion
    }
}
