using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SD.Infrastructure.SchedulerBase.Mediator;
using SD.Infrastructure.SchedulerBase.Tests.StubCrontabs;
using SD.IOC.Core.Mediators;
using SD.IOC.Extension.NetFx;
using System.Diagnostics;
using System.Threading;

namespace SD.Infrastructure.SchedulerBase.Tests.TestCases
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
        }

        /// <summary>
        /// 测试调度时间点
        /// </summary>
        [TestMethod]
        public void TestScheduleTimePoint()
        {
            //清空调度任务
            ScheduleMediator.Clear();

            //开始调度，在当前时间2秒后执行
            AlarmCrontab alarmCrontab = new AlarmCrontab("Hello World !");

            Assert.IsFalse(alarmCrontab.Rung);

            ScheduleMediator.Schedule(alarmCrontab);

            Assert.IsTrue(alarmCrontab.Count == 0);

            //线程睡眠
            Thread.Sleep(5000);

            Assert.IsTrue(alarmCrontab.Count == 1);
            Assert.IsTrue(alarmCrontab.Rung);
        }

        /// <summary>
        /// 测试调度轮询
        /// </summary>
        [TestMethod]
        public void TestScheduleInspect()
        {
            //清空调度任务
            ScheduleMediator.Clear();

            //开始调度
            ShowTimeCrontab showTimeCrontab = new ShowTimeCrontab("Hello World !");

            Assert.IsTrue(!showTimeCrontab.Handled);
            Assert.IsTrue(showTimeCrontab.Count == 0);

            ScheduleMediator.Schedule(showTimeCrontab);

            //线程睡眠
            Thread.Sleep(5100);

            Trace.WriteLine(showTimeCrontab.Handled);
            Trace.WriteLine(showTimeCrontab.Count);
            Assert.IsTrue(showTimeCrontab.Handled);
            Assert.IsTrue(showTimeCrontab.Count == 3);
        }

        /// <summary>
        /// 测试异常
        /// </summary>
        [TestMethod]
        public void TestException()
        {
            //清空调度任务
            ScheduleMediator.Clear();

            //开始调度
            ShowTimeCrontab showTimeCrontab = new ShowTimeCrontab("Exception");
            ScheduleMediator.Schedule(showTimeCrontab);

            //线程睡眠
            Thread.Sleep(5100);
        }
    }
}
