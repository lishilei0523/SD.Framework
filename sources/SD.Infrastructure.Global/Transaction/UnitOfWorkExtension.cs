using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using SD.Infrastructure.EventBase.Mediator;
using SD.Infrastructure.RepositoryBase;

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
        /// UnitOfWork集联合提交（包含处理领域事件）扩展方法
        /// </summary>
        /// <param name="unitOfWorks">工作单元实例集</param>
        public static void UnitedCommit(this IEnumerable<IUnitOfWork> unitOfWorks)
        {
            #region # 验证参数

            if (unitOfWorks == null)
            {
                throw new ArgumentNullException("unitOfWorks", "工作单元实例集不可为空！");
            }

            #endregion

            unitOfWorks = unitOfWorks.ToArray();

            try
            {
                //开启事务
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
                    //回滚工作单元
                    foreach (IUnitOfWork unitOfWork in unitOfWorks)
                    {
                        //提交工作单元
                        unitOfWork.Commit();
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
