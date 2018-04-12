using System;
using System.Runtime.Serialization;

namespace SD.Infrastructure.DTOBase
{
    /// <summary>
    /// 数据传输对象基类
    /// </summary>
    [Serializable]
    [DataContract]
    public abstract class BaseDTO
    {
        #region # 标识Id —— Guid Id
        /// <summary>
        /// 标识Id
        /// </summary>
        [DataMember]
        public Guid Id { get; set; }
        #endregion

        #region # 编号 —— string Number
        /// <summary>
        /// 编号
        /// </summary>
        [DataMember]
        public string Number { get; set; }
        #endregion

        #region # 名称 —— string Name
        /// <summary>
        /// 名称
        /// </summary>
        [DataMember]
        public string Name { get; set; }
        #endregion

        #region # 添加时间 —— DateTime AddedTime
        /// <summary>
        /// 添加时间
        /// </summary>
        [DataMember]
        public DateTime AddedTime { get; set; }
        #endregion
    }
}
