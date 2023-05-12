using SD.Infrastructure.RepositoryBase;

namespace SD.Infrastructure.Repository.EntityFrameworkCore.Tests.Base
{
    /// <summary>
    /// 单元事务 - Stub
    /// </summary>
    public interface IUnitOfWorkStub : IUnitOfWork
    {
        void RegisterAddOrderBySql(string orderNo);
    }
}
