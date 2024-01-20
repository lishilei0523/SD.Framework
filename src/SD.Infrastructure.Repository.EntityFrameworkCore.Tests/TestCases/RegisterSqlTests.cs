using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SD.Common;
using SD.Infrastructure.Repository.EntityFrameworkCore.Tests.Base;
using SD.Infrastructure.Repository.EntityFrameworkCore.Tests.Entities;
using SD.IOC.Core;
using SD.IOC.Core.Mediators;
using SD.IOC.Extension.NetCore;
using System;
using System.Configuration;
using System.Reflection;

namespace SD.Infrastructure.Repository.EntityFrameworkCore.Tests.TestCases
{
    /// <summary>
    /// 注册SQL测试
    /// </summary>
    [TestClass]
    public class RegisterSqlTests
    {
        #region # 测试初始化

        /// <summary>
        /// 单元事务
        /// </summary>
        private IUnitOfWorkStub _unitOfWork;

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
            DependencyInjectionSection.Initialize(configuration);

            //初始化依赖注入容器
            if (!ResolveMediator.ContainerBuilt)
            {
                IServiceCollection serviceCollection = ResolveMediator.GetServiceCollection();
                serviceCollection.RegisterConfigs();
                ResolveMediator.Build();
            }

            this._unitOfWork = ResolveMediator.Resolve<IUnitOfWorkStub>();
        }

        #endregion

        #region # 测试清理 —— void Cleanup()
        /// <summary>
        /// 测试清理
        /// </summary>
        [TestCleanup]
        public void Cleanup()
        {
            ResolveMediator.Dispose();
        }
        #endregion

        #region # 测试注册SQL命令 —— void TestRegisterSqlCommand()
        /// <summary>
        /// 测试注册SQL命令
        /// </summary>
        [TestMethod]
        public void TestRegisterSqlCommand()
        {
            Order order1 = new Order($"SL1-{DateTime.Now:yyyyMMddHHmmss}", $"销售订单1-{DateTime.Now:yyyyMMddHHmmss}");

            this._unitOfWork.RegisterAdd(order1);
            this._unitOfWork.RegisterAddOrderBySql($"测试订单1-{DateTime.Now:yyyyMMddHHmmss}");
            this._unitOfWork.Commit();

            Order order2 = new Order($"SL2-{DateTime.Now:yyyyMMddHHmmss}", $"销售订单2-{DateTime.Now:yyyyMMddHHmmss}");

            this._unitOfWork.RegisterAdd(order2);
            this._unitOfWork.RegisterAddOrderBySql($"测试订单2-{DateTime.Now:yyyyMMddHHmmss}");
            this._unitOfWork.Commit();
        }
        #endregion

        #region # 测试注册SQL命令异常 —— void TestRegisterSqlCommandException()
        /// <summary>
        /// 测试注册SQL命令异常
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SqlException))]
        public void TestRegisterSqlCommandException()
        {
            Order order = new Order("TESTSQL", $"销售订单-{DateTime.Now:yyyyMMddHHmmss}");

            this._unitOfWork.RegisterAdd(order);
            this._unitOfWork.RegisterAddOrderBySql("TESTSQL");
            this._unitOfWork.Commit();
        }
        #endregion
    }
}
