using CoreWCF.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using SD.Infrastructure.WCF.Server;
using SD.Infrastructure.WCF.Tests.Implements;
using SD.IOC.Integration.WCF.Behaviors;
using System.Configuration;

namespace SD.Infrastructure.WCF.Tests
{
    /// <summary>
    /// 应用程序启动器
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// 配置服务
        /// </summary>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddServiceModelServices();

            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            services.AddServiceModelConfigurationManagerFile(configuration.FilePath);
        }

        /// <summary>
        /// 配置应用程序
        /// </summary>
        public void Configure(IApplicationBuilder appBuilder)
        {
            //配置WCF服务
            DependencyInjectionBehavior dependencyInjectionBehavior = new DependencyInjectionBehavior();
            InitializationBehavior initializationBehavior = new InitializationBehavior();

            appBuilder.UseServiceModel(builder =>
            {
                builder.ConfigureServiceHostBase<OrderContract>(host =>
                {
                    host.Description.Behaviors.Add(dependencyInjectionBehavior);
                    host.Description.Behaviors.Add(initializationBehavior);
                });
            });
        }
    }
}
