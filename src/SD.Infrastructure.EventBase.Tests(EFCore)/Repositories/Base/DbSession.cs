using Microsoft.EntityFrameworkCore;
using SD.Infrastructure.Constants;
using SD.Infrastructure.Repository.EntityFrameworkCore.Base;

namespace SD.Infrastructure.EventBase.Tests.Repositories.Base
{
    /// <summary>
    /// EF Core上下文
    /// </summary>
    internal class DbSession : DbSessionBase
    {
        /// <summary>
        /// 配置
        /// </summary>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(GlobalSetting.WriteConnectionString);

            base.OnConfiguring(optionsBuilder);
        }
    }
}
