﻿using SD.Infrastructure.CrontabBase;
using SD.Infrastructure.SchedulerBase.Tests.StubCrontabs;
using System;
using System.Diagnostics;

namespace SD.Infrastructure.SchedulerBase.Tests.StubCrontabSchedulerss
{
    public class AlarmCrontabExecutor : CrontabExecutor<AlarmCrontab>
    {
        /// <summary>
        /// 执行任务
        /// </summary>
        /// <param name="crontab">定时任务</param>
        public override void Execute(AlarmCrontab crontab)
        {
            Trace.WriteLine(crontab.Word);
            Console.WriteLine(crontab.Word);

            crontab.Rung = true;
            crontab.Count++;
        }
    }
}
