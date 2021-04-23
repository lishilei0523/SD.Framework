using SD.Infrastructure.WorkflowBase;
using System;
using System.Activities;
using System.Activities.DurableInstancing;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.DurableInstancing;
using System.Threading;
using System.Threading.Tasks;

namespace SD.Infrastructure.Workflow.Base
{
    /// <summary>
    /// 工作流应用程序代理
    /// </summary>
    public sealed class WorkflowApplicationProxy
    {
        #region # 字段、事件及构造器

        /// <summary>
        /// 同步锁
        /// </summary>
        private static readonly object _Sync = new object();

        /// <summary>
        /// 异常日志路径
        /// </summary>
        private static readonly string _ExceptionLogPath = $"{AppDomain.CurrentDomain.BaseDirectory}\\Workflow.ExceptionLogs\\{{0:yyyy-MM-dd}}.txt";

        /// <summary>
        /// 工作流持久化数据库连接字符串
        /// </summary>
        private static readonly string _WorkflowPersistenceConnectionString;

        /// <summary>
        /// 最大实例锁定重试次数
        /// </summary>
        private static readonly int _MaxInstanceLockedRetriesCount;

        /// <summary>
        /// 实例锁定重试次数
        /// </summary>
        [ThreadStatic]
        private static int _InstanceLockedRetriesCount;

        /// <summary>
        /// 工作流实例书签卸载事件
        /// </summary>
        /// <remarks>[Guid, string[]]，[工作流实例Id，书签类型名称集]</remarks>
        public event Action<Guid, string[], WorkflowApplicationIdleEventArgs> BookmarksUnloadedEvent;

        /// <summary>
        /// 工作流实例书签异常事件
        /// </summary>
        /// <remarks>[Guid, string, string]，[工作流实例Id，书签类型名称，异常]</remarks>
        public event Action<Guid, string, Exception, WorkflowApplicationUnhandledExceptionEventArgs> BookmarkExceptionEvent;

        /// <summary>
        /// 工作流实例已完成事件
        /// </summary>
        /// <remarks>[Guid, WorkflowApplicationCompletedEventArgs]，[工作流实例Id，工作流已完成事件参数]</remarks>
        public event Action<Guid, WorkflowApplicationCompletedEventArgs> CompletedEvent;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static WorkflowApplicationProxy()
        {
            //初始化连接字符串
            string connectionStringName = FrameworkSection.Setting.WorkflowConnectionName.Value;
            ConnectionStringSettings connectionStringSetting = ConfigurationManager.ConnectionStrings?[connectionStringName];
            _WorkflowPersistenceConnectionString = connectionStringSetting?.ConnectionString; ;
            if (string.IsNullOrWhiteSpace(_WorkflowPersistenceConnectionString))
            {
                throw new ApplicationException("工作流持久化连接字符串未设置！");
            }

            //初始化最大实例锁定重试次数、默认20次
            _MaxInstanceLockedRetriesCount = FrameworkSection.Setting.WorkflowMaxInstanceLockedRetriesCount.Value.HasValue
                ? FrameworkSection.Setting.WorkflowMaxInstanceLockedRetriesCount.Value.Value
                : 20;
        }

