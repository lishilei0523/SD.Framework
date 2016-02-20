using System.Data.Entity.Migrations;
using ShSoft.Framework2015.LogSite.Model.Base;

namespace ShSoft.Framework2015.LogSite.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<DbSession>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(DbSession context)
        {

        }
    }
}
