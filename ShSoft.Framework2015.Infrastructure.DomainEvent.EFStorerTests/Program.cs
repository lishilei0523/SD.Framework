using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShSoft.Framework2015.Infrastructure.DomainEvent.EFProviderTests.EventSources;
using ShSoft.Framework2015.Infrastructure.DomainEvent.EFProviderTests.EventStorer;

namespace ShSoft.Framework2015.Infrastructure.DomainEvent.EFProviderTests
{
    class Program
    {
        static void Main(string[] args)
        {
            EntityFrameworkStorer storer = new EntityFrameworkStorer();

            ProductCreatedEvent newEvent = new ProductCreatedEvent();

            storer.Set<ProductCreatedEvent>().Add(newEvent);

            storer.SaveChanges();
            storer.Dispose();

            Console.WriteLine("OK");
            Console.ReadKey();
        }
    }
}
