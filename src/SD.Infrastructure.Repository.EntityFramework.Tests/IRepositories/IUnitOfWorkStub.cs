using SD.Infrastructure.RepositoryBase;

namespace SD.Infrastructure.Repository.EntityFramework.Tests.IRepositories
{
    /// <summary>
    /// 单元事务 - Stub
    /// </summary>
    public interface IUnitOfWorkStub : IUnitOfWork
    {
        void RegisterAddOrderBySql(string orderNo);
    }
}
