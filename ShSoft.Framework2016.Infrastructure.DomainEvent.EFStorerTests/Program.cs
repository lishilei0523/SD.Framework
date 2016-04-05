using System;
using ShSoft.Framework2016.Infrastructure.DomainEvent.EFStorerTests.EventSources;
using ShSoft.Framework2016.Infrastructure.DomainEvent.EFStorerTests.EventStorer;

namespace ShSoft.Framework2016.Infrastructure.DomainEvent.EFStorerTests
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
