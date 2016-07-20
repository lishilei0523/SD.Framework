using System;
using ShSoft.Infrastructure.EventBase.Mediator;
using ShSoft.Infrastructure.EventStoreProvider.EntityFrameworkTests.Entities;
using ShSoft.Infrastructure.Global;

namespace ShSoft.Infrastructure.EventStoreProvider.EntityFrameworkTests
{
    class Program
    {
        static void Main(string[] args)
        {
            Initializer.InitSessionId();

            Product product = new Product();

            EventMediator.HandleUncompletedEvents();

            Console.WriteLine("OK");
            Console.ReadKey();
        }
    }
}
