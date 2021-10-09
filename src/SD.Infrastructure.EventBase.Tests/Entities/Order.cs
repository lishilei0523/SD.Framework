using SD.Infrastructure.EntityBase;
using SD.Infrastructure.EventBase.Mediator;
using SD.Infrastructure.EventBase.Tests.EventSources;

namespace SD.Infrastructure.EventBase.Tests.Entities
{
    /// <summary>
    /// 单据
    /// </summary>
    public class Order : AggregateRootEntity
    {
        #region # 构造器

        #region 00.无参构造器
        /// <summary>
        /// 无参构造器
        /// </summary>
        protected Order()
        {
            //默认值
            this.Checked = false;
        }
        #endregion

        #region 01.创建单据构造器
        /// <summary>
        /// 创建单据构造器
        /// </summary>
        public Order(string orderNo, string orderName)
            : this()
        {
            base.Number = orderNo;
            base.Name = orderName;
        }
        #endregion

        #endregion

        #region # 属性

        #region 是否已审核 —— bool Checked
        /// <summary>
        /// 是否已审核
        /// </summary>
        public bool Checked { get; private set; }
        #endregion

        #endregion

        #region # 方法

        #region 审核 —— void Check()
        /// <summary>
        /// 审核
        /// </summary>
        public void Check()
        {
            this.Checked = true;

            //挂起领域事件
            EventMediator.Suspend(new OrderCheckedEvent(base.Number));
        }
        #endregion

        #endregion
    }
}
