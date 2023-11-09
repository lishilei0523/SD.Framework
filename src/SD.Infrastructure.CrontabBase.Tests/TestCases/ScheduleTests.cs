using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SD.Infrastructure.CrontabBase.Mediators;
using SD.Infrastructure.CrontabBase.Tests.StubCrontabs;
using SD.IOC.Core.Mediators;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
#if NET45_OR_GREATER
using SD.IOC.Extension.NetFramework;
#endif
#if NETCOREAPP3_1_OR_GREATER
using SD.Common;
using SD.Toolkits;
using System.Configuration;
using System.Reflection;
using SD.IOC.Extension.NetCore;
#endif

namespace SD.Infrastructure.CrontabBase.Tests.TestCases
{
    /// <summary>
    /// 调度测试
    /// <remarks>Warning：测试用例请单个执行，同时执行存在问题。</remarks>
    /// </summary>
    [TestClass]
    public class ScheduleTests
    {
        #region # Initialization && Finalization

        /// <summary>
        /// 测试初始化
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
#if NETCOREAPP3_1_OR_GREATER
            Assembly entryAssembly = Assembly.GetExecutingAssembly();
            Configuration configuration = ConfigurationExtension.GetConfigurationFromAssembly(entryAssembly);
            FrameworkSection.Initialize(configuration);
            RedisSection.Initialize(configuration);
#endif
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

        #region # 测试配置调度 —— void TestCrontabConfiguration()
        /// <summary>
        /// 测试配置调度
        /// </summary>
        [TestMethod]
        public void TestCrontabConfiguration()
        {
            ScheduleMediator.Schedule<RecurrenceCrontab>();
        }
        #endregion

        #region # 测试Hello World —— void TestHelloWorld()
        /// <summary>
        /// 测试Hello World
        /// </summary>
        [TestMethod]
        public void TestHelloWorld()
        {
            //开始调度
            ScheduleMediator.Schedule<HelloWorldCrontab>();

            //线程睡眠
            Thread.Sleep(10000);
        }
        #endregion

        #region # 测试调度固定时间点 —— void TestScheduleFixedTimePoint()
        /// <summary>
        /// 测试调度固定时间点
        /// </summary>
        [TestMethod]
        public void TestScheduleFixedTimePoint()
        {
            //开始调度
            DateTime triggerTime = DateTime.Now.AddSeconds(3);
            FixedTimeStrategy fixedTimeStrategy = new FixedTimeStrategy(triggerTime);
            AlarmCrontab alarmCrontab = new AlarmCrontab("Hello World !", fixedTimeStrategy);

            Assert.IsTrue(alarmCrontab.Count == 0);
            Assert.IsFalse(alarmCrontab.Rung);

            ScheduleMediator.Schedule(alarmCrontab, 0);

            //线程睡眠
            Thread.Sleep(5000);

            Assert.IsTrue(alarmCrontab.Count == 1);
            Assert.IsTrue(alarmCrontab.Rung);
        }
        #endregion

        #region # 测试调度循环时间点 —— void TestScheduleRepeatedTimePoint()
        /// <summary>
        /// 测试调度循环时间点
        /// </summary>
        [TestMethod]
        public void TestScheduleRepeatedTimePoint()
        {
            //开始调度
            TimeSpan triggerTime = DateTime.Now.AddSeconds(3).TimeOfDay;
            RepeatedTimeStrategy repeatedTimeStrategy = new RepeatedTimeStrategy(triggerTime);
            AlarmCrontab alarmCrontab = new AlarmCrontab("Hello World !", repeatedTimeStrategy);

            Assert.IsTrue(alarmCrontab.Count == 0);
            Assert.IsFalse(alarmCrontab.Rung);

            ScheduleMediator.Schedule(alarmCrontab, 0);

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
            RecurrenceStrategy recurrenceStrategy = new RecurrenceStrategy(timeSpan);
            ShowTimeCrontab showTimeCrontab = new ShowTimeCrontab("Hello World !", recurrenceStrategy);

            Assert.IsTrue(showTimeCrontab.Count == 0);

            ScheduleMediator.Schedule(showTimeCrontab, 0);

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
            RecurrenceStrategy recurrenceStrategy = new RecurrenceStrategy(timeSpan);
            ShowTimeCrontab showTimeCrontab = new ShowTimeCrontab("Hello World !", recurrenceStrategy);

            Assert.IsTrue(showTimeCrontab.Count == 0);

            ScheduleMediator.Schedule(showTimeCrontab, 0);

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
            RecurrenceStrategy recurrenceStrategy = new RecurrenceStrategy(timeSpan);
            ShowTimeCrontab showTimeCrontab = new ShowTimeCrontab("Hello World !", recurrenceStrategy);

            Assert.IsTrue(showTimeCrontab.Count == 0);

            ScheduleMediator.Schedule(showTimeCrontab, 0);

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
            RecurrenceStrategy recurrenceStrategy = new RecurrenceStrategy(timeSpan);
            ShowTimeCrontab showTimeCrontab = new ShowTimeCrontab("Hello World !", recurrenceStrategy);

            Assert.IsTrue(showTimeCrontab.Count == 0);

            ScheduleMediator.Schedule(showTimeCrontab, 0);

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
            RecurrenceStrategy recurrenceStrategy = new RecurrenceStrategy(timeSpan);
            string cronExpression = DateTime.Now.AddSeconds(2).ToCronExpression();
            CronExpressionStrategy cronExpressionStrategy = new CronExpressionStrategy(cronExpression);

            ShowTimeCrontab showTimeCrontab = new ShowTimeCrontab("Hello World !", recurrenceStrategy);
            AlarmCrontab alarmCrontab = new AlarmCrontab("Hello World !", cronExpressionStrategy);

            ScheduleMediator.Schedule(showTimeCrontab, 0);
            ScheduleMediator.Schedule(alarmCrontab, 0);

            IList<ICrontab> crontabs = ScheduleMediator.FindAll();

            Assert.IsTrue(crontabs.Count == 2);
        }
        #endregion

        #region # 测试异常恢复任务 —— void TestRecoverCrontabs()
        /// <summary>
        /// 测试异常恢复任务
        /// </summary>
        [TestMethod]
        public void TestRecoverCrontabs()
        {
            ScheduleMediator.Recover();

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
            RecurrenceStrategy recurrenceStrategy = new RecurrenceStrategy(timeSpan);
            ShowTimeCrontab showTimeCrontab = new ShowTimeCrontab("Exception", recurrenceStrategy);
            ScheduleMediator.Schedule(showTimeCrontab, 0);

            //线程睡眠
            Thread.Sleep(5100);

            Assert.IsTrue(File.Exists(log));
        }
        #endregion
    }
}
