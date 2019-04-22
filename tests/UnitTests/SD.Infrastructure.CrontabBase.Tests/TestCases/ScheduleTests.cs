using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SD.Infrastructure.CrontabBase.Mediator;
using SD.Infrastructure.CrontabBase.Tests.StubCrontabs;
using SD.Infrastructure.CrontabBase.Toolkits;
using SD.IOC.Core.Mediators;
using SD.IOC.Extension.NetFx;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace SD.Infrastructure.CrontabBase.Tests.TestCases
{
    /// <summary>
    /// 调度测试
    /// <remarks>
    /// Warning：测试用例请单个执行，同时执行存在问题。
    /// </remarks>
    /// </summary>
    [TestClass]
    public class ScheduleTests
    {
        #region # Initialization && Finalization

        /// <summary>
        /// 测试初始化
        /// </summary>
        [TestInitialize]
        public void Init()
        {
            if (!ResolveMediator.ContainerBuilt)
            {
                IServiceCollection builder = ResolveMediator.GetServiceCollection();
                builder.RegisterConfigs();

                ResolveMediator.Build();
            }

            ScheduleMediator.Clear();
        }

        /// <summary>
        /// 测试清理
        /// </summary>
        [TestCleanup]
        public void Cleanup()
        {
            if (ResolveMediator.ContainerBuilt)
            {
                ResolveMediator.Dispose();
            }

            ScheduleMediator.Clear();
        }

        #endregion

        #region # 测试调度时间点 —— void TestScheduleTimePoint()
        /// <summary>
        /// 测试调度时间点
        /// </summary>
        [TestMethod]
        public void TestScheduleTimePoint()
        {
            //开始调度
            string cronExpression = DateTime.Now.AddSeconds(2).ToCronExpression();
            CronExpressionStrategy cronExpressionStrategy = new CronExpressionStrategy(cronExpression);
            AlarmCrontab alarmCrontab = new AlarmCrontab("Hello World !", cronExpressionStrategy);

            Assert.IsTrue(alarmCrontab.Count == 0);
            Assert.IsFalse(alarmCrontab.Rung);

            ScheduleMediator.Schedule(alarmCrontab);

            //线程睡眠
            Thread.Sleep(5000);

            Assert.IsTrue(alarmCrontab.Count == 1);
            Assert.IsTrue(alarmCrontab.Rung);
        }
        #endregion

        #region # 测试调度轮询 —— void TestScheduleInspect()
        /// <summary>
        /// 测试调度轮询
        /// </summary>
        [TestMethod]
        public void TestScheduleInspect()
        {
            //开始调度
            TimeSpan timeSpan = new TimeSpan(0, 0, 2);
            TimeSpanStrategy timeSpanStrategy = new TimeSpanStrategy(timeSpan);
            ShowTimeCrontab showTimeCrontab = new ShowTimeCrontab("Hello World !", timeSpanStrategy);

            Assert.IsTrue(showTimeCrontab.Count == 0);

            ScheduleMediator.Schedule(showTimeCrontab);

            //线程睡眠
            Thread.Sleep(5000);

            Assert.IsTrue(showTimeCrontab.Count == 3);
        }
        #endregion

        #region # 测试暂停任务 —— void TestPauseCrontab()
        /// <summary>
        /// 测试暂停任务
        /// </summary>
        [TestMethod]
        public void TestPauseCrontab()
        {
            //开始调度
            TimeSpan timeSpan = new TimeSpan(0, 0, 2);
            TimeSpanStrategy timeSpanStrategy = new TimeSpanStrategy(timeSpan);
            ShowTimeCrontab showTimeCrontab = new ShowTimeCrontab("Hello World !", timeSpanStrategy);

            Assert.IsTrue(showTimeCrontab.Count == 0);

            ScheduleMediator.Schedule(showTimeCrontab);

            //线程睡眠
            Thread.Sleep(5000);

            Assert.IsTrue(showTimeCrontab.Count == 3);

            //暂停
            ScheduleMediator.Pause(showTimeCrontab.Id);

            //线程睡眠
            Thread.Sleep(3000);

            Assert.IsTrue(showTimeCrontab.Count == 3);
        }
        #endregion

        #region # 测试恢复任务 —— void TestResumeCrontab()
        /// <summary>
        /// 测试恢复任务
        /// </summary>
        [TestMethod]
        public void TestResumeCrontab()
        {
            //开始调度
            TimeSpan timeSpan = new TimeSpan(0, 0, 2);
            TimeSpanStrategy timeSpanStrategy = new TimeSpanStrategy(timeSpan);
            ShowTimeCrontab showTimeCrontab = new ShowTimeCrontab("Hello World !", timeSpanStrategy);

            Assert.IsTrue(showTimeCrontab.Count == 0);

            ScheduleMediator.Schedule(showTimeCrontab);

            //线程睡眠
            Thread.Sleep(5000);

            Assert.IsTrue(showTimeCrontab.Count == 3);

            //暂停
            ScheduleMediator.Pause(showTimeCrontab.Id);

            //线程睡眠
            Thread.Sleep(5000);

            Assert.IsTrue(showTimeCrontab.Count == 3);

            //恢复
            ScheduleMediator.Resume(showTimeCrontab.Id);

            //线程睡眠
            Thread.Sleep(5000);

            Assert.IsTrue(showTimeCrontab.Count > 3);
        }
        #endregion

        #region # 测试删除任务 —— void TestRemoveCrontab()
        /// <summary>
        /// 测试删除任务
        /// </summary>
        [TestMethod]
        public void TestRemoveCrontab()
        {
            //开始调度
            TimeSpan timeSpan = new TimeSpan(0, 0, 2);
            TimeSpanStrategy timeSpanStrategy = new TimeSpanStrategy(timeSpan);
            ShowTimeCrontab showTimeCrontab = new ShowTimeCrontab("Hello World !", timeSpanStrategy);

            Assert.IsTrue(showTimeCrontab.Count == 0);

            ScheduleMediator.Schedule(showTimeCrontab);

            //线程睡眠
            Thread.Sleep(5100);

            Assert.IsTrue(showTimeCrontab.Count == 3);

            ScheduleMediator.Remove(showTimeCrontab);

            //线程睡眠
            Thread.Sleep(5100);

            Assert.IsTrue(showTimeCrontab.Count == 3);
        }
        #endregion

        #region # 测试获取全部任务 —— void TestFindAllCrontabs()
        /// <summary>
        /// 测试获取全部任务
        /// </summary>
        [TestMethod]
        public void TestFindAllCrontabs()
        {
            TimeSpan timeSpan = new TimeSpan(0, 0, 2);
            TimeSpanStrategy timeSpanStrategy = new TimeSpanStrategy(timeSpan);
            string cronExpression = DateTime.Now.AddSeconds(2).ToCronExpression();
            CronExpressionStrategy cronExpressionStrategy = new CronExpressionStrategy(cronExpression);

            ShowTimeCrontab showTimeCrontab = new ShowTimeCrontab("Hello World !", timeSpanStrategy);
            AlarmCrontab alarmCrontab = new AlarmCrontab("Hello World !", cronExpressionStrategy);

            ScheduleMediator.Schedule(showTimeCrontab);
            ScheduleMediator.Schedule(alarmCrontab);

            IList<ICrontab> crontabs = ScheduleMediator.FindAll();

            Assert.IsTrue(crontabs.Count == 2);
        }
        #endregion

        #region # 测试恢复全部任务 —— void TestResumeAllCrontabs()
        /// <summary>
        /// 测试恢复全部任务
        /// </summary>
        [TestMethod]
        public void TestResumeAllCrontabs()
        {
            ScheduleMediator.ResumeAll();

            //线程睡眠
            Thread.Sleep(6000);
        }
        #endregion

        #region # 测试异常 —— void TestException()
        /// <summary>
        /// 测试异常
        /// </summary>
        [TestMethod]
        public void TestException()
        {
            string log = $"{AppDomain.CurrentDomain.BaseDirectory}\\ScheduleLogs\\ExceptionLogs\\{DateTime.Today:yyyyMMdd}.txt";

            //开始调度
            TimeSpan timeSpan = new TimeSpan(0, 0, 2);
            TimeSpanStrategy timeSpanStrategy = new TimeSpanStrategy(timeSpan);
            ShowTimeCrontab showTimeCrontab = new ShowTimeCrontab("Exception", timeSpanStrategy);
            ScheduleMediator.Schedule(showTimeCrontab);

            //线程睡眠
            Thread.Sleep(5100);

            Assert.IsTrue(File.Exists(log));
        }
        #endregion
    }
}
