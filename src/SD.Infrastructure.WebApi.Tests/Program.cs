using Microsoft.Owin;
using SD.Infrastructure.WebApi.Tests;
using Topshelf;

[assembly: OwinStartup(typeof(Startup))]
namespace SD.Infrastructure.WebApi.Tests
{
    class Program
    {
        static void Main()
        {
            HostFactory.Run(config =>
            {
                config.Service<ServiceLauncher>(host =>
                {
                    host.ConstructUsing(name => new ServiceLauncher());
                    host.WhenStarted(launcher => launcher.Start());
                    host.WhenStopped(launcher => launcher.Stop());
                });
                config.RunAsLocalSystem();

                config.SetServiceName("SD.Infrastructure.WebApi.Tests");
                config.SetDisplayName("SD.Infrastructure.WebApi.Tests");
                config.SetDescription("SD.Infrastructure.WebApi.Tests");
            });
        }
    }
}
