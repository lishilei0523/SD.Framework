using System;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShSoft.Framework2015.TaskScheduler.IScheduler;
using ShSoft.Framework2015.TaskSchedulerTests.StubTasks;

namespace ShSoft.Framework2015.TaskSchedulerTests.TestCases
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
        /// 测试调度时间点
        /// </summary>
        [TestMethod]
        public void TestScheduleTimePoint()
        {
            //清空调度任务
            ScheduleMediator.Clear();

            //开始调度，在当前时间2秒后执行
            ScheduleMediator.Schedule(StubAlarmTask.Detail, DateTime.Now.AddSeconds(2));

            //线程睡眠
            Thread.Sleep(2100);

            Assert.IsNotNull(StubAlarmTask.ReferenceParam);
        }

        /// <summary>
        /// 测试调度时间点
        /// </summary>
        [TestMethod]
        public void TestScheduleTimePoint_Error()
        {
            //清空调度任务
            ScheduleMediator.Clear();

            //开始调度，在当前时间2秒后执行
            ScheduleMediator.Schedule(StubAlarmTask.Detail, DateTime.Now.AddSeconds(2));

            Assert.IsNull(StubAlarmTask.ReferenceParam);
        }

        /// <summary>
        /// 测试调度轮询
        /// </summary>
        [TestMethod]
        public void TestScheduleInspect()
        {
            //清空调度任务
            ScheduleMediator.Clear();

            //清空参考系
            StubShowTimeTask.ReferenceTimes.Clear();

            //开始调度，每2秒执行一次
            ScheduleMediator.ScheduleBySecond(StubShowTimeTask.Detail, 2);

            //线程睡眠
            Thread.Sleep(4100);

            Assert.IsTrue(StubShowTimeTask.ReferenceTimes.Count == 3);
        }

        /// <summary>
        /// 测试调度轮询
        /// </summary>
        [TestMethod]
        public void TestScheduleInspect_Error()
        {
            //清空调度任务
            ScheduleMediator.Clear();

            //清空参考系
            StubShowTimeTask.ReferenceTimes.Clear();

            //开始调度，每2秒执行一次
            ScheduleMediator.ScheduleBySecond(StubShowTimeTask.Detail, 2);

            Assert.IsTrue(!StubShowTimeTask.ReferenceTimes.Any());
        }
    }
}
