using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SD.Common;
using SD.Infrastructure.Constants;
using SD.Infrastructure.Repository.EntityFrameworkCore.Tests.Base;
using SD.Infrastructure.Repository.EntityFrameworkCore.Tests.Entities;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Reflection;

namespace SD.Infrastructure.Repository.EntityFrameworkCore.Tests.TestCases
{
    /// <summary>
    /// CRUD测试
    /// </summary>
    [TestClass]
    public class CrudTests
    {
        /// <summary>
        /// 初始化测试
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            //初始化配置文件
            Assembly assembly = Assembly.GetExecutingAssembly();
            Configuration configuration = ConfigurationExtension.GetConfigurationFromAssembly(assembly);
            FrameworkSection.Initialize(configuration);
        }

        /// <summary>
        /// 测试创建用户
        /// </summary>
        [TestMethod]
        public void TestAddUser()
        {
            using (DbSession dbSession = new DbSession())
            {
                dbSession.Database.Migrate();
                User user = new User
                {
                    PrivateKey = Guid.NewGuid().ToString(),
                    Password = CommonConstants.InitialPassword,
                    Enabled = true
                };

                dbSession.Set<User>().Add(user);
                dbSession.SaveChanges();

                Trace.WriteLine("创建成功！");
            }
        }
    }
}
