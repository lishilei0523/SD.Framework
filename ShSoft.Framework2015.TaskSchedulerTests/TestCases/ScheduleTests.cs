using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShSoft.Framework2015.TaskScheduler.IScheduler;
using ShSoft.Framework2015.TaskSchedulerTests.StubTasks;

namespace ShSoft.Framework2015.TaskSchedulerTests.TestCases
{
    /// <summary>
    /// 调度测试
    /// </summary>
    [TestClass]
    public class ScheduleTests
    {
        /// <summary>
        /// 测试调度时间点
        /// </summary>
        [TestMethod]
        public void TestScheduleTimePoint()
        {
            //开始调度，在当前时间2秒后执行
            ScheduleMediator.Schedule(StubAlarmTask.Detail, DateTime.Now.AddSeconds(2));

            //线程睡眠
            Thread.Sleep(2100);

            Assert.IsNotNull(StubAlarmTask.ReferenceParam);
        }

        /// <summary>
        /// 测试调度轮询
        /// </summary>
        [TestMethod]
        public void TestScheduleInspect()
        {
            //清空参考系
            StubShowTimeTask.ReferenceTimes.Clear();

            //开始调度，每2秒执行一次
            ScheduleMediator.SchedulePerSecond(StubShowTimeTask.Detail, 2);

            //线程睡眠
            Thread.Sleep(4100);

            Assert.IsTrue(StubShowTimeTask.ReferenceTimes.Count == 3);
        }
    }
}
