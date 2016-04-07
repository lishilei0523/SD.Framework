using System;
using ShSoft.Framework2016.Infrastructure.DomainEvent.EFStorerTests.Entities;
using ShSoft.Framework2016.Infrastructure.DomainEvent.EFStorerTests.EventSources;
using ShSoft.Framework2016.Infrastructure.DomainEvent.EFStorerTests.EventStorer;
using ShSoft.Framework2016.Infrastructure.DomainEvent.Mediator;
using ShSoft.Framework2016.Infrastructure.Global;

namespace ShSoft.Framework2016.Infrastructure.DomainEvent.EFStorerTests
{
    class Program
    {
        static void Main(string[] args)
        {
            InitSessionId.Register();

            Product product = new Product();

            EventMediator.HandleUncompletedEvents();

            Console.WriteLine("OK");
            Console.ReadKey();
        }
    }
}
