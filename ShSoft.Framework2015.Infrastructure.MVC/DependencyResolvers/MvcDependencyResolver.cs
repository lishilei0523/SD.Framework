using System;
using System.Collections.Generic;
using System.Web.Mvc;
using ShSoft.Framework2015.Infrastructure.IOC.Mediator;

namespace ShSoft.Framework2015.Infrastructure.MVC.DependencyResolvers
{
    /// <summary>
    /// MVC依赖解决者
    /// </summary>
    public class MvcDependencyResolver : IDependencyResolver
    {
        /// <summary>
        /// 解析支持任意对象创建的一次注册的服务
        /// </summary>
        /// <param name="serviceType">所请求的服务或对象的类型</param>
        /// <returns> 请求的服务或对象 </returns>
        public object GetService(Type serviceType)
        {
            try
            {
                return ResolverMediator.Resolve(serviceType);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 解析多次注册的服务
        /// </summary>
        /// <param name="serviceType">所请求的服务的类型</param>
        /// <returns>请求的服务</returns>
        public IEnumerable<object> GetServices(Type serviceType)
        {
            try
            {
                return ResolverMediator.ResolveAll(serviceType);
            }
            catch
            {
                return null;
            }

        }
    }
}