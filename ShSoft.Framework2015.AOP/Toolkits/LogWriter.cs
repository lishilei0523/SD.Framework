using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Text;
using ShSoft.Framework2015.AOP.Models.Entities;
using ShSoft.Framework2015.Common.PoweredByLee;

namespace ShSoft.Framework2015.AOP.Toolkits
{
    /// <summary>
    /// 日志记录者
    /// </summary>
    internal static class LogWriter
    {
        #region # 常量、字段及构造器

        /// <summary>
        /// 日志数据库连接字符串名称AppSetting键
        /// </summary>
        private const string DefaultConnectionStringAppSettingKey = "LogConnection";

        /// <summary>
        /// SQL工具
        /// </summary>
        private static readonly SqlHelper _SqlHelper;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static LogWriter()
        {
            //初始化连接字符串
            string defaultConnectionStringName = ConfigurationManager.AppSettings[DefaultConnectionStringAppSettingKey];
            string connectionString = ConfigurationManager.ConnectionStrings[defaultConnectionStringName].ConnectionString;

            //初始化SQL工具
            _SqlHelper = new SqlHelper(connectionString);
        }

        #endregion

        #region # 记录异常日志 —— static Guid Error(ExceptionLog log)
        /// <summary>
        /// 记录异常日志
        /// </summary>
        /// <param name="log">异常日志</param>
        /// <returns>日志Id</returns>
        public static Guid Error(ExceptionLog log)
        {
            //01.构造sql语句
            string sql = "INSERT INTO ExceptionLogs (Id, Namespace, ClassName, MethodName, MethodType, ArgsJson, ExceptionType, ExceptionMessage, ExceptionInfo, InnerException, OccurredTime, IPAddress) OUTPUT inserted.Id VALUES (NEWID(), @Namespace, @ClassName, @MethodName, @MethodType, @ArgsJson, @ExceptionType, @ExceptionMessage, @ExceptionInfo, @InnerException, @OccurredTime, @IPAddress)";

            //02.初始化参数
            SqlParameter[] parameters = {
                    new SqlParameter("@Namespace",log.Namespace.ToDbValue()),
                    new SqlParameter("@ClassName", log.ClassName.ToDbValue()),
                    new SqlParameter("@MethodName", log.MethodName.ToDbValue()),
                    new SqlParameter("@MethodType", log.MethodType.ToDbValue()),
                    new SqlParameter("@ArgsJson", log.ArgsJson.ToDbValue()),
                    new SqlParameter("@ExceptionType", log.ExceptionType.ToDbValue()),
                    new SqlParameter("@ExceptionMessage", log.ExceptionMessage.ToDbValue()),
                    new SqlParameter("@ExceptionInfo", log.ExceptionInfo.ToDbValue()),
                    new SqlParameter("@InnerException", log.InnerException.ToDbValue()),
                    new SqlParameter("@OccurredTime", log.OccurredTime.ToDbValue()),
                    new SqlParameter("@IPAddress", log.IPAddress.ToDbValue())
                };

            //03.执行sql
            Guid newId = (Guid)_SqlHelper.ExecuteScalar(sql, parameters);
            return newId;
        }
        #endregion

        #region # 记录运行日志 —— static Guid Info(RunningLog log)
        /// <summary>
        /// 记录运行日志
        /// </summary>
        /// <param name="log">运行日志</param>
        /// <returns>日志Id</returns>
        public static Guid Info(RunningLog log)
        {
            //01.构造SQL语句
            string sql = "INSERT INTO RunningLogs (Id, Namespace, ClassName, MethodName, MethodType, ArgsJson, ReturnValue, ReturnValueType, UserId, UserName, StartTime, EndTime, IPAddress) OUTPUT inserted.Id VALUES (NEWID(), @Namespace, @ClassName, @MethodName, @MethodType, @ArgsJson, @ReturnValue, @ReturnValueType, @UserId, @UserName, @StartTime, @EndTime, @IPAddress)";

            //02.初始化参数
            SqlParameter[] parameters = {
                    new SqlParameter("@Namespace", log.Namespace.ToDbValue()),
                    new SqlParameter("@ClassName", log.ClassName.ToDbValue()),
                    new SqlParameter("@MethodName", log.MethodName.ToDbValue()),
                    new SqlParameter("@MethodType", log.MethodType.ToDbValue()),
                    new SqlParameter("@ArgsJson", log.ArgsJson.ToDbValue()),
                    new SqlParameter("@ReturnValue", log.ReturnValue.ToDbValue()),
                    new SqlParameter("@ReturnValueType", log.ReturnValueType.ToDbValue()),
                    new SqlParameter("@UserId", log.UserId.ToDbValue()),
                    new SqlParameter("@UserName", log.UserName.ToDbValue()),
                    new SqlParameter("@StartTime", log.StartTime.ToDbValue()),
                    new SqlParameter("@EndTime", log.EndTime.ToDbValue()),
                    new SqlParameter("@IPAddress", log.IPAddress.ToDbValue())
                };

            //03.执行sql
            Guid newId = (Guid)_SqlHelper.ExecuteScalar(sql, parameters);
            return newId;
        }
        #endregion

        #region # 初始化日志数据表 —— static void InitTable()
        /// <summary>
        /// 初始化日志数据表
        /// </summary>
        internal static void InitTable()
        {
            //构造sql语句
            StringBuilder sqlBuilder = new StringBuilder();

            //初始化异常日志
            sqlBuilder.Append("IF OBJECT_ID('[dbo].[ExceptionLogs]') IS NULL ");
            sqlBuilder.Append("BEGIN ");
            sqlBuilder.Append("CREATE TABLE [dbo].[ExceptionLogs] ([Id] [uniqueidentifier] PRIMARY KEY NOT NULL, [Namespace] [nvarchar](max) NULL, [ClassName] [nvarchar](max) NULL, [MethodName] [nvarchar](max) NULL, [MethodType] [nvarchar](max) NULL, [ArgsJson] [nvarchar](max) NULL, [ExceptionType] [nvarchar](max) NULL, [ExceptionMessage] [nvarchar](max) NULL, [ExceptionInfo] [nvarchar](max) NULL, [InnerException] [nvarchar](max) NULL, [OccurredTime] [datetime] NULL, [IPAddress] [nvarchar](max) NULL) ");
            sqlBuilder.Append("END ");

            //初始化程序运行日志
            sqlBuilder.Append("IF OBJECT_ID('[dbo].[RunningLogs]') IS NULL ");
            sqlBuilder.Append("BEGIN ");
            sqlBuilder.Append("CREATE TABLE [dbo].[RunningLogs] ([Id] [uniqueidentifier] PRIMARY KEY NOT NULL, [Namespace] [nvarchar](max) NULL, [ClassName] [nvarchar](max) NULL, [MethodName] [nvarchar](max) NULL, [MethodType] [nvarchar](max) NULL, [ArgsJson] [nvarchar](max) NULL, [ReturnValue] [nvarchar](max) NULL, [ReturnValueType] [nvarchar](max) NULL, [UserId] [uniqueidentifier] NULL, [UserName] [nvarchar](max) NULL, [StartTime] [datetime] NULL, [EndTime] [datetime] NULL, [IPAddress] [nvarchar](max) NULL) ");
            sqlBuilder.Append("END ");

            //执行创建表
            _SqlHelper.ExecuteNonQuery(sqlBuilder.ToString());
        }
        #endregion
    }
}
