namespace SD.Infrastructure.Repository.EntityFrameworkCore.Tests.Base
{
    /// <summary>
    /// 单元事务 - Stub
    /// </summary>
    public sealed class UnitOfWork : EFUnitOfWorkProvider, IUnitOfWorkStub
    {
        #region # 注册SQL创建单据 —— void RegisterAddOrderBySql(string orderNo)
        /// <summary>
        /// 注册SQL创建单据
        /// </summary>
        /// <param name="orderNo">单据编号</param>
        public void RegisterAddOrderBySql(string orderNo)
        {
            string sql =
                $"INSERT INTO dbo.[Order] (Id, Checked, Number, Name, Keywords, SavedTime, Deleted, DeletedTime, CreatorAccount, OperatorAccount,  OperatorName, AddedTime) VALUES (NEWID(), 0, '{orderNo}', '测试SQL', NULL, GETDATE(), 0, NULL, NULL, NULL, NULL, GETDATE());";

            this.RegisterSqlCommand(sql);
        }
        #endregion
    }
}
