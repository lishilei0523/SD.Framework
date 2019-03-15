using ArxOne.MrAdvice.Advice;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SD.Infrastructure.SchedulerBase.Aspects
{
    /// <summary>
    /// 运行日志特性类
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class RunningAspect : Attribute, IMethodAdvice
    {
        /// <summary>
        /// 拦截方法
        /// </summary>
        /// <param name="context">方法元数据</param>
        public void Advise(MethodAdviceContext context)
        {
            string log = $"{AppDomain.CurrentDomain.BaseDirectory}\\ScheduleLogs\\Log_{DateTime.Now.Date:yyyyMMdd}.txt";
            Stopwatch watch = new Stopwatch();

            try
            {
                watch.Start();
                context.Proceed();
                watch.Stop();

                Task.Run(() =>
                {
                    this.WriteFile(log, "===================================运行正常, 详细信息如下==================================="
                                        + Environment.NewLine + "［当前任务］" + context.TargetMethod.DeclaringType.FullName
                                        + Environment.NewLine + "［运行时间］" + DateTime.Now
                                        + Environment.NewLine + "［运行耗时］" + watch.Elapsed
                                        + Environment.NewLine + Environment.NewLine, true);
                });
            }
            catch (Exception exception)
            {
                Task.Run(() =>
                {
                    this.WriteFile(log, "===================================发生异常, 详细信息如下==================================="
                                        + Environment.NewLine + "［当前任务］" + context.TargetMethod.DeclaringType.FullName
                                        + Environment.NewLine + "［异常时间］" + DateTime.Now
                                        + Environment.NewLine + "［异常消息］" + exception.Message
                                        + Environment.NewLine + "［异常明细］" + exception
                                        + Environment.NewLine + "［内部异常］" + exception.InnerException
                                        + Environment.NewLine + "［堆栈信息］" + exception.StackTrace
                                        + Environment.NewLine + Environment.NewLine, true);
                });
            }
        }

        /// <summary>
        /// 写入文件方法
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="content">内容</param>
        /// <param name="append">是否附加</param>
        /// <exception cref="ArgumentNullException">路径为空</exception>
        public void WriteFile(string path, string content, bool append = false)
        {
            #region # 验证参数

            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path), @"路径不可为空！");
            }

            #endregion

            FileInfo file = new FileInfo(path);
            StreamWriter writer = null;
            if (file.Exists && !append)
            {
                file.Delete();
            }
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

                writer = append ? file.AppendText() : new StreamWriter(path, false, Encoding.UTF8);
                writer.Write(content);
            }
            finally
            {
                writer?.Dispose();
            }
        }
    }
}
