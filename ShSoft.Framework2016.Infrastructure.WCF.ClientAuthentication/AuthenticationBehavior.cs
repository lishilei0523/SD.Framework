using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace ShSoft.Framework2016.Infrastructure.WCF.ClientAuthentication
{
    /// <summary>
    /// 客户端身份认证行为
    /// </summary>
    internal class AuthenticationBehavior : IEndpointBehavior
    {
        public void Validate(ServiceEndpoint endpoint) { }

        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters) { }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher) { }

        /// <summary>
        /// 适用客户端行为
        /// </summary>
        /// <param name="endpoint">服务终结点</param>
        /// <param name="clientRuntime">客户端运行时</param>
        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            //添加消息拦截器
            clientRuntime.MessageInspectors.Add(new AuthenticationMessageInspector());
        }
    }
}
