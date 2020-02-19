using SD.Infrastructure.DTOBase;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SD.Infrastructure.Constants
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
        /// 工作流所在程序集AppSetting键
        /// </summary>
        public const string WorkflowAssemblyAppSettingKey = "WorkflowAssembly";

        /// <summary>
        /// 数据表名前缀AppSetting键
        /// </summary>
        public const string TablePrefixAppSettingKey = "TablePrefix";

        /// <summary>
        /// 开启数据自动迁移AppSetting键
        /// </summary>
        public const string AutoMigrationAppSettingKey = "AutoMigration";

        /// <summary>
        /// 身份认证过期时间AppSetting键
        /// </summary>
        public const string AuthenticationTimeoutAppSettingKey = "AuthenticationTimeout";

        #endregion

        #region # 数据库相关

        /// <summary>
        /// 默认连接字符串名称
        /// </summary>
        public const string DefaultConnectionStringName = "DefaultConnection";

        #endregion

        #region # 统一身份认证系统相关

        /// <summary>
        /// 超级管理员登录名
        /// </summary>
        public const string AdminLoginId = "admin";

        /// <summary>
        /// 系统管理员角色编号
        /// </summary>
        public const string ManagerRoleNo = "SystemAdmin";

        /// <summary>
        /// 初始密码
        /// </summary>
        public const string InitialPassword = "888888";

        /// <summary>
        /// WCF身份认证消息头名称
        /// </summary>
        public const string WcfAuthHeaderName = "WcfAuthHeaderName";

        /// <summary>
        /// WCF身份认证消息头命名空间
        /// </summary>
        public const string WcfAuthHeaderNamespace = "WcfAuthHeaderNamespace";

        #endregion

        #region # 其他

        /// <summary>
        /// 日期 格式化 字符串
        /// </summary>
        public const string TimeFormat = "yyyy-MM-dd HH:mm";

        /// <summary>
        /// 最小时间
        /// </summary>
        public static readonly DateTime MinDateTime = new DateTime(1901, 1, 1);

        /// <summary>
        /// 最大时间
        /// </summary>
        public static readonly DateTime MaxDateTime = new DateTime(2078, 6, 6);

        /// <summary>
        /// 基元数据类型数组
        /// </summary>
        public static readonly string[] PrimitiveDataTypes =
        {
            "System.String",
            "System.Guid",
            "System.Boolean",
            "System.Byte",
            "System.Int16",
            "System.Int32",
            "System.Int64",
            "System.Single",
            "System.Double",
            "System.Decimal",
            "System.DateTime"
        };

        #endregion

        #region # 方法

        #region 类型化参数 —— static IDictionary<string, object> TypifyParameters(...
        /// <summary>
        /// 类型化参数
        /// </summary>
        /// <param name="parameters">参数字典</param>
        /// <returns>类型化参数字典</returns>
        public static IDictionary<string, object> TypifyParameters(this IDictionary<string, Parameter> parameters)
        {
            parameters = parameters ?? new Dictionary<string, Parameter>();

            IDictionary<string, object> typifiedParameters = new Dictionary<string, object>();
            foreach (KeyValuePair<string, Parameter> kv in parameters)
            {
                if (kv.Value == null)
                {
                    typifiedParameters.Add(kv.Key, null);
                }
                else
                {
                    string key = kv.Key;
                    string value = kv.Value.Value;
                    string dataType = kv.Value.DataType;

                    if (!PrimitiveDataTypes.Contains(dataType))
                    {
                        throw new InvalidOperationException($"给定数据类型\"{dataType}\"不是可用的基元类型！");
                    }
                    if (string.IsNullOrWhiteSpace(value))
                    {
                        typifiedParameters.Add(kv.Key, null);
                    }
                    else
                    {
                        switch (dataType)
                        {
                            case "System.String":
                                typifiedParameters.Add(key, value);
                                break;
                            case "System.Guid":
                                Guid guidValue = new Guid(value);
                                typifiedParameters.Add(key, guidValue);
                                break;
                            case "System.Boolean":
                                bool boolValue = Convert.ToBoolean(value);
                                typifiedParameters.Add(key, boolValue);
                                break;
                            case "System.Byte":
                                byte byteValue = Convert.ToByte(value);
                                typifiedParameters.Add(key, byteValue);
                                break;
                            case "System.Int16":
                                short shortValue = Convert.ToInt16(value);
                                typifiedParameters.Add(key, shortValue);
                                break;
                            case "System.Int32":
                                int intValue = Convert.ToInt32(value);
                                typifiedParameters.Add(key, intValue);
                                break;
                            case "System.Int64":
                                long longValue = Convert.ToInt64(value);
                                typifiedParameters.Add(key, longValue);
                                break;
                            case "System.Single":
                                float floatValue = Convert.ToSingle(value);
                                typifiedParameters.Add(key, floatValue);
                                break;
                            case "System.Double":
                                double doubleValue = Convert.ToDouble(value);
                                typifiedParameters.Add(key, doubleValue);
                                break;
                            case "System.Decimal":
                                decimal decimalValue = Convert.ToDecimal(value);
                                typifiedParameters.Add(key, decimalValue);
                                break;
                            case "System.DateTime":
                                DateTime dateTimeValue = Convert.ToDateTime(value);
                                typifiedParameters.Add(key, dateTimeValue);
                                break;
                        }
                    }
                }
            }

            return typifiedParameters;
        }
        #endregion

        #endregion
    }
}
