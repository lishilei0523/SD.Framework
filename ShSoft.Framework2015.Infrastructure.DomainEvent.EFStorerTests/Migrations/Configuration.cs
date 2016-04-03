using System.Data.Entity.Migrations;
using ShSoft.Framework2015.Infrastructure.DomainEvent.EFStorer.EventStorer;

namespace ShSoft.Framework2015.Infrastructure.DomainEvent.EFStorer.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<EntityFrameworkStorer>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(EntityFrameworkStorer context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
