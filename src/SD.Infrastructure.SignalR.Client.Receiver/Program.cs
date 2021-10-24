using Microsoft.AspNetCore.SignalR.Client;
using SD.Infrastructure.SignalR.Client.Extensions;
using SD.Infrastructure.SignalR.Client.Mediators;
using SD.Infrastructure.SignalR.Stubs.Messages;
using System;
using System.Threading.Tasks;

namespace SD.Infrastructure.SignalR.Client.Receiver
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string url = "http://localhost:2209/StubMessage";

            HubConnectionBuilder hubConnectionBuilder = new HubConnectionBuilder();
            hubConnectionBuilder.WithUrl(url);
            hubConnectionBuilder.WithAutomaticReconnect();

            HubConnection hubConnection = hubConnectionBuilder.Build();
            hubConnection.RegisterClosedHandler();
            hubConnection.RegisterReconnectingHandler();
            hubConnection.RegisterReconnectedHandler();

            await hubConnection.StartAsync();

            MessageMediator.Receive<StubMessage>(hubConnection, message =>
            {
                Console.WriteLine("收到新消息！");
                Console.WriteLine($"标题：{message.Title}");
                Console.WriteLine($"内容：{message.Content}");
                Console.WriteLine("-------------------------------------");
            });

            while (true)
            {
                Console.WriteLine("输入q退出");
                string text = Console.ReadLine();
                if (text == "q")
                {
                    break;
                }
            }
        }
    }
}
