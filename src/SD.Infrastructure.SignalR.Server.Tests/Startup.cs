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
    /// Ӧ�ó���������
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// ���÷���
        /// </summary>
        public void ConfigureServices(IServiceCollection services)
        {
            //��ӿ���
            services.AddCors(options => options.AddPolicy(typeof(Startup).FullName!,
                builder =>
                {
                    builder.AllowAnyMethod()
                        .AllowAnyHeader()
                        .SetIsOriginAllowed(_ => true)
                        .AllowCredentials();
                }));

            //���SignalR
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
        /// ����Ӧ�ó���
        /// </summary>
        public void Configure(IApplicationBuilder appBuilder)
        {
            //���ÿ���
            appBuilder.UseCors(typeof(Startup).FullName!);

            appBuilder.UseRouting();
            appBuilder.UseEndpoints(endpoints =>
            {
                //����SignalR·��
                endpoints.MapHub<StubMessageHub>("/StubMessage");
            });
        }
    }
}
