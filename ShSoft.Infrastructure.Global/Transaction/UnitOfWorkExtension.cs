using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using SD.IOC.Core.Mediator;
using ShSoft.Infrastructure.EventBase.Mediator;
using ShSoft.Infrastructure.RepositoryBase;

namespace ShSoft.Infrastructure.Global.Transaction
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
            await Task.Run(() => UnitedCommit(unitOfWork));
        }

        /// <summary>
        /// 多UnitOfWork联合提交
        /// </summary>
        public static void UnitedCommit()
        {
            //获取UnitOfWork实现集合
            IUnitOfWork[] unitOfWorks = ResolveMediator.ResolveAll<IUnitOfWork>().ToArray();

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    foreach (IUnitOfWork unitOfWork in unitOfWorks)
                    {
                        //提交工作单元
                        unitOfWork.Commit();
                    }

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
                    foreach (IUnitOfWork unitOfWork in unitOfWorks)
                    {
                        //提交工作单元
                        unitOfWork.RollBack();
                    }

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
