using System.Linq;
using Raven.Client.Indexes;
using ShSoft.Infrastructure.EventBase;

namespace ShSoft.Infrastructure.EventStoreProvider.RavenDB.Configuration
{
    /// <summary>
    /// 
    /// </summary>
    public class EventIndex : AbstractMultiMapIndexCreationTask
    {
        public EventIndex()
        {
            this.AddMapForAll<Event>(events => events.Select(e => new { e.Id, e.AddedTime, e.Handled, e.SessionId }));
        }
    }
}
