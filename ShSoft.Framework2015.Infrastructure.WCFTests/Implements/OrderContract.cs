using System.ServiceModel;
using ShSoft.Framework2015.Infrastructure.StubIAppService.Interfaces;
using ShSoft.Framework2015.Infrastructure.WCF.IOC;
using ShSoft.Framework2015.Infrastructure.WCFTests.Interfaces;

namespace ShSoft.Framework2015.Infrastructure.WCFTests.Implements
{
    /// <summary>
    /// 订单服务契约实现
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    [IocServiceBehavior]
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
            return this._productContract.GetProducts();
        }
    }
}