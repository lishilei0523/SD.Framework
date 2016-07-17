using System.Data.Entity.Migrations;

namespace ShSoft.Infrastructure.EventStoreProvider.EntityFramework.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<EntityFrameworkStoreProvider>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(EntityFrameworkStoreProvider context)
        {

        }
    }
}
