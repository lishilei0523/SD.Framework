using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using SD.Toolkits.AspNet;

namespace SD.Infrastructure.AspNetCore.Tests
{
    class Program
    {
        static void Main()
        {
            IHostBuilder hostBuilder = Host.CreateDefaultBuilder();

            //WebHost配置
            hostBuilder.ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseKestrel(options =>
                {
                    foreach (int httpPort in AspNetSetting.HttpPorts)
                    {
                        options.ListenAnyIP(httpPort);
                    }
                });
                webBuilder.UseStartup<Startup>();
            });

            //依赖注入配置
            hostBuilder.UseServiceProviderFactory(new ServiceLocator());

            IHost host = hostBuilder.Build();
            host.Run();
        }
    }
}
