using SD.Infrastructure.Constants;
using SD.Infrastructure.StubIAppService.Interfaces;
using SD.Infrastructure.WCF.Tests.Interfaces;
using System;
using System.ServiceModel;

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
        /// <param name="productContract"></param>
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
            Console.WriteLine($"当前会话Id：\"{GlobalSetting.CurrentSessionId}\"");
            return this._productContract.GetProducts();
        }
    }
}