using Microsoft.EntityFrameworkCore;
using SD.Infrastructure.Constants;
using SD.Infrastructure.Repository.EntityFrameworkCore.Base;

namespace SD.Infrastructure.Repository.EntityFrameworkCore.Tests.Base
{
    /// <summary>
    /// EF Core上下文
    /// </summary>
    public class DbSession : DbSessionBase
    {
        /// <summary>
        /// 配置
        /// </summary>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(GlobalSetting.WriteConnectionString, options =>
            {
                options.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
            });
            base.OnConfiguring(optionsBuilder);
        }
    }
}
