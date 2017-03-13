using System;
using System.Runtime.Serialization;

// ReSharper disable once CheckNamespace
namespace SD.Infrastructure.CommandBase
{
    /// <summary>
    /// 命令基类
    /// </summary>
    [Serializable]
    [DataContract]
    public abstract class Command : ICommand
    {
        #region # 构造器
        /// <summary>
        /// 无参构造器
        /// </summary>
        protected Command()
        {
            //默认值
            this.Id = Guid.NewGuid();
            this.Handled = false;
            this.AddedTime = DateTime.Now;
            this.SessionId = Guid.Empty;
        }
        #endregion

        #region # 属性

        #region 标识 —— Guid Id
        /// <summary>
        /// 标识
        /// </summary>
        [DataMember]
        public Guid Id { get; set; }
        #endregion

        #region 是否已处理 —— bool Handled
        /// <summary>
        /// 是否已处理
        /// </summary>
        [DataMember]
        public bool Handled { get; set; }
        #endregion

        #region 添加时间 —— DateTime AddedTime
        /// <summary>
        /// 添加时间
        /// </summary>
        [DataMember]
        public DateTime AddedTime { get; set; }
        #endregion

        #region 会话Id —— Guid SessionId
        /// <summary>
        /// 会话Id
        /// </summary>
        [DataMember]
        public Guid SessionId { get; set; }
        #endregion

        #endregion
    }
}
