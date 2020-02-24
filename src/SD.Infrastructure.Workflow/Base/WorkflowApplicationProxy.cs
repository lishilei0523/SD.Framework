using System;
using System.Activities;
using System.Activities.DurableInstancing;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Runtime.DurableInstancing;
using System.Threading.Tasks;

namespace SD.Infrastructure.Workflow.Base
{
    /// <summary>
    /// 工作流应用程序代理
    /// </summary>
    public sealed class WorkflowApplicationProxy
    {
        #region # 常量、字段及构造器

        /// <summary>
        /// 工作流持久化数据库连接字符串名称AppSetting键
        /// </summary>
        private const string WorkflowPersistenceConnectionStringAppSettingKey = "WorkflowPersistenceConnection";

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
        /// 静态构造器
        /// </summary>
        static WorkflowApplicationProxy()
        {
            //初始化连接字符串
            string connectionStringName = ConfigurationManager.AppSettings[WorkflowPersistenceConnectionStringAppSettingKey];

            #region # 验证

            if (string.IsNullOrWhiteSpace(connectionStringName))
            {
                throw new ApplicationException("工作流持久化连接字符串名称未设置！");
            }

            #endregion

            ConnectionStringSettings connectionStringSetting = ConfigurationManager.ConnectionStrings[connectionStringName];

            #region # 验证

            if (connectionStringSetting == null)
            {
                throw new NullReferenceException($"未找到name为\"{connectionStringName}\"的连接字符串！");
            }

            #endregion

            string connectionString = connectionStringSetting.ConnectionString;

            #region # 验证

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ApplicationException("工作流持久化连接字符串未设置！");
            }

            #endregion

            _WorkflowPersistenceConnectionString = connectionString;
        }

        /// <summary>
        /// 创建工作流应用程序代理构造器
        /// </summary>
        /// <param name="workflowDefinition">工作流定义</param>
        /// <param name="parameters">参数字典</param>
        /// <param name="definitionIdentity">工作流定义标识</param>
        /// <param name="persistenceMode">持久化模式</param>
        public WorkflowApplicationProxy(Activity workflowDefinition, IDictionary<string, object> parameters = null, WorkflowIdentity definitionIdentity = null, InstanceCompletionAction persistenceMode = InstanceCompletionAction.DeleteNothing)
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
            SqlWorkflowInstanceStore instanceStore = new SqlWorkflowInstanceStore()
            {
                ConnectionString = _WorkflowPersistenceConnectionString,
                InstanceCompletionAction = persistenceMode
            };

            this.WorkflowApplication.InstanceStore = instanceStore;

            //设置空闲时卸载持久化
            this.WorkflowApplication.PersistableIdle = eventArgs => PersistableIdleAction.Unload;

            //异常处理
            this.WorkflowApplication.OnUnhandledException = eventArgs =>
            {
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
            try
            {
                this.WorkflowApplication.Load(workflowInstanceId);
            }
            catch (InstancePersistenceCommandException exception)
            {
                throw new InstancePersistenceCommandException($"工作流实例\"{workflowInstanceId}\"存在异常！", exception);
            }
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

        #region 创建工作流应用程序 —— static Guid CreateWorkflowApplication(Activity activity...
        /// <summary>
        /// 创建工作流应用程序
        /// </summary>
        /// <param name="activity">活动</param>
        /// <param name="parameters">参数字典</param>
        /// <returns>工作流实例Id</returns>
        /// <param name="definitionIdentity">工作流定义标识</param>
        /// <param name="persistenceMode">持久化模式</param>
        public static Guid CreateWorkflowApplication(Activity activity, IDictionary<string, object> parameters, WorkflowIdentity definitionIdentity = null, InstanceCompletionAction persistenceMode = InstanceCompletionAction.DeleteNothing)
        {
            WorkflowApplicationProxy proxy = new WorkflowApplicationProxy(activity, parameters, definitionIdentity, persistenceMode);

            proxy.Run();

            return proxy.WorkflowInstanceId;
        }
        #endregion

        #region 从书签恢复工作流应用程序 —— static void ResumeWorkflowApplicationFromBookmark(Activity activity...
        /// <summary>
        /// 从书签恢复工作流应用程序
        /// </summary>
        /// <param name="activity">活动</param>
        /// <param name="workflowInstanceId">工作流实例Id</param>
        /// <param name="bookmarkName">书签名称</param>
        /// <param name="parameters">参数字典</param>
        /// <param name="definitionIdentity">工作流定义标识</param>
        /// <param name="persistenceMode">持久化模式</param>
        public static void ResumeWorkflowApplicationFromBookmark(Activity activity, Guid workflowInstanceId, string bookmarkName, IDictionary<string, object> parameters, WorkflowIdentity definitionIdentity = null, InstanceCompletionAction persistenceMode = InstanceCompletionAction.DeleteNothing)
        {
            WorkflowApplicationProxy proxy = new WorkflowApplicationProxy(activity, null, definitionIdentity, persistenceMode);

            proxy.Load(workflowInstanceId);
            proxy.ResumeFromBookmark(bookmarkName, parameters);
        }
        #endregion


        //Private

        #region 记录异常日志 —— static void LogException(WorkflowApplicationUnhandledExceptionEventArgs...
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
                    + Environment.NewLine + "［当前活动］" + eventArgs.ExceptionSource.DisplayName
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
