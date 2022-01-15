using SD.Infrastructure.EventBase.Mediators;
using SD.Infrastructure.RepositoryBase;
using System.Data.Common;
using System.Threading.Tasks;
using System.Transactions;

// ReSharper disable once CheckNamespace
namespace SD.Infrastructure.Global.Transaction
{
    /// <summary>
    /// UnitOfWork领域事件扩展
    /// </summary>
    public static class UnitOfWorkExtension
    {
        #region # 常量

        /// <summary>
        /// 异步事务流设置
        /// </summary>
        private const TransactionScopeAsyncFlowOption AsyncFlowOption = TransactionScopeAsyncFlowOption.Enabled;

        #endregion

        #region # 工作单元联合处理领域事件提交 —— static void UnitedCommit(this IUnitOfWork unitOfWork)
        /// <summary>
        /// 工作单元联合处理领域事件提交
        /// </summary>
        /// <param name="unitOfWork">工作单元实例</param>
        /// <remarks>采用本地事务</remarks>
        public static void UnitedCommit(this IUnitOfWork unitOfWork)
        {
            try
            {
                if (unitOfWork.GetCurrentTransaction() == null)
                {
                    using (DbTransaction dbTransaction = unitOfWork.BeginTransaction())
                    {
                        //提交工作单元
                        unitOfWork.Commit();

                        //处理领域事件
                        EventMediator.HandleUncompletedEvents();

                        //提交事务
                        dbTransaction.Commit();
                    }
                }
                else
                {
                    //提交工作单元
                    unitOfWork.Commit();

                    //处理领域事件
                    EventMediator.HandleUncompletedEvents();
                }
            }
            catch
            {
                //不参与事务
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress, AsyncFlowOption))
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
        #endregion

        #region # 工作单元联合处理领域事件提交 —— static async Task UnitedCommitAsync(this IUnitOfWork unitOfWork)
        /// <summary>
        /// 工作单元联合处理领域事件提交
        /// </summary>
        /// <param name="unitOfWork">工作单元实例</param>
        /// <remarks>采用本地事务</remarks>
        public static async Task UnitedCommitAsync(this IUnitOfWork unitOfWork)
        {
            try
            {
                if (unitOfWork.GetCurrentTransaction() == null)
                {
                    using (DbTransaction dbTransaction = unitOfWork.BeginTransaction())
                    {
                        //提交工作单元
                        await unitOfWork.CommitAsync();

                        //处理领域事件
                        await EventMediator.HandleUncompletedEventsAsync();

                        //提交事务
                        dbTransaction.Commit();
                    }
                }
                else
                {
                    //提交工作单元
                    await unitOfWork.CommitAsync();

                    //处理领域事件
                    await EventMediator.HandleUncompletedEventsAsync();
                }
            }
            catch
            {
                //不参与事务
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress, AsyncFlowOption))
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
        #endregion

        #region # 工作单元联合处理领域事件提交 —— static void DistributedCommit(this IUnitOfWork unitOfWork)
        /// <summary>
        /// 工作单元联合处理领域事件提交
        /// </summary>
        /// <param name="unitOfWork">工作单元实例</param>
        /// <remarks>采用分布式事务</remarks>
        public static void DistributedCommit(this IUnitOfWork unitOfWork)
        {
            try
            {
                //开启事务
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, AsyncFlowOption))
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
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress, AsyncFlowOption))
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
        #endregion

        #region # 工作单元联合处理领域事件提交 —— static async Task DistributedCommitAsync(this IUnitOfWork unitOfWork)
        /// <summary>
        /// 工作单元联合处理领域事件提交
        /// </summary>
        /// <param name="unitOfWork">工作单元实例</param>
        /// <remarks>采用分布式事务</remarks>
        public static async Task DistributedCommitAsync(this IUnitOfWork unitOfWork)
        {
            try
            {
                //开启事务
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, AsyncFlowOption))
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
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress, AsyncFlowOption))
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
        #endregion
    }
}
