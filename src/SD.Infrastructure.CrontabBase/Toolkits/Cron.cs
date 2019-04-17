using System;

namespace SD.Infrastructure.CrontabBase.Toolkits
{
    /// <summary>
    /// Cron表达式扩展
    /// </summary>
    public static class CronExtension
    {
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
