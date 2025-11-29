using SD.Infrastructure.AspNetCore.Server.Base;
using SD.Infrastructure.SignalR.Stubs.Messages;

namespace SD.Infrastructure.SignalR.Server.Tests.Hubs
{
    /// <summary>
    /// 消息Hub
    /// </summary>
    public class StubMessageHub : MessageHub<StubMessage>
    {

    }
}
