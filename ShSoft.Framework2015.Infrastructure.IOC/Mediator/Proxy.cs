namespace ShSoft.Framework2015.Infrastructure.IOC.Mediator
{
    /// <summary>
    /// 实例代理
    /// </summary>
    public static class Proxy<T> where T : class
    {
        /// <summary>
        /// 实例
        /// </summary>
        public static T Instance
        {
            get { return ResolverMediator.Resolve<T>(); }
        }
    }
}
