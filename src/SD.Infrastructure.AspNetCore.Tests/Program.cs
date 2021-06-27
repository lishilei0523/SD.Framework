using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using SD.Toolkits.AspNet;
using SD.Toolkits.AspNet.Configurations;
using System.Collections.Generic;
using System.Linq;

namespace SD.Infrastructure.AspNetCore.Tests
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IHostBuilder hostBuilder = Host.CreateDefaultBuilder();

            //WebHost配置
            hostBuilder.ConfigureWebHostDefaults(webBuilder =>
            {
                ICollection<string> urls = new HashSet<string>();
                foreach (HostElement hostElement in AspNetSection.Setting.HostElements)
                {
                    urls.Add(hostElement.Url);
                }

                webBuilder.UseKestrel();
                webBuilder.UseUrls(urls.ToArray());
                webBuilder.UseStartup<Startup>();
            });

            //依赖注入配置
            hostBuilder.UseServiceProviderFactory(new ServiceLocator());

            IHost host = hostBuilder.Build();
            host.Run();
        }
    }
}
