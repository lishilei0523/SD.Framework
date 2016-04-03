using ShSoft.Framework2015.Infrastructure.Constants;
using ShSoft.Framework2015.Infrastructure.DomainEvent.EFStorer.Provider;

namespace ShSoft.Framework2015.Infrastructure.DomainEvent.EFProviderTests.EventStorer
{
    /// <summary>
    /// 领域事件源存储者
    /// </summary>
    public class EntityFrameworkStorer : EntityFrameworkStorerProvider
    {
        #region 事件源所在程序集 —— abstract string EventSourceAssembly
        /// <summary>
        /// 事件源所在程序集
        /// </summary>
        public override string EventSourceAssembly
        {
            get { return WebConfigSetting.EventSourceAssembly; }
        }
        #endregion

        #region 数据表名前缀 —— abstract string TablePrefix
        /// <summary>
        /// 数据表名前缀
        /// </summary>
        public override string TablePrefix
        {
            get { return WebConfigSetting.TablePrefix; }
        }
        #endregion
    }
}
