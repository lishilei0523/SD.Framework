using System;
using System.Configuration;
using System.Reflection;

namespace SD.Infrastructure.Constants
{
    /// <summary>
    /// .NET Core设置
    /// </summary>
    public static class NetCoreSetting
    {
        #region # 字段及构造器

        /// <summary>
        /// 应用程序配置
        /// </summary>
        private static readonly Configuration _Configuration;

        /// <summary>
        /// SD.Framework配置
        /// </summary>
        private static readonly FrameworkSection _FrameworkSettings;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static NetCoreSetting()
        {
            //读取配置文件
            Assembly entryAssembly = Assembly.GetEntryAssembly();
            string hostAssemblyName = entryAssembly?.GetName().Name;

            ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap
            {
                ExeConfigFilename = $"{AppDomain.CurrentDomain.BaseDirectory}{hostAssemblyName}.dll.config"
            };
            _Configuration = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);

            //读取SD.Framework配置
            _FrameworkSettings = (FrameworkSection)_Configuration.GetSection("sd.framework");
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
                ConnectionStringsSection connectionStringsSection = _Configuration?.ConnectionStrings;
                ConnectionStringSettingsCollection connectionStringSettings = connectionStringsSection?.ConnectionStrings;

                string defaultConnectionStringName = CommonConstants.DefaultConnectionStringName;
                ConnectionStringSettings connectionStringSetting = connectionStringSettings?[defaultConnectionStringName];
                if (connectionStringSetting == null)
                {
                    defaultConnectionStringName = _FrameworkSettings.ServiceConnectionName.Value;
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

        #region # SD.Framework配置 —— FrameworkSection FrameworkSettings
        /// <summary>
        /// SD.Framework配置
        /// </summary>
        public static FrameworkSection FrameworkSettings
        {
            get { return _FrameworkSettings; }
        }
        #endregion
    }
}
