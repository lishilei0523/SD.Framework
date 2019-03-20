using System;

namespace SD.Infrastructure.CrontabBase.Toolkits
{
    /// <summary>
    /// Cron表达式扩展
    /// </summary>
    public static class Cron
    {
        #region # 每若干小时间隔 —— static string Hourly(byte hours)
        /// <summary>
        /// 每若干小时间隔
        /// </summary>
        /// <param name="hours">时间间隔（小时）</param>
        /// <returns>Cron表达式</returns>
        public static string Hourly(byte hours)
        {
            hours = hours > (byte)23 ? (byte)23 : hours;

            string cron = $"0 * 0/{hours} * * ? ";

            return cron;
        }
        #endregion

        #region # 每若干分钟间隔 —— static string Minutely(byte minutes)
        /// <summary>
        /// 每若干分钟间隔
        /// </summary>
        /// <param name="minutes">时间间隔（分钟）</param>
        /// <returns>Cron表达式</returns>
        public static string Minutely(byte minutes)
        {
            minutes = minutes > (byte)59 ? (byte)59 : minutes;

            string cron = $"0 0/{minutes} * * * ? ";

            return cron;
        }
        #endregion

        #region # 每若干秒间隔 —— static string Secondly(byte seconds)
        /// <summary>
        /// 每若干秒间隔
        /// </summary>
        /// <param name="seconds">时间间隔（秒）</param>
        /// <returns>Cron表达式</returns>
        public static string Secondly(byte seconds)
        {
            seconds = seconds > (byte)59 ? (byte)59 : seconds;

            string cron = $"0/{seconds} * * * * ? ";

            return cron;
        }
        #endregion

        #region # DateTime转换Cron表达式扩展方法 —— static string ToCronExpression(this DateTime dateTime)
        /// <summary>
        /// DateTime转换Cron表达式扩展方法
        /// </summary>
        /// <returns>Cron表达式</returns>
        public static string ToCronExpression(this DateTime dateTime)
        {
            string cron = $"{dateTime.Second} {dateTime.Minute} {dateTime.Hour} {dateTime.Day} {dateTime.Month} ? {dateTime.Year}";

            return cron;
        }
        #endregion
    }
}
