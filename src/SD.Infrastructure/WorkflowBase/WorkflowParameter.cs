using System;
using System.Runtime.Serialization;

namespace SD.Infrastructure.WorkflowBase
{
    /// <summary>
    /// 工作流参数
    /// </summary>
    [Serializable]
    [DataContract]
    public class WorkflowParameter
    {
        /// <summary>
        /// 参数名称
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// 数据类型
        /// </summary>
        [DataMember]
        public string DataType { get; set; }

        /// <summary>
        /// 参数值
        /// </summary>
        [DataMember]
        public string Value { get; set; }

        /// <summary>
        /// 是否可空
        /// </summary>
        [DataMember]
        public bool Required { get; set; }
    }
}