        /// <summary>
        /// 创建工作流应用程序代理构造器
        /// </summary>
        /// <param name="workflowDefinition">工作流定义</param>
        /// <param name="parameters">参数字典</param>
        /// <param name="definitionIdentity">工作流定义标识</param>
        public WorkflowApplicationProxy(Activity workflowDefinition, IDictionary<string, object> parameters = null, WorkflowIdentity definitionIdentity = null)
        {
            //创建工作流应用程序
            if (workflowDefinition == null)
            {
                throw new ArgumentNullException(nameof(workflowDefinition), "工作流定义不可为空！");
            }
            if (parameters == null && definitionIdentity == null)
            {
                this.WorkflowApplication = new WorkflowApplication(workflowDefinition);
            }
            if (parameters == null && definitionIdentity != null)
            {
                this.WorkflowApplication = new WorkflowApplication(workflowDefinition, definitionIdentity);
            }
            if (parameters != null && definitionIdentity == null)
            {
                this.WorkflowApplication = new WorkflowApplication(workflowDefinition, parameters);
            }
            if (parameters != null && definitionIdentity != null)
            {
                this.WorkflowApplication = new WorkflowApplication(workflowDefinition, parameters, definitionIdentity);
            }

            //设置工作流持久化存储
            InstanceCompletionAction instanceCompletionAction = GetInstanceCompletionAction();
            SqlWorkflowInstanceStore instanceStore = new SqlWorkflowInstanceStore()
            {
                ConnectionString = _WorkflowPersistenceConnectionString,
                InstanceCompletionAction = instanceCompletionAction
            };

            this.WorkflowApplication.InstanceStore = instanceStore;

            //设置空闲时卸载持久化
            this.WorkflowApplication.PersistableIdle = eventArgs =>
            {
                //只有书签才持久化
                if (eventArgs.Bookmarks.Any())
                {
                    return PersistableIdleAction.Unload;
                }

                return PersistableIdleAction.None;
            };

            //设置书签卸载事件
            this.WorkflowApplication.Idle = eventArgs =>
            {
                if (eventArgs.Bookmarks.Any())
                {
                    string[] bookmarkNames = eventArgs.Bookmarks.Select(x => x.BookmarkName).ToArray();
                    this.BookmarksUnloadedEvent?.Invoke(this.WorkflowApplication.Id, bookmarkNames, eventArgs);
                }
            };

            //设置工作流实例完成事件
            this.WorkflowApplication.Completed = eventArgs =>
            {
                this.CompletedEvent?.Invoke(this.WorkflowApplication.Id, eventArgs);
            };

            //异常处理
            this.WorkflowApplication.OnUnhandledException = eventArgs =>
            {
                if (eventArgs.ExceptionSource is BookmarkActivity bookmarkActivity)
                {
                    //设置书签异常事件
                    Guid workflowInstanceId = this.WorkflowApplication.Id;
                    string bookmarkName = bookmarkActivity.GetType().FullName;
                    Exception unhandledException = eventArgs.UnhandledException;
                    this.BookmarkExceptionEvent?.Invoke(workflowInstanceId, bookmarkName, unhandledException, eventArgs);
                }

                //记录日志
                LogException(eventArgs);

                //中止工作流
                return UnhandledExceptionAction.Abort;
            };
        }

        #endregion

        #region # 属性

        #region 工作流应用程序 —— WorkflowApplication WorkflowApplication
        /// <summary>
        /// 工作流应用程序
        /// </summary>
        public WorkflowApplication WorkflowApplication { get; private set; }
        #endregion

        #region 只读属性 - 工作流实例Id —— Guid WorkflowInstanceId
        /// <summary>
        /// 只读属性 - 工作流实例Id
        /// </summary>
        public Guid WorkflowInstanceId
        {
            get { return this.WorkflowApplication.Id; }
        }
        #endregion

        #endregion

        #region # 实例方法

        #region 运行工作流应用程序 —— void Run()
        /// <summary>
        /// 运行工作流应用程序
        /// </summary>
        public void Run()
        {
            this.WorkflowApplication.Run();
        }
        #endregion

        #region 加载工作流实例 —— void Load(Guid workflowInstanceId)
        /// <summary>
        /// 加载工作流实例
        /// </summary>
        /// <param name="workflowInstanceId">工作流实例Id</param>
        public void Load(Guid workflowInstanceId)
        {
            this.WorkflowApplication.Load(workflowInstanceId);
        }
        #endregion

        #region 从书签恢复 —— void ResumeFromBookmark(string bookmarkName...
        /// <summary>
        /// 从书签恢复
        /// </summary>
        /// <param name="bookmarkName">书签名称</param>
        /// <param name="parameters">参数字典</param>
        /// <returns>恢复结果</returns>
        public void ResumeFromBookmark(string bookmarkName, IDictionary<string, object> parameters)
        {
            BookmarkResumptionResult result = this.WorkflowApplication.ResumeBookmark(bookmarkName, parameters);

            if (result == BookmarkResumptionResult.NotReady)
            {
                this.WorkflowApplication.Unload();

                throw new ArgumentOutOfRangeException(nameof(bookmarkName), $"工作流实例\"{this.WorkflowInstanceId}\"的书签\"{bookmarkName}\"未准备就绪，请稍后再试！");
            }
            if (result == BookmarkResumptionResult.NotFound)
            {
                this.WorkflowApplication.Unload();

                throw new ArgumentOutOfRangeException(nameof(bookmarkName), $"工作流实例\"{this.WorkflowInstanceId}\"不存在书签\"{bookmarkName}\"！");
            }
        }
        #endregion

        #endregion

        #region # 静态方法

        //Public

