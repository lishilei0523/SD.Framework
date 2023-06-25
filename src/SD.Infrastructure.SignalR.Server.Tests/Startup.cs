using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using SD.Infrastructure.SignalR.Server.Tests.Hubs;
using SD.Toolkits.Redis;
using StackExchange.Redis;
using System.Threading.Tasks;

namespace SD.Infrastructure.SignalR.Server.Tests
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
            //添加跨域
            services.AddCors(options => options.AddPolicy(typeof(Startup).FullName!,
                builder =>
                {
                    builder.AllowAnyMethod()
                        .AllowAnyHeader()
                        .SetIsOriginAllowed(_ => true)
                        .AllowCredentials();
                }));

            //添加SignalR
            ISignalRServerBuilder signalRServerBuilder = services.AddSignalR(options =>
            {
                options.EnableDetailedErrors = true;
            });
            signalRServerBuilder.AddStackExchangeRedis(options =>
            {
                options.ConnectionFactory = _ => Task.Run(() => RedisManager.Instance as IConnectionMultiplexer);
            });
        }

        /// <summary>
        /// 配置应用程序
        /// </summary>
        public void Configure(IApplicationBuilder appBuilder)
        {
            //配置跨域
            appBuilder.UseCors(typeof(Startup).FullName!);

            appBuilder.UseRouting();
            appBuilder.UseEndpoints(endpoints =>
            {
                //配置SignalR路由
                endpoints.MapHub<StubMessageHub>("/StubMessage");
            });
        }
    }
}
