using SD.Infrastructure.Repository.EntityFramework.Base;

namespace SD.Infrastructure.Repository.EntityFramework.Tests.Repositories.Base
{
    /// <summary>
    /// EF上下文
    /// </summary>
    internal class DbSession : DbSessionBase
    {
        public DbSession()
        {

        }

        public DbSession(string connectionString)
            : base(connectionString)
        {

        }
    }
}
