using SD.Infrastructure.Repository.EntityFramework.Tests.IRepositories;

namespace SD.Infrastructure.Repository.EntityFramework.Tests.Repositories.Base
{
    /// <summary>
    /// 单元事务 - Stub
    /// </summary>
    public sealed class UnitOfWork : EFUnitOfWorkProvider, IUnitOfWorkStub
    {
        public void RegisterAddOrderBySql(string orderNo)
        {
            string sql =
                $"INSERT INTO dbo.[Order] (Id, Checked, Number, Name, Keywords, SavedTime, Deleted, DeletedTime, CreatorAccount, OperatorAccount,  OperatorName, AddedTime) VALUES (NEWID(), 0, '{orderNo}', '测试SQL', NULL, GETDATE(), 0, NULL, NULL, NULL, NULL, GETDATE());";

            base.RegisterSqlCommand(sql);
        }
    }
}
