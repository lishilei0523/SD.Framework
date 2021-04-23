using SD.Infrastructure.Constants;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SD.Infrastructure.WorkflowBase
{
    /// <summary>
    /// 工作流相关扩展
    /// </summary>
    public static class WorkflowExtension
    {
        #region # 类型化工作流参数 —— static IDictionary<string, object> TypifyParameters(...
        /// <summary>
        /// 类型化工作流参数
        /// </summary>
        /// <param name="parameters">工作流参数字典</param>
        /// <returns>类型化工作流参数字典</returns>
        public static IDictionary<string, object> TypifyParameters(this IDictionary<string, WorkflowParameter> parameters)
        {
            parameters = parameters ?? new Dictionary<string, WorkflowParameter>();

            IDictionary<string, object> typifiedParameters = new Dictionary<string, object>();
            foreach (KeyValuePair<string, WorkflowParameter> kv in parameters)
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

                    if (!CommonConstants.PrimitiveDataTypes.Contains(dataType))
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

        #region # 获取工作流持久化模式 —— static PersistenceMode GetPersistenceMode()
        /// <summary>
        /// 获取工作流持久化模式
        /// </summary>
        /// <returns>持久化模式</returns>
        public static PersistenceMode GetPersistenceMode()
        {
            string persistenceModeText = FrameworkSection.Setting.WorkflowPersistenceMode.Value;
            if (string.IsNullOrWhiteSpace(persistenceModeText))
            {
                return PersistenceMode.Temporary;
            }
            if (!Enum.TryParse(persistenceModeText, out PersistenceMode persistenceMode))
            {
                return PersistenceMode.Temporary;
            }

            return persistenceMode;
        }
        #endregion
    }
}
