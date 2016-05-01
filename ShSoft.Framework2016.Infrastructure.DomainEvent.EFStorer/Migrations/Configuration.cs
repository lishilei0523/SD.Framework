using System.Data.Entity.Migrations;
using ShSoft.Framework2016.Infrastructure.DomainEvent.EFStorer.Provider;

namespace ShSoft.Framework2016.Infrastructure.DomainEvent.EFStorer.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<EntityFrameworkStorerProvider>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(EntityFrameworkStorerProvider context)
        {

        }
    }
}
