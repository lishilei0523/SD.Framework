using SD.Infrastructure.EventBase.Tests.Entities;
using SD.Infrastructure.EventBase.Tests.EventSources;
using SD.Infrastructure.EventBase.Tests.IRepositories;

namespace SD.Infrastructure.EventBase.Tests.EventHandlers
{
    /// <summary>
    /// 商品已创建事件处理者
    /// </summary>
    public class OrderCheckedEventHandler : IEventHandler<OrderCheckedEvent>
    {
        /// <summary>
        /// 单元事务
        /// </summary>
        private readonly IUnitOfWorkStub _unitOfWork;

        /// <summary>
        /// 依赖注入构造器
        /// </summary>
        public OrderCheckedEventHandler(IUnitOfWorkStub unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        /// <summary>
        /// 领域事件处理方法
        /// </summary>
        /// <param name="eventSource">领域事件源</param>
        public void Handle(OrderCheckedEvent eventSource)
        {
            CheckRecord record = new CheckRecord(eventSource.OrderNo);

            //throw new InvalidOperationException("Test");

            this._unitOfWork.RegisterAdd(record);
            this._unitOfWork.Commit();
        }
    }
}
