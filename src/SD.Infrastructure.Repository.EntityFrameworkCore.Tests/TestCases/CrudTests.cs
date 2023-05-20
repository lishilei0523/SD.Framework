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
        #region # 测试初始化 —— void Initialize()
        /// <summary>
        /// 测试初始化
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            //初始化配置文件
            Assembly assembly = Assembly.GetExecutingAssembly();
            Configuration configuration = ConfigurationExtension.GetConfigurationFromAssembly(assembly);
            FrameworkSection.Initialize(configuration);
        }
        #endregion

        #region # 测试创建用户 —— void TestCreate()
        /// <summary>
        /// 测试用户
        /// </summary>
        [TestMethod]
        public void TestCreate()
        {
            using DbSession dbSession = new DbSession();
            dbSession.Database.Migrate();
            User user = new User(CommonConstants.AdminLoginId, "超级管理员", CommonConstants.InitialPassword, Guid.NewGuid().ToString(), 18);

            dbSession.Set<User>().Add(user);
            dbSession.SaveChanges();

            Trace.WriteLine("创建成功！");
        }
        #endregion
    }
}
