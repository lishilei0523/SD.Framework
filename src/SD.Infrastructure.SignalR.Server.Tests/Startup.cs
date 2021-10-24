using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using SD.Infrastructure.SignalR.Server.Tests.Hubs;
using SD.Toolkits.Redis;
using StackExchange.Redis;
using System.Threading.Tasks;

namespace SD.Infrastructure.SignalR.Server.Tests
{
    public class Startup
    {
        /// <summary>
        /// ≈‰÷√∑˛ŒÒ
        /// </summary>
        public void ConfigureServices(IServiceCollection services)
        {
            //≈‰÷√øÁ”Ú
            services.AddCors(options => options.AddPolicy(typeof(Startup).FullName,
                builder =>
                {
                    builder.AllowAnyMethod()
                        .AllowAnyHeader()
                        .SetIsOriginAllowed(_ => true)
                        .AllowCredentials();
                }));

            //≈‰÷√SignalR
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
        /// ≈‰÷√”¶”√≥Ã–Ú
        /// </summary>
        public void Configure(IApplicationBuilder appBuilder)
        {
            //≈‰÷√øÁ”Ú
            appBuilder.UseCors(typeof(Startup).FullName);

            appBuilder.UseRouting();
            appBuilder.UseEndpoints(endpoints =>
            {
                //≈‰÷√SignalR¬∑”…
                endpoints.MapHub<StubMessageHub>("/StubMessage");
            });
        }
    }
}
