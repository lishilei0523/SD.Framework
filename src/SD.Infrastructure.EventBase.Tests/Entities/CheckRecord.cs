using SD.Infrastructure.EntityBase;

namespace SD.Infrastructure.EventBase.Tests.Entities
{
    /// <summary>
    /// 审核记录
    /// </summary>
    public class CheckRecord : AggregateRootEntity
    {
        #region # 构造器

        #region 00.无参构造器
        /// <summary>
        /// 无参构造器
        /// </summary>
        protected CheckRecord() { }
        #endregion

        #region 01.创建审核记录构造器
        /// <summary>
        /// 创建审核记录构造器
        /// </summary>
        public CheckRecord(string orderNo)
            : this()
        {
            this.OrderNo = orderNo;
        }
        #endregion

        #endregion

        #region # 属性

        #region 单据编号 —— string OrderNo
        /// <summary>
        /// 单据编号
        /// </summary>
        public string OrderNo { get; private set; }
        #endregion

        #endregion
    }
}
