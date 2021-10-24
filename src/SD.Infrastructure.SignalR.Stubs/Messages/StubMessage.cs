using SD.Infrastructure.MessageBase;

namespace SD.Infrastructure.SignalR.Stubs.Messages
{
    public class StubMessage : TransientMessage
    {
        public StubMessage()
            : base()
        {

        }

        public StubMessage(string title, string content)
            : this()
        {
            base.Title = title;
            base.Content = content;
        }
    }
}