        #region 创建工作流应用程序 —— static WorkflowApplicationProxy CreateWorkflowApplication(...
        /// <summary>
        /// 创建工作流应用程序
        /// </summary>
        /// <param name="activity">活动</param>
        /// <param name="parameters">参数字典</param>
        /// <param name="definitionIdentity">工作流定义标识</param>
        /// <returns>工作流应用程序代理</returns>
        public static WorkflowApplicationProxy CreateWorkflowApplication(Activity activity, IDictionary<string, object> parameters, WorkflowIdentity definitionIdentity = null)
        {
            WorkflowApplicationProxy proxy = new WorkflowApplicationProxy(activity, parameters, definitionIdentity);

            proxy.Run();

            return proxy;
        }
        #endregion

        #region 恢复工作流应用程序 —— static WorkflowApplicationProxy ResumeWorkflowApplicationFromBookmark(...
        /// <summary>
        /// 恢复工作流应用程序
        /// </summary>
        /// <param name="activity">活动</param>
        /// <param name="workflowInstanceId">工作流实例Id</param>
        /// <param name="bookmarkName">书签名称</param>
        /// <param name="parameters">参数字典</param>
        /// <param name="definitionIdentity">工作流定义标识</param>
        /// <returns>工作流应用程序代理</returns>
        public static WorkflowApplicationProxy ResumeWorkflowApplicationFromBookmark(Activity activity, Guid workflowInstanceId, string bookmarkName, IDictionary<string, object> parameters, WorkflowIdentity definitionIdentity = null)
        {
            try
            {
                WorkflowApplicationProxy proxy = new WorkflowApplicationProxy(activity, null, definitionIdentity);

                proxy.Load(workflowInstanceId);
                proxy.ResumeFromBookmark(bookmarkName, parameters);

                return proxy;
            }
            catch (InstanceLockedException exception)
            {
                Thread.Sleep(200);
                _InstanceLockedRetriesCount++;

                if (_InstanceLockedRetriesCount >= _MaxInstanceLockedRetriesCount)
                {
                    _InstanceLockedRetriesCount = 0;
                    throw new InstanceLockedException($"工作流实例\"{workflowInstanceId}\"已锁定，无法恢复！", exception);
                }

                return ResumeWorkflowApplicationFromBookmark(activity, workflowInstanceId, bookmarkName, parameters, definitionIdentity);
            }
            catch (InstancePersistenceCommandException exception)
            {
                throw new InstancePersistenceCommandException($"工作流实例\"{workflowInstanceId}\"存在异常！", exception);
            }
        }
        #endregion


        //Private

        #region 获取持久化模式 —— static InstanceCompletionAction GetInstanceCompletionAction()
        /// <summary>
        /// 获取持久化模式
        /// </summary>
        /// <returns>持久化模式</returns>
        private static InstanceCompletionAction GetInstanceCompletionAction()
        {
            PersistenceMode persistenceMode = WorkflowExtension.GetPersistenceMode();
            int enumValue = (int)persistenceMode;

            InstanceCompletionAction instanceCompletionAction = (InstanceCompletionAction)enumValue;

            return instanceCompletionAction;
        }
        #endregion

        #region 记录异常日志 —— static void LogException(EventArgs...
        /// <summary>
        /// 记录异常日志
        /// </summary>
        /// <param name="eventArgs">异常事件信息</param>
        private static void LogException(WorkflowApplicationUnhandledExceptionEventArgs eventArgs)
        {
            Exception exception = eventArgs.UnhandledException;
            Task.Run(() =>
            {
                WriteFile(string.Format(_ExceptionLogPath, DateTime.Today),
                    "===================================工作流运行异常, 详细信息如下==================================="
                    + Environment.NewLine + "［当前活动名称］" + eventArgs.ExceptionSource.DisplayName
                    + Environment.NewLine + "［当前活动类型］" + eventArgs.ExceptionSource.GetType().FullName
                    + Environment.NewLine + "［工作流实例Id］" + eventArgs.InstanceId
                    + Environment.NewLine + "［异常时间］" + DateTime.Now
                    + Environment.NewLine + "［异常消息］" + exception.Message
                    + Environment.NewLine + "［异常明细］" + exception
                    + Environment.NewLine + "［内部异常］" + exception.InnerException
                    + Environment.NewLine + "［堆栈信息］" + exception.StackTrace
                    + Environment.NewLine + Environment.NewLine);
            });
        }
        #endregion

        #region 写入文件方法 —— static void WriteFile(string path, string content)
        /// <summary>
        /// 写入文件方法
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="content">内容</param>
        private static void WriteFile(string path, string content)
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

        #endregion
    }
}
