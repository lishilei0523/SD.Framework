using SD.Infrastructure.WCF.Tests.Implements;
using System;
using System.ServiceModel;

namespace SD.Infrastructure.WCF.Tests
{
    class Program
    {
        static void Main()
        {
            ServiceHost orderContractHost = new ServiceHost(typeof(OrderContract));
            orderContractHost.Open();

            Console.WriteLine("服务已启动...");
            Console.ReadKey();
        }
    }
}
