using System;
using ShSoft.Framework2015.Infrastructure.DomainEvent.EFStorer.EventSources;
using ShSoft.Framework2015.Infrastructure.DomainEvent.EFStorer.EventStorer;

namespace ShSoft.Framework2015.Infrastructure.DomainEvent.EFStorer
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
