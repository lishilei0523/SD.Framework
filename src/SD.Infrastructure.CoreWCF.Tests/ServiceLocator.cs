using Microsoft.Extensions.DependencyInjection;
using SD.IOC.Core.Mediators;
using SD.IOC.Extension.NetCore;
using System;

namespace SD.Infrastructure.WCF.Tests
{
    /// <summary>
    /// 服务定位者
    /// </summary>
    public class ServiceLocator : IServiceProviderFactory<IServiceCollection>
    {
        #region # 创建服务建造者 —— IServiceCollection CreateBuilder(IServiceCollection services)
        /// <summary>
        /// 创建服务建造者
        /// </summary>
        public IServiceCollection CreateBuilder(IServiceCollection services)
        {
            IServiceCollection builder = ResolveMediator.GetServiceCollection();
            foreach (ServiceDescriptor serviceDescriptor in services)
            {
                builder.Add(serviceDescriptor);
            }

            builder.RegisterConfigs();
            ResolveMediator.Build();

            return builder;
        }
        #endregion

        #region # 创建服务提供者 —— IServiceProvider CreateServiceProvider(IServiceCollection...
        /// <summary>
        /// 创建服务提供者
        /// </summary>
        public IServiceProvider CreateServiceProvider(IServiceCollection containerBuilder)
        {
            IServiceProvider serviceProvider = ResolveMediator.GetServiceProvider();

            return serviceProvider;
        }
        #endregion
    }
}
