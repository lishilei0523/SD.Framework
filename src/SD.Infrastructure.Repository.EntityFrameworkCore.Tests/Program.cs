using SD.Infrastructure.Constants;
using SD.Infrastructure.Repository.EntityFrameworkCore.Stubs.Base;
using SD.Infrastructure.Repository.EntityFrameworkCore.Stubs.Entities;
using System;

namespace SD.Infrastructure.Repository.EntityFrameworkCore.Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = NetCoreSetting.DefaultConnectionString;
            Console.WriteLine(connectionString);

            FrameworkSection setting = NetCoreSetting.FrameworkSettings;

            Console.WriteLine(setting.ApplicationName.Value);
            Console.WriteLine(setting.ApplicationVersion.Value);
            Console.WriteLine(setting.ServiceName.Value);
            Console.WriteLine(setting.ServiceDisplayName.Value);
            Console.WriteLine(setting.ServiceDescription.Value);
            Console.WriteLine(setting.ServiceConnectionName.Value);
            Console.WriteLine(setting.EntityAssembly.Value);
            Console.WriteLine(setting.EntityConfigAssembly.Value);
            Console.WriteLine(setting.EntityTablePrefix.Value);
            Console.WriteLine(setting.EventSourceAssembly.Value);
            Console.WriteLine(setting.CrontabAssembly.Value);
            Console.WriteLine(setting.WorkflowAssembly.Value);
            Console.WriteLine(setting.WorkflowConnectionName.Value);
            Console.WriteLine(setting.WorkflowPersistenceMode.Value);
            Console.WriteLine(setting.WorkflowMaxInstanceLockedRetriesCount.Value);
            Console.WriteLine(setting.WindowsUpdateService.Value);
            Console.WriteLine(setting.AuthenticationTimeout.Value);
            Console.WriteLine(setting.AuthorizationEnabled.Value);

            using (DbSession dbSession = new DbSession())
            {
                User user = new User
                {
                    PrivateKey = Guid.NewGuid().ToString(),
                    Password = CommonConstants.InitialPassword,
                    Enabled = true
                };

                dbSession.Set<User>().Add(user);
                dbSession.SaveChanges();

                Console.WriteLine("创建成功！");
            }

            Console.ReadKey();
        }
    }
}
