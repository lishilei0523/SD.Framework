using System.Data.Entity.Migrations;
using ShSoft.Framework2015.Infrastructure.DomainEvent.EFStorerTests.EventStorer;

namespace ShSoft.Framework2015.Infrastructure.DomainEvent.EFStorerTests.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<EntityFrameworkStorer>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(EntityFrameworkStorer context)
        {

        }
    }
}
