using System;

namespace SD.Infrastructure.CrontabBase
{
    /// <summary>
    /// Cron表达式扩展
    /// </summary>
    public static class CronExtension
    {
        /// <summary>
        /// 日期时间转换Cron表达式
        /// </summary>
        /// <param name="dateTime">日期时间</param>
        /// <returns>Cron表达式</returns>
        public static string ToCronExpression(this DateTime dateTime)
        {
            string cron = $"{dateTime.Second} {dateTime.Minute} {dateTime.Hour} {dateTime.Day} {dateTime.Month} ? {dateTime.Year}";

            return cron;
        }
    }
}
