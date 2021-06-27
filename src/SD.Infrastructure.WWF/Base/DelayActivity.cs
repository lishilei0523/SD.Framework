using System;
using System.Activities;
using System.Threading;

namespace SD.Infrastructure.WWF.Base
{
    /// <summary>
    /// 延迟代码活动
    /// </summary>
    public sealed class DelayActivity : CodeActivity
    {
        /// <summary>
        /// 延迟时间段
        /// </summary>
        public InArgument<TimeSpan> TimeSpan { get; set; }

        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="context">活动上下文</param>
        protected override void Execute(CodeActivityContext context)
        {
            TimeSpan timeSpan = context.GetValue(this.TimeSpan);
            Thread.Sleep(timeSpan);
        }
    }
}
