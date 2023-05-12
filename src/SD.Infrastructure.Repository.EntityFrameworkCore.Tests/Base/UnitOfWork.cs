namespace SD.Infrastructure.Repository.EntityFrameworkCore.Tests.Base
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

            this.RegisterSqlCommand(sql);
        }
    }
}
