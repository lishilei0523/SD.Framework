using SD.Infrastructure.CrontabBase.Configurations;
using System;
using System.Configuration;

// ReSharper disable once CheckNamespace
namespace SD.Infrastructure
{
    /// <summary>
    /// SD.Framework定时任务配置
    /// </summary>
    public class CrontabSection : ConfigurationSection
    {
        #region # 字段及构造器

        /// <summary>
        /// 单例
        /// </summary>
        private static readonly CrontabSection _Setting;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static CrontabSection()
        {
            _Setting = (CrontabSection)ConfigurationManager.GetSection("sd.framework.crontab");

            #region # 非空验证

            if (_Setting == null)
            {
                throw new ApplicationException("SD.Framework定时任务节点未配置，请检查程序！");
            }

            #endregion
        }

        #endregion

        #region # 访问器 —— static CrontabSection Setting
        /// <summary>
        /// 访问器
        /// </summary>
        public static CrontabSection Setting
        {
            get { return _Setting; }
        }
        #endregion

        #region # 定时任务节点列表 —— CrontabElementCollection CrontabElements
        /// <summary>
        /// 定时任务节点列表
        /// </summary>
        [ConfigurationProperty("crontabs")]
        [ConfigurationCollection(typeof(CrontabElementCollection), AddItemName = "crontab")]
        public CrontabElementCollection CrontabElements
        {
            get
            {
                CrontabElementCollection collection = this["crontabs"] as CrontabElementCollection;
                return collection ?? new CrontabElementCollection();
            }
            set { this["crontabs"] = value; }
        }
        #endregion
    }
}
