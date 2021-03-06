﻿using System;
using System.Configuration;
using System.Reflection;
using System.Threading;

namespace SD.Infrastructure.Constants
{
    /// <summary>
    /// 全局设置
    /// </summary>
    public static class GlobalSetting
    {
        #region # 字段及静态构造器

        /// <summary>
        /// 默认连接字符串
        /// </summary>
        private static readonly string _DefaultConnectionString;

        /// <summary>
        /// SeesionId线程静态字段
        /// </summary>
        private static readonly AsyncLocal<Guid> _SessionId;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static GlobalSetting()
        {
            string defaultConnectionStringName = CommonConstants.DefaultConnectionStringName;
            string connectionString = ConfigurationManager.ConnectionStrings[defaultConnectionStringName]?.ConnectionString;
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                defaultConnectionStringName = FrameworkSection.Setting.ServiceConnectionName.Value;
                connectionString = ConfigurationManager.ConnectionStrings[defaultConnectionStringName]?.ConnectionString;
            }
            _DefaultConnectionString = connectionString;

            _SessionId = new AsyncLocal<Guid>();
        }

        #endregion

        #region # 默认连接字符串 —— static string DefaultConnectionString
        /// <summary>
        /// 默认连接字符串
        /// </summary>
        public static string DefaultConnectionString
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_DefaultConnectionString))
                {
                    throw new NullReferenceException("默认连接字符串未配置，请联系管理员！");
                }

                return _DefaultConnectionString;
            }
        }
        #endregion

        #region # 默认连接字符串 —— static string DefaultConnectionStringForNetCore
        /// <summary>
        /// 默认连接字符串
        /// </summary>
        /// <remarks>.NET Core适用</remarks>
        public static string DefaultConnectionStringForNetCore
        {
            get
            {
                string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string hostAssemblyName = Assembly.GetExecutingAssembly().GetName().Name;
                ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap
                {
                    ExeConfigFilename = $"{baseDirectory}{hostAssemblyName}.dll.config"
                };

                Configuration configuration = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
                ConnectionStringsSection connectionStringsSection = configuration?.ConnectionStrings;
                ConnectionStringSettingsCollection connectionStringSettings = connectionStringsSection?.ConnectionStrings;

                string defaultConnectionStringName = CommonConstants.DefaultConnectionStringName;
                ConnectionStringSettings connectionStringSetting = connectionStringSettings?[defaultConnectionStringName];
                if (connectionStringSetting == null)
                {
                    defaultConnectionStringName = FrameworkSection.Setting.ServiceConnectionName.Value;
                    connectionStringSetting = connectionStringSettings?[defaultConnectionStringName];
                }

                string defaultConnectionString = connectionStringSetting?.ConnectionString;
                if (string.IsNullOrWhiteSpace(defaultConnectionString))
                {
                    throw new NullReferenceException("默认连接字符串未配置，请联系管理员！");
                }

                return defaultConnectionString;
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
