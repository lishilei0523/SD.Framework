using SD.Infrastructure.EventBase.Mediator;
using SD.Infrastructure.RepositoryBase;
using System.Threading.Tasks;
using System.Transactions;

namespace SD.Infrastructure.Global.Transaction
{
    /// <summary>
    /// UnitOfWork扩展工具类
    /// </summary>
    public static class UnitOfWorkExtension
    {
        /// <summary>
        /// UnitOfWork联合提交（包含处理领域事件）扩展方法
        /// </summary>
        /// <param name="unitOfWork">工作单元实例</param>
        public static void UnitedCommit(this IUnitOfWork unitOfWork)
        {
            TransactionScopeAsyncFlowOption asyncFlowOption = TransactionScopeAsyncFlowOption.Enabled;

            try
            {
                //开启事务
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, asyncFlowOption))
                {
                    //提交工作单元
                    unitOfWork.Commit();

                    //处理领域事件
                    EventMediator.HandleUncompletedEvents();

                    //事务完成
                    scope.Complete();
                }
            }
            catch
            {
                //不参与事务
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
                {
                    //回滚工作单元
                    unitOfWork.RollBack();

                    //清空未处理的领域事件
                    EventMediator.ClearUncompletedEvents();

                    //事务完成 
                    scope.Complete();
                }
                throw;
            }
        }

        /// <summary>
        /// UnitOfWork联合提交（包含处理领域事件）异步扩展方法
        /// </summary>
        /// <param name="unitOfWork">工作单元实例</param>
        public static async Task UnitedCommitAsync(this IUnitOfWork unitOfWork)
        {
            TransactionScopeAsyncFlowOption asyncFlowOption = TransactionScopeAsyncFlowOption.Enabled;

            try
            {
                //开启事务
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, asyncFlowOption))
                {
                    //提交工作单元
                    await unitOfWork.CommitAsync();

                    //处理领域事件
                    await EventMediator.HandleUncompletedEventsAsync();

                    //事务完成
                    scope.Complete();
                }
            }
            catch
            {
                //不参与事务
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
                {
                    //回滚工作单元
                    unitOfWork.RollBack();

                    //清空未处理的领域事件
                    EventMediator.ClearUncompletedEvents();

                    //事务完成 
                    scope.Complete();
                }
                throw;
            }
        }
    }
}
