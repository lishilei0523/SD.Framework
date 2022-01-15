using SD.Infrastructure.Constants;
using SD.Infrastructure.Repository.EntityFramework.Base;

namespace SD.Infrastructure.EventBase.Tests.Repositories.Base
{
    /// <summary>
    /// EF上下文
    /// </summary>
    internal class DbSession : DbSessionBase
    {
        public DbSession()
            : base(GlobalSetting.ReadConnectionString)
        {

        }
    }
}
