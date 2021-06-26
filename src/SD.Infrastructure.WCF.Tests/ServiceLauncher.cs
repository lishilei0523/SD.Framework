using SD.Infrastructure.WCF.Tests.Implements;
using System;
using System.ServiceModel;

namespace SD.Infrastructure.WCF.Tests
{
    /// <summary>
    /// 服务启动器
    /// </summary>
    public class ServiceLauncher
    {
        private readonly ServiceHost _orderContractHost;

        /// <summary>
        /// 构造器
        /// </summary>
        public ServiceLauncher()
        {
            this._orderContractHost = new ServiceHost(typeof(OrderContract));
        }

        /// <summary>
        /// 开始
        /// </summary>
        public void Start()
        {
            this._orderContractHost.Open();

            Console.WriteLine("服务已启动...");
        }

        /// <summary>
        /// 停止
        /// </summary>
        public void Stop()
        {
            this._orderContractHost.Close();

            Console.WriteLine("服务已关闭...");
        }
    }
}
