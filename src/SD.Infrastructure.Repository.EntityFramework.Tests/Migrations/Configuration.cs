using SD.Infrastructure.Repository.EntityFramework.Tests.Repositories.Base;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;

namespace SD.Infrastructure.Repository.EntityFramework.Tests.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<DbSession>
    {
        public Configuration()
        {
            this.TargetDatabase = new DbConnectionInfo(FrameworkSection.Setting.DatabaseWriteConnectionName.Value);
            this.AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(DbSession context)
        {

        }
    }
}
