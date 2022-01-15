using SD.Infrastructure.EventBase.Tests.IRepositories;
using SD.Infrastructure.Repository.EntityFrameworkCore;

namespace SD.Infrastructure.EventBase.Tests.Repositories.Base
{
    /// <summary>
    /// 单元事务 - Stub
    /// </summary>
    public sealed class UnitOfWork : EFUnitOfWorkProvider, IUnitOfWorkStub
    {

    }
}
