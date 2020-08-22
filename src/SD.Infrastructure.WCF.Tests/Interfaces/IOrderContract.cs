using SD.Infrastructure.AppServiceBase;
using System.ServiceModel;

namespace SD.Infrastructure.WCF.Tests.Interfaces
{
    /// <summary>
    /// 订单服务契约接口
    /// </summary>
    [ServiceContract(Namespace = "http://SD.Infrastructure.WCFTests.Interfaces")]
    public interface IOrderContract : IApplicationService
    {
        /// <summary>
        /// 获取订单
        /// </summary>
        /// <returns>订单</returns>
        [OperationContract]
        string GetOrder();
    }
}
