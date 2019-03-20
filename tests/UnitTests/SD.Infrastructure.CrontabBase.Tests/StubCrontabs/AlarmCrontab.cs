﻿using System;
using SD.Infrastructure.CrontabBase.Toolkits;

namespace SD.Infrastructure.CrontabBase.Tests.StubCrontabs
{
    /// <summary>
    /// 闹钟任务
    /// </summary>
    public class AlarmCrontab : Crontab
    {
        protected AlarmCrontab()
        {
            this.CronExpression = DateTime.Now.AddSeconds(2).ToCronExpression();
        }

        /// <summary>
        /// 无参构造器
        /// </summary>
        public AlarmCrontab(string word)
            : this()
        {
            this.Word = word;
        }


        public int Count { get; set; }
        public bool Rung { get; set; }
        public string Word { get; set; }
    }
}