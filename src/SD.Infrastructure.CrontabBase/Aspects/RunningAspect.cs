using ArxOne.MrAdvice.Advice;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SD.Infrastructure.CrontabBase.Aspects
{
    /// <summary>
    /// 运行日志特性
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class RunningAspect : Attribute, IMethodAdvice
    {
        #region # 字段

        /// <summary>
        /// 日志目录
        /// </summary>
        private static readonly string _LogDirectory = $"{AppDomain.CurrentDomain.BaseDirectory}\\ScheduleLogs";

        /// <summary>
        /// 运行日志路径
        /// </summary>
        private static readonly string _RunningLogPath = $"{_LogDirectory}\\RunningLogs\\{{0:yyyyMMdd}}.txt";

        /// <summary>
        /// 异常日志路径
        /// </summary>
        private static readonly string _ExceptionLogPath = $"{_LogDirectory}\\ExceptionLogs\\{{0:yyyyMMdd}}.txt";

        /// <summary>
        /// 同步锁
        /// </summary>
        private static readonly object _Sync = new object();

        #endregion

        #region # 拦截方法 —— void Advise(MethodAdviceContext context)
        /// <summary>
        /// 拦截方法
        /// </summary>
        /// <param name="context">方法元数据</param>
        public void Advise(MethodAdviceContext context)
        {
            Type executorType = context.TargetMethod.DeclaringType;
            FieldInfo logAppenderfField = executorType.GetField("_logAppender", BindingFlags.NonPublic | BindingFlags.Instance);
            string crontabName = executorType.GenericTypeArguments.Single().Name;

            Stopwatch watch = new Stopwatch();

            try
            {
                DateTime startTime = DateTime.Now;
                watch.Start();

                context.Proceed();

                watch.Stop();
                DateTime endTime = DateTime.Now;

                StringBuilder logAppender = (StringBuilder)logAppenderfField.GetValue(context.Target);
                Task.Run(() =>
                {
                    this.WriteFile(string.Format(_RunningLogPath, DateTime.Today),
                        "===================================运行正常, 详细信息如下==================================="
                                        + Environment.NewLine + "［当前任务］" + crontabName
                                        + Environment.NewLine + "［开始时间］" + startTime
                                        + Environment.NewLine + "［结束时间］" + endTime
                                        + Environment.NewLine + "［运行耗时］" + watch.Elapsed
                                        + Environment.NewLine + logAppender
                                        + Environment.NewLine + Environment.NewLine);
                });
            }
            catch (Exception exception)
            {
                StringBuilder logAppender = (StringBuilder)logAppenderfField.GetValue(context.Target);
                Task.Run(() =>
                {
                    this.WriteFile(string.Format(_ExceptionLogPath, DateTime.Today),
                        "===================================运行异常, 详细信息如下==================================="
                                        + Environment.NewLine + "［当前任务］" + crontabName
                                        + Environment.NewLine + "［异常时间］" + DateTime.Now
                                        + Environment.NewLine + "［异常消息］" + exception.Message
                                        + Environment.NewLine + "［异常明细］" + exception
                                        + Environment.NewLine + "［内部异常］" + exception.InnerException
                                        + Environment.NewLine + "［堆栈信息］" + exception.StackTrace
                                        + Environment.NewLine + logAppender
                                        + Environment.NewLine + Environment.NewLine);
                });
            }
        }
        #endregion

        #region # 写入文件方法 —— void WriteFile(string path, string content)
        /// <summary>
        /// 写入文件方法
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="content">内容</param>
        public void WriteFile(string path, string content)
        {
            lock (_Sync)
            {
                FileInfo file = new FileInfo(path);
                StreamWriter writer = null;

                try
                {
                    //获取文件目录并判断是否存在
                    string directory = Path.GetDirectoryName(path);

                    if (string.IsNullOrEmpty(directory))
                    {
                        throw new ArgumentNullException(nameof(path), "目录不可为空！");
                    }
                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }

                    writer = file.AppendText();
                    writer.Write(content);
                }
                finally
                {
                    writer?.Dispose();
                }
            }
        }
        #endregion
    }
}
