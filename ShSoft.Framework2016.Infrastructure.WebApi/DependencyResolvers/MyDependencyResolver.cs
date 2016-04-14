using SD.IOC.Integration.WebApi;
using ShSoft.Framework2016.Infrastructure.Global.Finalization;

namespace ShSoft.Framework2016.Infrastructure.WebApi.DependencyResolvers
{
    /// <summary>
    /// WebApi依赖解决者
    /// </summary>
    internal class MyDependencyResolver : WebApiDependencyResolver
    {
        /// <summary>
        /// 释放资源
        /// </summary>
        public override void Dispose()
        {
            //清理数据库
            FinalizeDatabase.Register();
        }
    }
}