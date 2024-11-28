using SD.Infrastructure.Constants;
using SD.Infrastructure.StubIAppService.Interfaces;
using SD.Infrastructure.WCF.Tests.Interfaces;
using System;
using System.Diagnostics;
#if NET462_OR_GREATER
using System.ServiceModel;
#endif
#if NET8_0_OR_GREATER
using CoreWCF;
#endif

namespace SD.Infrastructure.WCF.Tests.Implements
{
    /// <summary>
    /// 订单服务契约实现
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class OrderContract : IOrderContract
    {
        /// <summary>
        /// 商品接口
        /// </summary>
        private readonly IProductContract _productContract;

        /// <summary>
        /// 依赖注入构造器
        /// </summary>
        public OrderContract(IProductContract productContract)
        {
            this._productContract = productContract;
        }

        /// <summary>
        /// 获取订单
        /// </summary>
        /// <returns>订单</returns>
        public string GetOrder()
        {
            Trace.WriteLine($"当前会话Id：\"{GlobalSetting.CurrentSessionId}\"");
            Console.WriteLine($"当前会话Id：\"{GlobalSetting.CurrentSessionId}\"");

            return this._productContract.GetProducts();
        }
    }
}
