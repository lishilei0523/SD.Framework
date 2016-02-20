using System;

namespace ShSoft.Framework2015.Infrastructure.Constants
{
    /// <summary>
    /// 公共常量
    /// </summary>
    public static class CommonConstants
    {
        #region # Config文件 AppSetting键

        /// <summary>
        /// 实体所在程序集AppSetting键
        /// </summary>
        public const string EntityAssemblyAppSettingKey = "EntityAssembly";

        /// <summary>
        /// 领域事件源所在程序集AppSetting键
        /// </summary>
        public const string EventSourceAssemblyAppSettingKey = "EventSourceAssembly";

        /// <summary>
        /// 实体配置所在程序集AppSetting键
        /// </summary>
        public const string EntityConfigAssemblyAppSettingKey = "EntityConfigAssembly";

        /// <summary>
        /// 领域事件源配置所在程序集AppSetting键
        /// </summary>
        public const string EventSourceConfigAssemblyAppSettingKey = "EventSourceConfigAssembly";

        /// <summary>
        /// 数据表名前缀AppSetting键
        /// </summary>
        public const string TablePrefixAppSettingKey = "TablePrefix";

        #endregion

        #region # WCF相关

        /// <summary>
        /// 信道实例属性名
        /// </summary>
        public const string ChannelPropertyName = "Channel";

        #endregion

        #region # 数据库相关

        /// <summary>
        /// DbSession构造函数默认参数
        /// </summary>
        public static readonly string DbSessionConstructorArg = string.Format("name={0}", DefaultConnectionStringName);

        /// <summary>
        /// 事件源DbContext构造函数默认参数
        /// </summary>
        public static readonly string EventDbContextConstructArg = string.Format("name={0}", EventSourceConnectionStringName);

        /// <summary>
        /// 默认连接字符串名称
        /// </summary>
        public const string DefaultConnectionStringName = "DefaultConnection";

        /// <summary>
        /// 事件源连接字符串名称
        /// </summary>
        public const string EventSourceConnectionStringName = "EventSourceConnection";

        /// <summary>
        /// 日志连接字符串名称
        /// </summary>
        public const string LogConnectionStringName = "LogConnection";

        #endregion

        #region # 系统相关

        /// <summary>
        /// 非法字符集
        /// </summary>
        public const string IllegalChars = "\\?<>|'";

        /// <summary>
        /// 字符串完整类名
        /// </summary>
        public const string StringFullName = "System.String";

        /// <summary>
        /// 数值类型完整类名集
        /// </summary>
        public const string NumbericFullNames = "System.Decimal,System.Int16,System.Int32,System.Int64,System.UInt16,System.UInt32,System.UInt64,System.Double,System.Single";

        #endregion

        #region # 人资权限相关

        /// <summary>
        /// 超级管理员登录名
        /// </summary>
        public const string AdminLoginId = "admin";

        /// <summary>
        /// 超级管理员员工编号
        /// </summary>
        public const string AdminEmployeeNo = "X1";

        /// <summary>
        /// 超级管理员角色Id
        /// </summary>
        public static readonly Guid AdminRoleId = new Guid("45C2E307-128D-4094-9366-A36CD6535F70");

        /// <summary>
        /// 管理中心系统类别编号
        /// </summary>
        public const string MCSystemKindNo = "X1";

        /// <summary>
        /// 供应商系统类别编号
        /// </summary>
        public const string SupplierSystemKindNo = "X2";

        /// <summary>
        /// 管理中心组织编号
        /// </summary>
        public const string MCOrganizationNo = "X1";

        /// <summary>
        /// 管理中心系统编号
        /// </summary>
        public const string MCSystemNo = "X1";

        #endregion
    }
}
