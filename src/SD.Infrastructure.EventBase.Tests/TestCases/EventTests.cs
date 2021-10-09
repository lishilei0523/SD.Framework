using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SD.Infrastructure.Constants;
using SD.Infrastructure.EventBase.Mediator;
using SD.Infrastructure.EventBase.Tests.Entities;
using SD.Infrastructure.EventBase.Tests.IRepositories;
using SD.Infrastructure.EventBase.Tests.Repositories.Base;
using SD.Infrastructure.Global.Transaction;
using SD.IOC.Core.Mediators;
using SD.IOC.Extension.NetFx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace SD.Infrastructure.EventBase.Tests.TestCases
{
    /// <summary>
    /// 领域事件测试
    /// </summary>
    [TestClass]
    public class EventTests
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
            if (!ResolveMediator.ContainerBuilt)
            {
                IServiceCollection builder = ResolveMediator.GetServiceCollection();
                builder.RegisterConfigs();

                ResolveMediator.Build();
            }

            DbSession dbSession = new DbSession();
            dbSession.Database.CreateIfNotExists();

            this._unitOfWork = ResolveMediator.Resolve<IUnitOfWorkStub>();

            GlobalSetting.InitCurrentSessionId();
        }

        /// <summary>
        /// 清理测试
        /// </summary>
        [TestCleanup]
        public void Cleanup()
        {
            GlobalSetting.FreeCurrentSessionId();
            ResolveMediator.Dispose();
        }

        /// <summary>
        /// 测试领域事件
        /// </summary>
        [TestMethod]
        public void TestEvent()
        {
            Order order = new Order($"SL-{DateTime.Now:yyyyMMddHHmmss}", $"销售订单-{DateTime.Now:yyyyMMddHHmmss}");
            order.Check();

            this._unitOfWork.RegisterAdd(order);
            this._unitOfWork.UnitedCommit();
        }

        /// <summary>
        /// 测试单据创建
        /// </summary>
        [TestMethod]
        public void TestCreateOrder()
        {
            const int runCount = 1000;

            IList<Guid> sessionIds = new List<Guid>();
            for (int i = 0; i < runCount; i++)
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    GlobalSetting.InitCurrentSessionId();
                    sessionIds.Add(GlobalSetting.CurrentSessionId);

                    Order order = new Order(i.ToString(), "测试单据" + i);
                    order.Check();

                    EventMediator.HandleUncompletedEvents();
                    scope.Complete();
                }
            }

            Assert.IsTrue(sessionIds.Count == runCount);
            Assert.IsTrue(sessionIds.Distinct().Count() == runCount);
        }
    }
}
