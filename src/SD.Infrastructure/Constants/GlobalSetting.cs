using System;
using System.Configuration;
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
        /// 读连接字符串
        /// </summary>
        private static string _ReadConnectionString;

        /// <summary>
        /// 写连接字符串
        /// </summary>
        private static string _WriteConnectionString;

#if !NET40 && !NET45
        /// <summary>
        /// SeesionId线程静态字段
        /// </summary>
        private static readonly AsyncLocal<Guid> _SessionId;
#endif

        /// <summary>
        /// 静态构造器
        /// </summary>
        static GlobalSetting()
        {
            _ReadConnectionString = null;
            _WriteConnectionString = null;
#if !NET40 && !NET45
            _SessionId = new AsyncLocal<Guid>();
#endif
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
                    string readConnectionStringName = FrameworkSection.Setting.DatabaseReadConnectionName.Value;
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
                if (string.IsNullOrWhiteSpace(_WriteConnectionString))
                {
                    string writeConnectionStringName = FrameworkSection.Setting.DatabaseWriteConnectionName.Value;
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

        #region # 当前SessionId —— static Guid CurrentSessionId
#if !NET40 && !NET45
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
#endif
        #endregion

        #region # 置空当前SessionId —— static void FreeCurrentSessionId()
#if !NET40 && !NET45
        /// <summary>
        /// 置空当前SessionId
        /// </summary>
        public static void FreeCurrentSessionId()
        {
            _SessionId.Value = default(Guid);
        }
#endif
        #endregion

        #region # 初始化当前SessionId —— static void InitCurrentSessionId()
#if !NET40 && !NET45
        /// <summary>
        /// 初始化当前SessionId
        /// </summary>
        public static void InitCurrentSessionId()
        {
            _SessionId.Value = Guid.NewGuid();
        }
#endif
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
