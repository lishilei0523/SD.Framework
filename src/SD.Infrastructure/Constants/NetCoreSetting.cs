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
        /// 读连接字符串
        /// </summary>
        private static string _ReadConnectionString;

        /// <summary>
        /// 写连接字符串
        /// </summary>
        private static string _WriteConnectionString;

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
            _ReadConnectionString = null;
            _WriteConnectionString = null;

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

        #region # 读连接字符串 —— static string ReadConnectionString
        /// <summary>
        /// 读连接字符串
        /// </summary>
        public static string ReadConnectionString
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_ReadConnectionString))
                {
                    ConnectionStringsSection connectionStringsSection = _Configuration?.ConnectionStrings;
                    ConnectionStringSettingsCollection connectionStringSettings = connectionStringsSection?.ConnectionStrings;
                    string readConnectionStringName = _FrameworkSettings.DatabaseReadConnectionName.Value;
                    _ReadConnectionString = connectionStringSettings?[readConnectionStringName]?.ConnectionString;
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
                if (string.IsNullOrWhiteSpace(_WriteConnectionString))
                {
                    ConnectionStringsSection connectionStringsSection = _Configuration?.ConnectionStrings;
                    ConnectionStringSettingsCollection connectionStringSettings = connectionStringsSection?.ConnectionStrings;
                    string writeConnectionStringName = _FrameworkSettings.DatabaseWriteConnectionName.Value;
                    _WriteConnectionString = connectionStringSettings?[writeConnectionStringName]?.ConnectionString;
                }
                if (string.IsNullOrWhiteSpace(_WriteConnectionString))
                {
                    throw new NullReferenceException("写连接字符串未配置，请联系管理员！");
                }

                return _WriteConnectionString;
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
