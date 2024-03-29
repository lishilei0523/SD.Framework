﻿using System;
using System.Configuration;
using System.Threading;

namespace SD.Infrastructure.Constants
{
    /// <summary>
    /// 全局设置
    /// </summary>
    public static class GlobalSetting
    {
        #region # 字段及构造器

        /// <summary>
        /// 读连接字符串
        /// </summary>
        private static string _ReadConnectionString;

        /// <summary>
        /// 写连接字符串
        /// </summary>
        private static string _WriteConnectionString;

        /// <summary>
        /// SeesionId线程静态字段
        /// </summary>
        private static readonly AsyncLocal<Guid> _SessionId;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static GlobalSetting()
        {
            _ReadConnectionString = null;
            _WriteConnectionString = null;
            _SessionId = new AsyncLocal<Guid>();
        }

        #endregion


        //属性

        #region # 应用程序Id —— static string ApplicationId
        /// <summary>
        /// 应用程序Id
        /// </summary>
        public static string ApplicationId
        {
            get
            {
                string applicationId = string.IsNullOrWhiteSpace(FrameworkSection.Setting.ApplicationId.Value)
                    ? CommonConstants.DefaultApplicationId
                    : FrameworkSection.Setting.ApplicationId.Value;

                return applicationId;
            }
            set
            {
                FrameworkSection.Setting.ApplicationId.Value = value;
            }
        }
        #endregion

        #region # 应用程序名称 —— static string ApplicationName
        /// <summary>
        /// 应用程序名称
        /// </summary>
        public static string ApplicationName
        {
            get
            {
                string applicationName = string.IsNullOrWhiteSpace(FrameworkSection.Setting.ApplicationName.Value)
                    ? "SD.Application"
                    : FrameworkSection.Setting.ApplicationName.Value;

                return applicationName;
            }
        }
        #endregion

        #region # 应用程序版本 —— static string ApplicationVersion
        /// <summary>
        /// 应用程序版本
        /// </summary>
        public static string ApplicationVersion
        {
            get
            {
                string applicationVersion = string.IsNullOrWhiteSpace(FrameworkSection.Setting.ApplicationVersion.Value)
                    ? "v1.0.0"
                    : FrameworkSection.Setting.ApplicationVersion.Value;

                return applicationVersion;
            }
        }
        #endregion

        #region # 读连接字符串 —— static string ReadConnectionString
        /// <summary>
        /// 读连接字符串
        /// </summary>
        public static string ReadConnectionString
        {
            get
            {
                string readConnectionStringName = FrameworkSection.Setting.DatabaseReadConnectionName.Value;
                if (string.IsNullOrWhiteSpace(_ReadConnectionString))
                {
                    ConnectionStringsSection section = FrameworkSection.Setting.CurrentConfiguration?.ConnectionStrings;
                    ConnectionStringSettingsCollection connectionStringsList = section?.ConnectionStrings;
                    ConnectionStringSettings connectionStrings = connectionStringsList?[readConnectionStringName];
                    _ReadConnectionString = connectionStrings?.ConnectionString;
                }
                if (string.IsNullOrWhiteSpace(_ReadConnectionString))
                {
                    _ReadConnectionString = ConfigurationManager.ConnectionStrings[readConnectionStringName]?.ConnectionString;
                }
                if (string.IsNullOrWhiteSpace(_ReadConnectionString))
                {
                    throw new NullReferenceException("读连接字符串未配置，请联系管理员！");
                }

                return _ReadConnectionString;
            }
        }
        #endregion

        #region # 写连接字符串 —— static string WriteConnectionString
        /// <summary>
        /// 写连接字符串
        /// </summary>
        public static string WriteConnectionString
        {
            get
            {
                string writeConnectionStringName = FrameworkSection.Setting.DatabaseWriteConnectionName.Value;
                if (string.IsNullOrWhiteSpace(_WriteConnectionString))
                {
                    ConnectionStringsSection section = FrameworkSection.Setting.CurrentConfiguration?.ConnectionStrings;
                    ConnectionStringSettingsCollection connectionStringsList = section?.ConnectionStrings;
                    ConnectionStringSettings connectionStrings = connectionStringsList?[writeConnectionStringName];
                    _WriteConnectionString = connectionStrings?.ConnectionString;
                }
                if (string.IsNullOrWhiteSpace(_WriteConnectionString))
                {
                    _WriteConnectionString = ConfigurationManager.ConnectionStrings[writeConnectionStringName]?.ConnectionString;
                }
                if (string.IsNullOrWhiteSpace(_WriteConnectionString))
                {
                    throw new NullReferenceException("写连接字符串未配置，请联系管理员！");
                }

                return _WriteConnectionString;
            }
        }
        #endregion

        #region # 分区数量 —— static int PartitionsCount
        /// <summary>
        /// 分区数量
        /// </summary>
        public static int PartitionsCount
        {
            get
            {
                int partitionsCount = FrameworkSection.Setting.DatabasePartitionsCount.Value.HasValue
                    ? FrameworkSection.Setting.DatabasePartitionsCount.Value.Value
                    : 1;
                return partitionsCount;
            }
        }
        #endregion

        #region # 当前SessionId —— static Guid CurrentSessionId
        /// <summary>
        /// 当前SessionId
        /// </summary>
        public static Guid CurrentSessionId
        {
            get
            {
                if (_SessionId.Value == default(Guid))
                {
                    throw new ApplicationException("SessionId未设置，请检查程序！");
                }

                return _SessionId.Value;
            }
        }
        #endregion

        #region # 身份过期时间 —— static int AuthenticationTimeout
        /// <summary>
        /// 身份过期时间
        /// </summary>
        public static int AuthenticationTimeout
        {
            get
            {
                return FrameworkSection.Setting.AuthenticationTimeout.Value.HasValue
                     ? FrameworkSection.Setting.AuthenticationTimeout.Value.Value
                     : 20;
            }
        }
        #endregion

        #region # 授权是否启用 —— static bool AuthorizationEnabled
        /// <summary>
        /// 授权是否启用
        /// </summary>
        public static bool AuthorizationEnabled
        {
            get
            {
                return !FrameworkSection.Setting.AuthorizationEnabled.Value.HasValue || FrameworkSection.Setting.AuthorizationEnabled.Value.Value;
            }
        }
        #endregion


        //方法

        #region # 置空当前SessionId —— static void FreeCurrentSessionId()
        /// <summary>
        /// 置空当前SessionId
        /// </summary>
        public static void FreeCurrentSessionId()
        {
            _SessionId.Value = default(Guid);
        }
        #endregion

        #region # 初始化当前SessionId —— static void InitCurrentSessionId()
        /// <summary>
        /// 初始化当前SessionId
        /// </summary>
        public static void InitCurrentSessionId()
        {
            _SessionId.Value = Guid.NewGuid();
        }
        #endregion

        #region # 初始化数据文件夹 —— static void InitDataDirectory()
        /// <summary>
        /// 初始化数据文件夹
        /// </summary>
        public static void InitDataDirectory()
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            AppDomain.CurrentDomain.SetData(CommonConstants.DataDirectory, baseDirectory);
        }
        #endregion
    }
}
