using SD.Infrastructure.RepositoryBase;

namespace SD.Infrastructure.Repository.EntityFrameworkCore.Tests.Base
{
    /// <summary>
    /// 单元事务 - Stub
    /// </summary>
    public interface IUnitOfWorkStub : IUnitOfWork
    {
        #region # 注册SQL创建单据 —— void RegisterAddOrderBySql(string orderNo)
        /// <summary>
        /// 注册SQL创建单据
        /// </summary>
        /// <param name="orderNo">单据编号</param>
        void RegisterAddOrderBySql(string orderNo);
        #endregion
    }
}
