using Topshelf;

namespace SD.Infrastructure.WCF.Tests
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

                config.SetServiceName("SD.Infrastructure.WCF.Tests");
                config.SetDisplayName("SD.Infrastructure.WCF.Tests");
                config.SetDescription("SD.Infrastructure.WCF测试");
            });
        }
    }
}
