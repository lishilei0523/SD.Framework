using ArxOne.MrAdvice.Advice;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SD.Infrastructure.CrontabBase.Aspects
{
    /// <summary>
    /// 运行日志特性类
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class RunningAspect : Attribute, IMethodAdvice
    {
        /// <summary>
        /// 同步锁
        /// </summary>
        private static readonly object _Sync = new object();

        /// <summary>
        /// 拦截方法
        /// </summary>
        /// <param name="context">方法元数据</param>
        public void Advise(MethodAdviceContext context)
        {
            string logDirectory = $"{AppDomain.CurrentDomain.BaseDirectory}\\ScheduleLogs";
            string runningLogPath = $"{logDirectory}\\RunningLogs\\{DateTime.Today:yyyyMMdd}.txt";
            string exceptionLogPath = $"{logDirectory}\\ExceptionLogs\\{DateTime.Today:yyyyMMdd}.txt";

            string crontabName = context.TargetMethod.DeclaringType.GenericTypeArguments.Single().Name;

            Stopwatch watch = new Stopwatch();

            try
            {
                DateTime startTime = DateTime.Now;
                watch.Start();

                context.Proceed();

                watch.Stop();
                DateTime endTime = DateTime.Now;

                Task.Run(() =>
                {
                    this.WriteFile(runningLogPath, "===================================运行正常, 详细信息如下==================================="
                                        + Environment.NewLine + "［当前任务］" + crontabName
                                        + Environment.NewLine + "［开始时间］" + startTime
                                        + Environment.NewLine + "［结束时间］" + endTime
                                        + Environment.NewLine + "［运行耗时］" + watch.Elapsed
                                        + Environment.NewLine + Environment.NewLine);
                });
            }
            catch (Exception exception)
            {
                Task.Run(() =>
                {
                    this.WriteFile(exceptionLogPath, "===================================运行异常, 详细信息如下==================================="
                                        + Environment.NewLine + "［当前任务］" + crontabName
                                        + Environment.NewLine + "［异常时间］" + DateTime.Now
                                        + Environment.NewLine + "［异常消息］" + exception.Message
                                        + Environment.NewLine + "［异常明细］" + exception
                                        + Environment.NewLine + "［内部异常］" + exception.InnerException
                                        + Environment.NewLine + "［堆栈信息］" + exception.StackTrace
                                        + Environment.NewLine + Environment.NewLine);
                });
            }
        }

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
    }
}
