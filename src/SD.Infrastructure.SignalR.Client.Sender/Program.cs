using Microsoft.AspNetCore.SignalR.Client;
using SD.Infrastructure.SignalR.Client.Extensions;
using SD.Infrastructure.SignalR.Client.Mediators;
using SD.Infrastructure.SignalR.Stubs.Messages;
using System;
using System.Threading.Tasks;

namespace SD.Infrastructure.SignalR.Client.Sender
{
    class Program
    {
        static async Task Main()
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

            while (true)
            {
                Console.WriteLine("请输入消息标题！");
                string title = Console.ReadLine();
                Console.WriteLine("请输入消息内容！");
                string content = Console.ReadLine();

                StubMessage message = new StubMessage(title, content);
                await MessageMediator.SendAsync(hubConnection, message);

                Console.WriteLine("发送成功！");
                Console.WriteLine("-------------------------------------");
            }
        }
    }
}
