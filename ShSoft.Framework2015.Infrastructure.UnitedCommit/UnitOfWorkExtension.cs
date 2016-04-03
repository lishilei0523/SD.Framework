using System.Transactions;
using ShSoft.Framework2015.Infrastructure.DomainEvent.Mediator;
using ShSoft.Framework2015.Infrastructure.IRepository;

namespace ShSoft.Framework2015.Infrastructure.UnitedCommit
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
            try
            {
                //开启事务
                using (TransactionScope scope = new TransactionScope())
                {
                    //提交事务
                    unitOfWork.Commit();

                    //处理领域事件
                    EventMediator.HandleUncompletedEvents();

                    scope.Complete();
                }
            }
            catch
            {
                //无事务
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
                {
                    unitOfWork.RollBack();
                    EventMediator.ClearUncompletedEvents();
                    scope.Complete();
                }
                throw;
            }
        }
    }
}
