using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SD.Infrastructure.Constants;
using SD.Infrastructure.Repository.EntityFramework.Tests.Entities;
using SD.Infrastructure.Repository.EntityFramework.Tests.IRepositories;
using SD.Infrastructure.Repository.EntityFramework.Tests.Repositories.Base;
using SD.IOC.Core.Mediators;
using SD.IOC.Extension.NetFx;
using System;
using System.Data.Entity.Infrastructure;

namespace SD.Infrastructure.Repository.EntityFramework.Tests.TestCases
{
    /// <summary>
    /// 注册SQL命令测试
    /// </summary>
    [TestClass]
    public class RegisterSqlCommandTests
    {
        /// <summary>
        /// 单元事务
        /// </summary>
        private IUnitOfWorkStub _unitOfWork;

        /// <summary>
        /// 初始化测试
        /// </summary>
        [TestInitialize]
        public void Init()
        {
            //初始化依赖注入容器
            if (!ResolveMediator.ContainerBuilt)
            {
                IServiceCollection serviceCollection = ResolveMediator.GetServiceCollection();
                serviceCollection.RegisterConfigs();
                ResolveMediator.Build();
            }

            DbSession dbSession = new DbSession(GlobalSetting.WriteConnectionString);
            dbSession.Database.CreateIfNotExists();

            this._unitOfWork = ResolveMediator.Resolve<IUnitOfWorkStub>();
        }

        /// <summary>
        /// 清理测试
        /// </summary>
        [TestCleanup]
        public void Cleanup()
        {
            ResolveMediator.Dispose();
        }

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
            this._unitOfWork.CommitAsync().Wait();
        }

        /// <summary>
        /// 测试注册SQL命令异常
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(DbUpdateException))]
        public void TestRegisterSqlCommandException()
        {
            Order order = new Order("TESTSQL", $"销售订单-{DateTime.Now:yyyyMMddHHmmss}");

            this._unitOfWork.RegisterAdd(order);
            this._unitOfWork.RegisterAddOrderBySql("TESTSQL");
            this._unitOfWork.Commit();
        }
    }
}
