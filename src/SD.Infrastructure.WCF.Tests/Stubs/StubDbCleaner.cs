using SD.Infrastructure.RepositoryBase;
using System;

namespace SD.Infrastructure.WCF.Tests.Stubs
{
    /// <summary>
    /// 数据库清理者桩
    /// </summary>
    public class StubDbCleaner : IDbCleaner
    {
        /// <summary>
        /// 清理
        /// </summary>
        public void Clean()
        {
            Console.WriteLine("数据库清理已执行..");
        }
    }
}