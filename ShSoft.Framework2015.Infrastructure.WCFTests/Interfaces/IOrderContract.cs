using System.ServiceModel;

namespace ShSoft.Framework2015.Infrastructure.WCFTests.Interfaces
{
    /// <summary>
    /// 订单服务契约接口
    /// </summary>
    [ServiceContract(Namespace = "http://ShSoft.Framework2015.Infrastructure.WCFTests.Interfaces")]
    public interface IOrderContract : IApplicationService.IApplicationService
    {
        /// <summary>
        /// 获取订单
        /// </summary>
        /// <returns>订单</returns>
        [OperationContract]
        string GetOrder();
    }
}
