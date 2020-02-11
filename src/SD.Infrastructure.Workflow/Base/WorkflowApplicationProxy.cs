using SD.AOP.Core.Mediators;
using SD.AOP.Core.Models.Entities;
using SD.AOP.Core.Toolkits;
using System;
using System.Activities;
using System.Activities.DurableInstancing;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.DurableInstancing;
using System.Transactions;

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

            string connectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;

            #region # 验证

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ApplicationException("工作流持久化连接字符串未设置！");
            }

            #endregion

            ConnectionStringSettings connectionStringSetting = ConfigurationManager.ConnectionStrings[connectionStringName];

            #region # 验证

            if (connectionStringSetting == null)
            {
                throw new NullReferenceException($"未找到name为\"{connectionStringName}\"的连接字符串！");
            }

            #endregion

            _WorkflowPersistenceConnectionString = connectionStringSetting.ConnectionString;
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
            SqlWorkflowInstanceStore instanceStore = new SqlWorkflowInstanceStore()
            {
                ConnectionString = _WorkflowPersistenceConnectionString,
                InstanceCompletionAction = InstanceCompletionAction.DeleteNothing
            };

            this.WorkflowApplication.InstanceStore = instanceStore;

            //设置空闲时卸载持久化
            this.WorkflowApplication.PersistableIdle = eventArgs => PersistableIdleAction.Unload;

            //异常处理
            this.WorkflowApplication.OnUnhandledException = eventArgs =>
            {
                //记录日志
                this.LogException(eventArgs);

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

        //Public

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
                throw new ArgumentOutOfRangeException($"工作流实例\"{workflowInstanceId}\"不存在！", exception);
            }
        }
        #endregion

        #region 从书签恢复 —— BookmarkResumptionResult ResumeFromBookmark(...
        /// <summary>
        /// 从书签恢复
        /// </summary>
        /// <param name="bookmarkName">书签名称</param>
        /// <param name="parameter">参数</param>
        /// <returns>恢复结果</returns>
        public BookmarkResumptionResult ResumeFromBookmark(string bookmarkName, dynamic parameter)
        {
            BookmarkResumptionResult result = this.WorkflowApplication.ResumeBookmark(bookmarkName, parameter);

            if (result == BookmarkResumptionResult.NotFound || result == BookmarkResumptionResult.NotReady)
            {
                this.WorkflowApplication.Unload();
            }

            return result;
        }
        #endregion


        //Private

        #region 记录异常日志 —— void LogException(WorkflowApplicationUnhandledExceptionEventArgs...
        /// <summary>
        /// 记录异常日志
        /// </summary>
        /// <param name="eventArgs">异常事件信息</param>
        private void LogException(WorkflowApplicationUnhandledExceptionEventArgs eventArgs)
        {
            Exception ex = eventArgs.UnhandledException;
            ExceptionLog exceptionLog = new ExceptionLog
            {
                ExceptionMessage = $"工作流运行异常：{ex.Message}",
                ExceptionInfo = ex.ToString(),
                InnerException = ex.InnerException?.ToString(),
                ExceptionType = ex.GetType().FullName,
                ArgsJson = "[]",
                Namespace = eventArgs.ExceptionSource.Id,
                ClassName = eventArgs.ExceptionSource.DisplayName,
                MethodName = eventArgs.ExceptionSourceInstanceId,
                MethodType = "Workflow",
                OccurredTime = DateTime.Now,
                IPAddress = Common.GetLocalIPAddress()
            };

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                LogMediator.Write(exceptionLog);
                scope.Complete();
            }
        }
        #endregion

        #endregion

        #region # 静态方法

        #region 创建工作流应用程序 —— static Guid CreateWorkflowApplication(Activity activity...
        /// <summary>
        /// 创建工作流应用程序
        /// </summary>
        /// <param name="activity">活动</param>
        /// <param name="parameter">参数</param>
        /// <returns>工作流实例Id</returns>
        public static Guid CreateWorkflowApplication(Activity activity, dynamic parameter)
        {
            IDictionary<string, object> inputs = new Dictionary<string, object>();

            if (parameter != null)
            {
                foreach (dynamic kv in parameter)
                {
                    inputs.Add(kv.Key, kv.Value);
                }
            }

            WorkflowApplicationProxy proxy = inputs.Any()
                ? new WorkflowApplicationProxy(activity, inputs)
                : new WorkflowApplicationProxy(activity);

            proxy.Run();

            return proxy.WorkflowInstanceId;
        }
        #endregion

        #region 从书签恢复工作流应用程序 —— static BookmarkResumptionResult ResumeWorkflowApplicationFromBookmark(...
        /// <summary>
        /// 从书签恢复工作流应用程序
        /// </summary>
        /// <param name="activity">活动</param>
        /// <param name="workflowInstanceId">工作流实例Id</param>
        /// <param name="bookmarkName">书签名称</param>
        /// <param name="parameter">参数</param>
        /// <returns>恢复结果</returns>
        public static BookmarkResumptionResult ResumeWorkflowApplicationFromBookmark(Activity activity, Guid workflowInstanceId, string bookmarkName, dynamic parameter)
        {
            WorkflowApplicationProxy proxy = new WorkflowApplicationProxy(activity);
            proxy.Load(workflowInstanceId);
            BookmarkResumptionResult result = proxy.ResumeFromBookmark(bookmarkName, parameter);

            return result;
        }
        #endregion 

        #endregion
    }
}
